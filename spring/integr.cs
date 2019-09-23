namespace spring
{
    public class Integr
    {
        public enum Types
        {
            EulerExpl, EulerImpl, Verlet, UVAF_P, PVAF_U, PUAF_V, PUVF_A, Gear
        }

        public static void Integrate(Types itegrType, ref float[][] now, float[][] before, float dt, float m)
        {
            switch (itegrType)
            {
                case Types.EulerExpl:
                    EulerExpl(ref now, before, dt, m);
                    break;

                case Types.EulerImpl:
                    EulerImpl(ref now, before, dt, m);
                    break;

                case Types.Verlet:
                    Verlet(ref now, before, dt, m);
                    break;

                case Types.UVAF_P:
                    UVAF_P(ref now, before, m);
                    break;

                case Types.PVAF_U:
                    PVAF_U(ref now, before, m);
                    break;

                case Types.PUAF_V:
                    PUAF_V(ref now, before, m);
                    break;

                case Types.PUVF_A:
                    PUVF_A(ref now, before, m);
                    break;

                case Types.Gear:
                    break;

                default:
                    break;
            }
        }

        private const float v1 = 1f / 12f;
        private const float v2 = 5f / 12f;
        private const float v3 = 1f;
        private const float v4 = 1f;

        private static void EulerExpl(ref float[][] now, float[][] before, float dt, float m)
        {
            now[(int)N.a][(int)C.x] = before[(int)N.f][(int)C.x] / m;
            now[(int)N.v][(int)C.x] = before[(int)N.v][(int)C.x] + before[(int)N.a][(int)C.x] * dt;
            now[(int)N.u][(int)C.x] = before[(int)N.u][(int)C.x] + before[(int)N.v][(int)C.x] * dt;
            now[(int)N.p][(int)C.x] = before[(int)N.p][(int)C.x] + before[(int)N.u][(int)C.x];
        }

        private static void EulerImpl(ref float[][] now, float[][] before, float dt, float m)
        {
            now[(int)N.a][(int)C.x] = now[(int)N.f][(int)C.x] / m;
            now[(int)N.v][(int)C.x] = before[(int)N.v][(int)C.x] + now[(int)N.a][(int)C.x] * dt;
            now[(int)N.u][(int)C.x] = before[(int)N.u][(int)C.x] + now[(int)N.v][(int)C.x] * dt;
            now[(int)N.p][(int)C.x] = before[(int)N.p][(int)C.x] + now[(int)N.u][(int)C.x];
        }

        private static void Verlet(ref float[][] now, float[][] before, float dt, float m)
        {
            for (int axis = 0; axis < 3; axis++)
            {
                now[(int)N.a][axis] = before[(int)N.f][axis] / m;
                now[(int)N.v][axis] = before[(int)N.v][axis] + (maf.hlf * (before[(int)N.a][axis] + now[(int)N.a][axis]) * dt);
                now[(int)N.u][axis] = before[(int)N.u][axis] + before[(int)N.v][axis] * dt + (maf.hlf * before[(int)N.a][axis] * maf.P2(dt));
                now[(int)N.p][axis] = before[(int)N.p][axis] + before[(int)N.u][axis];
            }
        }

        private static void UVAF_P(ref float[][] now, float[][] before, float m)
        {
            for (int axis = 0; axis < 3; axis++)
            {
                now[(int)N.u][axis] = now[(int)N.p][axis] - before[(int)N.p][axis];
                now[(int)N.v][axis] = now[(int)N.u][axis] - before[(int)N.u][axis];
                now[(int)N.a][axis] = now[(int)N.v][axis] - before[(int)N.v][axis];
                now[(int)N.f][axis] = now[(int)N.a][axis] / m;
            }
        }

        private static void PVAF_U(ref float[][] now, float[][] before, float m)
        {
            for (int axis = 0; axis < 3; axis++)
            {
                now[(int)N.p][axis] = before[(int)N.p][axis] + now[(int)N.u][axis];
                now[(int)N.v][axis] = now[(int)N.u][axis] - before[(int)N.u][axis];
                now[(int)N.a][axis] = now[(int)N.v][axis] - before[(int)N.v][axis];
                now[(int)N.f][axis] = now[(int)N.a][axis] / m;
            }
        }

        private static void PUAF_V(ref float[][] now, float[][] before, float m)
        {
            for (int axis = 0; axis < 3; axis++)
            {
                now[(int)N.p][axis] = before[(int)N.p][axis] + now[(int)N.u][axis];
                now[(int)N.u][axis] = before[(int)N.u][axis] + now[(int)N.v][axis];
                now[(int)N.a][axis] = now[(int)N.v][axis] - before[(int)N.v][axis];
                now[(int)N.f][axis] = now[(int)N.a][axis] / m;
            }
        }

        private static void PUVF_A(ref float[][] now, float[][] before, float m)
        {
            for (int axis = 0; axis < 3; axis++)
            {
                now[(int)N.p][axis] = before[(int)N.p][axis] + now[(int)N.u][axis];
                now[(int)N.u][axis] = before[(int)N.u][axis] + now[(int)N.v][axis];
                now[(int)N.v][axis] = before[(int)N.v][axis] + now[(int)N.a][axis];
                now[(int)N.f][axis] = now[(int)N.a][axis] / m;
            }
        }

        private static void GearP(ref float[][] now, float[][] before, float dt, float m)
        {
            now[(int)N.b][(int)C.x] = before[(int)N.f][(int)C.x] / m * dt;
            now[(int)N.a][(int)C.x] = before[(int)N.a][(int)C.x] + before[(int)N.b][(int)C.x] * dt;
            now[(int)N.v][(int)C.x] = before[(int)N.v][(int)C.x] + before[(int)N.a][(int)C.x] * dt + maf.hlf * before[(int)N.b][(int)C.x] * maf.P2(dt);
            now[(int)N.u][(int)C.x] = before[(int)N.u][(int)C.x] + before[(int)N.v][(int)C.x] * dt +
                maf.hlf * before[(int)N.a][(int)C.x] * maf.P2(dt) + maf.sxt * before[(int)N.b][(int)C.x] * maf.P3(dt);
            now[(int)N.p][(int)C.x] = before[(int)N.p][(int)C.x] + before[(int)N.u][(int)C.x];
        }

        private static void GearC(ref float[][] now, float[][] before, float dt, float m)
        {
            now[(int)N.a][(int)C.x] = before[(int)N.f][(int)C.x] / m;
            float[] da = GearDa(now[(int)N.a], before[(int)N.a]);
            now[(int)N.b][(int)C.x] = before[(int)N.b][(int)C.x] + v4 * (da[(int)C.x] / dt);
            now[(int)N.a][(int)C.x] = before[(int)N.a][(int)C.x] + v3 * da[(int)C.x];
            now[(int)N.v][(int)C.x] = before[(int)N.v][(int)C.x] + v2 * da[(int)C.x] * dt;
            now[(int)N.u][(int)C.x] = before[(int)N.u][(int)C.x] + v1 * da[(int)C.x] * maf.P2(dt);
            now[(int)N.p][(int)C.x] = before[(int)N.p][(int)C.x] + before[(int)N.u][(int)C.x];
        }

        private static float[] GearDa(float[] aNow, float[] aBefore)
        {
            return new float[3] { aNow[(int)C.x] - aBefore[(int)C.x], 0, 0 };
        }
    }
}