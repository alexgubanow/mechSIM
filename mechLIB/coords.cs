using System;

namespace mechLIB
{
    public class crds
    {
        public static xyz_t GetLbyXYZ(xyz_t zeroP, xyz_t targetP)
        {
            return new xyz_t()
            {
                x = (float)Math.Sqrt(Math.Pow(targetP.x - zeroP.x, 2)),
                y = (float)Math.Sqrt(Math.Pow(targetP.y - zeroP.y, 2)),
                z = (float)Math.Sqrt(Math.Pow(targetP.z - zeroP.z, 2))
            };
        }
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

        
    }
}