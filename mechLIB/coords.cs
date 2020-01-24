using System;
using System.Runtime.InteropServices;

namespace mechLIB
{
    public class crds
    {
        public static xyz_t GetLbyXYZ(xyz_t zeroP, xyz_t targetP)
        {
            return new xyz_t()
            {
                x = (float)Math.Sqrt(maf.P2(targetP.x - zeroP.x)),
                y = (float)Math.Sqrt(maf.P2(targetP.y - zeroP.y)),
                z = (float)Math.Sqrt(maf.P2(targetP.z - zeroP.z))
            };
        }
        //in developer cmd type dumpbin.exe /EXPORTS mf.dll to have full name of function
        [DllImport("mf.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = @"?_getTotL@@YAMMMMMMM@Z")]
        private static extern float _getTotL(float x1, float y1, float z1, float x2, float y2, float z2);
        [DllImport("mf.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = @"?_getTotL@@YAMMMM@Z")]
        private static extern float _getTotL(float x, float y, float z);
        public static float GetTotL(xyz_t zeroP, xyz_t targetP)
        {
            //return _getTotL(zeroP.x, zeroP.y, zeroP.z, targetP.x, targetP.y, targetP.z);
            return (float)Math.Sqrt(
               maf.P2(targetP.x - zeroP.x) +
                maf.P2(targetP.y - zeroP.y) +
                maf.P2(targetP.z - zeroP.z));
        }
        public static float GetTotL(float x1, float y1, float z1, float x2, float y2, float z2)
        {
            return (float)Math.Sqrt(
               maf.P2(x2 - x1) +
                maf.P2(y2 - y1) +
                maf.P2(z2 - z1));
        }
        public static float GetTotL(xyz_t targetP)
        {
            //return _getTotL(targetP.x, targetP.y, targetP.z);
            return (float)Math.Sqrt( maf.P2(targetP.x) + maf.P2(targetP.y) + maf.P2(targetP.z));
        }


    }
}