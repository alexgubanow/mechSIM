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
    static void EulerExpl(NodeLoad &nodeLoad, deriv_t &now, deriv_t &before, deriv_t &zero, float dt)
    {
        now.v.x = before.v.x + before.a.x * dt;
        now.v.y = before.v.y + before.a.y * dt;
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