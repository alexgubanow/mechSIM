#pragma once
#include "../mechLIB_CPPWrapper/NodeLoad.h"
#include "../mechLIB_CPPWrapper/NodeFreedom.h"
#include <d3d11_1.h>
#include <DirectXMath.h>
#include "SimpleMath.h"
#include <vector>
class Rope_t;
class Element_t;
class Node_t
{
public:
	mechLIB_CPPWrapper::NodeFreedom freedom;
	mechLIB_CPPWrapper::NodeLoad LoadType;
	std::vector<DirectX::SimpleMath::Vector3> p;
	std::vector<DirectX::SimpleMath::Vector3> u;
	std::vector<DirectX::SimpleMath::Vector3> v;
	std::vector<DirectX::SimpleMath::Vector3> a;
	std::vector<DirectX::SimpleMath::Vector3> F;
	std::vector<Element_t*> Neigs;
	//int ID;
	DirectX::SimpleMath::Vector3 radiusPoint;
	Node_t();
	~Node_t();
	void init(int tCounts, DirectX::SimpleMath::Vector3 coords, DirectX::SimpleMath::Vector3 _radiusPoint,
		mechLIB_CPPWrapper::NodeFreedom _freedom, mechLIB_CPPWrapper::NodeLoad _LoadType, std::vector<Element_t *> _Neigs);
	void CalcAccel(int t, float m);
	void GetForces(int t, float m, float c);
	void GetPhysicParam(int t, float Re, float& m, float& c);
	void Integrate(int now, int before, float dt);
	//float w = 300;
   //float n = 1f;
};

