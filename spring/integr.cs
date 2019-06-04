namespace spring
{
    public class Integr
    {
        public static void EulerExpl(ref float[][] now, float[][] before, float dt, float m,int ff)
        {
            now[(int)N.a][(int)C.x] = before[(int)N.f][(int)C.x] / m;
            now[(int)N.v][(int)C.x] = before[(int)N.v][(int)C.x] + before[(int)N.a][(int)C.x] * dt;
            now[(int)N.u][(int)C.x] = before[(int)N.u][(int)C.x] + before[(int)N.v][(int)C.x] * dt;
            now[(int)N.p][(int)C.x] = before[(int)N.p][(int)C.x] + before[(int)N.u][(int)C.x];
        }
    }
}