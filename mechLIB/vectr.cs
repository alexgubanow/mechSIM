namespace mechLIB
{
    public class vectr
    {
        public static void Invert(ref xyz_t vec)
        {
            vec.x = 0 - vec.x;
            vec.y = 0 - vec.y;
            vec.z = 0 - vec.z;
        }

        public static void Minus(ref xyz_t vec, float arg)
        {
            vec.x -= arg;
            vec.y -= arg;
            vec.z -= arg;
        }

        public static void Minus(ref xyz_t vec, xyz_t arg)
        {
            vec.x -= arg.x;
            vec.y -= arg.y;
            vec.z -= arg.z;
        }
        public static void Minus(xyz_t arg1, xyz_t arg2, ref xyz_t res)
        {
            res.x = arg1.x - arg2.x;
            res.y = arg1.y - arg2.y;
            res.z = arg1.z - arg2.z;    
        }

        public static void Plus(ref xyz_t vec, float arg)
        {
            vec.x += arg;
            vec.y += arg;
            vec.z += arg;
        }

        public static void Plus(ref xyz_t vec, xyz_t arg)
        {
            vec.x += arg.x;
            vec.y += arg.y;
            vec.z += arg.z;
        }
        public static void Plus(xyz_t arg1, xyz_t arg2, ref xyz_t res)
        {
            res.x = arg1.x + arg2.x;
            res.y = arg1.y + arg2.y;
            res.z = arg1.z + arg2.z;
        }
    }
}