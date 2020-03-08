using System.Numerics;

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
        public dcm_t(Vector3 endPoint, Vector3 radiusPoint)
        {
            Xx = endPoint.X / endPoint.Length();
            Yx = endPoint.Y / endPoint.Length();
            Zx = endPoint.Z / endPoint.Length();

            Xy = radiusPoint.X / radiusPoint.Length();
            Yy = radiusPoint.Y / radiusPoint.Length();
            Zy = radiusPoint.Z / radiusPoint.Length();

            Xz = Yx * Zy - Zx * Yy;
            Yz = 0 - (Xx * Zy - Zx * Xy);
            Zz = Xx * Yy - Yx * Xy;
        }

        public void ToGlob(Vector3 Lp, ref Vector3 gA)
        {
            gA.X = Xx * Lp.X + Xy * Lp.Y + Xz * Lp.Z;
            gA.Y = Yx * Lp.X + Yy * Lp.Y + Yz * Lp.Z;
            gA.Z = Zx * Lp.X + Zy * Lp.Y + Zz * Lp.Z;
        }

        public void ToLoc(Vector3 Gp, ref Vector3 lA)
        {
            lA.X = Xx * Gp.X + Yx * Gp.Y + Zx * Gp.Z;
            lA.Y = Xy * Gp.X + Yy * Gp.Y + Zy * Gp.Z;
            lA.Z = Xz * Gp.X + Yz * Gp.Y + Zz * Gp.Z;
        }
    }
}
