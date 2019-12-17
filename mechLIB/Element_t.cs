using System;

namespace mechLIB
{
    public class Element_t
    {
        //private float k0;
        private readonly float DampRatio;
        //private float c;
        //private float m;
        private readonly float ro;
        private readonly float E;
        private readonly float A;
        private readonly float I;
        public int n1;
        public int n2;
        public int ID;
        public xyz_t[] F;
        public xyz_t radiusPoint;
        public Element_t(int _n1, int _n2, xyz_t _radiusPoint, int Counts, int _ID, props_t props)
        {
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
        public void CalcForce(ref Rope_t model, int t, float Re, float bloodV, float bloodP)
        {
            //getting length of link by measure between coords
            float L = crds.GetTotL(model.GetNodeRef(n1).deriv[t - 1].p, model.GetNodeRef(n2).deriv[t - 1].p);
            if (L == 0)
            {
                throw new Exception("Calculated length of element can't be eaqul to zero");
            }
            GetFn(ref model, t, L);
            GetPressureForce(ref model, t, bloodP, L);
            GetDragForce(ref model, t, Re, bloodV, L);
        }
        public void GetPhysicParam(ref Rope_t model, int t, float Re, ref float m, ref float c)
        {
            float L = crds.GetTotL(model.GetNodeRef(n1).deriv[t].p, model.GetNodeRef(n2).deriv[t].p);
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
        private void GetFn(ref Rope_t model, int t, float L)
        {
            xyz_t Fn = new xyz_t();

            //getting position of link according base point
            xyz_t LinkPos = new xyz_t();
            LinkPos.Minus(model.GetNodeRef(n1).deriv[t - 1].p, model.GetNodeRef(n2).deriv[t - 1].p);
            //getting DCM for this link
            dcm_t dcm = new dcm_t(LinkPos, radiusPoint);
            //convert base point Ux to local coords
            xyz_t lBpUx = new xyz_t();
            dcm.ToLoc(model.GetNodeRef(n1).deriv[t - 1].u, ref lBpUx);
            //convert n point Ux to local coords
            xyz_t lNpUx = new xyz_t();
            dcm.ToLoc(model.GetNodeRef(n2).deriv[t - 1].u, ref lNpUx);
            //calc Fn of link
            Fn.x = 0 - (E * A / L * (lBpUx.x - lNpUx.x));
            //Fn[(int)C.y] = 12f * E * I / maf.P3(oldL2) * oldUy2;
            F[t].Plus(Fn);
        }
        private void GetFnMR(ref Rope_t model, int t, float L)
        {
            xyz_t lamda = new xyz_t();
            lamda.x = (currxi[i] - lastxi[i]) / L + 1;



            for (int i = 0; i < currxi->Length; i++)
            {
                lamda1[i] = (currxi[i] - lastxi[i]) / l + 1;
                double sigma = 2 * C10 * (lamda1[i] - (1 / (lamda1[i] * lamda1[i]))) + 2 * C01 * (1 - (1 / (lamda1[i] * lamda1[i] * lamda1[i])));
                F[i] = F[i] + A * sigma;
            }
            return 0;
        };
        private void GetPressureForce(ref Rope_t model, int t, float bloodP, float L)
        {
            float Fpress = 0 - bloodP * A * 2 * L;
            F[t].Plus(Fpress);
        }
        private void GetDragForce(ref Rope_t model, int t, float Re, float v, float L)
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
