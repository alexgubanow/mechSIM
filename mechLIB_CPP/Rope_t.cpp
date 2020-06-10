#include "pch.h"
#include "Rope_t.h"
#include <algorithm>    // std::for_each
#include <execution>
#include "maf.hpp"

void Rope_t::init(mechLIB_CPP::props_t* props)
{

	L = std::vector<float>(props->Counts);
	L[0] = props->L;
	NodesSize = props->nodes;
	Nodes = std::vector<Node_t>(NodesSize);
	ElementsSize = props->nodes - 1;
	Elements = std::vector<Element_t>(ElementsSize);
}

void Rope_t::SetupNodesPositions(mechLIB_CPP::props_t* props)
{
	SetupNodesPositions(props, DirectX::SimpleMath::Vector3{ 0,0,0 }, DirectX::SimpleMath::Vector3{ props->L,0,0 });
}

void Rope_t::SetupNodesPositions(mechLIB_CPP::props_t* props, DirectX::SimpleMath::Vector3 startCoord,
	DirectX::SimpleMath::Vector3 endCoord)
{
	int lastNode = NodesSize - 1;
	float dl = props->L / lastNode;
	float cosA = endCoord.x / props->L;
	float sinA = endCoord.y / props->L;
	Nodes[0].init(props->Counts, startCoord,
		mechLIB_CPP::NodeFreedom::xyz, mechLIB_CPP::NodeLoad::none, std::vector<Element_t*>{ &Elements[0] });
	for (size_t i = 1; i < lastNode; i++)
	{
		DirectX::SimpleMath::Vector3 flatC(i * dl, 0, 0);
		DirectX::SimpleMath::Vector3 coords(

			(flatC.x - startCoord.x) * cosA - (flatC.y - startCoord.y) * sinA + startCoord.x,
			(flatC.x - startCoord.x) * sinA + (flatC.y - startCoord.y) * cosA + startCoord.y, 0
		);
		Nodes[i].init(props->Counts, coords,
			mechLIB_CPP::NodeFreedom::xyz, mechLIB_CPP::NodeLoad::none, std::vector<Element_t*>{ &Elements[i - 1], & Elements[i] });
	}
	Nodes[lastNode].init(props->Counts, endCoord,
		mechLIB_CPP::NodeFreedom::xyz, mechLIB_CPP::NodeLoad::none, std::vector<Element_t*>{ &Elements[lastNode - 1] });
}

void Rope_t::EvalElements(mechLIB_CPP::props_t* props)
{
	for (size_t i = 0; i < ElementsSize; i++)
	{
		Elements[i].init(&Nodes[i], &Nodes[i + 1],
			DirectX::SimpleMath::Vector3{
				Nodes[i].p[0].x ,Nodes[i].p[0].y + props->D,Nodes[i].p[0].z
			}, props->Counts, props);
		Elements[i].L[0] = Elements[i].GetOwnLength(0);
	}
}

void Rope_t::StepOverElems(int t, float Re, float bloodV, float bloodP)
{
//#pragma omp parallel for
	for (int i = 0; i < ElementsSize; i++)
	{
		Elements[i].CalcForce(t, Re, bloodV, bloodP);
	}
}

void Rope_t::StepOverNodes(int t, float Re, float dt)
{
//#pragma omp parallel for
	for (int i = 0; i < NodesSize; i++)
	{
		float m = 0;
		float c = 0;
		Nodes[i].GetPhysicParam(t - 1, Re, m, c);
		Nodes[i].GetForces(t, m, c);
		Nodes[i].CalcAccel(t, m);
		/*integrate*/
		Nodes[i].Integrate(t, t - 1, dt);
	}
}
