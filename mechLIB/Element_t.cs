using System;

namespace mechLIB
{
    
    public class Element_t
    {

        private readonly float DampRatio;
        private readonly float ro;
        private readonly float E;
        private readonly float A;
        private readonly float I;

        public int n1;
        public int n2;
        public int ID;
        public xyz_t[] F;
        public xyz_t radiusPoint;
        private readonly PhModels phMod;
        public Element_t(int _n1, int _n2, xyz_t _radiusPoint, int Counts, int _ID, props_t props)
        {
            phMod = props.phMod;
            ro = props.ro;
            DampRatio = props.DampRatio;
            ID = _ID;
            F = new xyz_t[Counts];
            for (int i = 0; i < F.Length; i++)
            {
                F[i] = new xyz_t();
            }
            E = props.E;
            n1 = _n1;
            n2 = _n2;
            radiusPoint = _radiusPoint;
            A = (float)Math.PI * maf.P2(_radiusPoint.z);
            I = maf.P3(A) / 12.0f;
        }
        public bool IsMyNode(int id) => (n1 == id || n2 == id) ? true : false;
        public void CalcForce(ref Rope_t rope, int t, float Re, float bloodV, float bloodP)
        {
            //getting length of link by measure between coords
            float L = 0;
            switch (phMod)
            {
                case PhModels.hook:
                    L = GetOwnLength(ref rope, 0);
                    break;
                case PhModels.hookGeomNon:
                    L = GetOwnLength(ref rope, t - 1);
                    break;
                case PhModels.mooneyRiv:
                    L = GetOwnLength(ref rope, t - 1);
                    break;
                default:
                    throw new Exception("unexpected behavior");
            }
            //getting position of link according base point
            xyz_t LinkPos = new xyz_t();
            LinkPos.Minus(rope.GetNodeRef(n1).deriv[t - 1].p, rope.GetNodeRef(n2).deriv[t - 1].p);
            //getting DCM for this link
            dcm_t dcm = new dcm_t(LinkPos, radiusPoint);
            //convert base point Ux to local coords
            xyz_t lBpUx = new xyz_t();
            dcm.ToLoc(rope.GetNodeRef(n1).deriv[t - 1].u, ref lBpUx);
            //convert n point Ux to local coords
            xyz_t lNpUx = new xyz_t();
            dcm.ToLoc(rope.GetNodeRef(n2).deriv[t - 1].u, ref lNpUx);
            //store delta of expansion
            xyz_t deltaL = new xyz_t();
            //lBpUx.x - lNpUx.x
            deltaL.Minus(lBpUx, lNpUx);
            GetFn(t, L, deltaL);
            //GetPressureForce(t, bloodP, L);
            //GetDragForce(t, Re, bloodV, L);
        }
        public void GetPhysicParam(ref Rope_t rope, int t, float Re, ref float m, ref float c)
        {
            float L = GetOwnLength(ref rope, t);
            m += ro * A * L;
            if (Re > 0)
            {
                //calc h of fluid on rod
                float thFluid = (radiusPoint.z * 2) / maf.sqrt(Re);
                //calc mass of fluid on rod
                m += (float)Math.PI * L * (maf.P2(radiusPoint.z + thFluid) - maf.P2(radiusPoint.z)) * 1060;
                //add mass of this fluid to mass of rod
            }

            c = DampRatio * 2f * (float)Math.Sqrt(m * ((E * A) / L));
            if (c <= 0)
            {
                throw new Exception("Calculated damping ratio of element can't be eaqul to zero");
            }

        }

        private float GetOwnLength(ref Rope_t rope, int t)
        {
            float L = crds.GetTotL(rope.GetNodeRef(n1).deriv[t].p, rope.GetNodeRef(n2).deriv[t].p);
            if (L == 0)
            {
                throw new Exception("Calculated length of element can't be eaqul to zero");
            }

            return L;
        }

        private void GetFn(int t, float L, xyz_t deltaL)
        {
            //calc Fn of link
            xyz_t Fn = new xyz_t();
            switch (phMod)
            {
                case PhModels.hook:
                    calcHookFn(ref Fn, L, deltaL);
                    break;
                case PhModels.hookGeomNon:
                    calcHookFn(ref Fn, L, deltaL);
                    break;
                case PhModels.mooneyRiv:
                    calcMooneyRivlinFn(ref Fn, L, deltaL);
                    break;
                default:
                    throw new Exception("unexpected behavior");
            }
            F[t].Plus(Fn);
        }
        private void calcHookFn(ref xyz_t Fn, float oldL, xyz_t deltaL)
        {
            Fn.x = 0 - (E * A / oldL * deltaL.x);
            //Fn[(int)C.y] = 12f * E * I / maf.P3(oldL2) * oldUy2;
        }
        private void calcMooneyRivlinFn(ref xyz_t Fn, float oldL, xyz_t deltaL)
        {
            xyz_t lamda = new xyz_t
            {
                x = deltaL.x / (oldL + 1)
            };
            const float C10 = 22956961.3f;
            const float C01 = -23512872.8f;
            float sigma = 2 * C10 * (lamda.x - (1 / maf.P2(lamda.x))) + 2 * C01 * (1 - (1 / maf.P3(lamda.x)));
            Fn.x = A * sigma;
        }
        private void GetPressureForce(int t, float bloodP, float L)
        {
            float Fpress = 0 - bloodP * A * 2 * L;
            F[t].Plus(Fpress);
        }
        private void GetDragForce(int t, float Re, float v, float L)
        {
            float Awet = 2 * (float)Math.PI * radiusPoint.z * L;
            float bloodViscosity = 3E-3f;
            float Be = 0.9f;
            float Cd = (Awet / A) * (Be / Re);
            float Fdrag = maf.hlf * 1060 * maf.P2(v) * Cd * A;
            //is it has to be applied only on moving direction??
            F[t].Plus(Fdrag);
        }
    }
}
