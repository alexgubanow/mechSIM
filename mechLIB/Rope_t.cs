using System;

namespace mechLIB
{
    public class Rope_t
    {
        public Node_t[] Nodes;
        public Element_t[] Elements;
        public Rope_t(props_t Props)
        {
            Nodes = new Node_t[Props.nodes];
            Elements = new Element_t[Props.nodes - 1];
            SetupNodesPositions(Props);
            EvalElements(Props);
        }
        private void SetupNodesPositions(props_t props)
        {
            float dl = props.L / Nodes.Length;
            int lastNode = Nodes.Length - 1;
            //xyz_t tmpCoord = new xyz_t { y = props.initDrop * (float)Math.Pow((0 * dl) - (props.L - dl) / 2, 2) + 1E-3f };
            xyz_t tmpRadPoint = new xyz_t { z = props.D };
            Nodes[0] = new Node_t(props.Counts, 
                new xyz_t { y = props.initDrop * (float)Math.Pow((0 * dl) - (props.L - dl) / 2, 2) + 1E-3f },
                tmpRadPoint, NodeFreedom.xyz, NodeLoad.p, 0, new int[1] { 1 });
            for (int i = 1; i < lastNode; i++)
            {
                Nodes[i] = new Node_t(props.Counts,
                new xyz_t { x = i * dl, y = props.initDrop * (float)Math.Pow((i * dl) - (props.L - dl) / 2, 2) + 1E-3f },
                tmpRadPoint, NodeFreedom.xyz, NodeLoad.none, i, new int[2] { i - 1, i + 1 });
            }
            Nodes[lastNode] = new Node_t(props.Counts,
                new xyz_t { x = lastNode * dl, y = props.initDrop * (float)Math.Pow((lastNode * dl) - (props.L - dl) / 2, 2) + 1E-3f },
                tmpRadPoint, NodeFreedom.xyz, NodeLoad.p, lastNode, new int[1] { lastNode - 1 });
        }
        private void EvalElements(props_t props)
        {
            for (int i = 0; i < Elements.Length; i++)
            {
                xyz_t tmpRadPoint = new xyz_t { z = props.D };
                Elements[i] = new Element_t(i, i + 1, tmpRadPoint, props.Counts, i, props);
            }
        }

        public ref Node_t GetNodeRef(int id)
        {
            if (id < 0)
            {
                throw new Exception("Not valid ID");
            }
            return ref Nodes[id];
        }
        public ref Element_t GetElemRef(int baseNode, int neigNode)
        {
            for (int i = 0; i < Elements.Length; i++)
            {
                if (Elements[i].IsMyNode(baseNode) && Elements[i].IsMyNode(neigNode))
                {
                    return ref Elements[i];
                }
            }
            throw new Exception("Element not found");
        }
    }
}