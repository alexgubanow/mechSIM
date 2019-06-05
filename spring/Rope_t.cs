﻿using System;

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
            Nodes[0].tm[0][(int)N.p] = new float[] { 0, float.Parse("12.5e-4"), 0 };
            Nodes[1].tm[0][(int)N.p] = new float[] { float.Parse("24.6202e-4"), float.Parse("8.1588e-4"), 0 };
            Nodes[2].tm[0][(int)N.p] = new float[] { float.Parse("49.2404e-4"), float.Parse("3.8176e-4"), 0 };
            Nodes[3].tm[0][(int)N.p] = new float[] { float.Parse("74.2404e-4"), float.Parse("3.8176e-4"), 0 };
            Nodes[4].tm[0][(int)N.p] = new float[] { float.Parse("99.2404e-4"), float.Parse("3.8176e-4"), 0 };
            Nodes[5].tm[0][(int)N.p] = new float[] { float.Parse("124.2404e-4"), float.Parse("3.8176e-4"), 0 };
            Nodes[6].tm[0][(int)N.p] = new float[] { float.Parse("149.2404e-4"), float.Parse("3.8176e-4"), 0 };
            Nodes[7].tm[0][(int)N.p] = new float[] { float.Parse("174.2404e-4"), float.Parse("3.8176e-4"), 0 };
            Nodes[8].tm[0][(int)N.p] = new float[] { float.Parse("198.8606e-4"), float.Parse("8.1588e-4"), 0 };
            Nodes[9].tm[0][(int)N.p] = new float[] { float.Parse("223.4808e-4"), float.Parse("12.5e-4"), 0 };
            Nodes[Nodes.Length / 2].LoadType = NodeLoad.y;
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
                IntegrateForce(node, t);
            }
        }
        public void IntegrateForce(Node_t node, int t)
        {
            Integr.Verlet(ref node.tm[t], node.tm[t - 1], dt, node.m);
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

        private void IterateOverContacts(Node_t node, int t)
        {
            if (node.LoadType != NodeLoad.none)
            {
                getLoading(node, t);
            }
            else
            {
                getForces(node, t);
            }
        }

        private void getLoading(Node_t node, int t)
        {
            //here need to read ext load
            node.tm[t][(int)N.f] = vectr.Plus(node.tm[t][(int)N.f], load[node.NodeID][t]);
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