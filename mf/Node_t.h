#pragma once
#include "xyz_t.h"
#include "NodeLoad.h"
#include "NodeFreedom.h"
#include <vector>
#include "deriv_t.h"
#include "Rope_t.h"

class Node_t
{
public:
	NodeFreedom freedom;
	NodeLoad LoadType;
	std::vector<deriv_t> deriv;
	std::vector<xyz_t> F;
	std::vector<int> Neigs;
	int ID;
	xyz_t radiusPoint;
	Node_t(int tCounts, xyz_t coords, xyz_t _radiusPoint, NodeFreedom _freedom, NodeLoad _LoadType, int _ID, std::vector<int> _Neigs);
	void CalcAccel(int t, float m);
	void GetForces(Rope_t* model, int t, float m, float c);
	void GetPhysicParam(Rope_t* rope, int t, float Re, float& m, float& c);
	void Integrate(int now, int before, float dt);
	//float w = 300;
   //float n = 1f;
};

