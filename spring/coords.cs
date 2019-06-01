using System;

namespace spring
{
    public class N
    {
        //X coordinate
        public const int x = 0;
        //Y coordinate
        public const int y = 1;
        //Z coordinate
        public const int z = 2;

        //coordinates
        public const int c = 0;
        //displacement
        public const int u = 1;
        //velocity
        public const int v = 2;
        //acceleration
        public const int a = 3;
        //jerk
        public const int b = 4;
        //force
        public const int f = 5;
    }
    public class coords
    {
        private enum Cosine { Xx, Yx, Zx, Xy, Yy, Zy, Xz, Yz, Zz, }

        public static float GetTotL(float[] zeroP, float[] targetP)
        {
            return (float)Math.Sqrt(
                Math.Pow(targetP[N.x] - zeroP[N.x], 2) +
                Math.Pow(targetP[N.y] - zeroP[N.y], 2) +
                Math.Pow(targetP[N.z] - zeroP[N.z], 2));
        }

        public static float[] GetDCM(float[] GlobA, float[] GlobB)
        {
            //baseX = 0;
            //baseY = 0;
            //baseZ = 0;
            //ax = 75;
            //ay = 25;
            //az = 0;
            //bx = -4.25;
            //by = 12.73;
            //bz = 0;
            //lax = 79.06;
            float lax = GetTotL(new float[3] { 0, 0, 0 }, GlobA);
            //lby = 13.42;
            float lby = GetTotL(new float[3] { 0, 0, 0 }, GlobB);

            //lax = sqrt((ax - baseX) ^ 2 + (ay - baseY) ^ 2 + (az - baseZ) ^ 2)
            //lay = 0;
            //laz = 0;
            //lbx = 0;
            //lby = sqrt((bx - baseX) ^ 2 + (by - baseY) ^ 2 + (bz - baseZ) ^ 2)
            //lbz = 0;
            float[] dcm = new float[9];
            //cosXa = ax / lax
            dcm[(int)Cosine.Xx] = GlobA[N.x] / lax;
            //cosYa = ay / lax
            dcm[(int)Cosine.Yx] = GlobA[N.y] / lax;
            //cosZa = az / lax
            dcm[(int)Cosine.Zx] = GlobA[N.z] / lax;

            //cosXb = bx / lby
            dcm[(int)Cosine.Xy] = GlobB[N.x] / lby;
            //cosYb = by / lby
            dcm[(int)Cosine.Yy] = GlobB[N.y] / lby;
            //cosZb = bz / lby
            dcm[(int)Cosine.Zy] = GlobB[N.z] / lby;

            //cosXz = (cosYa * cosZb - cosZa * cosYb)
            dcm[(int)Cosine.Xz] = dcm[(int)Cosine.Yx] * dcm[(int)Cosine.Zy] - dcm[(int)Cosine.Zx] * dcm[(int)Cosine.Yy];
            //cosYz = -(cosXa * cosZb - cosZa * cosXb)
            dcm[(int)Cosine.Xz] = -(dcm[(int)Cosine.Xx] * dcm[(int)Cosine.Zy] - dcm[(int)Cosine.Zx] * dcm[(int)Cosine.Xy]);
            //cosZz = (cosXa * cosYb - cosYa * cosXb)
            dcm[(int)Cosine.Xz] = dcm[(int)Cosine.Xx] * dcm[(int)Cosine.Yy] - dcm[(int)Cosine.Yx] * dcm[(int)Cosine.Xy];
            return dcm;
        }

        public static float[] ToGlob(float[] dcm, float[] Lp)
        {
            float[] gA = new float[3];
            gA[N.x] = dcm[(int)Cosine.Xx] * Lp[N.x] + dcm[(int)Cosine.Xy] * Lp[N.y] + dcm[(int)Cosine.Xz] * Lp[N.z];
            gA[N.y] = dcm[(int)Cosine.Yx] * Lp[N.x] + dcm[(int)Cosine.Yy] * Lp[N.y] + dcm[(int)Cosine.Yz] * Lp[N.z];
            gA[N.z] = dcm[(int)Cosine.Zx] * Lp[N.x] + dcm[(int)Cosine.Zy] * Lp[N.y] + dcm[(int)Cosine.Zz] * Lp[N.z];
            return gA;
        }

        public static float[] ToLoc(float[] dcm, float[] Gp)
        {
            float[] lA = new float[3];
            lA[N.x] = dcm[(int)Cosine.Xx] * Gp[N.x] + dcm[(int)Cosine.Yx] * Gp[N.y] + dcm[(int)Cosine.Zx] * Gp[N.z];
            lA[N.y] = dcm[(int)Cosine.Xy] * Gp[N.x] + dcm[(int)Cosine.Yy] * Gp[N.y] + dcm[(int)Cosine.Zy] * Gp[N.z];
            lA[N.z] = dcm[(int)Cosine.Xz] * Gp[N.x] + dcm[(int)Cosine.Yz] * Gp[N.y] + dcm[(int)Cosine.Zz] * Gp[N.z];
            return lA;
        }
    }
}