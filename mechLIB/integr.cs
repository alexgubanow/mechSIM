namespace mechLIB
{
    public class Integr
    {
        public enum Types
        {
            EulerExpl, EulerImpl, Verlet, UVAF_P, PVAF_U, PUAF_V, PUVF_A, Gear
        }

        public static void Integrate(Types itegrType, ref deriv_t now, deriv_t before, float dt, float m)
        {
            switch (itegrType)
            {
                case Types.EulerExpl:
                    EulerExpl(ref now, before, dt, m);
                    break;

                case Types.EulerImpl:
                    EulerImpl(ref now, before, dt, m);
                    break;

                //case Types.Verlet:
                //    Verlet(ref now, before, dt, m);
                //    break;

                //case Types.UVAF_P:
                //    UVAF_P(ref now, before, m);
                //    break;

                //case Types.PVAF_U:
                //    PVAF_U(ref now, before, m);
                //    break;

                //case Types.PUAF_V:
                //    PUAF_V(ref now, before, m);
                //    break;

                //case Types.PUVF_A:
                //    PUVF_A(ref now, before, m);
                //    break;

                //case Types.Gear:
                //    break;

                default:
                    break;
            }
        }

        private const float v1 = 1f / 12f;
        private const float v2 = 5f / 12f;
        private const float v3 = 1f;
        private const float v4 = 1f;

        private static void EulerExpl(ref deriv_t now, deriv_t before, float dt, float m)
        {
            now.v.x = before.v.x + before.a.x * dt;
            now.u.x = before.u.x + before.v.x * dt;
            now.p.x = before.p.x + before.u.x;
        }

        private static void EulerImpl(ref deriv_t now, deriv_t before, float dt, float m)
        {
            now.v.x = before.v.x + now.a.x * dt;
            now.u.x = before.u.x + now.v.x * dt;
            now.p.x = before.p.x + now.u.x;
        }

        //private static void Verlet(ref float[][] now, float[][] before, float dt, float m)
        //{
        //    for (int axis = 0; axis < 3; axis++)
        //    {
        //        now.a][axis] = before.f][axis] / m;
        //        now.v][axis] = before.v][axis] + (maf.hlf * (before.a][axis] + now.a][axis]) * dt);
        //        now.u][axis] = before.u][axis] + before.v][axis] * dt + (maf.hlf * before.a][axis] * maf.P2(dt));
        //        now.p][axis] = before.p][axis] + before.u][axis];
        //    }
        //}

        //private static void UVAF_P(ref float[][] now, float[][] before, float m)
        //{
        //    for (int axis = 0; axis < 3; axis++)
        //    {
        //        now.u][axis] = now.p][axis] - before.p][axis];
        //        now.v][axis] = now.u][axis] - before.u][axis];
        //        now.a][axis] = now.v][axis] - before.v][axis];
        //        now.f][axis] = now.a][axis] / m;
        //    }
        //}

        //private static void PVAF_U(ref float[][] now, float[][] before, float m)
        //{
        //    for (int axis = 0; axis < 3; axis++)
        //    {
        //        now.p][axis] = before.p][axis] + now.u][axis];
        //        now.v][axis] = now.u][axis] - before.u][axis];
        //        now.a][axis] = now.v][axis] - before.v][axis];
        //        now.f][axis] = now.a][axis] / m;
        //    }
        //}

        //private static void PUAF_V(ref float[][] now, float[][] before, float m)
        //{
        //    for (int axis = 0; axis < 3; axis++)
        //    {
        //        now.p][axis] = before.p][axis] + now.u][axis];
        //        now.u][axis] = before.u][axis] + now.v][axis];
        //        now.a][axis] = now.v][axis] - before.v][axis];
        //        now.f][axis] = now.a][axis] / m;
        //    }
        //}

        //private static void PUVF_A(ref float[][] now, float[][] before, float m)
        //{
        //    for (int axis = 0; axis < 3; axis++)
        //    {
        //        now.p][axis] = before.p][axis] + now.u][axis];
        //        now.u][axis] = before.u][axis] + now.v][axis];
        //        now.v][axis] = before.v][axis] + now.a][axis];
        //        now.f][axis] = now.a][axis] / m;
        //    }
        //}

        //private static void GearP(ref float[][] now, float[][] before, float dt, float m)
        //{
        //    now.b.x] = before.f.x] / m * dt;
        //    now.a.x] = before.a.x] + before.b.x] * dt;
        //    now.v.x] = before.v.x] + before.a.x] * dt + maf.hlf * before.b.x] * maf.P2(dt);
        //    now.u.x] = before.u.x] + before.v.x] * dt +
        //        maf.hlf * before.a.x] * maf.P2(dt) + maf.sxt * before.b.x] * maf.P3(dt);
        //    now.p.x] = before.p.x] + before.u.x];
        //}

        //private static void GearC(ref float[][] now, float[][] before, float dt, float m)
        //{
        //    now.a.x] = before.f.x] / m;
        //    float[] da = GearDa(now.a], before.a]);
        //    now.b.x] = before.b.x] + v4 * (da[(int)C.x] / dt);
        //    now.a.x] = before.a.x] + v3 * da[(int)C.x];
        //    now.v.x] = before.v.x] + v2 * da[(int)C.x] * dt;
        //    now.u.x] = before.u.x] + v1 * da[(int)C.x] * maf.P2(dt);
        //    now.p.x] = before.p.x] + before.u.x];
        //}

        //private static float[] GearDa(float[] aNow, float[] aBefore)
        //{
        //    return new float[3] { aNow[(int)C.x] - aBefore[(int)C.x], 0, 0 };
        //}
    }
}