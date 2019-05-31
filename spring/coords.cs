using System;

namespace spring
{
    public enum C : int
    {
        x, y, z
    }
    public class coords
    {
        private enum Cosine { Xx, Yx, Zx, Xy, Yy, Zy, Xz, Yz, Zz, }

        public static float GetTotL(float[] zeroP, float[] targetP)
        {
            return (float)Math.Sqrt(
                Math.Pow(targetP[(int)C.x] - zeroP[(int)C.x], 2) +
                Math.Pow(targetP[(int)C.y] - zeroP[(int)C.y], 2) +
                Math.Pow(targetP[(int)C.z] - zeroP[(int)C.z], 2));
        }

        public static float[] GetDCM(float[] a, float[] b)
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
            float lax = GetTotL(new float[3] { 0, 0, 0 }, a);
            //lby = 13.42;
            float lby = GetTotL(new float[3] { 0, 0, 0 }, b);

            //lax = sqrt((ax - baseX) ^ 2 + (ay - baseY) ^ 2 + (az - baseZ) ^ 2)
            //lay = 0;
            //laz = 0;
            //lbx = 0;
            //lby = sqrt((bx - baseX) ^ 2 + (by - baseY) ^ 2 + (bz - baseZ) ^ 2)
            //lbz = 0;
            float[] dcm = new float[9];
            //cosXa = ax / lax
            dcm[(int)Cosine.Xx] = a[(int)C.x] / lax;
            //cosYa = ay / lax
            dcm[(int)Cosine.Yx] = a[(int)C.y] / lax;
            //cosZa = az / lax
            dcm[(int)Cosine.Zx] = a[(int)C.z] / lax;

            //cosXb = bx / lby
            dcm[(int)Cosine.Xy] = b[(int)C.x] / lby;
            //cosYb = by / lby
            dcm[(int)Cosine.Yy] = b[(int)C.y] / lby;
            //cosZb = bz / lby
            dcm[(int)Cosine.Zy] = b[(int)C.z] / lby;

            //cosXz = (cosYa * cosZb - cosZa * cosYb)
            dcm[(int)Cosine.Xz] = dcm[(int)Cosine.Yx] * dcm[(int)Cosine.Zy] - dcm[(int)Cosine.Zx] * dcm[(int)Cosine.Yy];
            //cosYz = -(cosXa * cosZb - cosZa * cosXb)
            dcm[(int)Cosine.Xz] = -(dcm[(int)Cosine.Xx] * dcm[(int)Cosine.Zy] - dcm[(int)Cosine.Zx] * dcm[(int)Cosine.Xy]);
            //cosZz = (cosXa * cosYb - cosYa * cosXb)
            dcm[(int)Cosine.Xz] = dcm[(int)Cosine.Xx] * dcm[(int)Cosine.Yy] - dcm[(int)Cosine.Yx] * dcm[(int)Cosine.Xy];
            return dcm;
        }

        public static float[] ToGlob(float[] dcm, float[] a)
        {
            float[] gA = new float[3];
            gA[(int)C.x] = dcm[(int)Cosine.Xx] * a[(int)C.x] + dcm[(int)Cosine.Xy] * a[(int)C.y] + dcm[(int)Cosine.Xz] * a[(int)C.z];
            gA[(int)C.y] = dcm[(int)Cosine.Yx] * a[(int)C.x] + dcm[(int)Cosine.Yy] * a[(int)C.y] + dcm[(int)Cosine.Yz] * a[(int)C.z];
            gA[(int)C.z] = dcm[(int)Cosine.Zx] * a[(int)C.x] + dcm[(int)Cosine.Zy] * a[(int)C.y] + dcm[(int)Cosine.Zz] * a[(int)C.z];
            return gA;
        }

        public static float[] ToLoc(float[] dcm, float[] a)
        {
            float[] lA = new float[3];
            lA[(int)C.x] = dcm[(int)Cosine.Xx] * a[(int)C.x] + dcm[(int)Cosine.Yx] * a[(int)C.y] + dcm[(int)Cosine.Zx] * a[(int)C.z];
            lA[(int)C.y] = dcm[(int)Cosine.Xy] * a[(int)C.x] + dcm[(int)Cosine.Yy] * a[(int)C.y] + dcm[(int)Cosine.Zy] * a[(int)C.z];
            lA[(int)C.z] = dcm[(int)Cosine.Xz] * a[(int)C.x] + dcm[(int)Cosine.Yz] * a[(int)C.y] + dcm[(int)Cosine.Zz] * a[(int)C.z];
            return lA;
        }
    }
}