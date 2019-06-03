using System;

namespace spring
{
    public class Rope_t
    {
        public Node_t[] Nodes;
        private float[][] load;
        public Rope_t(float[] time, int nCount, float L, float E, float D, float ro, ref float[][] _load)
        {
            float dl = L / nCount;
            load = _load;
            float pos = 0;
            Nodes = new Node_t[nCount];
            Nodes[0] = new Node_t(time, new float[3] { pos, 0, 0 }, NodeFreedom.xyz, 0, new int[1] { 1 }, E, D);
            pos += dl;
            for (int i = 1; i < Nodes.Length - 1; i++)
            {
                Nodes[i] = new Node_t(time, new float[3] { pos, 0, 0 }, NodeFreedom.xyz, i, new int[2] { i - 1, i + 1 }, E, D);
                pos += dl;
            }
            Nodes[Nodes.Length - 1] = new Node_t(time, new float[3] { pos, 0, 0 }, NodeFreedom.none, Nodes.Length - 1, new int[1] { Nodes.Length - 2 }, E, D);
            foreach (var node in Nodes)
            {
                EvalLinksLength(node, D, ro);
            }
        }
        
        public void IterateOverNodes(int t)
        {
            foreach (var node in Nodes)
            {
                IterateOverContacts(node, t);
                node.IntegrateForce(t);
            }
        }
        public void EvalLinksLength(Node_t node, float _D, float ro)
        {
            node.A = (float)Math.PI * (float)Math.Pow(_D, 2) / 4;
            float L = 0;
            foreach (var link in node.ngb)
            {
                L += crds.GetTotL(Nodes[node.NodeID].tm[0][N.c], Nodes[link].tm[0][N.c]);
            }
            float vu = node.A * L / 2;
            node.m = ro * vu;
        }

        private void IterateOverContacts(Node_t node, int t)
        {
            switch (node.freedom)
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
                    foreach (var np in node.ngb)
                    {
                        //get Fn from link between this point and np
                        float[] gFn = Element.GetFn(node.tm[t - 1], Nodes[np].tm[t - 1], node.r, node.A, node.E);
                        //push it to this force pull
                        node.tm[t][N.f][C.x] += 0 - gFn[C.x];
                    }
                    //here need to read ext load
                    node.tm[t][N.f][C.x] += load[node.NodeID][t];
                    float dscd = node.tm[t][N.f][C.x];
                    break;
                case NodeFreedom.none:
                    break;
                default:
                    break;
            }
        }
    }
}