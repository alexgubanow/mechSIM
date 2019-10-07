namespace mechLIB
{
    public enum N_t
    {
        //coordinates
        p,

        //displacement
        u,

        //velocity
        v,

        //acceleration
        a,

        //jerk
        b
    }
    public class deriv_t
    {
        public xyz_t p;
        public xyz_t u;
        public xyz_t v;
        public xyz_t a;
        public xyz_t b;
        public deriv_t()
        {
            p = new xyz_t();
            u = new xyz_t();
            v = new xyz_t();
            a = new xyz_t();
            b = new xyz_t();
        }
        public xyz_t GetByN(N_t N)
        {
            switch (N)
            {
                case N_t.p:
                    return p;
                case N_t.u:
                    return u;
                case N_t.v:
                    return v;
                case N_t.a:
                    return a;
                case N_t.b:
                    return b;
                default:
                    throw new System.Exception();
            }
        }
        public void SetByN(N_t N, xyz_t val)
        {
            switch (N)
            {
                case N_t.p:
                    p = val;
                    break;
                case N_t.u:
                    u = val;
                    break;
                case N_t.v:
                    v = val;
                    break;
                case N_t.a:
                    a = val;
                    break;
                case N_t.b:
                    b = val;
                    break;
                default:
                    throw new System.Exception();
            }
        }
    }
}
