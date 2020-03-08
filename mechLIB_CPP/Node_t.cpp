#include "pch.h"
#include "Node_t.h"
#include "maf.hpp"
#include "dcm_t.hpp"
#include "integr.h"
#include "Rope_t.h"

Node_t::Node_t(int tCounts, DirectX::SimpleMath::Vector3 coords, DirectX::SimpleMath::Vector3 _radiusPoint,
	NodeFreedom _freedom, NodeLoad _LoadType, int _ID, std::vector<int> _Neigs)
{
	ID = _ID;
	freedom = _freedom;
	LoadType = _LoadType;
	Neigs = _Neigs;
	F.reserve(tCounts);
	deriv.reserve(tCounts);
	/*for (int i = 0; i < tCounts; i++)
	{
		F.push_back(xyz_t());
		deriv.push_back(deriv_t());
	}*/
	deriv[0].p = coords;
	deriv[0].a.y = _g;
	deriv[0].v.y = _g * 5E-06f;
	radiusPoint = _radiusPoint;
}
void Node_t::CalcAccel(int t, float m)
{
	if (LoadType != NodeLoad::p)
	{
		deriv[t].a.x = F[t].x / m;
		deriv[t].a.y = F[t].y / m;
		//deriv[t].a.z = F[t].z / m;//has to be different
	}

}
void Node_t::GetForces(Rope_t* rope, int t, float m, float c)
{
	//gravity force
	F[t].y += m * _g;
	//damping force
	F[t].x += 0 - (c * deriv[t - 1].v.x);
	F[t].y += 0 - (c * deriv[t - 1].v.y);
	/*getting element forces*/
	for (auto neigNode : Neigs)
	{
		//push it to this force pull
		F[t] += rope->GetElemRef(ID, neigNode)->F[t];
	}
}
void Node_t::GetPhysicParam(Rope_t* rope, int t, float Re, float& m, float& c)
{
	for (auto neigNode : Neigs)
	{
		float mElem = 0;
		float cElem = 0;
		rope->GetElemRef(ID, neigNode)->GetPhysicParam(rope, t, Re, mElem, cElem);
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