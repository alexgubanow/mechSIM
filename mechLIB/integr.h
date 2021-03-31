#pragma once
#include "maf.hpp"
#include "DerivativesEnum.h"
#include "DerivativesContainer.h"
#include "IntegrationSchemesEnum.h"

const float epsi = 0.193183328f;
const float oneMinusTwoEpsi = 1 - 2 * 0.193183328f;

class Integr
{
private:
	static void EulerExpl(DerivativesContainer& now, const DerivativesContainer& before, float dt, float m)
	{
		now.a = now.F / m;
		now.v = before.v + now.a * dt;
		now.p = before.p + before.v * dt;
	};
	static void SymplecticEuler(DerivativesContainer& now, const DerivativesContainer& before, float dt, float m)
	{
		now.a = now.F / m;
		now.v = before.v + now.a * dt;
		now.p = before.p + now.v * dt;
	};
	static void Verlet(DerivativesContainer& now, const DerivativesContainer& before, float dt, float m)
	{
		now.a = now.F / m;
		now.v = before.v + hlf * (now.a + before.a) * dt;
		now.p = before.p + before.v * dt + hlf * (before.a) * maf::P2(dt);
	}
	static void BABAB(DerivativesContainer& now, const DerivativesContainer& before, float dt)
	{
		DirectX::SimpleMath::Vector3 v1 = before.v + epsi * before.a * dt;
		DirectX::SimpleMath::Vector3 p1 = before.p + (v1 * dt) / 2;
	}

	static void GearP(mechLIB::DerivativesEnum nodeLoad, DerivativesContainer& now, const DerivativesContainer& before,
		float dt, float m)
	{
		/*now.a = now.F / m;
		now.v = before.v + before.a * dt + maf.hlf * before.b * maf.P2(dt);
		now.p = before.p + before.v * dt +
			maf.hlf * before.a * maf.P2(dt) + maf.sxt * before.b * maf.P3(dt);*/
	}

	static void GearC(mechLIB::DerivativesEnum nodeLoad, DerivativesContainer& now, const DerivativesContainer& before,
		float dt, float m)
	{
		/*now.a.X] = before.f.X] / m;
		float[] da = GearDa(now.a], before.a]);
		now.b.X] = before.b.X] + v4 * (da[(int)C.X] / dt);
		now.a.X] = before.a.X] + v3 * da[(int)C.X];
		now.v.X] = before.v.X] + v2 * da[(int)C.X] * dt;
		now.u.X] = before.u.X] + v1 * da[(int)C.X] * maf.P2(dt);
		now.p.X] = before.p.X] + before.u.X];*/
	}

	static DirectX::SimpleMath::Vector3 GearDa(DirectX::SimpleMath::Vector3 aNow, DirectX::SimpleMath::Vector3 aBefore)
	{
		return DirectX::SimpleMath::Vector3(aNow - aBefore);
	}
public:
	static void Integrate(mechLIB::IntegrationSchemesEnum IntegrationSchema, DerivativesContainer& now, const DerivativesContainer& before, float dt, float m)
	{

		switch (IntegrationSchema)
		{
		case mechLIB::IntegrationSchemesEnum::Euler:
			EulerExpl(now, before, dt, m);
			break;
		case mechLIB::IntegrationSchemesEnum::SymplecticEuler:
			SymplecticEuler(now, before, dt, m);
			break;
		case mechLIB::IntegrationSchemesEnum::Verlet:
			Verlet(now, before, dt, m);
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