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
                now.v.X = before.v.X + now.a.X * dt;
                now.u.X = before.u.X + now.v.X * dt;
                now.p.X = before.p.X + now.u.X;

                now.v.Y = before.v.Y + now.a.Y * dt;
                now.u.Y = before.u.Y + now.v.Y * dt;
                now.p.Y = before.p.Y + now.u.Y;

                //now.v.Z = before.v.Z + now.a.Z * dt;
                //now.u.Z = before.u.Z + now.v.Z * dt;
                //now.p.Z = before.p.Z + now.u.Z;
            }
            else
            {
                now.u.X = now.p.X - before.p.X;
                now.v.X = (now.u.X - before.u.X) / dt;
                now.a.X = (now.v.X - before.v.X) / dt;

                now.u.Y = now.p.Y - before.p.Y;
                now.v.Y = (now.u.Y - before.u.Y) / dt;
                now.a.Y = (now.v.Y - before.v.Y) / dt;

                //now.u.Z = now.p.Z - before.p.Z;
                //now.v.Z = (now.u.Z - before.u.Z) / dt;
                //now.a.Z = (now.v.Z - before.v.Z) / dt;
            }
        }

        //private static void EulerImpl(Direction dir, ref deriv_t now, deriv_t before, float dt)
        //{
        //    now.v.X = before.v.X + now.a.X * dt;
        //    now.u.X = before.u.X + now.v.X * dt;
        //    now.p.X = before.p.X + now.u.X;
        //}

        public static void Verlet(Direction dir, ref deriv_t now, deriv_t before, float dt)
        {
            if (dir == Direction.Forward)
            {
                now.v.X = before.v.X + (maf.hlf * (before.a.X + now.a.X)) * dt;
                now.u.X = before.u.X + now.v.X * dt + (maf.hlf * before.a.X * maf.P2(dt));
                now.p.X = before.p.X + now.u.X;

                now.v.Y = before.v.Y + (maf.hlf * (before.a.Y + now.a.Y)) * dt;
                now.u.Y = before.u.Y + now.v.Y * dt + (maf.hlf * before.a.Y * maf.P2(dt));
                now.p.Y = before.p.Y + now.u.Y;

                //now.v.Z = before.v.Z + (maf.hlf * (before.a.Z + now.a.Z)) * dt;
                //now.u.Z = before.u.Z + now.v.Z * dt + (maf.hlf * before.a.Z * maf.P2(dt));
                //now.p.Z = before.p.Z + now.u.Z;
            }
            else
            {
                throw new System.NotImplementedException();
                //now.u.X = before.u.X + now.v.X * dt + (maf.hlf * before.a.X * maf.P2(dt));
                //now.v.X = before.v.X + (maf.hlf * before.a.X + now.a.X) * dt;
                //now.p.X = before.p.X + now.u.X;
            }
        }

        //private static void GearP(ref float[][] now, float[][] before, float dt, float m)
        //{
        //    now.b.X] = before.f.X] / m * dt;
        //    now.a.X] = before.a.X] + before.b.X] * dt;
        //    now.v.X] = before.v.X] + before.a.X] * dt + maf.hlf * before.b.X] * maf.P2(dt);
        //    now.u.X] = before.u.X] + before.v.X] * dt +
        //        maf.hlf * before.a.X] * maf.P2(dt) + maf.sxt * before.b.X] * maf.P3(dt);
        //    now.p.X] = before.p.X] + before.u.X];
        //}

        //private static void GearC(ref float[][] now, float[][] before, float dt, float m)
        //{
        //    now.a.X] = before.f.X] / m;
        //    float[] da = GearDa(now.a], before.a]);
        //    now.b.X] = before.b.X] + v4 * (da[(int)C.X] / dt);
        //    now.a.X] = before.a.X] + v3 * da[(int)C.X];
        //    now.v.X] = before.v.X] + v2 * da[(int)C.X] * dt;
        //    now.u.X] = before.u.X] + v1 * da[(int)C.X] * maf.P2(dt);
        //    now.p.X] = before.p.X] + before.u.X];
        //}

        //private static float[] GearDa(float[] aNow, float[] aBefore)
        //{
        //    return new float[3] { aNow[(int)C.X] - aBefore[(int)C.X], 0, 0 };
        //}
    }
}