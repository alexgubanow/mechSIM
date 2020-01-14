using System;
using System.Numerics;

namespace mechLIB
{
    public unsafe class Node_t
    {
        public NodeFreedom freedom;
        public NodeLoad LoadType;
        public deriv_t[] deriv;
        //public deriv_t[] derivAn;
        public Vector3[] F;
        public int[] Neigs;
        public int ID;
        public Vector3 radiusPoint;
        //float w = 300;
        //float n = 1f;

        public Node_t(int tCounts, Vector3 coords, Vector3 _radiusPoint, NodeFreedom _freedom, NodeLoad _LoadType, int _ID, int[] _Neigs)
        {
            ID = _ID;
            freedom = _freedom;
            LoadType = _LoadType;
            Neigs = _Neigs;
            F = new Vector3[tCounts];
            deriv = new deriv_t[tCounts];
            for (int i = 0; i < deriv.Length; i++)
            {
                F[i] = new Vector3();
                deriv[i] = new deriv_t();
            }
            deriv[0].p = coords;
            deriv[0].a = new Vector3() { X = 0, Y = maf._g, Z = 0 };
            deriv[0].v = new Vector3() { X = 0, Y = maf._g * 5E-06f, Z = 0 };
            radiusPoint = _radiusPoint;
        }
        public void CalcAccel(int t, float m)
        {
            if (LoadType == NodeLoad.none || LoadType == NodeLoad.f)
            {
                deriv[t].a.X = F[t].X / m;
                deriv[t].a.Y = F[t].Y / m;
                //deriv[t].a.Z = F[t].Z / m;//has to be different
            }

        }
        public void GetForces(Rope_t rope, int t, float m, float c)
        {
            //xyz_t Fg = new xyz_t();
            //Fg.y = m * maf._g;
            //F[t].Plus(Fg);

            Vector3 Fd = new Vector3
            {
                X = 0 - (c * deriv[t - 1].v.X),
                Y = 0 - (c * deriv[t - 1].v.Y),
                //Z = 0 - (c * deriv[t - 1].v.Z)
            };
            F[t] += Fd;
            /*getting element forces*/
            foreach (var neigNode in Neigs)
            {
                //getting position of link according base point
                Vector3 LinkPos = deriv[t - 1].p - rope.GetNodeRef(neigNode).deriv[t - 1].p;
                //LinkPos.Minus(deriv[t - 1].p, rope.GetNodeRef(neigNode).deriv[t - 1].p);
                //getting DCM for this link
                dcm_t dcm = new dcm_t(LinkPos, radiusPoint);
                Vector3 gFn = new Vector3();
                //convert Fn to global coords and return
                dcm.ToGlob(rope.GetElemRef(ID, neigNode).F[t], ref gFn);
                //push it to this force pull
                F[t] += gFn;
            }
            F[t].Y += -1E-07f;
        }
        public void GetPhysicParam(Rope_t rope, int t, float Re, ref float m, ref float c)
        {
            foreach (var neigNode in Neigs)
            {
                float mElem = 0;
                float cElem = 0;
                rope.GetElemRef(ID, neigNode).GetPhysicParam(rope, t, Re, ref mElem, ref cElem);
                c += cElem;
                m += mElem;
            }
            //m = 1E-04f;
            c /= Neigs.Length;
            if (m <= 0)
            {
                throw new Exception("Calculated mass of node can't be eaqul to zero");
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