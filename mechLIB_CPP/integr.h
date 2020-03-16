#pragma once
#include "maf.hpp"

class Integr
{

private:
	const float v1 = 1 / 12;
	const float v2 = 5 / 12;
	const float v3 = 1;
	const float v4 = 1;
public:
    static void EulerExpl(mechLIB_CPPWrapper::NodeLoad &nodeLoad, DirectX::SimpleMath::Vector3& now_p, 
        DirectX::SimpleMath::Vector3& now_u, DirectX::SimpleMath::Vector3& now_v,
        DirectX::SimpleMath::Vector3& now_a, DirectX::SimpleMath::Vector3& before_p, DirectX::SimpleMath::Vector3& before_u, 
        DirectX::SimpleMath::Vector3& before_v, DirectX::SimpleMath::Vector3& before_a, DirectX::SimpleMath::Vector3&zero_p, float dt)
    {
        now_v.x = before_v.x + before_a.x * dt;
        now_v.y = before_v.y + before_a.y * dt;
        switch (nodeLoad)
        {
        case mechLIB_CPPWrapper::NodeLoad::p:
            break;
        case mechLIB_CPPWrapper::NodeLoad::u:
            break;
        case mechLIB_CPPWrapper::NodeLoad::v:
            break;
        case mechLIB_CPPWrapper::NodeLoad::a:
            break;
        case mechLIB_CPPWrapper::NodeLoad::f:
            break;
        case mechLIB_CPPWrapper::NodeLoad::none:
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