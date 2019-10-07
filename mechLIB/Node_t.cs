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
        public int NodeID;
        public xyz_t radiusPoint;

        public Node_t(int tCounts, xyz_t coords, xyz_t _radiusPoint, NodeFreedom _freedom, NodeLoad _LoadType, int ID, int[] _Neigs)
        {
            NodeID = ID;
            freedom = _freedom;
            LoadType = _LoadType;
            Neigs = _Neigs;
            deriv = new deriv_t[tCounts];
            deriv[0].p = coords;
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
                    xyz_t.Minus(model.GetNodeRef(NodeID).deriv[t].p, model.GetNodeRef(neigNode).deriv[t].p, ref LinkPos);
                    //getting DCM for this link
                    dcm_t dcm = new dcm_t();
                    crds.GetDCM(ref dcm, LinkPos, radiusPoint);
                    //get Fn from link between this point and np
                    xyz_t lFn = model.GetElemRef(NodeID, neigNode).F[t];
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
        public void CalcMass(ref Rope_t model, float ro)
        {
            foreach (var neigNode in Neigs)
            {
                m += model.GetElemRef(NodeID, neigNode).CalcMass(ref model, ro);
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
            //if (LoadType == NodeLoad.f || LoadType == NodeLoad.none)
            //{
            //    Integr.Integrate(Integr.Types.Verlet, ref tm[now], tm[before], dt, m);
            //}
            //else if(LoadType == NodeLoad.p )
            //{
            //    Integr.Integrate(Integr.Types.UVAF_P, ref tm[now], tm[before], dt, m);
            //}
        }

    }
}