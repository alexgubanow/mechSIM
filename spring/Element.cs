namespace spring
{
    public class Element
    {
        public static float[] GetFn(float[][] Bp, float[][] Np, float[] radiusPoint, float A, float E)
        {
            float[] Fn = new float[3] { 0, 0, 0 };
            //getting length of link by measure between coords
            float oldL2 = crds.GetTotL(Bp[(int)N.p], Np[(int)N.p]);
            //getting position of link according base point
            float[] LinkPos = vectr.Minus(Np[(int)N.p], Bp[(int)N.p]);
            //getting DCM for this link
            float[] dcm = crds.GetDCM(LinkPos, radiusPoint);
            //convert base point Ux to local coords
            float[] lBpUx = crds.ToLoc(dcm, Bp[(int)N.u]);
            //convert n point Ux to local coords
            float[] lNpUx = crds.ToLoc(dcm, Np[(int)N.u]);
            //calc delta of Ux
            float oldUx2 = lBpUx[(int)C.x] - lNpUx[(int)C.x];
            float oldUy2 = lBpUx[(int)C.y] - lNpUx[(int)C.y];
            //calc Fn of link
            Fn[(int)C.x] = E * A / oldL2 * oldUx2;
            Fn[(int)C.y] = E * A / oldL2 * oldUy2;
            //convert Fn to global coords and return
            return crds.ToLoc(dcm, Fn);
        }
    }
}
