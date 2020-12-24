#include "Rope_t.h"
#include "maf.hpp"

Rope_t::Rope_t(ModelPropertiesNative* props) : RopeProperties(props), Nodes(0), Elements(0), L(std::vector<float>(props->Counts))
{
	L[0] = RopeProperties->L;
	Nodes = std::vector<Node_t>(RopeProperties->nodes);
	Elements = std::vector<Element_t>(RopeProperties->nodes - 1);
}

void Rope_t::SetupNodesPositions()
{
	SetupNodesPositions(DirectX::SimpleMath::Vector3{ 0,0,0 }, DirectX::SimpleMath::Vector3{ RopeProperties->L,0,0 });
}

void Rope_t::SetupNodesPositions(DirectX::SimpleMath::Vector3 startCoord,
	DirectX::SimpleMath::Vector3 endCoord)
{
	size_t lastNode = RopeProperties->nodes - 1;
	float dl = RopeProperties->L / lastNode;
	float cosA = endCoord.x / RopeProperties->L;
	float sinA = endCoord.y / RopeProperties->L;
	Nodes[0].init(RopeProperties->Counts, startCoord,
		NodeFreedom::xyz, mechLIB::DerivativesEnum::DerivativesEnumMAX, std::vector<Element_t*>{ &Elements[0] });
	for (size_t i = 1; i < lastNode; i++)
	{
		DirectX::SimpleMath::Vector3 flatC(i * dl, 0, 0);
		DirectX::SimpleMath::Vector3 coords(

			(flatC.x - startCoord.x) * cosA - (flatC.y - startCoord.y) * sinA + startCoord.x,
			(flatC.x - startCoord.x) * sinA + (flatC.y - startCoord.y) * cosA + startCoord.y, 0
		);
		Nodes[i].init(RopeProperties->Counts, coords,
			NodeFreedom::xyz, mechLIB::DerivativesEnum::DerivativesEnumMAX, std::vector<Element_t*>{ &Elements[i - 1], &Elements[i] });
	}
	Nodes[lastNode].init(RopeProperties->Counts, endCoord,
		NodeFreedom::xyz, mechLIB::DerivativesEnum::DerivativesEnumMAX, std::vector<Element_t*>{ &Elements[lastNode - 1] });
}

void Rope_t::EvalElements()
{
	for (size_t i = 0; i < RopeProperties->nodes - 1; i++)
	{
		Elements[i].init(&Nodes[i], &Nodes[i + 1], RopeProperties);
		Elements[i].L[0] = Elements[i].GetOwnLength(0);
	}
}

void Rope_t::StepOverNodes(size_t t, float Re, float dt)
{
	//#pragma omp parallel for
	for (size_t i = 0; i < RopeProperties->nodes; i++)
	{
		float m = 0;
		Nodes[i].GetPhysicParam(t - 1, Re, m);
		Nodes[i].GetForces(t, m);
		Nodes[i].CalcAccel(t, m);
		/*integrate*/
		Nodes[i].Integrate(RopeProperties->IntegrationSchema, t, t - 1, dt);
	}
}
