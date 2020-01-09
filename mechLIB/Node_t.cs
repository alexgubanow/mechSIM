using System;

namespace mechLIB
{
    public unsafe class Node_t
    {
        public NodeFreedom freedom;
        public NodeLoad LoadType;
        public deriv_t[] deriv;
        //public deriv_t[] derivAn;
        public xyz_t[] F;
        public int[] Neigs;
        public int ID;
        public xyz_t radiusPoint;
        //float w = 300;
        //float n = 1f;

        public Node_t(int tCounts, xyz_t coords, xyz_t _radiusPoint, NodeFreedom _freedom, NodeLoad _LoadType, int _ID, int[] _Neigs)
        {
            ID = _ID;
            freedom = _freedom;
            LoadType = _LoadType;
            Neigs = _Neigs;
            F = new xyz_t[tCounts];
            deriv = new deriv_t[tCounts];
            for (int i = 0; i < deriv.Length; i++)
            {
                F[i] = new xyz_t();
                deriv[i] = new deriv_t();
            }
            deriv[0].p = coords;
            deriv[0].a = new xyz_t() { x = 0, y = maf._g, z = 0 };
            deriv[0].v = new xyz_t() { x = 0, y = maf._g * 5E-06f, z = 0 };
            radiusPoint = _radiusPoint;
        }
        public void CalcAccel(int t, float m)
        {
            if (LoadType == NodeLoad.none || LoadType == NodeLoad.f)
            {
                deriv[t].a.x = F[t].x / m;
                deriv[t].a.y = F[t].y / m;
                deriv[t].a.z = F[t].z / m;//has to be different
            }

        }
        public void GetForces(ref Rope_t model, int t, float m, float c)
        {
            //xyz_t Fg = new xyz_t();
            //Fg.y = m * maf._g;
            //F[t].Plus(Fg);

            xyz_t Fd = new xyz_t();
            Fd.x = 0 - (c * deriv[t - 1].v.x);
            Fd.y = 0 - (c * deriv[t - 1].v.y);
            Fd.z = 0 - (c * deriv[t - 1].v.z);
            F[t].Plus(Fd);
            /*getting element forces*/
            foreach (var neigNode in Neigs)
            {
                //getting position of link according base point
                xyz_t LinkPos = new xyz_t();
                LinkPos.Minus(deriv[t - 1].p, model.GetNodeRef(neigNode).deriv[t - 1].p);
                //getting DCM for this link
                dcm_t dcm = new dcm_t(LinkPos, radiusPoint);
                xyz_t gFn = new xyz_t();
                //convert Fn to global coords and return
                dcm.ToGlob(model.GetElemRef(ID, neigNode).F[t], ref gFn);
                //push it to this force pull
                F[t].Plus(gFn);
            }
        }
        public void GetPhysicParam(ref Rope_t rope, int t, float Re, ref float m, ref float c)
        {
            foreach (var neigNode in Neigs)
            {
                float mElem = 0;
                float cElem = 0;
                rope.GetElemRef(ID, neigNode).GetPhysicParam(ref rope, t, Re, ref mElem, ref cElem);
                c += cElem;
                m += mElem;
            }
            //m = 1E-04f;
            c /= Neigs.Length;
            if (m <= 0)
            {
                throw new Exception("Calculated mass of node can't be eaqul to zero");
            }
        }
        public void Integrate(int now, int before, float dt)
        {
            switch (LoadType)
            {
                case NodeLoad.p:
                    Integr.EulerExpl(Integr.Direction.Backward, ref deriv[now], deriv[before], dt);
                    break;
                case NodeLoad.none:
                    Integr.Verlet(Integr.Direction.Forward, ref deriv[now], deriv[before], dt);
                    break;
                case NodeLoad.f:
                    Integr.Verlet(Integr.Direction.Forward, ref deriv[now], deriv[before], dt);
                    break;
                default:
                    throw new Exception();
            }
        }

    }
}