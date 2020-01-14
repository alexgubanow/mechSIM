using System;
using System.Numerics;
using System.Threading.Tasks;

namespace mechLIB
{
    public class Rope_t
    {
        public Node_t[] Nodes;
        public Element_t[] Elements;
        public float[] L;
        public Rope_t(props_t Props)
        {
            L = new float[Props.Counts];
            L[0] = Props.L;
            Nodes = new Node_t[Props.nodes];
            Elements = new Element_t[Props.nodes - 1];
            SetupNodesPositions(Props);
            EvalElements(Props);
        }
        public Rope_t(props_t Props, Vector3 startCoord, Vector3 endCoord)
        {
            L = new float[Props.Counts];
            L[0] = Props.L;
            Nodes = new Node_t[Props.nodes];
            Elements = new Element_t[Props.nodes - 1];
            SetupNodesPositions(Props, startCoord, endCoord);
            EvalElements(Props);
        }
        private void SetupNodesPositions(props_t props)
        {
            float dl = props.L / Nodes.Length;
            int lastNode = Nodes.Length - 1;
            //xyz_t tmpCoord = new xyz_t { y = props.initDrop * maf.P2((0 * dl) - (props.L - dl) / 2) + 1E-3f };
            //Vector3 tmpRadPoint = new Vector3 { Z = props.D };
            Nodes[0] = new Node_t(props.Counts,
                new Vector3 { Y = props.initDrop * maf.P2((0 * dl) - (props.L - dl) / 2) + 1E-3f },
                new Vector3 { Y = props.initDrop * maf.P2((0 * dl) - (props.L - dl) / 2) + 1E-3f , Z = props.D },
                NodeFreedom.xyz, NodeLoad.none, 0, new int[1] { 1 });
            for (int i = 1; i < lastNode; i++)
            {
                Nodes[i] = new Node_t(props.Counts,
                new Vector3 { X = i * dl, Y = props.initDrop * maf.P2((i * dl) - (props.L - dl) / 2) + 1E-3f },
                new Vector3 { X = i * dl, Y = props.initDrop * maf.P2((i * dl) - (props.L - dl) / 2) + 1E-3f, Z = props.D },
                NodeFreedom.xyz, NodeLoad.none, i, new int[2] { i - 1, i + 1 });
            }
            Nodes[lastNode] = new Node_t(props.Counts,
                new Vector3 { X = lastNode * dl, Y = props.initDrop * maf.P2((lastNode * dl) - (props.L - dl) / 2) + 1E-3f },
                new Vector3 { X = lastNode * dl, Y = props.initDrop * maf.P2((lastNode * dl) - (props.L - dl) / 2) + 1E-3f, Z = props.D },
                NodeFreedom.xyz, NodeLoad.none, lastNode, new int[1] { lastNode - 1 });
        }
        private void SetupNodesPositions(props_t props, Vector3 startCoord, Vector3 endCoord)
        {
            int lastNode = Nodes.Length - 1;
            float dl = props.L / lastNode;
            xyz_t tmpRadPoint = new xyz_t { z = props.D };
            float cosA = endCoord.X / props.L;
            float sinA = endCoord.Y / props.L;
            //xyz_t startCoordL = new xyz_t();
            //startCoordL.Plus(new xyz_t() { y = props.initDrop * maf.P2((0 * dl) - (props.L - dl) / 2) + 1E-3f }, startCoord);
            Nodes[0] = new Node_t(props.Counts, startCoord, new Vector3() { X = startCoord.X, Y = startCoord.Y, Z = props.D },
                NodeFreedom.xyz, NodeLoad.none, 0, new int[1] { 1 });
            for (int i = 1; i < lastNode; i++)
            {
                //xyz_t flatC = new xyz_t { x = i * dl, y = props.initDrop * maf.P2((i * dl) - (props.L - dl) / 2) + 1E-3f };
                Vector3 flatC = new Vector3 { X = i * dl };
                Vector3 coords = new Vector3
                {
                    /*X = (x — x0) * cos(alpha) — (y — y0) * sin(alpha) + x0;
                    Y = (x — x0) * sin(alpha) + (y — y0) * cos(alpha) + y0;*/
                    X = (flatC.X - startCoord.X) * cosA - (flatC.Y - startCoord.Y) * sinA + startCoord.X,
                    Y = (flatC.X - startCoord.X) * sinA + (flatC.Y - startCoord.Y) * cosA + startCoord.Y
                };
                //dcm_.ToGlob(flatC, ref coords);
                Nodes[i] = new Node_t(props.Counts, coords, new Vector3() { X = coords.X, Y = coords.Y, Z = props.D },
                    NodeFreedom.xyz, NodeLoad.none, i, new int[2] { i - 1, i + 1 });
            }
            Nodes[lastNode] = new Node_t(props.Counts, endCoord, new Vector3() { X = endCoord.X, Y = endCoord.Y, Z = props.D },
                NodeFreedom.xyz, NodeLoad.none, lastNode, new int[1] { lastNode - 1 });
        }
        private void EvalElements(props_t props)
        {
            for (int i = 0; i < Elements.Length; i++)
            {
                Elements[i] = new Element_t(i, i + 1, new Vector3 { Z = props.D }, props.Counts, i, props);
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
        public void StepOverElems(int t, float Re, float bloodV, float bloodP)
        {
            Parallel.ForEach(Elements, (elem, loopState) =>
            {
                elem.CalcForce(this, t, Re, bloodV, bloodP);
            });
        }
        public void StepOverNodes(int t, float Re, float dt)
        {
            Parallel.ForEach(Nodes, (node, loopState) =>
            {
                float m = 0;
                float c = 0;
                node.GetPhysicParam(this, t - 1, Re, ref m, ref c);
                node.GetForces(this, t, m, c);
                node.CalcAccel(t, m);
                /*integrate*/
                node.Integrate(t, t - 1, dt);
            });
        }

    }
}