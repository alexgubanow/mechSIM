using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace spring
{
    public class Element
    {

        public static float[] GetFn(float[] UxBp, float[] UxNp, float[] CoordBp, float[] CoordNp, float A, float E)
        {
            float[] Fn = new float[3] { 0, 0, 0 };
            /* oldL2 = cd(node, t - 1) - cd(node - 1, t - 1);
             * oldUx2 = ux(node, t - 1) - ux(node - 1, t - 1);
             * Fe2 = ((E * A) / (oldL2)) * (oldUx2);*/
            //calc Fn of link
            float oldL2 = CoordBp[(int)C.x] - CoordNp[(int)C.x];
            float oldUx2 = UxBp[(int)C.x] - UxNp[(int)C.x];
            Fn[(int)C.x] = E * A / oldL2 * oldUx2;
            //ADD LOAD ON ELEMENT
            return Fn;
        }
    }
}
