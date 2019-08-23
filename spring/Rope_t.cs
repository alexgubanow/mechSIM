using System;
using System.Globalization;

namespace spring
{
    public class Rope_t
    {
        public Node_t[] Nodes;
        private float[][][] load;
        private float dt;

        public Rope_t(float[] time, int nCount, float L, float E, float D, float ro, ref float[][][] _load)
        {
            float dl = L / nCount;
            load = _load;
            dt = time[1];
            float pos = 0;
            Nodes = new Node_t[nCount];
            Nodes[0] = new Node_t(time.Length, new float[3] { pos, 0, 0 }, NodeFreedom.locked, NodeLoad.none, 0, new int[1] { 1 }, E, D);
            pos += dl;
            for (int i = 1; i < Nodes.Length - 1; i++)
            {
                Nodes[i] = new Node_t(time.Length, new float[3] { pos, 0, 0 }, NodeFreedom.xyz, NodeLoad.none, i, new int[2] { i - 1, i + 1 }, E, D);
                pos += dl;
            }
            Nodes[Nodes.Length - 1] = new Node_t(time.Length, new float[3] { pos, 0, 0 }, NodeFreedom.locked, NodeLoad.none, Nodes.Length - 1, new int[1] { Nodes.Length - 2 }, E, D);

            Nodes[0].tm[0][(int)N.p] = new float[] { float.Parse("1e-3", CultureInfo.InvariantCulture), float.Parse("6e-3", CultureInfo.InvariantCulture), 0 };
            Nodes[1].tm[0][(int)N.p] = new float[] { float.Parse("2e-3", CultureInfo.InvariantCulture), float.Parse("4e-3", CultureInfo.InvariantCulture), 0 };
            Nodes[2].tm[0][(int)N.p] = new float[] { float.Parse("4e-3", CultureInfo.InvariantCulture), float.Parse("2e-3", CultureInfo.InvariantCulture), 0 };
            Nodes[3].tm[0][(int)N.p] = new float[] { float.Parse("7e-3", CultureInfo.InvariantCulture), float.Parse("2e-3", CultureInfo.InvariantCulture), 0 };
            Nodes[4].tm[0][(int)N.p] = new float[] { float.Parse("10e-3", CultureInfo.InvariantCulture), float.Parse("2e-3", CultureInfo.InvariantCulture), 0 };
            Nodes[5].tm[0][(int)N.p] = new float[] { float.Parse("12e-3", CultureInfo.InvariantCulture), float.Parse("4e-3", CultureInfo.InvariantCulture), 0 };
            Nodes[6].tm[0][(int)N.p] = new float[] { float.Parse("13e-3", CultureInfo.InvariantCulture), float.Parse("6e-3", CultureInfo.InvariantCulture), 0 };
            Nodes[Nodes.Length / 2].LoadType = NodeLoad.f;
            //Nodes[Nodes.Length - 1].freedom = NodeFreedom.xyz;
            foreach (var node in Nodes)
            {
                EvalLinksLength(node, D, ro);
            }
        }

        public void EvalLinksLength(Node_t node, float _D, float ro)
        {
            node.A = (float)Math.PI * (float)Math.Pow(_D, 2) / 4;
            node.I = maf.P3(node.A) / 12f;
            float L = 0;
            foreach (var link in node.ngb)
            {
                L += crds.GetTotL(Nodes[node.NodeID].tm[0][(int)N.p], Nodes[link].tm[0][(int)N.p]);
            }
            if (L == 0)
            {
                throw new Exception();
            }
            float vu = node.A * L / 2;
            node.m = ro * vu;
        }

        public void IterateOverNodes(int t)
        {
            foreach (var node in Nodes)
            {
                if (node.LoadType == NodeLoad.f)
                {
                    getLoad(node, t, N.f);
                }
                else
                {
                    getForces(node, t);
                }
                if (node.LoadType != NodeLoad.f && node.LoadType != NodeLoad.none)
                {
                    Integr.VerletUSD(ref node.tm[t], node.tm[t - 1], dt, node.m);
                }
                else
                {
                    IntegrateForce(node, t);
                }
            }
        }

        private void getLoad(Node_t node, int t, N typ)
        {
            switch (typ)
            {
                case N.p:
                    node.tm[t][(int)typ] = load[node.NodeID][t];
                    break;

                case N.u:
                    node.tm[t][(int)typ] = load[node.NodeID][t];
                    break;

                case N.v:
                    break;

                case N.a:
                    break;

                case N.b:
                    break;

                case N.f:
                    node.tm[t][(int)typ] = vectr.Plus(node.tm[t][(int)typ], load[node.NodeID][t]);
                    break;

                default:
                    break;
            }
        }

        public void IntegrateForce(Node_t node, int t)
        {
            Integr.Verlet(ref node.tm[t], node.tm[t - 1], dt, node.m);
        }

        private void getForces(Node_t node, int t)
        {
            if (node.freedom != NodeFreedom.locked)
            {
                foreach (var np in node.ngb)
                {
                    //get Fn from link between this point and np
                    float[] gFn = Element.GetFn(node.tm[t - 1], Nodes[np].tm[t - 1], node.r, node.A, node.E, node.I);
                    //dirty fix of dcm, just turn - to + and vs
                    gFn = vectr.Minus(0, gFn);
                    //push it to this force pull
                    node.tm[t][(int)N.f] = vectr.Plus(node.tm[t][(int)N.f], gFn);
                }
            }
        }
    }
}