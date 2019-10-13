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
            GetFn(ref model, t, ref F[t]);
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
        public void GetFn(ref Rope_t model, int before, ref xyz_t Fn)
        {
            //getting length of link by measure between coords
            float oldL = crds.GetTotL(model.GetNodeRef(n1).deriv[before].p, model.GetNodeRef(n2).deriv[before].p);
            if (oldL == 0)
            {
                throw new Exception("Calculated length of element can't be eaqul to zero");
            }
            //getting position of link according base point
            xyz_t LinkPos = new xyz_t();
            LinkPos.Minus(model.GetNodeRef(n1).deriv[before].p, model.GetNodeRef(n2).deriv[before].p);
            //getting DCM for this link
            dcm_t dcm = new dcm_t(LinkPos, radiusPoint);
            //convert base point Ux to local coords
            xyz_t lBpUx = new xyz_t();
            dcm.ToLoc(model.GetNodeRef(n1).deriv[before].u, ref lBpUx);
            //convert n point Ux to local coords
            xyz_t lNpUx = new xyz_t();
            dcm.ToLoc(model.GetNodeRef(n2).deriv[before].u, ref lNpUx);
            //calc Fn of link
            Fn.x = E * A / oldL * (lBpUx.x - lNpUx.x);
            //Fn[(int)C.y] = 12f * E * I / maf.P3(oldL2) * oldUy2;
        }
    }
}
