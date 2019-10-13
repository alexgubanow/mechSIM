namespace mechLIB
{

    public enum C_t
    {
        //X coordinate
        x,

        //Y coordinate
        y,

        //Z coordinate
        z
    }
    public class xyz_t
    {
        public float x;
        public float y;
        public float z;
        public void Invert()
        {
            x = 0 - x;
            y = 0 - y;
            z = 0 - z;
        }

        public void Minus(float arg)
        {
            x -= arg;
            y -= arg;
            z -= arg;
        }

        public void Minus(xyz_t arg)
        {
            x -= arg.x;
            y -= arg.y;
            z -= arg.z;
        }
        public void Minus(xyz_t arg1, xyz_t arg2)
        {
            x = arg1.x - arg2.x;
            y = arg1.y - arg2.y;
            z = arg1.z - arg2.z;
        }

        public void Plus(float arg)
        {
            x += arg;
            y += arg;
            z += arg;
        }

        public void Plus(xyz_t arg)
        {
            x += arg.x;
            y += arg.y;
            z += arg.z;
        }
        public void PlusHalf(xyz_t arg)
        {
            x += arg.x / 2;
            y += arg.y / 2;
            z += arg.z / 2;
        }
        public void Plus(xyz_t arg1, xyz_t arg2)
        {
            x = arg1.x + arg2.x;
            y = arg1.y + arg2.y;
            z = arg1.z + arg2.z;
        }
        public float GetByC(C_t C)
        {
            switch (C)
            {
                case C_t.x:
                    return x;
                case C_t.y:
                    return y;
                case C_t.z:
                    return z;
                default:
                    throw new System.Exception();
            }
        }
        public void SetByC(C_t C, float val)
        {
            switch (C)
            {
                case C_t.x:
                    x = val;
                    break;
                case C_t.y:
                    y = val;
                    break;
                case C_t.z:
                    z = val;
                    break;
                default:
                    throw new System.Exception();
            }
        }
    }
}
