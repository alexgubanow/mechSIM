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


    }
}