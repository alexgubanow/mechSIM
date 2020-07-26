#include "pch.h"
#include "Rope_t.h"
#include "Element_t.h"
#include "dcm_t.hpp"
#define _USE_MATH_DEFINES
#include <math.h>
#include "maf.hpp"

void Element_t::init(Node_t* _n1, Node_t* _n2, mechLIB_CPP::props_t* _props)
{
	props = _props;
	L = std::vector<float>(props->Counts);
	F = std::vector<DirectX::SimpleMath::Vector3>(props->Counts);
	n1 = _n1;
	n2 = _n2;
	radiusPoint = std::vector<DirectX::SimpleMath::Vector3>(props->Counts);
	radiusPoint[0] = DirectX::SimpleMath::Vector3( 0, props->D, 0);
	A = (float)M_PI * maf::P2(props->D);
	I = maf::P3(A) / 12.0f;
}

void Element_t::CalcForce(Node_t* baseNode, size_t t, float Re, float bloodV, float bloodP)
{
	//getting length of link by measure between coords
	//L[t] = 0;
	switch (props->phMod)
	{
	case mechLIB_CPP::PhModels::hook:
		L[t] = GetOwnLength(0);
		break;
	case mechLIB_CPP::PhModels::hookGeomNon:
		L[t] = GetOwnLength(t - 1);
		break;
	case mechLIB_CPP::PhModels::mooneyRiv:
		L[t] = GetOwnLength(t - 1);
		break;
	default:
		throw "unexpected behavior";
	}
	Node_t* oppositeNode = n1;
	if (oppositeNode == baseNode)
	{
		oppositeNode = n2;
	}
	//absolute link in glob coord
	DirectX::SimpleMath::Vector3 absoluteCoords(
		oppositeNode->p[t - 1].x - baseNode->p[t - 1].x,
		oppositeNode->p[t - 1].y - baseNode->p[t - 1].y,
		oppositeNode->p[t - 1].z - baseNode->p[t - 1].z);

	//getting DCM for this link
	dcm_t dcm(absoluteCoords, radiusPoint[t - 1]);
	radiusPoint[t] = radiusPoint[t - 1];
	DirectX::SimpleMath::Vector3 force;
	GetFn(t, dcm.ToLoc(baseNode->u[t - 1]) - dcm.ToLoc(oppositeNode->u[t - 1]), force);
	//GetPressureForce(t, bloodP, L[t]);
	//GetDragForce(t, Re, bloodV, L);

	force.y += props->MaxU * 1E3f;
	dcm.ToGlob(force, F[t]);
}

void Element_t::GetForceForNode(size_t t, Node_t* baseP, DirectX::SimpleMath::Vector3& force)
{
	force = F[t];
	if (n1 != baseP)
	{
		force.x = -force.x;
	}
}

void Element_t::GetPhysicParam(size_t t, float Re, float& m, float& c)
{
	float len = GetOwnLength(t);
	m += props->ro * A * len;
	if (Re > 0)
	{
		//calc h of fluid on rod
		float thFluid = (radiusPoint[t - 1].z * 2) / sqrtf(Re);
		//calc mass of fluid on rod
		//add mass of this fluid to mass of rod
		m += (float)M_PI * len * (maf::P2(radiusPoint[t - 1].z + thFluid) -
			maf::P2(radiusPoint[t - 1].z)) * 1060;
	}
	float alpha = 0 - ((sqrtf(5) * log10f(props->DampRatio)) /
		(sqrtf(maf::P2(log10f(props->DampRatio)) + maf::P2((float)M_PI))));
	float k = (props->E * A) / len;
	c = alpha * sqrtf(m * k);
}

float Element_t::GetOwnLength(size_t t)
{
	float len = DirectX::SimpleMath::Vector3::Distance(n1->p[t], n2->p[t]);
	if (len == 0)
	{
		throw "Calculated length of element can't be eaqul to zero";
	}
	return len;
}

void Element_t::GetFn(size_t t, DirectX::SimpleMath::Vector3 deltaL, DirectX::SimpleMath::Vector3& force)
{
	switch (props->phMod)
	{
	case mechLIB_CPP::PhModels::hook:
		calcHookFn(force, L[0], deltaL);
		break;
	case mechLIB_CPP::PhModels::hookGeomNon:
		calcHookFn(force, L[t], deltaL);
		break;
	case mechLIB_CPP::PhModels::mooneyRiv:
		calcMooneyRivlinFn(force, L[t], deltaL);
		break;
	default:
		throw "unexpected behavior";
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

void Element_t::GetPressureForce(size_t t, float bloodP, float L)
{
	//float Fpress = bloodP * radiusPoint.z * 2 * L;
	//F[t].y = -1E-08f;
}

void Element_t::GetDragForce(size_t t, float Re, float v, float L, DirectX::SimpleMath::Vector3& force)
{
	/*float Awet = 2 * (float)M_PI * radiusPoint.z * L;
	float bloodViscosity = 3E-3f;
	float Be = 0.9f;
	float Cd = (Awet / A) * (Be / Re);
	float Fdrag = hlf * 1060 * maf::P2(v) * Cd * A;
	//is it has to be applied only on moving direction??
	force.y += Fdrag;
	force.z += Fdrag;*/
}
