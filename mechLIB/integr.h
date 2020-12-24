#pragma once
#include "maf.hpp"
#include "DerivativesEnum.h"
#include "DerivativesContainer.h"
#include "IntegrationSchemesEnum.h"

class Integr
{
private:
	const float v1 = 1.0f / 12.0f;
	const float v2 = 5.0f / 12.0f;
	const float v3 = 1.0f;
	const float v4 = 1.0f;
	static void EulerExpl(mechLIB::DerivativesEnum nodeLoad, DerivativesContainer& now, const DerivativesContainer& before,
		const DerivativesContainer& zero, float dt)
	{
		now.v.x = before.v.x + before.a.x * dt;
		now.v.y = before.v.y + before.a.y * dt;
		switch (nodeLoad)
		{
		case mechLIB::DerivativesEnum::p:
			break;
		case mechLIB::DerivativesEnum::u:
			break;
		case mechLIB::DerivativesEnum::v:
			break;
		case mechLIB::DerivativesEnum::a:
			break;
		case mechLIB::DerivativesEnum::f:
			break;
		case mechLIB::DerivativesEnum::DerivativesEnumMAX:
			now.u.x = now.v.x * dt;
			now.u.y = now.v.y * dt;
			now.p.x = zero.p.x + now.u.x;
			now.p.y = zero.p.y + now.u.y;
			break;
		default:
			break;
		}
	};
	static void Verlet(mechLIB::DerivativesEnum nodeLoad, DerivativesContainer& now, const DerivativesContainer& before,
		const DerivativesContainer& zero, float dt)
	{
		now.v.x = before.v.x + hlf * (before.a.x + now.a.x) * dt;
		now.v.y = before.v.y + hlf * (before.a.y + now.a.y) * dt;
		switch (nodeLoad)
		{
		case mechLIB::DerivativesEnum::p:
			break;
		case mechLIB::DerivativesEnum::u:
			break;
		case mechLIB::DerivativesEnum::v:
			break;
		case mechLIB::DerivativesEnum::a:
			break;
		case mechLIB::DerivativesEnum::f:
			break;
		case mechLIB::DerivativesEnum::DerivativesEnumMAX:
			now.u.x = now.v.x * dt + hlf * (before.a.x * maf::P2(dt));
			now.u.y = now.v.y * dt + hlf * (before.a.y * maf::P2(dt));
			now.p.x = zero.p.x + now.u.x;
			now.p.y = zero.p.y + now.u.y;
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
public:
	static void Integrate(mechLIB::IntegrationSchemesEnum IntegrationSchema, mechLIB::DerivativesEnum nodeLoad, DerivativesContainer& now, const DerivativesContainer& before,
		const DerivativesContainer& zero, float dt)
	{

		switch (IntegrationSchema)
		{
		case mechLIB::IntegrationSchemesEnum::Euler:
			EulerExpl(nodeLoad, now, before, zero, dt);
			break;
		case mechLIB::IntegrationSchemesEnum::Verlet:
			Verlet(nodeLoad, now, before, zero, dt);
			break;
		case mechLIB::IntegrationSchemesEnum::GearPC:
			break;
		case mechLIB::IntegrationSchemesEnum::maxIntegrationSchemesEnum:
			throw "Unexpected IntegrationSchemesEnum";
			break;
		default:
			throw "Unexpected IntegrationSchemesEnum";
			break;
		}
	}
};