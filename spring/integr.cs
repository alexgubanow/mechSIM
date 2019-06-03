using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace spring
{
    public class Integr
    {
        public static void EulerExpl(ref float[][] now, float[][] before, float dt, float m)
        {
            now[N.a][C.x] = before[N.f][C.x] / m;
            now[N.v][C.x] = before[N.v][C.x] + before[N.a][C.x] * dt;
            now[N.u][C.x] = before[N.u][C.x] + before[N.v][C.x] * dt;
            now[N.c][C.x] = before[N.c][C.x] + before[N.u][C.x];
        }
    }
}
