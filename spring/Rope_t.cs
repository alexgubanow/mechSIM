using System;
using System.Globalization;

namespace spring
{
    public class Rope_t
    {
        public static void EvalLinksLength(Node_t[] Nodes, int NodeID, float _D, float ro)
        {
            Nodes[NodeID].A = (float)Math.PI * (float)Math.Pow(_D, 2) / 4;
            Nodes[NodeID].I = maf.P3(Nodes[NodeID].A) / 12f;
            float L = 0;
            foreach (var link in Nodes[NodeID].ngb)
            {
                L += crds.GetTotL(Nodes[NodeID].tm[0][(int)N.p], Nodes[link].tm[0][(int)N.p]);
            }
            if (L == 0)
            {
                throw new Exception();
            }
            float vu = Nodes[NodeID].A * L / 2;
            Nodes[NodeID].m = ro * vu;
        }

        public static void IterateOverNodes(Node_t[] Nodes, int t, float dt, float[][][] load)
        {
            foreach (var node in Nodes)
            {
                if (node.LoadType == NodeLoad.f)
                {
                    getLoad(node, t, N.f, load[node.NodeID][t]);
                }
                else
                {
                    getForces(Nodes, node.NodeID, t);
                }
                if (node.LoadType != NodeLoad.f && node.LoadType != NodeLoad.none)
                {
                    Integr.VerletUSD(ref node.tm[t], node.tm[t - 1], dt, node.m);
                }
                else
                {
                    IntegrateForce(node, t, dt);
                }
            }
        }

        private static void getLoad(Node_t node, int t, N typ, float[] load)
        {
            switch (typ)
            {
                case N.p:
                    node.tm[t][(int)typ] = load;
                    break;

                case N.u:
                    node.tm[t][(int)typ] = load;
                    break;

                case N.v:
                    break;

                case N.a:
                    break;

                case N.b:
                    break;

                case N.f:
                    node.tm[t][(int)typ] = vectr.Plus(node.tm[t][(int)typ], load);
                    break;

                default:
                    break;
            }
        }

        public static void IntegrateForce(Node_t node, int t, float dt)
        {
            Integr.Verlet(ref node.tm[t], node.tm[t - 1], dt, node.m);
        }

        private static void getForces(Node_t[] Nodes, int NodeID, int t)
        {
            if (Nodes[NodeID].freedom != NodeFreedom.locked)
            {
                foreach (var np in Nodes[NodeID].ngb)
                {
                    //get Fn from link between this point and np
                    float[] gFn = Element.GetFn(Nodes[NodeID].tm[t - 1], Nodes[np].tm[t - 1], Nodes[NodeID].r, Nodes[NodeID].A, Nodes[NodeID].E, Nodes[NodeID].I);
                    //dirty fix of dcm, just turn - to + and vs
                    gFn = vectr.Minus(0, gFn);
                    //push it to this force pull
                    Nodes[NodeID].tm[t][(int)N.f] = vectr.Plus(Nodes[NodeID].tm[t][(int)N.f], gFn);
                }
            }
        }
    }
}