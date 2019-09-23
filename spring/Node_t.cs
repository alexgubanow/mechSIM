using System;

namespace spring
{
    public class Node_t
    {
        public NodeFreedom freedom;
        public NodeLoad LoadType;
        public int[] ngb;
        public float m;
        public float E;
        public float A;
        public float I;
        public float[][][] tm;
        public float[] r;
        public int NodeID;

        public Node_t(int tCounts, float[] coords, NodeFreedom _freedom, NodeLoad _LoadType, int ID, int[] neighbours, float _E, float _D)
        {
            NodeID = ID;
            freedom = _freedom;
            LoadType = _LoadType;
            r = new float[3] { 0, 0, _D / 2 };
            E = _E;
            ngb = neighbours;
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
        public void GetForces(Node_t[] Nodes, int t)
        {
            if (freedom != NodeFreedom.locked && (LoadType == NodeLoad.f|| LoadType == NodeLoad.none))
            {
                /*getting element forces*/
                foreach (var np in ngb)
                {
                    //get Fn from link between this point and np
                    float[] gFn = Element.GetFn(tm[t - 1], Nodes[np].tm[t - 1], r, A, E, I);
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

        //private void GetLoad(int t, float[] load)
        //{
        //    switch (LoadType)
        //    {
        //        case N.p:
        //            tm[t][(int)LoadType] = load;
        //            break;

        //        case N.u:
        //            tm[t][(int)LoadType] = load;
        //            break;

        //        case N.v:
        //            break;

        //        case N.a:
        //            break;

        //        case N.b:
        //            break;

        //        case N.f:
        //            tm[t][(int)LoadType] = vectr.Plus(tm[t][(int)LoadType], load);
        //            break;

        //        default:
        //            break;
        //    }
        //}
    }
}