#pragma once
#include "NodeLoad.h"
#include "NodeFreedom.h"
#include <d3d11_1.h>
#include <DirectXMath.h>
#include "SimpleMath.h"
#include <vector>
#include "deriv_t.hpp"
class Rope_t;
class Node_t
{
public:
	NodeFreedom freedom;
	NodeLoad LoadType;
	std::vector<deriv_t> deriv;
	std::vector<DirectX::SimpleMath::Vector3> F;
	std::vector<int> Neigs;
	int ID;
	DirectX::SimpleMath::Vector3 radiusPoint;
	Node_t(int tCounts, DirectX::SimpleMath::Vector3 coords, DirectX::SimpleMath::Vector3
		_radiusPoint, NodeFreedom _freedom, NodeLoad _LoadType, int _ID, std::vector<int> _Neigs);
	void CalcAccel(int t, float m);
	void GetForces(Rope_t* model, int t, float m, float c);
	void GetPhysicParam(Rope_t* rope, int t, float Re, float& m, float& c);
	void Integrate(int now, int before, float dt);
	//float w = 300;
   //float n = 1f;
};

