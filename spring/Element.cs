namespace spring
{
    public class Element
    {
        public static void GetFn(ref float[][] Bp, ref float[][] Np, float A, float E)
        {
            float[] Fn = new float[3] { 0, 0, 0 };
            //getting length of link by measure between coords
            float oldL2 = Bp[(int)D.c][(int)C.x] - Np[(int)D.c][(int)C.x];
            //getting DCM for this link
            float[] dcm = coords.GetDCM(Bp[(int)D.c], Np[(int)D.c]);
            //convert base point Ux to local coords
            float[] lBpUx = coords.ToLoc(dcm, Bp[(int)D.u]);
            //convert n point Ux to local coords
            float[] lNpUx = coords.ToLoc(dcm, Np[(int)D.u]);
            //calc delta of Ux
            float oldUx2 = lBpUx[(int)C.x] - lNpUx[(int)C.x];
            //calc Fn of link
            Fn[(int)C.x] = E * A / oldL2 * oldUx2;
            //convert Fn to global coords
            float[] Gfn = coords.ToLoc(dcm, Fn);
            //push Fn of link to node force
            Bp[(int)D.f][(int)C.x] += Gfn[(int)C.x];
        }
    }
}
