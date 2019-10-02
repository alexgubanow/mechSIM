using System;

namespace mechLIB
{
    public unsafe class Node_t
    {
        public NodeFreedom freedom;
        public NodeLoad LoadType;
        public deriv_t[] deriv;
        public int[] Elems;
        public float[][][] tm;
        public int NodeID;

        public Node_t(int tCounts, float[] coords, NodeFreedom _freedom, NodeLoad _LoadType, int ID, int[] _Elems)
        {
            NodeID = ID;
            freedom = _freedom;
            LoadType = _LoadType;
            Elems = _Elems;
            tm = new float[tCounts][][];
            for (int t = 0; t < tCounts; t++)
            {
                tm[t] = new float[Enum.GetNames(typeof(N)).Length][];
                for (int n = 0; n < tm[t].Length; n++)
                {
                    tm[t][n] = new float[3];
                }
            }
            tm[0][(int)N.p] = coords;
        }
        public void GetForces(Element_t[] Elems, int t)
        {
            if (freedom != NodeFreedom.locked && (LoadType == NodeLoad.f|| LoadType == NodeLoad.none))
            {
                /*getting element forces*/
                foreach (var element in Elems)
                {
                    //get Fn from link between this point and np
                    float[] gFn = element.GetFn(tm[t - 1], Nodes[np].tm[t - 1]);
                    //dirty fix of dcm, just turn - to + and vs
                    vectr.Invert(ref gFn);
                    //push it to this force pull
                    vectr.Plus(ref tm[t][(int)N.f], gFn);
                }
            }
        }
        public void Integrate(int now, int before, float dt)
        {
            if (LoadType == NodeLoad.f || LoadType == NodeLoad.none)
            {
                Integr.Integrate(Integr.Types.Verlet, ref tm[now], tm[before], dt, m);
            }
            else if(LoadType == NodeLoad.p )
            {
                Integr.Integrate(Integr.Types.UVAF_P, ref tm[now], tm[before], dt, m);
            }
        }

    }
}