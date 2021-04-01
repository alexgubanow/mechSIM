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
	ModelPropertiesNative* RopeProperties;
public:
	std::vector<Node_t> Nodes;
	std::vector<Element_t> Elements;
	std::vector<float> L;
	Rope_t(ModelPropertiesNative* props);
	void SetupNodesPositions();
	void SetupNodesPositions(DirectX::SimpleMath::Vector3 startCoord, DirectX::SimpleMath::Vector3 endCoord);
	void EvalElements();
	void StepOverNodes(size_t t, float Re);
	void Integrate(size_t t, float dt);
	void StepOverElements(size_t t, float Re);
};