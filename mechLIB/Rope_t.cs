using System;

namespace mechLIB
{
    public class Rope_t
    {
        public Node_t[] Nodes;
        public Element_t[] Elements;
        public float[][][] load;
        public Rope_t(props_t Props, float[][][] _load)
        {
            Nodes = new Node_t[Props.nodes];
            Elements = new Element_t[Props.nodes - 1];
            load = _load;
            SetupNodesPositions(Props);
            EvalElements(Props);
        }
        private void SetupNodesPositions(props_t props)
        {
            float dl = props.L / Nodes.Length;
            int lastNode = Nodes.Length - 1;
            xyz_t tmpCoord = new xyz_t();
            tmpCoord.y = props.initDrop * (float)Math.Pow((0 * dl) - (props.L - dl) / 2, 2) + 1E-3f;
            Nodes[0] = new Node_t(props.Counts, tmpCoord, NodeFreedom.xyz, NodeLoad.f, 0, ref Elements, new int[1] { 0 });
            for (int i = 1; i < lastNode; i++)
            {
                tmpCoord.x = i * dl;
                tmpCoord.y = props.initDrop * (float)Math.Pow((i * dl) - (props.L - dl) / 2, 2) + 1E-3f;
                Nodes[i] = new Node_t(props.Counts, tmpCoord, NodeFreedom.xyz, NodeLoad.none, i, ref Elements, new int[2] { i-1, i+1 });
            }
            tmpCoord.x = lastNode * dl;
            tmpCoord.y = props.initDrop * (float)Math.Pow((lastNode * dl) - (props.L - dl) / 2, 2) + 1E-3f;
            Nodes[lastNode] = new Node_t(props.Counts, tmpCoord, NodeFreedom.xyz, NodeLoad.none, lastNode, ref Elements, new int[1] { lastNode });
        }
        private void EvalElements(props_t props)
        {
            for (int i = 0; i < Elements.Length; i++)
            {
                xyz_t tmpCoord = new xyz_t();
                tmpCoord.z = props.D;
                Elements[i]= new Element_t(ref Nodes[i], ref Nodes[i + 1], tmpCoord, props.E, props.Counts);
            }
        }

        public void IterateOverNodes(int t, float dt)
        {
            foreach (var node in Nodes)
            {
                /*get loading*/
                vectr.Plus(ref node.tm[t][(int)node.LoadType], load[node.NodeID][t]);
                /*calc force*/
                node.GetForces(t);
                /*integrate*/
                node.Integrate(t, t - 1, dt);
                //}
            }
    }
}