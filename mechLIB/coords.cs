using System;

namespace mechLIB
{
    public class crds
    {
        public static float GetTotL(xyz_t zeroP, xyz_t targetP)
        {
            return (float)Math.Sqrt(
                Math.Pow(targetP.x - zeroP.x, 2) +
                Math.Pow(targetP.y - zeroP.y, 2) +
                Math.Pow(targetP.z - zeroP.z, 2));
        }
        public static float GetTotL(xyz_t targetP)
        {
            return (float)Math.Sqrt(
                Math.Pow(targetP.x, 2) +
                Math.Pow(targetP.y, 2) +
                Math.Pow(targetP.z, 2));
        }

        public static void GetDCM(ref dcm_t dcm, xyz_t endPoint, xyz_t radiusPoint)
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
            float lax = GetTotL(endPoint);
            //lby = 13.42;
            float lby = GetTotL(radiusPoint);

            //lax = sqrt((ax - baseX) ^ 2 + (ay - baseY) ^ 2 + (az - baseZ) ^ 2)
            //lay = 0;
            //laz = 0;
            //lbx = 0;
            //lby = sqrt((bx - baseX) ^ 2 + (by - baseY) ^ 2 + (bz - baseZ) ^ 2)
            //lbz = 0;
            //cosXa = ax / lax
            dcm.Xx = endPoint.x / lax;
            //cosYa = ay / lax
            dcm.Yx = endPoint.y / lax;
            //cosZa = az / lax
            dcm.Zx = endPoint.z / lax;

            //cosXb = bx / lby
            dcm.Xy = radiusPoint.x / lby;
            //cosYb = by / lby
            dcm.Yy = radiusPoint.y / lby;
            //cosZb = bz / lby
            dcm.Zy = radiusPoint.z / lby;

            //cosXz = (cosYa * cosZb - cosZa * cosYb)
            dcm.Xz = dcm.Yx * dcm.Zy - dcm.Zx * dcm.Yy;
            //cosYz = -(cosXa * cosZb - cosZa * cosXb)
            dcm.Yz = 0 - (dcm.Xx * dcm.Zy - dcm.Zx * dcm.Xy);
            //cosZz = (cosXa * cosYb - cosYa * cosXb)
            dcm.Zz = dcm.Xx * dcm.Yy - dcm.Yx * dcm.Xy;
        }

        public static void ToGlob(dcm_t dcm, xyz_t Lp, ref xyz_t gA)
        {
            gA.x = dcm.Xx * Lp.x + dcm.Xy * Lp.y + dcm.Xz * Lp.z;
            gA.y = dcm.Yx * Lp.x + dcm.Yy * Lp.y + dcm.Yz * Lp.z;
            gA.z = dcm.Zx * Lp.x + dcm.Zy * Lp.y + dcm.Zz * Lp.z;
        }

        public static void ToLoc(dcm_t dcm, xyz_t Gp, ref xyz_t lA)
        {
            lA.x = dcm.Xx * Gp.x + dcm.Yx * Gp.y + dcm.Zx * Gp.z;
            lA.y = dcm.Xy * Gp.x + dcm.Yy * Gp.y + dcm.Zy * Gp.z;
            lA.z = dcm.Xz * Gp.x + dcm.Yz * Gp.y + dcm.Zz * Gp.z;
        }
    }
}