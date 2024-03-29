#pragma once
#include "DerivativesEnum.h"
#include "NodeFreedom.h"
#include <d3d11_1.h>
#include "SimpleMath.h"
#include <vector>
#include "DerivativesContainer.h"
#include "IntegrationSchemesEnum.h"

class Rope_t;
class Element_t;

class Node_t
{
public:
	float m;
	NodeFreedom freedom;
	mechLIB::DerivativesEnum LoadType;
	std::vector<DerivativesContainer> Derivatives;
	std::vector<Element_t*> Members;
	Node_t();
	~Node_t();
	void init(size_t tCounts, DirectX::SimpleMath::Vector3 coords, NodeFreedom _freedom, 
		mechLIB::DerivativesEnum _LoadType, std::vector<Element_t*> _Members);
	void GetForces(size_t t);
	void GetPhysicParam(size_t t, float Re);
};

