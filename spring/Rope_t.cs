namespace spring
{
    public class Rope_t
    {
        public Node_t[] Nodes;
        public Rope_t(float[] time, int nCount, float L, float E, float D, float ro)
        {
            float dl = L / nCount;
            float pos = 0;
            Nodes = new Node_t[nCount];
            Nodes[0] = new Node_t(time, new float[3] { pos, 0, 0 }, ref Nodes, new int[1] { 1 }, E, D, ro);
            for (int i = 1; i < Nodes.Length - 1; i++)
            {
                Nodes[i] = new Node_t(time, new float[3] { pos, 0, 0 }, ref Nodes, new int[2] { i - 1, i + 1 }, E, D, ro);
                pos += dl;
            }
            Nodes[Nodes.Length - 1] = new Node_t(time, new float[3] { pos, 0, 0 }, ref Nodes, new int[1] { Nodes.Length - 2 }, E, D, ro);
        }
        
        public void IterateOverNodes(int t)
        {
            foreach (var node in Nodes)
            {
                node.Make(t);
            }
        }
    }
}