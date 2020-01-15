#pragma once
#include "maf.h"
#include "deriv_t.h"
class Integr
{

private:
	const float v1 = 1 / 12;
	const float v2 = 5 / 12;
	const float v3 = 1;
	const float v4 = 1;
public:
	static void EulerExplForward(deriv_t& now, deriv_t before, float dt)
	{
		now.v.x = before.v.x + now.a.x * dt;
		now.u.x = before.u.x + now.v.x * dt;
		now.p.x = before.p.x + now.u.x;

		now.v.y = before.v.y + now.a.y * dt;
		now.u.y = before.u.y + now.v.y * dt;
		now.p.y = before.p.y + now.u.y;

		now.v.z = before.v.z + now.a.z * dt;
		now.u.z = before.u.z + now.v.z * dt;
		now.p.z = before.p.z + now.u.z;
	}
	static void EulerExplBackward(deriv_t& now, deriv_t before, float dt)
	{
		now.u.x = now.p.x - before.p.x;
		now.v.x = (now.u.x - before.u.x) / dt;
		now.a.x = (now.v.x - before.v.x) / dt;

		now.u.y = now.p.y - before.p.y;
		now.v.y = (now.u.y - before.u.y) / dt;
		now.a.y = (now.v.y - before.v.y) / dt;

		now.u.z = now.p.z - before.p.z;
		now.v.z = (now.u.z - before.u.z) / dt;
		now.a.z = (now.v.z - before.v.z) / dt;
	}
	static void VerletForward(deriv_t& now, deriv_t before, float dt)
	{
		now.v.x = before.v.x + (hlf * (before.a.x + now.a.x)) * dt;
		now.u.x = before.u.x + now.v.x * dt + (hlf * before.a.x * maf::P2(dt));
		now.p.x = before.p.x + now.u.x;

		now.v.y = before.v.y + (hlf * (before.a.y + now.a.y)) * dt;
		now.u.y = before.u.y + now.v.y * dt + (hlf * before.a.y * maf::P2(dt));
		now.p.y = before.p.y + now.u.y;

		now.v.z = before.v.z + (hlf * (before.a.z + now.a.z)) * dt;
		now.u.z = before.u.z + now.v.z * dt + (hlf * before.a.z * maf::P2(dt));
		now.p.z = before.p.z + now.u.z;
	};
};