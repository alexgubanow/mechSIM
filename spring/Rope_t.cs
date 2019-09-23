using System;

namespace spring
{
    public class Rope_t
    {
        public Node_t[] Nodes;
        public float[][][] load;
        public Rope_t(int nodeCount, float[][][] _load)
        {
            Nodes = new Node_t[nodeCount];
            load = _load;
        }
        public void SetupNodesPositions(int Counts, float initDrop, float L, float E, float D)
        {
            float dl = L / Nodes.Length;
            Nodes[0] = new Node_t(Counts, new float[3] { 0, initDrop * (float)Math.Pow((0 * dl) - (L - dl) / 2, 2) + 1E-3f, 0 }, NodeFreedom.xyz, NodeLoad.f, 0, new int[1] { 1 }, E, D);
            for (int i = 1; i < Nodes.Length - 1; i++)
            {
                Nodes[i] = new Node_t(Counts, new float[3] { i * dl, initDrop * (float)Math.Pow((i * dl) - (L - dl) / 2, 2) + 1E-3f, 0 }, NodeFreedom.xyz, NodeLoad.none, i, new int[2] { i - 1, i + 1 }, E, D);
            }
            Nodes[Nodes.Length - 1] = new Node_t(Counts, new float[3] { (Nodes.Length - 1) * dl, initDrop * (float)Math.Pow(((Nodes.Length - 1) * dl) - (L - dl) / 2, 2) + 1E-3f, 0 }, NodeFreedom.xyz, NodeLoad.f, Nodes.Length - 1, new int[1] { Nodes.Length - 2 }, E, D);
        }
        public void EvalLinksLength(float _D, float ro)
        {
            foreach (var node in Nodes)
            {
                node.A = (float)Math.PI * (float)Math.Pow(_D, 2) / 4;
                node.I = maf.P3(node.A) / 12f;
                float L = 0;
                foreach (var link in node.ngb)
                {
                    L += crds.GetTotL(node.tm[0][(int)N.p], Nodes[link].tm[0][(int)N.p]);
                }
                if (L == 0)
                {
                    throw new Exception();
                }
                float vu = node.A * L / 2;
                node.m = ro * vu;
            }
        }

        public void IterateOverNodes(int t, float dt)
        {
            foreach (var node in Nodes)
            {
                /*get loading*/
                vectr.Plus(ref node.tm[t][(int)node.LoadType], load[node.NodeID][t]);
                /*calc force*/
                node.GetForces(Nodes, t);
                /*integrate*/
                node.Integrate(t, t - 1, dt);
            }
        }
    }
}