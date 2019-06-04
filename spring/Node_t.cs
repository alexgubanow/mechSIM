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
        private float dt;
        public float[][][] tm;
        public float[] r;
        public int NodeID;

        public Node_t(float[] time, float[] coords, NodeFreedom _freedom, NodeLoad _LoadType, int ID, int[] neighbours, float _E, float _D)
        {
            NodeID = ID;
            freedom = _freedom;
            LoadType = _LoadType;
            dt = time[1];
            r = new float[3] { 0, 0, _D / 2 };
            E = _E;
            ngb = neighbours;
            tm = new float[time.Length][][];
            for (int t = 0; t < time.Length; t++)
            {
                tm[t] = new float[Enum.GetNames(typeof(N)).Length][];
                for (int n = 0; n < tm[t].Length; n++)
                {
                    tm[t][n] = new float[3];
                }
            }
            tm[0][(int)N.p] = coords;
        }


        public void IntegrateForce(int t)
        {
            Integr.EulerExpl(ref tm[t], tm[t - 1], dt, m);
        }
    }
}