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
	Node_t* Nodes;
	int NodesSize;
	Element_t* Elements;
	int ElementsSize;
	float* L;
	Rope_t(mechLIB_CPPWrapper::props_t* props)
	{
		L = new float[props->Counts];
		L[0] = props->L;
		NodesSize = props->nodes;
		ElementsSize = props->nodes - 1;
		Nodes = new Node_t[NodesSize];
		Elements = new Element_t[ElementsSize];
		SetupNodesPositions(props);
		EvalElements(props);
	};
	Rope_t(mechLIB_CPPWrapper::props_t* props, DirectX::SimpleMath::Vector3 startCoord, DirectX::SimpleMath::Vector3 endCoord)
	{
		L = new float[props->Counts];
		L[0] = props->L;
		NodesSize = props->nodes;
		ElementsSize = props->nodes - 1;
		Nodes = new Node_t[NodesSize];
		Elements = new Element_t[ElementsSize];
		SetupNodesPositions(props, startCoord, endCoord);
		EvalElements(props);
	};
	~Rope_t()
	{
		delete L;
		delete[] Nodes;
		delete[] Elements;
	}
	void SetupNodesPositions(mechLIB_CPPWrapper::props_t* props);
	void SetupNodesPositions(mechLIB_CPPWrapper::props_t* props, DirectX::SimpleMath::Vector3 startCoord, DirectX::SimpleMath::Vector3 endCoord);
	void EvalElements(mechLIB_CPPWrapper::props_t* props)
	{
		for (int i = 0; i < ElementsSize; i++)
		{
			Elements[i].init(i, i + 1, props->Counts, i, props);
		}
	};

	Node_t* GetNodeRef(int id)
	{
		if (id < 0)
		{
			throw std::exception("Not valid ID");
		}
		return &Nodes[id];
	};
	Element_t* GetElemRef(int baseNode, int neigNode)
	{
		for (int i = 0; i < ElementsSize; i++)
		{
			if (Elements[i].IsMyNode(baseNode) && Elements[i].IsMyNode(neigNode))
			{
				return &Elements[i];
			}
		}
		throw std::exception("Element not found");
	};
	void StepOverElems(int t, float Re, float bloodV, float bloodP);
	void StepOverNodes(int t, float Re, float dt);

};