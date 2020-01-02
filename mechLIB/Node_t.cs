using System;

namespace mechLIB
{
    public unsafe class Node_t
    {
        public float k0;
        public float DampRatio;
        //public float c;
        public float m;
        public NodeFreedom freedom;
        public NodeLoad LoadType;
        public deriv_t[] deriv;
        public deriv_t[] derivAn;
        public xyz_t[] F;
        public int[] Neigs;
        public int ID;
        public xyz_t radiusPoint;
        float w = 300;
        float n = 1f;

        public Node_t(int tCounts, xyz_t coords, xyz_t _radiusPoint, NodeFreedom _freedom, NodeLoad _LoadType, int _ID, int[] _Neigs)
        {
            ID = _ID;
            freedom = _freedom;
            LoadType = _LoadType;
            Neigs = _Neigs;
            F = new xyz_t[tCounts];
            deriv = new deriv_t[tCounts];
            derivAn = new deriv_t[tCounts];
            for (int i = 0; i < deriv.Length; i++)
            {
                F[i] = new xyz_t();
                deriv[i] = new deriv_t();
                derivAn[i] = new deriv_t();
                //deriv[i] = new deriv_t
                //{
                //    p = coords
                //};
            }
            deriv[0].p = coords;
            derivAn[0].p = coords;
            radiusPoint = _radiusPoint;
        }
        public void CalcAccel(ref Rope_t model, int t, float maxLoad, float time)
        {
            if (LoadType == NodeLoad.none || LoadType == NodeLoad.f)
            {
                deriv[t].a.x = F[t].x / m;
                deriv[t].a.y = F[t].y / m;
                deriv[t].a.z = F[t].z / m;//has to be different
            }
            if (LoadType == NodeLoad.f)
            {
                float w0 = maf.sqrt(k0 / m);
                float Zm = maf.sqrt(maf.P2(2 * w0 * DampRatio) + (1 / maf.P2(w)) * maf.P2(maf.P2(w0) - maf.P2(w)));
                float phi = maf.atan((2 * w * w0 * DampRatio) / (maf.P2(w) - maf.P2(w0))) + (n * maf.pi);
                //https://www.wolframalpha.com/input/?i=d%5E2%2Fdt%5E2+%28F0%2F%28w*k%29%29*sin%28wt%2Bphi%29
                derivAn[t].a.x = (maxLoad / (m * Zm)) * (0 - maf.sin(w * time + phi)) * w;
                //https://www.wolframalpha.com/input/?i=d%2Fdt+%28F0%2F%28w*k%29%29*sin%28wt%2Bphi%29
                derivAn[t].v.x = (maxLoad / (m * Zm)) * maf.cos(w * time + phi);
                derivAn[t].u.x = (maxLoad / (m * Zm * w)) * maf.sin(w * time + phi);
            }

        }
        public void GetForces(ref Rope_t model, int t, float maxLoad, float time)
        {
            if (LoadType == NodeLoad.f)
            {
                F[t].Plus(new xyz_t
                {
                    x = (1 / m) * maxLoad * maf.sin(w * time)
                });
            }
            else
            {
                xyz_t Fd = new xyz_t();
                Fd.x = 0 - (2 * DampRatio * maf.sqrt(k0 / m) * deriv[t - 1].v.x);
                Fd.y = 0 - (2 * DampRatio * maf.sqrt(k0 / m) * deriv[t - 1].v.y);
                Fd.z = 0 - (2 * DampRatio * maf.sqrt(k0 / m) * deriv[t - 1].v.z);
                F[t].Plus(Fd);
                /*getting element forces*/
                foreach (var neigNode in Neigs)
                {
                    //getting position of link according base point
                    xyz_t LinkPos = new xyz_t();
                    LinkPos.Minus(deriv[t - 1].p, model.GetNodeRef(neigNode).deriv[t - 1].p);
                    //getting DCM for this link
                    dcm_t dcm = new dcm_t(LinkPos, radiusPoint);
                    xyz_t gFn = new xyz_t();
                    //convert Fn to global coords and return
                    dcm.ToGlob(model.GetElemRef(ID, neigNode).F[t], ref gFn);
                    //push it to this force pull
                    F[t].Plus(gFn);
                }
            }
        }
        public void CalcMass(ref Rope_t model, float maxLoad)
        {
            foreach (var neigNode in Neigs)
            {
                m += model.GetElemRef(ID, neigNode).m / 2;
                //c += model.GetElemRef(ID, neigNode).c;
                k0 += model.GetElemRef(ID, neigNode).k0;
                DampRatio += model.GetElemRef(ID, neigNode).DampRatio;
            }
            //c /= Neigs.Length;
            //if (c <= 0)
            //{
            //    throw new Exception("Calculated damping ratio of node can't be eaqul to zero");
            //}
            DampRatio /= Neigs.Length;
            k0 /= Neigs.Length;
            if (m <= 0)
            {
                throw new Exception("Calculated mass of node can't be eaqul to zero");
            }
            m = 1;
            float w0 = maf.sqrt(k0 / m);
            float Zm = maf.sqrt(maf.P2(2 * w0 * DampRatio) + (1 / maf.P2(w)) * maf.P2(maf.P2(w0) - maf.P2(w)));
            float phi = maf.atan((2 * w * w0 * DampRatio) / (maf.P2(w) - maf.P2(w0))) + (n * maf.pi);

            if (LoadType == NodeLoad.f)
            {
                float v0 = (maxLoad / (m * Zm)) * maf.cos(phi);
                deriv[0].v.x = (maxLoad / (m * Zm)) * maf.cos(phi);
            }
        }
        public void Integrate(int now, int before, float dt)
        {
            switch (LoadType)
            {
                case NodeLoad.p:
                    Integr.EulerExpl(Integr.Direction.Backward, ref deriv[now], deriv[before], dt);
                    break;
                case NodeLoad.none:
                    Integr.Verlet(Integr.Direction.Forward, ref deriv[now], deriv[before], dt);
                    break;
                case NodeLoad.f:
                    Integr.Verlet(Integr.Direction.Forward, ref deriv[now], deriv[before], dt);
                    break;
                default:
                    throw new Exception();
            }
        }

    }
}