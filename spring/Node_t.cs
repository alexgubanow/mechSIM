using System;

namespace spring
{
    public class Node_t
    {
        private Node_t[] Nodes;
        private NodeFreedom freedom;
        private float[][] load;
        private int[] ngb;
        private float m;
        private float E;
        private float A;
        private float dt;
        public float[][][] tm;
        private float[] r;
        public int NodeID;

        public Node_t(float[] time, float[] coords, ref Node_t[] _Nodes, NodeFreedom _freedom, int ID, int[] neighbours, float _E, float _D, float ro, ref float[][] _load)
        {
            NodeID = ID;
            load = _load;
            freedom = _freedom;
            dt = time[1];
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
                tm[t] = new float[6][];
                tm[t][N.c] = coords;
                tm[t][N.u] = new float[3];
                tm[t][N.v] = new float[3];
                tm[t][N.a] = new float[3];
                tm[t][N.b] = new float[3];
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
            switch (freedom)
            {
                case NodeFreedom.x:
                    break;
                case NodeFreedom.y:
                    break;
                case NodeFreedom.z:
                    break;
                case NodeFreedom.xy:
                    break;
                case NodeFreedom.xz:
                    break;
                case NodeFreedom.yz:
                    break;
                case NodeFreedom.xyz:
                    foreach (var np in ngb)
                    {
                        //get Fn from link between this point and np
                        float[] gFn = Element.GetFn(this.tm[t - 1], Nodes[np].tm[t - 1], this.r, A, E);
                        //push it to this force pull
                        this.tm[t][N.f][C.x] += gFn[C.x];
                    }
                    //here need to read ext load
                    this.tm[t][N.f][C.x] += load[NodeID][t];
                    break;
                case NodeFreedom.none:
                    break;
                default:
                    break;
            }
        }

        private void IntegrateForce(int t)
        {
            Integr.EulerExpl(ref tm[t], tm[t - 1], dt, m);
        }
    }
}