#include "pch.h"
#include "Node_t.h"
#include "maf.hpp"
#include "dcm_t.hpp"
#include "integr.h"
#include "Rope_t.h"

Node_t::Node_t() : freedom(), LoadType(), deriv(0), F(0), Neigs(0), radiusPoint()
{
}
Node_t::~Node_t()
{
	/*delete[] deriv;
	delete[] F;*/
}
void Node_t::init(int tCounts, DirectX::SimpleMath::Vector3 coords, DirectX::SimpleMath::Vector3 _radiusPoint,
	mechLIB_CPPWrapper::NodeFreedom _freedom, mechLIB_CPPWrapper::NodeLoad _LoadType, std::vector<Element_t *> _Neigs)
{
	freedom = _freedom;
	LoadType = _LoadType;
	Neigs = _Neigs;
	F = std::vector<DirectX::SimpleMath::Vector3>(tCounts);
	deriv = std::vector<deriv_t>(tCounts);
	/*for (int i = 0; i < tCounts; i++)
	{
		deriv[i].a.y = _g;
		deriv[i].v.y = _g * 1E-06f;
	}*/
	deriv[0].p = coords;
	radiusPoint = _radiusPoint;
}
void Node_t::CalcAccel(int t, float m)
{
	if (LoadType != mechLIB_CPPWrapper::NodeLoad::p)
	{
		deriv[t].a.x = F[t].x / m;
		deriv[t].a.y = F[t].y / m;
		//deriv[t].a.z = F[t].z / m;//has to be different
	}

}
void Node_t::GetForces(int t, float m, float c)
{
	//gravity force
	F[t].y += m * _g;
	//damping force
	F[t].x += 0 - (c * deriv[t - 1].v.x);
	F[t].y += 0 - (c * deriv[t - 1].v.y);
	/*getting element forces*/
	for (auto element : Neigs)
	{
		//push it to this force pull
		F[t] += element->F[t];
	}
}
void Node_t::GetPhysicParam(int t, float Re, float& m, float& c)
{
	for (auto element : Neigs)
	{
		float mElem = 0;
		float cElem = 0;
		element->GetPhysicParam(t, Re, mElem, cElem);
		c += cElem;
		m += mElem;
	}
	//m = 1E-04f;
	c /= Neigs.size();
	if (m <= 0)
	{
		throw "Calculated mass of node can't be eaqul to zero";
	}
}
void Node_t::Integrate(int now, int before, float dt)
{
	Integr::EulerExpl(LoadType, deriv[now], deriv[before], deriv[0], dt);
}