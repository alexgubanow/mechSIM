namespace spring
{
    public class Node_t
    {
        public float[] k;
        public float[][] dxs;
        public int[] ngb;

        public Node_t(float[] coords, int[] neighbours)
        {
            k = coords;
            k[(int)c.y] = k[(int)c.y] + 1;
            ngb = neighbours;
            for (int i = 0; i < 5; i++)
            {
                dxs[i] = new float[3];
            }
            dxs[(int)Dx.coord] = coords;
        }

        public void AddForce(float[] Fglob)
        {
            for (int i = 0; i < 3; i++)
            {
                dxs[(int)Dx.F][i] += Fglob[i];
            }
        }
    }
}
