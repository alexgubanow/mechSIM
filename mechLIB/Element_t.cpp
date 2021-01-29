#include "Rope_t.h"
#include "Element_t.h"
#include "dcm_t.hpp"
#define _USE_MATH_DEFINES
#include <math.h>
#include "maf.hpp"

using namespace DirectX::SimpleMath;
using namespace mechLIB;

void Element_t::init(Node_t* _n1, Node_t* _n2, ModelPropertiesNative* _props)
{
	modelProperties = _props;
	L = std::vector<float>(modelProperties->Counts);
	F = std::vector<Vector3>(modelProperties->Counts);
	n1 = _n1;
	n2 = _n2;
}

void Element_t::CalcForce(Node_t* baseNode, size_t t, float Re, float bloodV, float bloodP)
{
	//getting length of link by measure between coords
	//L[t] = 0;
	switch (modelProperties->PhysicalModel)
	{
	case PhysicalModelEnum::hook:
		L[t] = GetOwnLength(0);
		break;
	case PhysicalModelEnum::hookGeomNon:
		L[t] = GetOwnLength(t - 1);
		break;
	case PhysicalModelEnum::mooneyRiv:
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
	//getting DCM for this link
	dcm_t dcm(oppositeNode->Derivatives[t - 1].p - baseNode->Derivatives[t - 1].p, oppositeNode->Derivatives[t - 1].p.Cross(baseNode->Derivatives[t - 1].p));
	Vector3 forceInLocal;
	GetFn(t, dcm.ToLoc(baseNode->Derivatives[t - 1].u) - dcm.ToLoc(oppositeNode->Derivatives[t - 1].u), forceInLocal);
	//GetPressureForce(t, bloodP, L[t]);
	//GetDragForce(t, Re, bloodV, L);
	if (modelProperties)
	{
		forceInLocal.z += -(modelProperties->MaxU);
	}
	dcm.ToGlob(forceInLocal, F[t]);
}


void Element_t::GetPhysicParam(size_t t, float Re, float& m)
{
	float len = GetOwnLength(t);
	m += modelProperties->ro * modelProperties->A * len;
	//if (Re > 0)
	//{
	//	//calc h of fluid on rod
	//	float thFluid = (rP1[t - 1].z * 2) / sqrtf(Re);
	//	//calc mass of fluid on rod
	//	//add mass of this fluid to mass of rod
	//	m += (float)M_PI * len * (maf::P2(rP1[t - 1].z + thFluid) -
	//		maf::P2(rP1[t - 1].z)) * 1060;
	//}
}

float Element_t::GetOwnLength(size_t t)
{
	float len = Vector3::Distance(n1->Derivatives[t].p, n2->Derivatives[t].p);
	if (len == 0)
	{
		throw "Calculated length of element can't be eaqul to zero";
	}
	return len;
}

void Element_t::GetFn(size_t t, const Vector3& deltaL, Vector3& force)
{
	switch (modelProperties->PhysicalModel)
	{
	case PhysicalModelEnum::hook:
		calcHookFn(force, L[0], deltaL);
		break;
	case PhysicalModelEnum::hookGeomNon:
		calcHookFn(force, L[t], deltaL);
		break;
	case PhysicalModelEnum::mooneyRiv:
		calcMooneyRivlinFn(force, L[t], deltaL);
		break;
	default:
		throw "unexpected behavior";
	}
}

void Element_t::calcHookFn(Vector3& Fn, float oldL, const Vector3& deltaL)
{
	Fn.x = 0 - (modelProperties->E * modelProperties->A / oldL * deltaL.x);
	//Fn[(int)C.y] = 12f * E * I / maf.P3(oldL2) * oldUy2;
}

void Element_t::calcMooneyRivlinFn(Vector3& Fn, float oldL, const Vector3& deltaL)
{
	float lamdax = (deltaL.x / oldL) + 1;
	const float C10 = 22956961.3f;
	const float C01 = -23512872.8f;
	float sigma = 2 * C10 * (lamdax - (1 / maf::P2(lamdax))) + 2 * C01 * (1 - (1 / maf::P3(lamdax)));
	Fn.x = modelProperties->A * sigma;
}

void Element_t::GetPressureForce(size_t t, float bloodP, float L)
{
	//float Fpress = bloodP * radiusPoint.z * 2 * L;
	//F[t].y = -1E-08f;
}

void Element_t::GetDragForce(size_t t, float Re, float v, float L, Vector3& force)
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
