using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace spring
{
    public class Element
    {

        public static float[] GetFn(float[] UxBasePoint, float[] UxNeighbour, float l, float A, float E)
        {
            float[] Fn = new float[3] { 0, 0, 0 };
            //calc Fn of link
            Fn[(int)c.x] = ((E * A) / l) * (UxNeighbour[(int)c.x] - UxBasePoint[(int)c.x]);
            //ADD LOAD ON ELEMENT
            return Fn;
        }
    }
}
