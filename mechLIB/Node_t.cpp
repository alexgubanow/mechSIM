#include "Node_t.h"
#include "maf.hpp"
#include "dcm_t.hpp"
#include "integr.h"
#include "Rope_t.h"
#include "omp.h"

using namespace DirectX::SimpleMath;
using namespace mechLIB;

Node_t::Node_t() : freedom(), LoadType(), Members(0)
{
}
Node_t::~Node_t()
{
}
void Node_t::init(size_t tCounts, Vector3 coords,
	NodeFreedom _freedom, DerivativesEnum _LoadType, std::vector<Element_t*> _Members)
{
	freedom = _freedom;
	LoadType = _LoadType;
	Members = _Members;
	Derivatives = std::vector<DerivativesContainer>(tCounts);
	Derivatives[0].p = coords;
}
void Node_t::CalcAccel(size_t t, float m)
{
	if (LoadType != DerivativesEnum::p)
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
	for (size_t i = 0; i < Members.size(); i++)
	{
		//push it to this force pull
		Members[i]->CalcForce(this, t, 0, 0, 0);
		x += Members[i]->F[t].x;
		y += Members[i]->F[t].y;
		z += Members[i]->F[t].z;
	}
	Derivatives[t].F.x += x;
	Derivatives[t].F.y += y;
	Derivatives[t].F.z += z;
}
void Node_t::GetPhysicParam(size_t t, float Re, float& m)
{
	for (size_t i = 0; i < Members.size(); i++)
	{
		float mElem = 0;
		Members[i]->GetPhysicParam(t, Re, mElem);
		m += mElem;
	}
	if (m <= 0)
	{
		throw "Calculated mass of node can't be eaqul to zero";
	}
}
void Node_t::Integrate(IntegrationSchemesEnum IntegrationSchema, size_t now, size_t before, float dt, float m)
{
	Integr::Integrate(IntegrationSchema, Derivatives[now], Derivatives[before], dt, m);
}
