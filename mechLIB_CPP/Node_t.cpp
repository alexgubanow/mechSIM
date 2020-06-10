#include "pch.h"
#include "Node_t.h"
#include "maf.hpp"
#include "dcm_t.hpp"
#include "integr.h"
#include "Rope_t.h"

Node_t::Node_t() : freedom(), LoadType(), p(0), u(0), v(0), a(0), F(0), Neigs(0), radiusPoint()
{
}
Node_t::~Node_t()
{
	/*delete[] deriv;
	delete[] F;*/
}
void Node_t::init(size_t tCounts, DirectX::SimpleMath::Vector3 coords, DirectX::SimpleMath::Vector3 _radiusPoint,
	mechLIB_CPP::NodeFreedom _freedom, mechLIB_CPP::NodeLoad _LoadType, std::vector<Element_t*> _Neigs)
{
	freedom = _freedom;
	LoadType = _LoadType;
	Neigs = _Neigs;
	F = std::vector<DirectX::SimpleMath::Vector3>(tCounts);
	p = std::vector<DirectX::SimpleMath::Vector3>(tCounts);
	u = std::vector<DirectX::SimpleMath::Vector3>(tCounts);
	v = std::vector<DirectX::SimpleMath::Vector3>(tCounts);
	a = std::vector<DirectX::SimpleMath::Vector3>(tCounts);
	p[0] = coords;
	radiusPoint = _radiusPoint;
}
void Node_t::CalcAccel(size_t t, float m)
{
	if (LoadType != mechLIB_CPP::NodeLoad::p)
	{
		a[t].x = F[t].x / m;
		a[t].y = F[t].y / m;
		//deriv[t].a.z = F[t].z / m;//has to be different
	}

}
void Node_t::GetForces(size_t t, float m, float c)
{
	//gravity force
	F[t].y += m * _g;
	//damping force
	F[t].x += 0 - (c * v[t - 1].x);
	F[t].y += 0 - (c * v[t - 1].y);
	/*getting element forces*/
	for (auto element : Neigs)
	{
		//push it to this force pull
		F[t] += element->F[t];
	}
}
void Node_t::GetPhysicParam(size_t t, float Re, float& m, float& c)
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
void Node_t::Integrate(size_t now, size_t before, float dt)
{
	Integr::EulerExpl(LoadType, p[now], u[now], v[now], a[now], p[before], u[before], v[before], a[before], p[0], dt);
}