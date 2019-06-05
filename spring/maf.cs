using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace spring
{
    public class maf
    {
        public const float hlf = 1f / 2f;
        public const float sxt = 1f / 6f;

        public static float P2(float value)
        {
            return (float)Math.Pow(value, 2);
        }

        public static float P3(float value)
        {
            return (float)Math.Pow(value, 3);
        }

        public static float P4(float value)
        {
            return (float)Math.Pow(value, 4);
        }
    }
}
