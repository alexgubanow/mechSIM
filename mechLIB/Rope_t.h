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
	static void SetupNodesPositions(std::vector<Node_t>& Nodes, std::vector<Element_t>& Elements,
		DirectX::SimpleMath::Vector3 startCoord, DirectX::SimpleMath::Vector3 endCoord, const ModelPropertiesNative& phProps)
	{
		size_t lastNode = Nodes.size() - 1;
		float elementLength = phProps.L / lastNode;
		float cosA = endCoord.x / phProps.L;
		float sinA = endCoord.y / phProps.L;
		Nodes[0].init(phProps.Counts, startCoord,
			NodeFreedom::xyz, mechLIB::DerivativesEnum::DerivativesEnumMAX, std::vector<Element_t*>{ &Elements[0] });
		for (size_t i = 1; i < lastNode; i++)
		{
			DirectX::SimpleMath::Vector3 flatC(i * elementLength, 0, 0);
			DirectX::SimpleMath::Vector3 coords(
				(flatC.x - startCoord.x) * cosA - (flatC.y - startCoord.y) * sinA + startCoord.x,
				(flatC.x - startCoord.x) * sinA + (flatC.y - startCoord.y) * cosA + startCoord.y, 0
			);
			Nodes[i].init(phProps.Counts, coords,
				NodeFreedom::xyz, mechLIB::DerivativesEnum::DerivativesEnumMAX, std::vector<Element_t*>{ &Elements[i - 1], & Elements[i] });
		}
		Nodes[lastNode].init(phProps.Counts, endCoord,
			NodeFreedom::xyz, mechLIB::DerivativesEnum::DerivativesEnumMAX, std::vector<Element_t*>{ &Elements[lastNode - 1] });
	}
};