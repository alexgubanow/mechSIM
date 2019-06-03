namespace spring
{
    public class Element
    {
        public static float[] GetFn(float[][] Bp, float[][] Np, float[] radiusPoint, float A, float E)
        {
            float[] Fn = new float[3] { 0, 0, 0 };
            //getting length of link by measure between coords
            float oldL2 = crds.GetTotL(Bp[N.c], Np[N.c]);
            //getting position of link according base point
            float[] LinkPos = vectr.Minus(Np[N.c], Bp[N.c]);
            //getting DCM for this link
            float[] dcm = crds.GetDCM(LinkPos, radiusPoint);
            //convert base point Ux to local coords
            float[] lBpUx = crds.ToLoc(dcm, Bp[N.u]);
            //convert n point Ux to local coords
            float[] lNpUx = crds.ToLoc(dcm, Np[N.u]);
            //calc delta of Ux
            float oldUx2 = lBpUx[C.x] - lNpUx[C.x];
            //calc Fn of link
            Fn[C.x] = E * A / oldL2 * oldUx2;
            //convert Fn to global coords and return
            return crds.ToLoc(dcm, Fn);
        }
    }
}
