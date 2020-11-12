#pragma once
#include "maf.hpp"
#include "NodeLoad.h"

class Integr
{

private:
	const float v1 = 1.0f / 12.0f;
	const float v2 = 5.0f / 12.0f;
	const float v3 = 1.0f;
	const float v4 = 1.0f;
public:
    static void EulerExpl(NodeLoad &nodeLoad, DirectX::SimpleMath::Vector3& now_p,
        DirectX::SimpleMath::Vector3& now_u, DirectX::SimpleMath::Vector3& now_v,
        DirectX::SimpleMath::Vector3& now_a, const DirectX::SimpleMath::Vector3& before_p, const DirectX::SimpleMath::Vector3& before_u,
        const DirectX::SimpleMath::Vector3& before_v, const  DirectX::SimpleMath::Vector3& before_a, const DirectX::SimpleMath::Vector3& zero_p, float dt)
    {
        now_v.x = before_v.x + before_a.x * dt;
        now_v.y = before_v.y + before_a.y * dt;
        switch (nodeLoad)
        {
        case NodeLoad::p:
            break;
        case NodeLoad::u:
            break;
        case NodeLoad::v:
            break;
        case NodeLoad::a:
            break;
        case NodeLoad::b:
            break;
        case NodeLoad::f:
            break;
        case NodeLoad::none:
            now_u.x = now_v.x * dt;
            now_u.y = now_v.y * dt;
            now_p.x = zero_p.x + now_u.x;
            now_p.y = zero_p.y + now_u.y;
            break;
        default:
            break;
        }
    };
    static void Verlet(NodeLoad nodeLoad, DirectX::SimpleMath::Vector3& now_p,
        DirectX::SimpleMath::Vector3& now_u, DirectX::SimpleMath::Vector3& now_v,
        DirectX::SimpleMath::Vector3& now_a, const DirectX::SimpleMath::Vector3& before_p, const DirectX::SimpleMath::Vector3& before_u,
        const DirectX::SimpleMath::Vector3& before_v, const  DirectX::SimpleMath::Vector3& before_a, const DirectX::SimpleMath::Vector3& zero_p, float dt)
    {
        now_v.x = before_v.x + hlf * (before_a.x + now_a.x) * dt;
        now_v.y = before_v.y + hlf * (before_a.y + now_a.y) * dt;
        switch (nodeLoad)
        {
        case NodeLoad::p:
            break;
        case NodeLoad::u:
            break;
        case NodeLoad::v:
            break;
        case NodeLoad::a:
            break;
        case NodeLoad::b:
            break;
        case NodeLoad::f:
            break;
        case NodeLoad::none:
            now_u.x = now_v.x * dt + hlf * (before_a.x * maf::P2(dt));
            now_u.y = now_v.y * dt + hlf * (before_a.y * maf::P2(dt));
            now_p.x = zero_p.x + now_u.x;
            now_p.y = zero_p.y + now_u.y;
            break;
        default:
            break;
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
};