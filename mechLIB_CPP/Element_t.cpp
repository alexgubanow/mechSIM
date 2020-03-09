#include "pch.h"
#include "Rope_t.h"
#include "Element_t.h"
#include "dcm_t.hpp"
#define _USE_MATH_DEFINES
#include <math.h>
#include "maf.hpp"
#include "coords.hpp"

void Element_t::init(int _n1, int _n2, int Counts, int _ID, mechLIB_CPPWrapper::props_t* _props)
{
	props = _props;
	L = std::vector<float>(props->Counts);
	F = std::vector<DirectX::SimpleMath::Vector3>(props->Counts);
	ID = _ID;
	n1 = _n1;
	n2 = _n2;
	radiusPoint.z = props->D;
	A = (float)M_PI * maf::P2(props->D);
	I = maf::P3(A) / 12.0f;
}

void Element_t::CalcForce(Rope_t* rope, int t, float Re, float bloodV, float bloodP)
{
	//getting length of link by measure between coords
	L[t] = 0;
	switch (props->phMod)
	{
	case mechLIB_CPPWrapper::PhModels::hook:
		L[t] = GetOwnLength(rope, 0);
		break;
	case mechLIB_CPPWrapper::PhModels::hookGeomNon:
		L[t] = GetOwnLength(rope, t - 1);
		break;
	case mechLIB_CPPWrapper::PhModels::mooneyRiv:
		L[t] = GetOwnLength(rope, t - 1);
		break;
	default:
		throw std::exception("unexpected behavior");
	}
	//getting position of link according base point
	DirectX::SimpleMath::Vector3 LinkPos = 
		rope->GetNodeRef(n1)->deriv[t - 1].p - rope->GetNodeRef(n2)->deriv[t - 1].p;
	//getting DCM for this link
	dcm_t dcm(LinkPos, radiusPoint);
	//convert base point Ux to local coords
	DirectX::SimpleMath::Vector3 lBpUx;
	dcm.ToLoc(rope->GetNodeRef(n1)->deriv[t - 1].u, lBpUx);
	//convert n point Ux to local coords
	DirectX::SimpleMath::Vector3 lNpUx;
	dcm.ToLoc(rope->GetNodeRef(n2)->deriv[t - 1].u, lNpUx);
	//store delta of expansion
	DirectX::SimpleMath::Vector3 deltaL = lBpUx - lNpUx;
	DirectX::SimpleMath::Vector3 force;
	GetFn(t, deltaL, force);
	//GetPressureForce(t, bloodP, L[t]);
	//GetDragForce(t, Re, bloodV, L);

	//force.Y += -1E-07f;
	DirectX::SimpleMath::Vector3 gforce;
	dcm.ToGlob(force, gforce);
	F[t] += gforce;
}

void Element_t::GetPhysicParam(Rope_t* rope, int t, float Re, float& m, float& c)
{
	float len = GetOwnLength(rope, t);
	m += props->ro * A * len;
	if (Re > 0)
	{
		//calc h of fluid on rod
		float thFluid = (radiusPoint.z * 2) / sqrtf(Re);
		//calc mass of fluid on rod
		m += (float)M_PI * len * (maf::P2(radiusPoint.z + thFluid) - maf::P2(radiusPoint.z)) * 1060;
		//add mass of this fluid to mass of rod
	}

	c = props->DampRatio * 2 * sqrtf(m * ((props->E * A) / len));
	if (c <= 0)
	{
		throw std::exception("Calculated damping ratio of element can't be eaqul to zero");
	}

}

float Element_t::GetOwnLength(Rope_t* rope, int t)
{
	float len = crds::GetTotL(rope->GetNodeRef(n1)->deriv[t].p, rope->GetNodeRef(n2)->deriv[t].p);
	if (len == 0)
	{
		throw std::exception("Calculated length of element can't be eaqul to zero");
	}
	return len;
}

void Element_t::GetFn(int t, DirectX::SimpleMath::Vector3 deltaL, DirectX::SimpleMath::Vector3& force)
{
	switch (props->phMod)
	{
	case mechLIB_CPPWrapper::PhModels::hook:
		calcHookFn(force, L[0], deltaL);
		break;
	case mechLIB_CPPWrapper::PhModels::hookGeomNon:
		calcHookFn(force, L[t], deltaL);
		break;
	case mechLIB_CPPWrapper::PhModels::mooneyRiv:
		calcMooneyRivlinFn(force, L[t], deltaL);
		break;
	default:
		throw std::exception("unexpected behavior");
	}
}

void Element_t::calcHookFn(DirectX::SimpleMath::Vector3& Fn, float oldL, DirectX::SimpleMath::Vector3 deltaL)
{
	Fn.x = 0 - (props->E * A / oldL * deltaL.x);
	//Fn[(int)C.y] = 12f * E * I / maf.P3(oldL2) * oldUy2;
}

void Element_t::calcMooneyRivlinFn(DirectX::SimpleMath::Vector3& Fn, float oldL, DirectX::SimpleMath::Vector3 deltaL)
{
	float lamdax = (deltaL.x / oldL) + 1;
	const float C10 = 22956961.3f;
	const float C01 = -23512872.8f;
	float sigma = 2 * C10 * (lamdax - (1 / maf::P2(lamdax))) + 2 * C01 * (1 - (1 / maf::P3(lamdax)));
	Fn.x = 0 - (A * sigma);
}

void Element_t::GetPressureForce(int t, float bloodP, float L)
{
	//float Fpress = bloodP * radiusPoint.z * 2 * L;
	F[t].y = -1E-08f;
}

void Element_t::GetDragForce(int t, float Re, float v, float L, DirectX::SimpleMath::Vector3& force)
{
	float Awet = 2 * (float)M_PI * radiusPoint.z * L;
	float bloodViscosity = 3E-3f;
	float Be = 0.9f;
	float Cd = (Awet / A) * (Be / Re);
	float Fdrag = hlf * 1060 * maf::P2(v) * Cd * A;
	//is it has to be applied only on moving direction??
	force.y += Fdrag;
	force.z += Fdrag;
}
