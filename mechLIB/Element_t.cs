using System;
using System.Numerics;

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
        public float[] L;
        public Vector3[] F;
        public Vector3 radiusPoint;
        private readonly PhModels phMod;
        public Element_t(int _n1, int _n2, Vector3 _radiusPoint, int Counts, int _ID, props_t props)
        {
            L = new float[props.Counts];
            phMod = props.phMod;
            ro = props.ro;
            DampRatio = props.DampRatio;
            ID = _ID;
            F = new Vector3[Counts];
            for (int i = 0; i < F.Length; i++)
            {
                F[i] = new Vector3();
            }
            E = props.E;
            n1 = _n1;
            n2 = _n2;
            radiusPoint = _radiusPoint;
            A = (float)Math.PI * maf.P2(_radiusPoint.Z);
            I = maf.P3(A) / 12.0f;
        }
        public bool IsMyNode(int id) => (n1 == id || n2 == id) ? true : false;
        public void CalcForce(Rope_t rope, int t, float Re, float bloodV, float bloodP)
        {
            //getting length of link by measure between coords
            L[t] = 0;
            switch (phMod)
            {
                case PhModels.hook:
                    L[t] = GetOwnLength(rope, 0);
                    break;
                case PhModels.hookGeomNon:
                    L[t] = GetOwnLength(rope, t - 1);
                    break;
                case PhModels.mooneyRiv:
                    L[t] = GetOwnLength(rope, t - 1);
                    break;
                default:
                    throw new Exception("unexpected behavior");
            }
            //getting position of link according base point
            Vector3 LinkPos = rope.GetNodeRef(n1).deriv[t - 1].p - rope.GetNodeRef(n2).deriv[t - 1].p;
            //LinkPos.Minus(rope.GetNodeRef(n1).deriv[t - 1].p, rope.GetNodeRef(n2).deriv[t - 1].p);
            //getting DCM for this link
            dcm_t dcm = new dcm_t(LinkPos, radiusPoint);
            //convert base point Ux to local coords
            Vector3 lBpUx = new Vector3();
            dcm.ToLoc(rope.GetNodeRef(n1).deriv[t - 1].u, ref lBpUx);
            //convert n point Ux to local coords
            Vector3 lNpUx = new Vector3();
            dcm.ToLoc(rope.GetNodeRef(n2).deriv[t - 1].u, ref lNpUx);
            //store delta of expansion
            Vector3 deltaL = lBpUx - lNpUx;
            //deltaL.Minus(lBpUx, lNpUx);
            Vector3 force = new Vector3();
            GetFn(t, L[t], deltaL, ref force);
            //GetPressureForce(t, bloodP, L[t]);
            //GetDragForce(t, Re, bloodV, L);

            //force.Y += -1E-07f;
            Vector3 gforce = new Vector3();
            dcm.ToGlob(force, ref gforce);
            F[t] += gforce;
        }
        public void GetPhysicParam(Rope_t rope, int t, float Re, ref float m, ref float c)
        {
            float L = GetOwnLength(rope, t);
            m += ro * A * L;
            if (Re > 0)
            {
                //calc h of fluid on rod
                float thFluid = (radiusPoint.Z * 2) / maf.sqrt(Re);
                //calc mass of fluid on rod
                m += (float)Math.PI * L * (maf.P2(radiusPoint.Z + thFluid) - maf.P2(radiusPoint.Z)) * 1060;
                //add mass of this fluid to mass of rod
            }

            c = DampRatio * 2f * (float)Math.Sqrt(m * ((E * A) / L));
            if (c <= 0 && DampRatio != 0)
            {
                throw new Exception("Calculated damping ratio of element can't be eaqul to zero");
            }

        }

        private float GetOwnLength(Rope_t rope, int t)
        {
            float L = Vector3.Distance(rope.GetNodeRef(n1).deriv[t].p, rope.GetNodeRef(n2).deriv[t].p);
            if (L == 0)
            {
                throw new Exception("Calculated length of element can't be eaqul to zero");
            }

            return L;
        }

        private void GetFn(int t, float L, Vector3 deltaL, ref Vector3 force)
        {
            switch (phMod)
            {
                case PhModels.hook:
                    calcHookFn(ref force, L, deltaL);
                    break;
                case PhModels.hookGeomNon:
                    calcHookFn(ref force, L, deltaL);
                    break;
                case PhModels.mooneyRiv:
                    calcMooneyRivlinFn(ref force, L, deltaL);
                    break;
                default:
                    throw new Exception("unexpected behavior");
            }
        }
        private void calcHookFn(ref Vector3 Fn, float oldL, Vector3 deltaL)
        {
            Fn.X = 0 - (E * A / oldL * deltaL.X);
            //Fn[(int)C.y] = 12f * E * I / maf.P3(oldL2) * oldUy2;
        }
        private void calcMooneyRivlinFn(ref Vector3 Fn, float oldL, Vector3 deltaL)
        {
            xyz_t lamda = new xyz_t
            {
                x = (deltaL.X / oldL) + 1
            };
            const float C10 = 2295.69613f;
            const float C01 = -2351.28728f;
            float sigma = 2 * C10 * (lamda.x - (1 / maf.P2(lamda.x))) + 2 * C01 * (1 - (1 / maf.P3(lamda.x)));
            Fn.X = A * sigma;
        }
        private void GetPressureForce(int t, float bloodP, float L)
        {
            //float Fpress = bloodP * radiusPoint.z * 2 * L;
            //F[t].Y += -1E-07f;
            //F[t].Z += -1E-07f;
        }
        private void GetDragForce(int t, float Re, float v, float L, ref Vector3 force)
        {
            float Awet = 2 * (float)Math.PI * radiusPoint.Z * L;
            float bloodViscosity = 3E-3f;
            float Be = 0.9f;
            float Cd = (Awet / A) * (Be / Re);
            float Fdrag = maf.hlf(1060 * maf.P2(v) * Cd * A);
            //is it has to be applied only on moving direction??
            force.Y += Fdrag;
            force.Z += Fdrag;
        }
    }
}
