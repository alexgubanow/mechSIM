#include "pch.h"

#include "mechLIB_CPPWrapper.h"
#include <msclr\marshal_cppstd.h>
#include "Derivatives.h"
#include <d3d11_1.h>
#include <DirectXMath.h>
#include "SimpleMath.h"

mechLIB_CPPWrapper::Enviro::Enviro()
{
}

void mechLIB_CPPWrapper::Enviro::CreateWorld(float DampRatio, float MaxU, float initDrop, int nodes, float E, float L, 
	float D, int Counts, float dt, float ro, mechLIB_CPPWrapper::PhModels phMod, System::String^ loadFile)
{
	props_t phProps;
	phProps.DampRatio = DampRatio;
	phProps.MaxU = MaxU;
	phProps.initDrop = initDrop;
	phProps.nodes = nodes;
	phProps.E = E;
	phProps.L = L;
	phProps.D = D;
	phProps.Counts = Counts;
	phProps.dt = dt;
	phProps.ro = ro;
	phProps.phMod = phMod;
	world = new mechLIB_CPP::Enviro(phProps, msclr::interop::marshal_as<std::string>(loadFile));
}

void mechLIB_CPPWrapper::Enviro::Run()
{
	world->Run();
}

void mechLIB_CPPWrapper::Enviro::GetNodesDerivs(array<array<array<array<float>^>^>^>^% arr)
{
	arr = gcnew array<array<array<array<float>^>^>^>(world->rope->NodesSize);
	for (int i = 0; i < world->rope->NodesSize; i++)
	{
		arr[i] = gcnew array<array<array<float>^>^>((int)Derivatives::maxDerivatives);
		arr[i][(int)Derivatives::a] = gcnew array<array<float>^>(world->phProps.Counts);
		arr[i][(int)Derivatives::p] = gcnew array<array<float>^>(world->phProps.Counts);
		arr[i][(int)Derivatives::u] = gcnew array<array<float>^>(world->phProps.Counts);
		arr[i][(int)Derivatives::v] = gcnew array<array<float>^>(world->phProps.Counts);
		arr[i][(int)Derivatives::f] = gcnew array<array<float>^>(world->phProps.Counts);
		for (int t = 0; t < world->phProps.Counts; t++)
		{
			arr[i][(int)Derivatives::a][t] = gcnew array<float>(3);
			arr[i][(int)Derivatives::a][t][0] = world->rope->Nodes[i].deriv[t].a.x;
			arr[i][(int)Derivatives::a][t][1] = world->rope->Nodes[i].deriv[t].a.y;
			arr[i][(int)Derivatives::a][t][2] = world->rope->Nodes[i].deriv[t].a.z;
			arr[i][(int)Derivatives::p][t] = gcnew array<float>(3);
			arr[i][(int)Derivatives::p][t][0] = world->rope->Nodes[i].deriv[t].p.x;
			arr[i][(int)Derivatives::p][t][1] = world->rope->Nodes[i].deriv[t].p.y;
			arr[i][(int)Derivatives::p][t][2] = world->rope->Nodes[i].deriv[t].p.z;
			arr[i][(int)Derivatives::f][t] = gcnew array<float>(3);
			arr[i][(int)Derivatives::f][t][0] = world->rope->Nodes[i].F[t].x;
			arr[i][(int)Derivatives::f][t][1] = world->rope->Nodes[i].F[t].y;
			arr[i][(int)Derivatives::f][t][2] = world->rope->Nodes[i].F[t].z;
			arr[i][(int)Derivatives::u][t] = gcnew array<float>(3);
			arr[i][(int)Derivatives::u][t][0] = world->rope->Nodes[i].deriv[t].u.x;
			arr[i][(int)Derivatives::u][t][1] = world->rope->Nodes[i].deriv[t].u.y;
			arr[i][(int)Derivatives::u][t][2] = world->rope->Nodes[i].deriv[t].u.z;
			arr[i][(int)Derivatives::v][t] = gcnew array<float>(3);
			arr[i][(int)Derivatives::v][t][0] = world->rope->Nodes[i].deriv[t].v.x;
			arr[i][(int)Derivatives::v][t][1] = world->rope->Nodes[i].deriv[t].v.y;
			arr[i][(int)Derivatives::v][t][2] = world->rope->Nodes[i].deriv[t].v.z;
		}
	}
	/*pin_ptr<float> pinned1 = &arr[0];
	pin_ptr<float> pinned2 = &world->rope->L[0];
	std::memcpy(pinned1, pinned2, world->phProps.Counts);*/
}

void mechLIB_CPPWrapper::Enviro::GetTimeArr(array<float>^% arr)
{
	arr = gcnew array<float>(world->phProps.Counts);
	pin_ptr<float> pinned1 = &arr[0];
	std::memcpy(pinned1, &world->time[0], (world->phProps.Counts)*sizeof(world->time[0]));
}
