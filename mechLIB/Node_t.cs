using System;

namespace mechLIB
{
    public unsafe class Node_t
    {
        public float m;
        public NodeFreedom freedom;
        public NodeLoad LoadType;
        public deriv_t[] deriv;
        public xyz_t[] F;
        public int[] Neigs;
        public int ID;
        public xyz_t radiusPoint;

        public Node_t(int tCounts, xyz_t coords, xyz_t _radiusPoint, NodeFreedom _freedom, NodeLoad _LoadType, int _ID, int[] _Neigs)
        {
            ID = _ID;
            freedom = _freedom;
            LoadType = _LoadType;
            Neigs = _Neigs;
            deriv = new deriv_t[tCounts];
            F = new xyz_t[tCounts];
            for (int i = 0; i < deriv.Length; i++)
            {
                F[i] = new xyz_t();
                deriv[i] = new deriv_t();
                //deriv[i] = new deriv_t
                //{
                //    p = coords
                //};
            }
            deriv[0].p = coords;
            radiusPoint = _radiusPoint;
        }
        public void CalcAccel(ref Rope_t model, int t)
        {
            if (LoadType == NodeLoad.none || LoadType == NodeLoad.f)
            {
                GetForces(ref model, t);
                //F[t].x = 0.1f * (deriv[t - 1].u.x);
                deriv[t].a.x = F[t].x / m;
                deriv[t].a.y = F[t].y / m;
                deriv[t].a.z = F[t].z / m;//has to be different
            }
        }
        public void GetForces(ref Rope_t model, int t)
        {
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
        public void CalcMass(ref Rope_t model)
        {
            foreach (var neigNode in Neigs)
            {
                m += model.GetElemRef(ID, neigNode).m / 2;
            }
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