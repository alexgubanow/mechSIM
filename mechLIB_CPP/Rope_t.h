﻿#pragma once
#include <vector>
#include "../mechLIB_CPPWrapper/props_t.h"
#include "Node_t.h"
#include "Element_t.h"
#include <d3d11_1.h>
#include <DirectXMath.h>
#include "SimpleMath.h"

class Node_t;
class Element_t;

class Rope_t
{
public:
	std::vector<Node_t> Nodes;
	int NodesSize;
	std::vector<Element_t> Elements;
	int ElementsSize;
	std::vector<float> L;
	Rope_t() : Nodes(0), NodesSize(0), Elements(0), ElementsSize(0), L(0)
	{
	}
	~Rope_t()
	{
	}
	void init(mechLIB_CPPWrapper::props_t* props);
	void SetupNodesPositions(mechLIB_CPPWrapper::props_t* props);
	void SetupNodesPositions(mechLIB_CPPWrapper::props_t* props, DirectX::SimpleMath::Vector3 startCoord, DirectX::SimpleMath::Vector3 endCoord);
	void EvalElements(mechLIB_CPPWrapper::props_t* props);
	void StepOverElems(int t, float Re, float bloodV, float bloodP);
	void StepOverNodes(int t, float Re, float dt);

};