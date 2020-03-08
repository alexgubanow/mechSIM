using System.Numerics;

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
        //coordinates
        public Vector3 p;
        //displacement
        public Vector3 u;
        //velocity
        public Vector3 v;
        //acceleration
        public Vector3 a;
        //jerk
        public Vector3 b;
        public deriv_t()
        {
            p = new Vector3();
            u = new Vector3();
            v = new Vector3();
            a = new Vector3();
            b = new Vector3();
        }
        public Vector3 GetByN(N_t N)
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
    }
}
