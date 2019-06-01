﻿namespace spring
{
    public class Element
    {
        public static void GetFn(ref float[][] Bp, ref float[][] Np, float A, float E)
        {
            float[] Fn = new float[3] { 0, 0, 0 };
            //getting length of link by measure between coords
            float oldL2 = Bp[N.c][N.x] - Np[N.c][N.x];
            //getting DCM for this link
            float[] dcm = coords.GetDCM(Bp[N.c], Np[N.c]);
            //convert base point Ux to local coords
            float[] lBpUx = coords.ToLoc(dcm, Bp[N.u]);
            //convert n point Ux to local coords
            float[] lNpUx = coords.ToLoc(dcm, Np[N.u]);
            //calc delta of Ux
            float oldUx2 = lBpUx[N.x] - lNpUx[N.x];
            //calc Fn of link
            Fn[N.x] = E * A / oldL2 * oldUx2;
            //convert Fn to global coords
            float[] Gfn = coords.ToLoc(dcm, Fn);
            //push Fn of link to node force
            Bp[N.f][N.x] += Gfn[N.x];
        }
    }
}
