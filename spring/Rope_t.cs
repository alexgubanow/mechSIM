namespace spring
{
    public class Rope_t
    {
        private float dt;
        private int tLength;
        public Node_t[] nds;

        public Rope_t(float _dt, int _tLength, int nCount)
        {
            dt = _dt;
            tLength = _tLength;
            nds = new Node_t[nCount];
            nds[0] = new Node_t(new float[3] { 0, 0, 0 }, new int[1] { 1 }, tLength);
            nds[nds.Length - 1] = new Node_t(new float[3] { nds.Length - 1, 0, 0 }, new int[1] { nds.Length - 2 }, tLength);
            for (int i = 1; i < nds.Length - 1; i++)
            {
                nds[i] = new Node_t(new float[3] { i, 0, 0 }, new int[2] { i - 1, i + 1 }, tLength);
            }
        }

        public void Sim()
        {
            for (int i = 1; i < tLength; i++)
            {
                GetState(i, i - 1);
            }
        }

        private void GetState(int now, int before)
        {
            //intgrate collected force for all Dx orders
            if (now < 2)
            {
                nds[0].a[now][0] = 1 / 0.1f;
            nds[0].v[now][0] = nds[0].v[before][0] + nds[0].a[now][0] * dt;
            nds[0].x[now][0] = nds[0].x[before][0] + nds[0].v[now][0] * dt;
            nds[0].coord[now][0] = nds[0].coord[before][0] + nds[0].x[now][0];
            }

            for (int node = 1; node < nds.Length - 1; node++)
            {
                //nds[node].AddForce(EXTERNAL LOAD));
                //getDCM
                float[] dcm = coords.getDCM(nds[node].coord[before], nds[node].k[before]);
                foreach (int neighbour in nds[node].ngb)
                {
                    //convert Ux(basePoint) and Ux(neighbour) to local coordinate of link
                    float[] lUxb = coords.toLoc(dcm, nds[node].x[before]);
                    float[] lUxn = coords.toLoc(dcm, nds[neighbour].x[before]);
                    //get length of link
                    float l = coords.getTotL(nds[node].coord[before], nds[neighbour].coord[before]);
                    //get Fn of link, convert Fn to gloabal coordinates
                    float[] FnGlobal = coords.toGlob(dcm, Element.GetFn(lUxb, lUxn, l, 0.1f, 1E9f));
                    //push to node forces
                    nds[node].AddForce(now, FnGlobal);
                }
                //intgrate collected force for all Dx orders
                nds[node].a[now][0] = nds[node].F[now][0] / 0.1f;
                nds[node].v[now][0] = nds[node].v[before][0] + nds[node].a[now][0] * dt;
                nds[node].x[now][0] = nds[node].x[before][0] + nds[node].v[now][0] * dt;
                nds[node].coord[now][0] = nds[node].coord[before][0] + nds[node].x[now][0];
            }
        }
    }
}