#pragma once
#include <vector>
#include "ModelPropertiesNative.h"
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
	size_t NodesSize;
	std::vector<Element_t> Elements;
	size_t ElementsSize;
	std::vector<float> L;
	Rope_t() : Nodes(0), NodesSize(0), Elements(0), ElementsSize(0), L(0)
	{
	}
	void init(ModelPropertiesNative* props);
	void SetupNodesPositions(ModelPropertiesNative* props);
	void SetupNodesPositions(ModelPropertiesNative* props, DirectX::SimpleMath::Vector3 startCoord, DirectX::SimpleMath::Vector3 endCoord);
	void EvalElements(ModelPropertiesNative* props);
	void StepOverNodes(size_t t, float Re, float dt);
};