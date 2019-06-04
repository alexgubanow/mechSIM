using System;

namespace spring
{
    public class Integr
    {
        private const float v1 = 1f / 12f;
        private const float v2 = 5f / 12f;
        private const float v3 = 1f;
        private const float v4 = 1f;

        private const float hlf = 1f / 2f;
        private const float sxt = 1f / 6f;

        private static float P2(float value)
        {
            return (float)Math.Pow(value, 2);
        }

        private static float P3(float value)
        {
            return (float)Math.Pow(value, 3);
        }

        private static float P4(float value)
        {
            return (float)Math.Pow(value, 4);
        }

        public static void EulerExpl(ref float[][] now, float[][] before, float dt, float m)
        {
            now[(int)N.a][(int)C.x] = before[(int)N.f][(int)C.x] / m;
            now[(int)N.v][(int)C.x] = before[(int)N.v][(int)C.x] + before[(int)N.a][(int)C.x] * dt;
            now[(int)N.u][(int)C.x] = before[(int)N.u][(int)C.x] + before[(int)N.v][(int)C.x] * dt;
            now[(int)N.p][(int)C.x] = before[(int)N.p][(int)C.x] + before[(int)N.u][(int)C.x];
        }

        public static void EulerImpl(ref float[][] now, float[][] before, float dt, float m)
        {
            now[(int)N.a][(int)C.x] = now[(int)N.f][(int)C.x] / m;
            now[(int)N.v][(int)C.x] = before[(int)N.v][(int)C.x] + now[(int)N.a][(int)C.x] * dt;
            now[(int)N.u][(int)C.x] = before[(int)N.u][(int)C.x] + now[(int)N.v][(int)C.x] * dt;
            now[(int)N.p][(int)C.x] = before[(int)N.p][(int)C.x] + now[(int)N.u][(int)C.x];
        }

        public static void Verlet(ref float[][] now, float[][] before, float dt, float m)
        {
            now[(int)N.a][(int)C.x] = before[(int)N.f][(int)C.x] / m;
            now[(int)N.v][(int)C.x] = before[(int)N.v][(int)C.x] + (hlf * (before[(int)N.a][(int)C.x] + now[(int)N.a][(int)C.x]) * dt);
            now[(int)N.u][(int)C.x] = before[(int)N.u][(int)C.x] + before[(int)N.v][(int)C.x] * dt + (hlf * before[(int)N.a][(int)C.x] * P2(dt));
            now[(int)N.p][(int)C.x] = before[(int)N.p][(int)C.x] + before[(int)N.u][(int)C.x];
        }

        public static void GearP(ref float[][] now, float[][] before, float dt, float m)
        {
            now[(int)N.b][(int)C.x] = before[(int)N.f][(int)C.x] / m * dt;
            now[(int)N.a][(int)C.x] = before[(int)N.a][(int)C.x] + before[(int)N.b][(int)C.x] * dt;
            now[(int)N.v][(int)C.x] = before[(int)N.v][(int)C.x] + before[(int)N.a][(int)C.x] * dt + hlf * before[(int)N.b][(int)C.x] * P2(dt);
            now[(int)N.u][(int)C.x] = before[(int)N.u][(int)C.x] + before[(int)N.v][(int)C.x] * dt +
                hlf * before[(int)N.a][(int)C.x] * P2(dt) + sxt * before[(int)N.b][(int)C.x] * P3(dt);
            now[(int)N.p][(int)C.x] = before[(int)N.p][(int)C.x] + before[(int)N.u][(int)C.x];
        }

        public static void GearC(ref float[][] now, float[][] before, float dt, float m)
        {
            now[(int)N.a][(int)C.x] = before[(int)N.f][(int)C.x] / m;
            float[] da = GearDa(now[(int)N.a], before[(int)N.a]);
            now[(int)N.b][(int)C.x] = before[(int)N.b][(int)C.x] + v4 * (da[(int)C.x] / dt);
            now[(int)N.a][(int)C.x] = before[(int)N.a][(int)C.x] + v3 * da[(int)C.x];
            now[(int)N.v][(int)C.x] = before[(int)N.v][(int)C.x] + v2 * da[(int)C.x] * dt;
            now[(int)N.u][(int)C.x] = before[(int)N.u][(int)C.x] + v1 * da[(int)C.x] * P2(dt);
            now[(int)N.p][(int)C.x] = before[(int)N.p][(int)C.x] + before[(int)N.u][(int)C.x];
        }

        public static float[] GearDa(float[] aNow, float[] aBefore)
        {
            return new float[3] { aNow[(int)C.x] - aBefore[(int)C.x], 0, 0 };
        }
    }
}