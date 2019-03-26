using System;

namespace spring
{
    public class coords
    {
        private enum c
        {
            x, y, z
        }

        private enum cosine { Xx, Yx, Zx, Xy, Yy, Zy, Xz, Yz, Zz, }

        private float getTotL(float[] zeroP, float[] targetP)
        {
            return (float)Math.Sqrt(
                Math.Pow(targetP[(int)c.x] - zeroP[(int)c.x], 2) +
                Math.Pow(targetP[(int)c.y] - zeroP[(int)c.y], 2) +
                Math.Pow(targetP[(int)c.z] - zeroP[(int)c.z], 2));
        }

        public float[] getDCM(float[] a, float[] b, float h)
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
            float lax = getTotL(new float[3] { 0, 0, 0 }, a);
            //lby = 13.42;
            float lby = getTotL(new float[3] { 0, 0, 0 }, b);

            //lax = sqrt((ax - baseX) ^ 2 + (ay - baseY) ^ 2 + (az - baseZ) ^ 2)
            //lay = 0;
            //laz = 0;
            //lbx = 0;
            //lby = sqrt((bx - baseX) ^ 2 + (by - baseY) ^ 2 + (bz - baseZ) ^ 2)
            //lbz = 0;
            float[] dcm = new float[9];
            //cosXa = ax / lax
            dcm[(int)cosine.Xx] = a[(int)c.x] / lax;
            //cosYa = ay / lax
            dcm[(int)cosine.Yx] = a[(int)c.y] / lax;
            //cosZa = az / lax
            dcm[(int)cosine.Zx] = a[(int)c.z] / lax;

            //cosXb = bx / lby
            dcm[(int)cosine.Xy] = b[(int)c.x] / lby;
            //cosYb = by / lby
            dcm[(int)cosine.Yy] = b[(int)c.y] / lby;
            //cosZb = bz / lby
            dcm[(int)cosine.Zy] = b[(int)c.z] / lby;

            //cosXz = (cosYa * cosZb - cosZa * cosYb)
            dcm[(int)cosine.Xz] = dcm[(int)cosine.Yx] * dcm[(int)cosine.Zy] - dcm[(int)cosine.Zx] * dcm[(int)cosine.Yy];
            //cosYz = -(cosXa * cosZb - cosZa * cosXb)
            dcm[(int)cosine.Xz] = -(dcm[(int)cosine.Xx] * dcm[(int)cosine.Zy] - dcm[(int)cosine.Zx] * dcm[(int)cosine.Xy]);
            //cosZz = (cosXa * cosYb - cosYa * cosXb)
            dcm[(int)cosine.Xz] = dcm[(int)cosine.Xx] * dcm[(int)cosine.Yy] - dcm[(int)cosine.Yx] * dcm[(int)cosine.Xy];
            return dcm;
        }

        public float[] toGlob(float[] dcm, float[] a)
        {
            float[] gA = new float[3];
            gA[(int)c.x] = dcm[(int)cosine.Xx] * a[(int)c.x] + dcm[(int)cosine.Yx] * a[(int)c.x] + dcm[(int)cosine.Zx] * a[(int)c.x];
            gA[(int)c.y] = dcm[(int)cosine.Xy] * a[(int)c.y] + dcm[(int)cosine.Yy] * a[(int)c.y] + dcm[(int)cosine.Zy] * a[(int)c.y];
            gA[(int)c.z] = dcm[(int)cosine.Xz] * a[(int)c.z] + dcm[(int)cosine.Yz] * a[(int)c.z] + dcm[(int)cosine.Zz] * a[(int)c.z];
            return gA;
        }

        public float[] toLoc(float[] dcm, float[] a)
        {
            float[] lA = new float[3];
            lA[(int)c.x] = dcm[(int)cosine.Xx] * a[(int)c.x] + dcm[(int)cosine.Xy] * a[(int)c.x] + dcm[(int)cosine.Xz] * a[(int)c.x];
            lA[(int)c.y] = dcm[(int)cosine.Yx] * a[(int)c.y] + dcm[(int)cosine.Yy] * a[(int)c.y] + dcm[(int)cosine.Yz] * a[(int)c.y];
            lA[(int)c.z] = dcm[(int)cosine.Zx] * a[(int)c.z] + dcm[(int)cosine.Zy] * a[(int)c.z] + dcm[(int)cosine.Zz] * a[(int)c.z];
            return lA;
        }
    }
}