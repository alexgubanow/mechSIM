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
                deriv[i].a = new Vector3() { X = 0, Y = maf._g, Z = 0 };
                deriv[i].v = new Vector3() { X = 0, Y = maf._g * 1E-06f, Z = 0 };
            }
            deriv[0].p = coords;
            //deriv[0].a = new Vector3() { X = 0, Y = maf._g, Z = 0 };
            //deriv[0].v = new Vector3() { X = 0, Y = maf._g * 1E-06f, Z = 0 };
            radiusPoint = _radiusPoint;
        }
        public void CalcAccel(int t, float m)
        {
            if (LoadType != NodeLoad.p)
            {
                deriv[t].a.X = F[t].X / m;
                deriv[t].a.Y = F[t].Y / m;
                //deriv[t].a.Z = F[t].Z / m;//has to be different
            }
        }
        public void GetForces(Rope_t rope, int t, float m, float c)
        {
            //gravity force
            F[t] += new Vector3
            {
                Y = m * maf._g
            };
            //damping force
            F[t] += new Vector3
            {
                X = 0 - (c * deriv[t - 1].v.X),
                Y = 0 - (c * deriv[t - 1].v.Y),
                //Z = 0 - (c * deriv[t - 1].v.Z)
            };
            /*getting element forces*/
            foreach (var neigNode in Neigs)
            {
                //push it to this force pull
                F[t] += rope.GetElemRef(ID, neigNode).F[t];
            }
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
            Integr.EulerExpl(LoadType, ref deriv[now], deriv[before], deriv[0], dt);
        }

    }
}