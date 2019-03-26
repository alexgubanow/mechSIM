namespace spring
{
    public class Node_t
    {
        public float[][] k;
        public float[][] x;
        public float[][] v;
        public float[][] a;
        public float[][] coord;
        public float[][] F;
        public float[][] Fext;
        public int[] ngb;
        public float massa;

        public Node_t(float[] coords, int[] neighbours, int length)
        {
            k = new float[length][];
            x = new float[length][];
            v = new float[length][];
            a = new float[length][];
            coord = new float[length][];
            F = new float[length][];
            for (int t = 0; t < length; t++)
            {
                k[t] = coords;
                k[t][(int)c.y] = k[t][(int)c.y] + 1;
                ngb = neighbours;
                x[t] = new float[3];
                v[t] = new float[3];
                a[t] = new float[3];
                coord[t] = coords;
                F[t] = new float[3];
            }
        }

        public void AddForce(int t, float[] Fglob)
        {
            for (int i = 0; i < 3; i++)
            {
                F[t][i] += Fglob[i];
            }
        }
    }
}