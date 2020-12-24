#include "Node_t.h"
#include "maf.hpp"
#include "dcm_t.hpp"
#include "integr.h"
#include "Rope_t.h"
#include "omp.h"

using namespace DirectX::SimpleMath;

Node_t::Node_t() : freedom(), LoadType(), Neigs(0)
{
}
Node_t::~Node_t()
{
}
void Node_t::init(size_t tCounts, Vector3 coords,
	NodeFreedom _freedom, mechLIB::DerivativesEnum _LoadType, std::vector<Element_t*> _Neigs)
{
	freedom = _freedom;
	LoadType = _LoadType;
	Neigs = _Neigs;
	Derivatives = std::vector<DerivativesContainer>(tCounts);
	Derivatives[0].p = coords;
}
void Node_t::CalcAccel(size_t t, float m)
{
	if (LoadType != mechLIB::DerivativesEnum::p)
	{
		Derivatives[t].a.x = Derivatives[t].F.x / m;
		Derivatives[t].a.y = Derivatives[t].F.y / m;
		//deriv[t].a.z = F[t].z / m;//has to be different
	}

}
void Node_t::GetForces(size_t t, float m)
{
	//gravity force
	Derivatives[t].F.y += m * _g;
	/*getting element forces*/
	float x = 0.0, y = 0.0, z = 0.0;
	for (auto element : Neigs)
	{
		//push it to this force pull
		element->CalcForce(this, t, 0, 0, 0);
		x += element->F[t].x;
		y += element->F[t].y;
		z += element->F[t].z;
	}
	Derivatives[t].F.x += x;
	Derivatives[t].F.y += y;
	Derivatives[t].F.z += z;
}
void Node_t::GetPhysicParam(size_t t, float Re, float& m)
{
	for (auto element : Neigs)
	{
		float mElem = 0;
		element->GetPhysicParam(t, Re, mElem);
		m += mElem;
	}
	if (m <= 0)
	{
		throw "Calculated mass of node can't be eaqul to zero";
	}
}
void Node_t::Integrate(size_t now, size_t before, float dt)
{
	Integr::EulerExpl(LoadType, Derivatives[now], Derivatives[before], Derivatives[0], dt);
}
