﻿using System;

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
            A = (float)Math.PI * maf.P2(_radiusPoint.z) / 4;
            I = maf.P3(A) / 12.0f;
        }
        public bool IsMyNode(int id) => (n1 == id || n2 == id) ? true : false;
        public void CalcForce(ref Rope_t model, int t)
        {
            GetFn(ref model, t);
            GetPressureForce(ref model, t);
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
            c = DampRatio * 2f * (float)Math.Sqrt(m *((E * A) / L));
            if (c <= 0)
            {
                throw new Exception("Calculated damping ratio of element can't be eaqul to zero");
            }
        }
        private void GetFn(ref Rope_t model, int t)
        {
            xyz_t Fn = new xyz_t();
            //getting length of link by measure between coords
            float oldL = crds.GetTotL(model.GetNodeRef(n1).deriv[t - 1].p, model.GetNodeRef(n2).deriv[t - 1].p);
            if (oldL == 0)
            {
                throw new Exception("Calculated length of element can't be eaqul to zero");
            }
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
            Fn.x = 0 - (E * A / oldL * (lBpUx.x - lNpUx.x));
            //Fn[(int)C.y] = 12f * E * I / maf.P3(oldL2) * oldUy2;
            F[t].Plus(Fn);
        }
        private void GetPressureForce(ref Rope_t model, int t)
        {
            xyz_t Fpress = new xyz_t();
            F[t].Plus(Fpress);
        }
    }
}
