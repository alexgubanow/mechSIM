using System;

namespace mechLIB
{
    public class Element_t
    {
        public float m;
        public float E;
        public float A;
        public float I;
        public int n1;
        public int n2;
        public int ID;
        public xyz_t[] F;
        public xyz_t radiusPoint;
        public Element_t(int _n1, int _n2, xyz_t _radiusPoint, float _E, int Counts, int _ID)
        {
            ID = _ID;
            F = new xyz_t[Counts];
            for (int i = 0; i < F.Length; i++)
            {
                F[i] = new xyz_t();
            }
            E = _E;
            n1 = _n1;
            n2 = _n2;
            radiusPoint = _radiusPoint;
            A = (float)Math.PI * (float)Math.Pow(_radiusPoint.z, 2) / 4;
            I = maf.P3(A) / 12.0f;
        }
        public bool IsMyNode(int id) => (n1 == id || n2 == id) ? true : false;
        public void CalcForce(ref Rope_t model, int t)
        {
            //xyz_t Fn = new xyz_t();
            GetFn(ref model, t, ref F[t]);
            //dirty fix of dcm, just turn - to + and vs
            //vectr.Invert(ref gFn);
            //F[t] = Fn;
        }
        public void CalcMass(ref Rope_t model, float ro)
        {
            float L = crds.GetTotL(model.GetNodeRef(n1).deriv[0].p, model.GetNodeRef(n2).deriv[0].p);
            if (L == 0)
            {
                throw new Exception("Calculated length of element can't be eaqul to zero");
            }
            m = ro * A * L;
        }
        public void GetFn(ref Rope_t model, int t, ref xyz_t Fn)
        {
            //getting length of link by measure between coords
            float oldL2 = crds.GetTotL(model.GetNodeRef(n1).deriv[t].p, model.GetNodeRef(n2).deriv[t].p);
            //getting position of link according base point
            xyz_t LinkPos = new xyz_t();
            xyz_t.Minus(model.GetNodeRef(n1).deriv[t].p, model.GetNodeRef(n2).deriv[t].p, ref LinkPos);
            //getting DCM for this link
            dcm_t dcm = new dcm_t();
            crds.GetDCM(ref dcm, LinkPos, radiusPoint);
            //convert base point Ux to local coords
            xyz_t lBpUx = new xyz_t();
            crds.ToLoc(dcm, model.GetNodeRef(n1).deriv[t].u, ref lBpUx);
            //convert n point Ux to local coords
            xyz_t lNpUx = new xyz_t();
            crds.ToLoc(dcm, model.GetNodeRef(n2).deriv[t].u, ref lNpUx);
            //calc delta of Ux
            float oldUx2 = lBpUx.x - lNpUx.x;
            //float oldUy2 = lBpUx[(int)C.y] - lNpUx[(int)C.y];
            //calc Fn of link
            Fn.x = (E * A / oldL2 * oldUx2) / 2;
            //Fn[(int)C.y] = 12f * E * I / maf.P3(oldL2) * oldUy2;
        }
    }
}
