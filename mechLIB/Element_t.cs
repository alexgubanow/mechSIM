namespace mechLIB
{
    public unsafe class Element_t
    {
        public float m;
        public float E;
        public float A;
        public float I;
        public Node_t n1;
        public Node_t n2;
        public xyz_t[] F;
        public xyz_t radiusPoint;
        void GetForces(int t)
        {
            xyz_t gFn = new xyz_t();
            GetFn(t, ref gFn);
            //dirty fix of dcm, just turn - to + and vs
            //vectr.Invert(ref gFn);
            F[t] = gFn;
        }
        float CalcMass(float ro)
        {
            float L = crds.GetTotL(n1.deriv[0].p, n2.deriv[0].p);
            if (L == 0)
            {
                throw new System.Exception();
            }
            return ro * A * L / 2;
        }
        public void GetFn(int t, ref xyz_t gFn)
        {
            xyz_t Fn =new xyz_t();
            //getting length of link by measure between coords
            float oldL2 = crds.GetTotL(n1.deriv[t].p, n2.deriv[t].p);
            //getting position of link according base point
            xyz_t LinkPos = new xyz_t();
            vectr.Minus(n1.deriv[t].p, n2.deriv[t].p, ref LinkPos);
            //getting DCM for this link
            dcm_t dcm = new dcm_t();
            crds.GetDCM(ref dcm, LinkPos, radiusPoint);
            //convert base point Ux to local coords
            xyz_t lBpUx = new xyz_t();
            crds.ToLoc(dcm, n1.deriv[t].u, ref lBpUx);
            //convert n point Ux to local coords
            xyz_t lNpUx = new xyz_t();
            crds.ToLoc(dcm, n2.deriv[t].u, ref lNpUx);
            //calc delta of Ux
            float oldUx2 = lBpUx.x - lNpUx.x;
            //float oldUy2 = lBpUx[(int)C.y] - lNpUx[(int)C.y];
            //calc Fn of link
            Fn.x = (E * A / oldL2 * oldUx2) / 2;
            //Fn[(int)C.y] = 12f * E * I / maf.P3(oldL2) * oldUy2;
            //convert Fn to global coords and return
            crds.ToGlob(dcm, Fn, ref gFn);
        }
    }
}
