using System.Runtime.InteropServices;

namespace mechLIB
{
    [StructLayout(LayoutKind.Sequential)]
    public struct props_t
    {
        public float DampRatio;
        public float MaxU;
        public float initDrop;
        public int nodes;
        public float E;
        public float L;
        public float D;
        public int Counts;
        public float dt;
        public float ro;
        public PhModels phMod;
    }
}