#pragma once
#include "NodeLoad.h"
#include "NodeFreedom.h"
#include <d3d11_1.h>
#include <DirectXMath.h>
#include "SimpleMath.h"
#include <vector>
class Rope_t;
class Element_t;
class Node_t
{
public:
	NodeFreedom freedom;
	NodeLoad LoadType;
	std::vector<DirectX::SimpleMath::Vector3> p;
	std::vector<DirectX::SimpleMath::Vector3> u;
	std::vector<DirectX::SimpleMath::Vector3> v;
	std::vector<DirectX::SimpleMath::Vector3> a;
	std::vector<DirectX::SimpleMath::Vector3> F;
	std::vector<Element_t*> Neigs;
	Node_t();
	~Node_t();
	void init(size_t tCounts, DirectX::SimpleMath::Vector3 coords, NodeFreedom _freedom, 
		NodeLoad _LoadType, std::vector<Element_t*> _Neigs);
	void CalcAccel(size_t t, float m);
	void GetForces(size_t t, float m, float c);
	void GetPhysicParam(size_t t, float Re, float& m, float& c);
	void Integrate(size_t now, size_t before, float dt);
	//float w = 300;
   //float n = 1f;
};

