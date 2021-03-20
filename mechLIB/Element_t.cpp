#include "Rope_t.h"
#include "Element_t.h"
#include "dcm_t.hpp"
//#define _USE_MATH_DEFINES
#include <math.h>
#include "maf.hpp"

using namespace DirectX::SimpleMath;
using namespace mechLIB;
const float C10 = 22956961.3f;
const float C01 = -23512872.8f;

void Element_t::init(Node_t* _n1, Node_t* _n2, ModelPropertiesNative* _props)
{
	modelProperties = _props;
	n1 = _n1;
	n2 = _n2;
	F = std::vector<Vector3>(modelProperties->Counts);
	L = std::vector<float>(modelProperties->Counts);
	L[0] = GetOwnLength(0);
}
void Element_t::GetForces(size_t t, float Re, float bloodV, float bloodP)
{
	float currLength = GetOwnLength(t - 1);
	float oldLength = modelProperties->PhysicalModel == PhysicalModelEnum::hook ? L[0] : currLength;
	float deltaU = currLength - L[0];
	F[t] = GetFn(oldLength, deltaU);
}
void Element_t::GetPhysicParam(size_t t, float Re, float& m)
{
	m += modelProperties->ro * modelProperties->A * L[0];
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
Vector3 Element_t::GetFn(float oldL, float deltaL)
{
	switch (modelProperties->PhysicalModel)
	{
	case PhysicalModelEnum::hook:
	case PhysicalModelEnum::hookGeomNon:
		return Vector3((modelProperties->E * modelProperties->A / oldL) * deltaL, 0, 0);
	case PhysicalModelEnum::mooneyRiv:
	{
		float lamdax = (deltaL / oldL) + 1;
		float sigma = 2 * C10 * (lamdax - (1 / maf::P2(lamdax))) + 2 * C01 * (1 - (1 / maf::P3(lamdax)));
		return Vector3(modelProperties->A * sigma, 0, 0);
	}
	default:
		throw "unexpected behavior";
	}
}
Vector3 Element_t::GetPressureForce(size_t t, float bloodP, float L)
{
	return Vector3(0, 0, 0);
	//float Fpress = bloodP * radiusPoint.z * 2 * L;
	//F[t].y = -1E-08f;
}
Vector3 Element_t::GetDragForce(size_t t, float Re, float v, float L)
{
	return Vector3(0, 0, 0);
	/*float Awet = 2 * (float)M_PI * radiusPoint.z * L;
	float bloodViscosity = 3E-3f;
	float Be = 0.9f;
	float Cd = (Awet / A) * (Be / Re);
	float Fdrag = hlf * 1060 * maf::P2(v) * Cd * A;
	//is it has to be applied only on moving direction??
	force.y += Fdrag;
	force.z += Fdrag;*/
}
