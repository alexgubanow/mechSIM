using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace spring
{
    public class vectr
    {
        public static float[] Minus(float[] vec, float arg)
        {
            float[] res = new float[vec.Length];
            for (int i = 0; i < vec.Length; i++)
            {
                res[i] = vec[i] - arg;
            }
            return res;
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
    }
}
