using System;

namespace mechLIB
{
    public unsafe class Node_t
    {
        public float m;
        public NodeFreedom freedom;
        public NodeLoad LoadType;
        public deriv_t[] deriv;
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
            for (int i = 0; i < deriv.Length; i++)
            {
                deriv[i] = new deriv_t
                {
                    p = coords
                };
            }
            radiusPoint = _radiusPoint;
        }
        public void GetForces(ref Rope_t model, int t, ref xyz_t nodeForce)
        {
            if (freedom != NodeFreedom.locked && (LoadType == NodeLoad.f || LoadType == NodeLoad.none))
            {
                /*getting element forces*/
                foreach (var neigNode in Neigs)
                {
                    //getting position of link according base point
                    xyz_t LinkPos = new xyz_t();
                    xyz_t.Minus(deriv[t].p, model.GetNodeRef(neigNode).deriv[t].p, ref LinkPos);
                    //getting DCM for this link
                    dcm_t dcm = new dcm_t();
                    crds.GetDCM(ref dcm, LinkPos, radiusPoint);
                    //get Fn from link between this point and np
                    xyz_t lFn = model.GetElemRef(ID, neigNode).F[t];
                    //dirty fix of dcm, just turn - to + and vs
                    //vectr.Invert(ref gFn);
                    xyz_t gFn = new xyz_t();
                    //convert Fn to global coords and return
                    crds.ToGlob(dcm, lFn, ref gFn);
                    //push it to this force pull
                    nodeForce.Plus(gFn);
                }
            }
        }
        public void CalcMass(ref Rope_t model)
        {
            foreach (var neigNode in Neigs)
            {
                m += model.GetElemRef(ID, neigNode).m / 2;
            }
        }
        public void CalcAccel(int now, xyz_t Force)
        {
            deriv[now].a.x = Force.x / m;
            deriv[now].a.y = Force.y / m;
            deriv[now].a.z = Force.z / m;//has to be different
        }
        public void Integrate(int now, int before, float dt)
        {
            switch (LoadType)
            {
                case NodeLoad.u:
                    Integr.EulerExpl(Integr.Direction.Backward, ref deriv[now], deriv[before], dt);
                    break;
                case NodeLoad.none:
                    Integr.EulerExpl(Integr.Direction.Forward, ref deriv[now], deriv[before], dt);
                    break;
                default:
                    throw new Exception();
            }
        }

    }
}