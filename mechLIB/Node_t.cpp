#include "Node_t.h"
#include "maf.hpp"
#include "dcm_t.hpp"
#include "integr.h"
#include "Rope_t.h"
#include "omp.h"

using namespace DirectX::SimpleMath;
using namespace mechLIB;

Node_t::Node_t() : m(0), freedom(), LoadType(), Members(0)
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
	Derivatives[1].p = coords;
}
void Node_t::GetForces(size_t t)
{
	//gravity force
	/*if (modelProperties->isGravityEnabled)
	{
		Derivatives[t].F.y += m * _g;
	}*/
	//Derivatives[t].F.y += m * _g;
	//pressure
	//Derivatives[t].F.y += -0.1;
	for (size_t i = 0; i < Members.size(); i++)
	{
		Node_t* oppositeNode = Members[i]->n1;
		if (oppositeNode == this)
		{
			oppositeNode = Members[i]->n2;
		}
		//getting DCM for this link
		dcm_t dcm(oppositeNode->Derivatives[t - 1].p - Derivatives[t - 1].p,
			oppositeNode->Derivatives[t - 1].p.Cross(Derivatives[t - 1].p));
		Derivatives[t].F += dcm.ToGlob(Members[i]->F[t] / 2);
	}
	//damping force
	Derivatives[t].F += -( (1-0.001f) * Derivatives[t - 1].v);
}
void Node_t::GetPhysicParam(size_t t, float Re)
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