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
            /*
        a(node,t) = f(node, t - 1) / m;
        v(node,t) = v(node, t - 1) + a(node, t - 1) * dt;
        ux(node,t) = ux(node, t - 1) + v(node, t - 1) * dt;
        cd(node,t) = cd(node, t - 1) + ux(node,t - 1);*/
            now[N.a][N.x] = before[N.f][N.x] / m;
            now[N.v][N.x] = before[N.v][N.x] + before[N.a][N.x] * dt;
            now[N.u][N.x] = before[N.u][N.x] + before[N.v][N.x] * dt;
            now[N.c][N.x] = before[N.c][N.x] + before[N.u][N.x];
        }
    }
}
