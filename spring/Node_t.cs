using System;

namespace spring
{
    public class Node_t
    {
        public Node_t[] Nodes;
        private int[] ngb;
        private float m;
        private float E;
        private float A;
        private float dt;
        private float[][][] tm;
        private float[] r;

        public Node_t(float[] time, float[] coords, ref Node_t[] _Nodes, int[] neighbours, float _E, float _D, float ro)
        {
            r = new float[3] { 0, 0, _D / 2 };
            E = _E;
            Nodes = _Nodes;
            A = (float)Math.PI * (float)Math.Pow(_D, 2) / 4;
            float L = 0;
            float vu = A * L;
            m = ro * vu;

            ngb = neighbours;
            tm = new float[time.Length][][];
            for (int t = 0; t < time.Length; t++)
            {
                tm[t][N.c] = coords;
                tm[t][N.x] = new float[3];
                tm[t][N.v] = new float[3];
                tm[t][N.a] = new float[3];
                tm[t][N.f] = new float[3];
            }
        }

        public void Make(int t)
        {
            IterateOverContacts(t);
            IntegrateForce(t);
        }

        private void IterateOverContacts(int t)
        {
            foreach (var np in ngb)
            {
                //get Fn from link between this point and np
                float[] gFn = Element.GetFn(this.tm[t - 1], Nodes[np].tm[t - 1], this.r, A, E);
                //push it to this force pull
                this.tm[t][N.f][N.x] += gFn[N.x];
            }
            //here need to read ext load
        }

        private void IntegrateForce(int t)
        {
            Integr.EulerExpl(ref tm[t], tm[t - 1], dt, m);
        }
    }
}