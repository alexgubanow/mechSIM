using System;

namespace spring
{
    public class N
    {
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

    public class C
    {
        //X coordinate
        public const int x = 0;
        //Y coordinate
        public const int y = 1;
        //Z coordinate
        public const int z = 2;
    }
    public class crds
    {
        private enum Cosine { Xx, Yx, Zx, Xy, Yy, Zy, Xz, Yz, Zz, }

        public static float GetTotL(float[] zeroP, float[] targetP)
        {
            return (float)Math.Sqrt(
                Math.Pow(targetP[C.x] - zeroP[C.x], 2) +
                Math.Pow(targetP[C.y] - zeroP[C.y], 2) +
                Math.Pow(targetP[C.z] - zeroP[C.z], 2));
        }

        public static float[] GetDCM(float[] endPoint, float[] radiusPoint)
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
            float lax = GetTotL(new float[3] { 0, 0, 0 }, endPoint);
            //lby = 13.42;
            float lby = GetTotL(new float[3] { 0, 0, 0 }, radiusPoint);

            //lax = sqrt((ax - baseX) ^ 2 + (ay - baseY) ^ 2 + (az - baseZ) ^ 2)
            //lay = 0;
            //laz = 0;
            //lbx = 0;
            //lby = sqrt((bx - baseX) ^ 2 + (by - baseY) ^ 2 + (bz - baseZ) ^ 2)
            //lbz = 0;
            float[] dcm = new float[9];
            //cosXa = ax / lax
            dcm[(int)Cosine.Xx] = endPoint[C.x] / lax;
            //cosYa = ay / lax
            dcm[(int)Cosine.Yx] = endPoint[C.y] / lax;
            //cosZa = az / lax
            dcm[(int)Cosine.Zx] = endPoint[C.z] / lax;

            //cosXb = bx / lby
            dcm[(int)Cosine.Xy] = radiusPoint[C.x] / lby;
            //cosYb = by / lby
            dcm[(int)Cosine.Yy] = radiusPoint[C.y] / lby;
            //cosZb = bz / lby
            dcm[(int)Cosine.Zy] = radiusPoint[C.z] / lby;

            //cosXz = (cosYa * cosZb - cosZa * cosYb)
            dcm[(int)Cosine.Xz] = dcm[(int)Cosine.Yx] * dcm[(int)Cosine.Zy] - dcm[(int)Cosine.Zx] * dcm[(int)Cosine.Yy];
            //cosYz = -(cosXa * cosZb - cosZa * cosXb)
            dcm[(int)Cosine.Yz] =0 -(dcm[(int)Cosine.Xx] * dcm[(int)Cosine.Zy] - dcm[(int)Cosine.Zx] * dcm[(int)Cosine.Xy]);
            //cosZz = (cosXa * cosYb - cosYa * cosXb)
            dcm[(int)Cosine.Zz] = dcm[(int)Cosine.Xx] * dcm[(int)Cosine.Yy] - dcm[(int)Cosine.Yx] * dcm[(int)Cosine.Xy];
            return dcm;
        }

        public static float[] ToGlob(float[] dcm, float[] Lp)
        {
            float[] gA = new float[3];
            gA[C.x] = dcm[(int)Cosine.Xx] * Lp[C.x] + dcm[(int)Cosine.Xy] * Lp[C.y] + dcm[(int)Cosine.Xz] * Lp[C.z];
            gA[C.y] = dcm[(int)Cosine.Yx] * Lp[C.x] + dcm[(int)Cosine.Yy] * Lp[C.y] + dcm[(int)Cosine.Yz] * Lp[C.z];
            gA[C.z] = dcm[(int)Cosine.Zx] * Lp[C.x] + dcm[(int)Cosine.Zy] * Lp[C.y] + dcm[(int)Cosine.Zz] * Lp[C.z];
            return gA;
        }

        public static float[] ToLoc(float[] dcm, float[] Gp)
        {
            float[] lA = new float[3];
            lA[C.x] = dcm[(int)Cosine.Xx] * Gp[C.x] + dcm[(int)Cosine.Yx] * Gp[C.y] + dcm[(int)Cosine.Zx] * Gp[C.z];
            lA[C.y] = dcm[(int)Cosine.Xy] * Gp[C.x] + dcm[(int)Cosine.Yy] * Gp[C.y] + dcm[(int)Cosine.Zy] * Gp[C.z];
            lA[C.z] = dcm[(int)Cosine.Xz] * Gp[C.x] + dcm[(int)Cosine.Yz] * Gp[C.y] + dcm[(int)Cosine.Zz] * Gp[C.z];
            return lA;
        }
    }
}