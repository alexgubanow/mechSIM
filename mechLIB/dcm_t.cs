using System;

namespace mechLIB
{
    public class dcm_t
    {
        public float Xx;
        public float Yx;
        public float Zx;

        public float Xy;
        public float Yy;
        public float Zy;

        public float Xz;
        public float Yz;
        public float Zz;
        public dcm_t(xyz_t endPoint, xyz_t radiusPoint)
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
            float lax = crds.GetTotL(endPoint);
            //lby = 13.42;
            float lby = crds.GetTotL(radiusPoint);

            //lax = sqrt((ax - baseX) ^ 2 + (ay - baseY) ^ 2 + (az - baseZ) ^ 2)
            //lay = 0;
            //laz = 0;
            //lbx = 0;
            //lby = sqrt((bx - baseX) ^ 2 + (by - baseY) ^ 2 + (bz - baseZ) ^ 2)
            //lbz = 0;
            //cosXa = ax / lax
            Xx = endPoint.x / lax;
            //cosYa = ay / lax
            Yx = endPoint.y / lax;
            //cosZa = az / lax
            Zx = endPoint.z / lax;

            //cosXb = bx / lby
            Xy = radiusPoint.x / lby;
            //cosYb = by / lby
            Yy = radiusPoint.y / lby;
            //cosZb = bz / lby
            Zy = radiusPoint.z / lby;

            //cosXz = (cosYa * cosZb - cosZa * cosYb)
            Xz = Yx * Zy - Zx * Yy;
            //cosYz = -(cosXa * cosZb - cosZa * cosXb)
            Yz = 0 - (Xx * Zy - Zx * Xy);
            //cosZz = (cosXa * cosYb - cosYa * cosXb)
            Zz = Xx * Yy - Yx * Xy;
        }

        public void ToGlob(xyz_t Lp, ref xyz_t gA)
        {
            gA.x = Xx * Lp.x + Xy * Lp.y + Xz * Lp.z;
            gA.y = Yx * Lp.x + Yy * Lp.y + Yz * Lp.z;
            gA.z = Zx * Lp.x + Zy * Lp.y + Zz * Lp.z;
        }

        public void ToLoc(xyz_t Gp, ref xyz_t lA)
        {
            lA.x = Xx * Gp.x + Yx * Gp.y + Zx * Gp.z;
            lA.y = Xy * Gp.x + Yy * Gp.y + Zy * Gp.z;
            lA.z = Xz * Gp.x + Yz * Gp.y + Zz * Gp.z;
        }
    }
}
