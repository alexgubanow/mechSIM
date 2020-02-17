using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mechLIB
{
    public class maf
    {
        public const float _g = -9.80666f;
        //public const float hlf = 1f / 2f;
        public const float sxt = 1f / 6f;
        public const float pi = (float)Math.PI;

        public static float hlf(float value)
        {
            return 0.5f * value;
        }
        public static float P2(float value)
        {
            return value * value;
        }

        public static float P3(float value)
        {
            return value * value * value;
        }

        public static float P4(float value)
        {
            return value * value * value * value;
        }
        public static float sqrt(float value)
        {
            return (float)Math.Sqrt(value);
        }
        public static float sin(float value)
        {
            return (float)Math.Sin(value);
        }
        public static float cos(float value)
        {
            return (float)Math.Cos(value);
        }
        public static float atan(float value)
        {
            return (float)Math.Atan(value);
        }
        public static void Linip(float[] y, int ratio, ref float[] yd)
        {
            for (int i = 0; i < y.Length -1; i++)
            {
                int start = i * ratio;
                yd[start] = y[i];
                for (int j = 1; j < ratio; j++)
                {
                    yd[start + j] = yd[start + j - 1] + (y[i + 1] - y[i]) / ratio;
                }
            }
        }
    }
}
