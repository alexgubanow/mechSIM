#include "pch.h"
#include "Rope_t.h"
#include <algorithm>    // std::for_each
#include <execution>

void Rope_t::SetupNodesPositions(mechLIB_CPPWrapper::props_t* props)
{
	/*int lastNode = Nodes.size() - 1;
	float dl = props.L / lastNode;
	Nodes[0] = new Node_t(props.Counts,
		new Vector3{ y = props.initDrop * maf::P2((0 * dl) - (props.L - dl) / 2) + 1E-3f },
		new Vector3{ Y = props.initDrop * maf::P2((0 * dl) - (props.L - dl) / 2) + 1E-3f , Z = props.D },
		NodeFreedom::xyz, NodeLoad::none, 0, new int[1]{ 1 });
	for (int i = 1; i < lastNode; i++)
	{
		Nodes[i] = new Node_t(props.Counts,
			new Vector3{ X = i * dl, Y = props.initDrop * maf::P2((i * dl) - (props.L - dl) / 2) + 1E-3f },
			new Vector3{ X = i * dl, Y = props.initDrop * maf::P2((i * dl) - (props.L - dl) / 2) + 1E-3f, Z = props.D },
			NodeFreedom::xyz, NodeLoad::none, i, new int[2]{ i - 1, i + 1 });
	}
	Nodes[lastNode] = new Node_t(props.Counts,
		new Vector3{ X = lastNode * dl, Y = props.initDrop * maf::P2((lastNode * dl) - (props.L - dl) / 2) + 1E-3f },
		new Vector3{ X = lastNode * dl, Y = props.initDrop * maf::P2((lastNode * dl) - (props.L - dl) / 2) + 1E-3f, Z = props.D },
		NodeFreedom::xyz, NodeLoad::none, lastNode, new int[1]{ lastNode - 1 });*/
}

void Rope_t::SetupNodesPositions(mechLIB_CPPWrapper::props_t* props, DirectX::SimpleMath::Vector3 startCoord,
	DirectX::SimpleMath::Vector3 endCoord)
{
	//int lastNode = Nodes.Length - 1;
	//float dl = props.L / lastNode;
	//xyz_t tmpRadPoint = new xyz_t{ z = props.D };
	//float cosA = endCoord.x / props.L;
	//float sinA = endCoord.y / props.L;
	////xyz_t startCoordL = new xyz_t();
	////startCoordL.Plus(new xyz_t() { y = props.initDrop * maf.P2((0 * dl) - (props.L - dl) / 2) + 1E-3f }, startCoord);
	//Nodes[0] = new Node_t(props.Counts, startCoord, tmpRadPoint, NodeFreedom.xyz, NodeLoad.none, 0, new int[1]{ 1 });
	//for (int i = 1; i < lastNode; i++)
	//{
	//	//xyz_t flatC = new xyz_t { x = i * dl, y = props.initDrop * maf.P2((i * dl) - (props.L - dl) / 2) + 1E-3f };
	//	xyz_t flatC = new xyz_t{ x = i * dl };
	//	xyz_t coords = new xyz_t
	//	{
	//		/*X = (x — x0) * cos(alpha) — (y — y0) * sin(alpha) + x0;
	//		Y = (x — x0) * sin(alpha) + (y — y0) * cos(alpha) + y0;*/
	//		x = (flatC.x - startCoord.x) * cosA - (flatC.y - startCoord.y) * sinA + startCoord.x,
	//		y = (flatC.x - startCoord.x) * sinA + (flatC.y - startCoord.y) * cosA + startCoord.y
	//	};
	//	//dcm_.ToGlob(flatC, ref coords);
	//	Nodes[i] = new Node_t(props.Counts, coords, tmpRadPoint, NodeFreedom.xyz, NodeLoad.none, i, new int[2]{ i - 1, i + 1 });
	//}
	//Nodes[lastNode] = new Node_t(props.Counts, endCoord, tmpRadPoint, NodeFreedom.xyz, NodeLoad.none, lastNode, new int[1]{ lastNode - 1 });
}

void Rope_t::StepOverElems(int t, float Re, float bloodV, float bloodP)
{
	/*std::for_each(
	std::execution::par_unseq,
	Elements.begin(),
	Elements.end(),
	[](auto&& item)
	{
	item->CalcForce(this, t, Re, bloodV, bloodP);
	});*/

	/*Parallel.ForEach(Elements, (elem, loopState) = >
	{
	elem.CalcForce(this, t, Re, bloodV, bloodP);
	});*/
}

void Rope_t::StepOverNodes(int t, float Re, float dt)
{
	//Parallel.ForEach(Nodes, (node, loopState) = >
	//{
	//	float m = 0;
	//	float c = 0;
	//	node.GetPhysicParam(this, t - 1, Re, ref m, ref c);
	//	node.GetForces(this, t, m, c);
	//	node.CalcAccel(t, m);
	//	/*integrate*/
	//	node.Integrate(t, t - 1, dt);
	//});
}
