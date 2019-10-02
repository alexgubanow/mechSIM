namespace mechLIB
{
    public class vectr
    {
        public static void Invert(ref float[] vec)
        {
            for (int i = 0; i < vec.Length; i++)
            {
                vec[i] = 0 - vec[i];
            }
        }

        public static void Minus(ref float[] vec, float arg)
        {
            for (int i = 0; i < vec.Length; i++)
            {
                vec[i] -= arg;
            }
        }

        public static void Minus(ref float[] vec, float[] arg)
        {
            for (int i = 0; i < vec.Length; i++)
            {
                vec[i] -= arg[i];
            }
        }
        public static float[] Minus(float[] vec, float[] arg)
        {
            float[] res = new float[vec.Length];
            for (int i = 0; i < vec.Length; i++)
            {
                res[i] = vec[i] - arg[i];
            }
            return res;
        }

        public static void Plus(ref float[] vec, float arg)
        {
            for (int i = 0; i < vec.Length; i++)
            {
                vec[i] += arg;
            }
        }

        public static void Plus(ref float[] vec, float[] arg)
        {
            for (int i = 0; i < vec.Length; i++)
            {
                vec[i] += arg[i];
                //vec[i] = vec[i]+ arg[i];
            }
        }
        public static float[] Plus(float[] vec, float[] arg)
        {
            float[] res = new float[vec.Length];
            for (int i = 0; i < vec.Length; i++)
            {
                res[i] = vec[i] + arg[i];
            }
            return res;
        }
    }
}