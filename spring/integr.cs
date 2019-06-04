using System;

namespace spring
{
    public class Integr
    {
        private const float hlf = 1f / 2f;
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
            now[(int)N.u][(int)C.x] = before[(int)N.u][(int)C.x] + before[(int)N.v][(int)C.x] * dt + (hlf * before[(int)N.a][(int)C.x] * (float)Math.Pow(dt, 2));
            now[(int)N.p][(int)C.x] = before[(int)N.p][(int)C.x] + before[(int)N.u][(int)C.x];
        }
        public static void Gear(ref float[][] now, float[][] before, float dt, float m)
        {
            now[(int)N.a][(int)C.x] = before[(int)N.f][(int)C.x] / m;
            now[(int)N.v][(int)C.x] = before[(int)N.v][(int)C.x] + before[(int)N.a][(int)C.x] * dt;
            now[(int)N.u][(int)C.x] = before[(int)N.u][(int)C.x] + before[(int)N.v][(int)C.x] * dt;
            now[(int)N.p][(int)C.x] = before[(int)N.p][(int)C.x] + before[(int)N.u][(int)C.x];
        }
    }
}