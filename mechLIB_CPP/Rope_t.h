#pragma once
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
	std::vector<Node_t*> Nodes;
	std::vector<Element_t*> Elements;
	float* L;
	Rope_t(mechLIB_CPPWrapper::props_t* Props)
	{
		L = new float[Props->Counts];
		L[0] = Props->L;
		Nodes.reserve(Props->nodes);
		Elements.reserve(Props->nodes - 1);
		SetupNodesPositions(Props);
		EvalElements(Props);
	};
	Rope_t(mechLIB_CPPWrapper::props_t* Props, DirectX::SimpleMath::Vector3 startCoord, DirectX::SimpleMath::Vector3 endCoord)
	{
		L = new float[Props->Counts];
		L[0] = Props->L;
		Nodes.reserve(Props->nodes);
		Elements.reserve(Props->nodes - 1);
		SetupNodesPositions(Props, startCoord, endCoord);
		EvalElements(Props);
	};
	~Rope_t()
	{
		delete L;
		for (auto itr : Elements)
		{
			delete itr;
		}
	}
	void SetupNodesPositions(mechLIB_CPPWrapper::props_t* props);
	void SetupNodesPositions(mechLIB_CPPWrapper::props_t* props, DirectX::SimpleMath::Vector3 startCoord, DirectX::SimpleMath::Vector3 endCoord);
	void EvalElements(mechLIB_CPPWrapper::props_t* props)
	{
		for (size_t i = 0; i < Elements.capacity(); i++)
		{
			Elements.push_back(new Element_t(i, i + 1, props->Counts, i, props));
		}
	};

	Node_t* GetNodeRef(int id)
	{
		if (id < 0)
		{
			throw std::exception("Not valid ID");
		}
		return Nodes[id];
	};
	Element_t* GetElemRef(int baseNode, int neigNode)
	{
		for (auto elem : Elements)
		{
			if (elem->IsMyNode(baseNode) && elem->IsMyNode(neigNode))
			{
				return elem;
			}
		}
		throw std::exception("Element not found");
	};
	void StepOverElems(int t, float Re, float bloodV, float bloodP);
	void StepOverNodes(int t, float Re, float dt);

};