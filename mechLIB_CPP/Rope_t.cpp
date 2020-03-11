#include "pch.h"
#include "Rope_t.h"
#include <algorithm>    // std::for_each
#include <execution>
#include "maf.hpp"

void Rope_t::init(mechLIB_CPPWrapper::props_t* props)
{

	L = std::vector<float>(props->Counts);
	L[0] = props->L;
	NodesSize = props->nodes;
	Nodes = std::vector<Node_t>(NodesSize);
	ElementsSize = props->nodes - 1;
	Elements = std::vector<Element_t>(ElementsSize);
}

void Rope_t::SetupNodesPositions(mechLIB_CPPWrapper::props_t* props)
{
	int lastNode = NodesSize - 1;
	float dl = props->L / lastNode;
	Nodes[0].init(props->Counts,
		DirectX::SimpleMath::Vector3{ 0, props->initDrop * maf::P2((0 * dl) - (props->L - dl) / 2) + 1E-3f, 0 },
		DirectX::SimpleMath::Vector3{ 0, props->initDrop * maf::P2((0 * dl) - (props->L - dl) / 2) + 1E-3f, props->D },
		mechLIB_CPPWrapper::NodeFreedom::xyz, mechLIB_CPPWrapper::NodeLoad::none, std::vector<Element_t*>{ &Elements[0] });
	for (int i = 1; i < lastNode; i++)
	{
		Nodes[i].init(props->Counts,
			DirectX::SimpleMath::Vector3{ i * dl, props->initDrop * maf::P2((i * dl) - (props->L - dl) / 2) + 1E-3f, 0 },
			DirectX::SimpleMath::Vector3{ i * dl, props->initDrop * maf::P2((i * dl) - (props->L - dl) / 2) + 1E-3f, props->D },
			mechLIB_CPPWrapper::NodeFreedom::xyz, mechLIB_CPPWrapper::NodeLoad::none, std::vector<Element_t*>{ &Elements[i - 1], &Elements[i] });
	}
	Nodes[lastNode].init(props->Counts,
		DirectX::SimpleMath::Vector3{ lastNode * dl, props->initDrop * maf::P2((lastNode * dl) - (props->L - dl) / 2) + 1E-3f, 0 },
		DirectX::SimpleMath::Vector3{ lastNode * dl, props->initDrop * maf::P2((lastNode * dl) - (props->L - dl) / 2) + 1E-3f, props->D },
		mechLIB_CPPWrapper::NodeFreedom::xyz, mechLIB_CPPWrapper::NodeLoad::none, std::vector<Element_t*>{ &Elements[lastNode - 1] });
}

void Rope_t::SetupNodesPositions(mechLIB_CPPWrapper::props_t* props, DirectX::SimpleMath::Vector3 startCoord,
	DirectX::SimpleMath::Vector3 endCoord)
{
	//int lastNode = Nodes.Length - 1;
	//float dl = props->L / lastNode;
	//xyz_t tmpRadPoint = new xyz_t{ z = props->D };
	//float cosA = endCoord.x / props->L;
	//float sinA = endCoord.y / props->L;
	////xyz_t startCoordL = new xyz_t();
	////startCoordL.Plus(new xyz_t() { y = props->initDrop * maf.P2((0 * dl) - (props->L - dl) / 2) + 1E-3f }, startCoord);
	//Nodes[0] = new Node_t(props->Counts, startCoord, tmpRadPoint, NodeFreedom.xyz, NodeLoad.none, 0, new int[1]{ 1 });
	//for (int i = 1; i < lastNode; i++)
	//{
	//	//xyz_t flatC = new xyz_t { x = i * dl, y = props->initDrop * maf.P2((i * dl) - (props->L - dl) / 2) + 1E-3f };
	//	xyz_t flatC = new xyz_t{ x = i * dl };
	//	xyz_t coords = new xyz_t
	//	{
	//		/*X = (x — x0) * cos(alpha) — (y — y0) * sin(alpha) + x0;
	//		Y = (x — x0) * sin(alpha) + (y — y0) * cos(alpha) + y0;*/
	//		x = (flatC.x - startCoord.x) * cosA - (flatC.y - startCoord.y) * sinA + startCoord.x,
	//		y = (flatC.x - startCoord.x) * sinA + (flatC.y - startCoord.y) * cosA + startCoord.y
	//	};
	//	//dcm_.ToGlob(flatC, ref coords);
	//	Nodes[i] = new Node_t(props->Counts, coords, tmpRadPoint, NodeFreedom.xyz, NodeLoad.none, i, new int[2]{ i - 1, i + 1 });
	//}
	//Nodes[lastNode] = new Node_t(props->Counts, endCoord, tmpRadPoint, NodeFreedom.xyz, NodeLoad.none, lastNode, new int[1]{ lastNode - 1 });
}

void Rope_t::EvalElements(mechLIB_CPPWrapper::props_t* props)
{
	for (int i = 0; i < ElementsSize; i++)
	{
		Elements[i].init(&Nodes[i], &Nodes[i + 1], props->Counts, props);
	}
}

void Rope_t::StepOverElems(int t, float Re, float bloodV, float bloodP)
{
#pragma omp parallel for
	for (int i = 0; i < ElementsSize; i++)
	{
		Elements[i].CalcForce(t, Re, bloodV, bloodP);
	}
}

void Rope_t::StepOverNodes(int t, float Re, float dt)
{
#pragma omp parallel for
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
