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

        public static void EulerExpl(NodeLoad nodeLoad, ref deriv_t now, deriv_t before, deriv_t zero, float dt)
        {
            now.v.X = before.v.X + before.a.X * dt;
            now.v.Y = before.v.Y + before.a.Y * dt;
            switch (nodeLoad)
            {
                case NodeLoad.p:
                    break;
                case NodeLoad.u:
                    break;
                case NodeLoad.v:
                    break;
                case NodeLoad.a:
                    break;
                case NodeLoad.b:
                    break;
                case NodeLoad.f:
                    break;
                case NodeLoad.none:
                    now.u.X = maf.hlf(before.a.X * maf.P2(dt));
                    now.u.Y = before.u.Y + before.v.Y * dt;
                    now.p.X = zero.p.X + now.u.X;
                    now.p.Y = zero.p.Y + now.u.Y;
                    break;
                default:
                    break;
            }
        }
        
        public static void Verlet(NodeLoad nodeLoad, ref deriv_t now, deriv_t before, float dt)
        {
            //now.v.X = before.v.X + (maf.hlf * (before.a.X + now.a.X)) * dt;
            //now.v.Y = before.v.Y + (maf.hlf * (before.a.Y + now.a.Y)) * dt;
            //switch (nodeLoad)
            //{
            //    case NodeLoad.p:
            //        break;
            //    case NodeLoad.u:
            //        break;
            //    case NodeLoad.v:
            //        break;
            //    case NodeLoad.a:
            //        break;
            //    case NodeLoad.b:
            //        break;
            //    case NodeLoad.f:
            //        break;
            //    case NodeLoad.none:
            //        now.u.X = before.u.X + now.v.X * dt + (maf.hlf * before.a.X * maf.P2(dt));
            //        now.u.Y = before.u.Y + now.v.Y * dt + (maf.hlf * before.a.Y * maf.P2(dt));
            //        now.p.X = before.p.X + now.u.X;
            //        now.p.Y = before.p.Y + now.u.Y;
            //        break;
            //    default:
            //        break;
            //}
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