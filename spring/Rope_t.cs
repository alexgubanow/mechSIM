using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace spring
{
    public class Rope_t
    {
        private Node_t[] nds;

        private Rope_t()
        {
            nds = new Node_t[4];
            nds[0] = new Node_t(new float[3] { 0, 0, 0 }, new int[1] { 1 });
            nds[nds.Length - 1] = new Node_t(new float[3] { nds.Length - 1, 0, 0 }, new int[1] { nds.Length - 2 });
            for (int i = 1; i < nds.Length - 1; i++)
            {
                nds[i] = new Node_t(new float[3] { i, 0, 0 }, new int[2] { i - 1, i + 1 });
            }
        }

        public void GetState()
        {
            for (int node = 0; node < nds.Length; node++)
            {
                //nds[node].AddForce(EXTERNAL LOAD));
                //getDCM
                foreach (int neighbour in nds[node].ngb)
                {
                    nds[node].AddForce(getNodeAction(node, neighbour));
                }
                //intgrate collected force for all Dx orders
            }
        }

        private float[] getNodeAction(int basePoint, int neighbour)
        {
            float[] Fglob = new float[3] { 0, 0, 0 };
            //convert Ux(basePoint) and Ux(neighbour) to local coordinate of link
            //get Fn of link
            Element.GetFn(nds[basePoint].dxs[(int)Dx.x], nds[neighbour].dxs[(int)Dx.x]);
            //convert Fn to gloabal coordinates
            return Fglob;
        }
    }
}
