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
        const DirectX::SimpleMath::Vector3& before_v, const  DirectX::SimpleMath::Vector3& before_a, const DirectX::SimpleMath::Vector3&zero_p, float dt)
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
};