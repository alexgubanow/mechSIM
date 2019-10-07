namespace mechLIB
{
    public class Integr
    {
        public enum Direction
        {
            Forward, Backward
        }

        private const float v1 = 1f / 12f;
        private const float v2 = 5f / 12f;
        private const float v3 = 1f;
        private const float v4 = 1f;

        public static void EulerExpl(Direction dir, ref deriv_t now, deriv_t before, float dt)
        {
            if (dir == Direction.Forward)
            {
                now.v.x = before.v.x + before.a.x * dt;
                now.u.x = before.u.x + before.v.x * dt;
                now.p.x = before.p.x + before.u.x;
            }
            else
            {
                now.u.x = (now.p.x - before.p.x) / dt;
                now.v.x = (now.u.x - before.u.x) / dt;
                now.a.x = (now.v.x - before.v.x) / dt;
            }
        }

        //private static void EulerImpl(Direction dir, ref deriv_t now, deriv_t before, float dt)
        //{
        //    now.v.x = before.v.x + now.a.x * dt;
        //    now.u.x = before.u.x + now.v.x * dt;
        //    now.p.x = before.p.x + now.u.x;
        //}

        public static void Verlet(Direction dir, ref deriv_t now, deriv_t before, float dt)
        {
            throw new System.NotImplementedException();
            //if (dir == Direction.Forward)
            //{
            //    now.v.x = before.v.x + (maf.hlf * before.a.x + now.a.x) * dt;
            //    now.u.x = before.u.x + now.v.x * dt + (maf.hlf * before.a.x * maf.P2(dt));
            //    now.p.x = before.p.x + now.u.x;
            //}
            //else
            //{
            //    now.u.x = before.u.x + now.v.x * dt + (maf.hlf * before.a.x * maf.P2(dt));
            //    now.v.x = before.v.x + (maf.hlf * before.a.x + now.a.x) * dt;
            //    now.p.x = before.p.x + now.u.x;
            //}
        }

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