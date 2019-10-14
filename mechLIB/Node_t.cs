﻿using System;

namespace mechLIB
{
    public unsafe class Node_t
    {
        public float m;
        public NodeFreedom freedom;
        public NodeLoad LoadType;
        public deriv_t[] deriv;
        public xyz_t[] F;
        public int[] Neigs;
        public int ID;
        public xyz_t radiusPoint;

        public Node_t(int tCounts, xyz_t coords, xyz_t _radiusPoint, NodeFreedom _freedom, NodeLoad _LoadType, int _ID, int[] _Neigs)
        {
            ID = _ID;
            freedom = _freedom;
            LoadType = _LoadType;
            Neigs = _Neigs;
            deriv = new deriv_t[tCounts];
            F = new xyz_t[tCounts];
            for (int i = 0; i < deriv.Length; i++)
            {
                F[i] = new xyz_t();
                deriv[i] = new deriv_t
                {
                    p = coords
                };
            }
            radiusPoint = _radiusPoint;
        }
        public void CalcAccel(ref Rope_t model, int t)
        {
            if (LoadType == NodeLoad.none)
            {
                GetForces(ref model, t, ref F[t]);
            }
            deriv[t].a.x = F[t].x / m;
            deriv[t].a.y = F[t].y / m;
            deriv[t].a.z = F[t].z / m;//has to be different
        }
        public void GetForces(ref Rope_t model, int t, ref xyz_t nodeForce)
        {
            /*getting element forces*/
            foreach (var neigNode in Neigs)
            {
                //getting position of link according base point
                xyz_t LinkPos = new xyz_t();
                xyz_t.Minus(deriv[t].p, model.GetNodeRef(neigNode).deriv[t].p, ref LinkPos);
                //getting DCM for this link
                dcm_t dcm = new dcm_t();
                crds.GetDCM(ref dcm, LinkPos, radiusPoint);
                //get Fn from link between this point and np
                xyz_t lFn = model.GetElemRef(ID, neigNode).F[t];
                xyz_t gFn = new xyz_t();
                //convert Fn to global coords and return
                crds.ToGlob(dcm, lFn, ref gFn);
                //push it to this force pull
                nodeForce.Plus(gFn);
                //dirty fix of dcm, just turn - to + and vs
                gFn.Invert();
            }
        }
        public void CalcMass(ref Rope_t model)
        {
            foreach (var neigNode in Neigs)
            {
                m += model.GetElemRef(ID, neigNode).m / 2;
            }
        }
        public void Integrate(int now, int before, float dt)
        {
            switch (LoadType)
            {
                case NodeLoad.u:
                    Integr.EulerExpl(Integr.Direction.Backward, ref deriv[now], deriv[before], dt);
                    break;
                case NodeLoad.none:
                    Integr.EulerExpl(Integr.Direction.Forward, ref deriv[now], deriv[before], dt);
                    break;
                case NodeLoad.f:
                    Integr.EulerExpl(Integr.Direction.Forward, ref deriv[now], deriv[before], dt);
                    break;
                default:
                    throw new Exception();
            }
        }

    }
}