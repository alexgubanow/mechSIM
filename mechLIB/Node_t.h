#pragma once
#include "DerivativesEnum.h"
#include "NodeFreedom.h"
#include <d3d11_1.h>
#include "SimpleMath.h"
#include <vector>
#include "DerivativesContainer.h"

class Rope_t;
class Element_t;

class Node_t
{
public:
	NodeFreedom freedom;
	mechLIB::DerivativesEnum LoadType;
	std::vector<DerivativesContainer> Derivatives;
	std::vector<Element_t*> Neigs;
	Node_t();
	~Node_t();
	void init(size_t tCounts, DirectX::SimpleMath::Vector3 coords, NodeFreedom _freedom, 
		mechLIB::DerivativesEnum _LoadType, std::vector<Element_t*> _Neigs);
	void CalcAccel(size_t t, float m);
	void GetForces(size_t t, float m, float c);
	void GetPhysicParam(size_t t, float Re, float& m, float& c);
	void Integrate(size_t now, size_t before, float dt);
};

