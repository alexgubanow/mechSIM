#pragma once
#include "maf.hpp"
#include "deriv_t.hpp"

class Integr
{

private:
	const float v1 = 1 / 12;
	const float v2 = 5 / 12;
	const float v3 = 1;
	const float v4 = 1;
public:
    static void EulerExpl(mechLIB_CPPWrapper::NodeLoad &nodeLoad, deriv_t &now, deriv_t &before, deriv_t &zero, float dt)
    {
        now.v.x = before.v.x + before.a.x * dt;
        now.v.y = before.v.y + before.a.y * dt;
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
        case mechLIB_CPPWrapper::NodeLoad::b:
            break;
        case mechLIB_CPPWrapper::NodeLoad::f:
            break;
        case mechLIB_CPPWrapper::NodeLoad::none:
            now.u.x = now.v.x * dt;
            now.u.y = now.v.y * dt;
            now.p.x = zero.p.x + now.u.x;
            now.p.y = zero.p.y + now.u.y;
            break;
        default:
            break;
        }
    };
};