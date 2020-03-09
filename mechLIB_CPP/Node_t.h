#pragma once
#include "../mechLIB_CPPWrapper/NodeLoad.h"
#include "../mechLIB_CPPWrapper/NodeFreedom.h"
#include <d3d11_1.h>
#include <DirectXMath.h>
#include "SimpleMath.h"
#include <vector>
#include "deriv_t.hpp"
class Rope_t;
class Node_t
{
public:
	mechLIB_CPPWrapper::NodeFreedom freedom;
	mechLIB_CPPWrapper::NodeLoad LoadType;
	std::vector<deriv_t> deriv;
	std::vector<DirectX::SimpleMath::Vector3> F;
	std::vector<int> Neigs;
	size_t NeigsSize;
	int ID;
	DirectX::SimpleMath::Vector3 radiusPoint;
	Node_t();
	~Node_t();
	void init(int tCounts, DirectX::SimpleMath::Vector3 coords, DirectX::SimpleMath::Vector3 _radiusPoint,
		mechLIB_CPPWrapper::NodeFreedom _freedom, mechLIB_CPPWrapper::NodeLoad _LoadType, int _ID, std::vector<int> _Neigs, size_t _NeigsSize);
	void CalcAccel(int t, float m);
	void GetForces(Rope_t* model, int t, float m, float c);
	void GetPhysicParam(Rope_t* rope, int t, float Re, float& m, float& c);
	void Integrate(int now, int before, float dt);
	//float w = 300;
   //float n = 1f;
};

