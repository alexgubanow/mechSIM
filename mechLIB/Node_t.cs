using System;

namespace mechLIB
{
    public unsafe class Node_t
    {
        public NodeFreedom freedom;
        public NodeLoad LoadType;
        public deriv_t[] deriv;
        public Element_t[] Elems;
        public int NodeID;
        public int[] ElemItr;

        public Node_t(int tCounts, xyz_t coords, NodeFreedom _freedom, NodeLoad _LoadType, int ID, ref Element_t[] _Elems, int[] _ElemItr)
        {
            NodeID = ID;
            freedom = _freedom;
            LoadType = _LoadType;
            Elems = _Elems;
            ElemItr = _ElemItr;
            deriv = new deriv_t[tCounts];
            deriv[0].p = coords;
        }
        public void GetForces(int t)
        {
            if (freedom != NodeFreedom.locked && (LoadType == NodeLoad.f || LoadType == NodeLoad.none))
            {
                /*getting element forces*/
                foreach (var element in Elems)
                {
                    //get Fn from link between this point and np
                    xyz_t gFn = new xyz_t();
                    element.GetFn(t - 1, ref gFn);
                    //dirty fix of dcm, just turn - to + and vs
                    vectr.Invert(ref gFn);
                    //push it to this force pull
                    vectr.Plus(ref deriv, gFn);
                }
            }
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