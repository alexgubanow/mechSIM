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

void mechLIB_CPPWrapper::Enviro::GetNodesF(int step, array<array<mechLIB_CPPWrapper::DataPointCPP^>^>^% arr)
{
	arr = gcnew array<array<mechLIB_CPPWrapper::DataPointCPP^>^>(world->rope->NodesSize);
#pragma omp parallel for
	for (int n = 0; n < world->rope->NodesSize; n++)
	{
		arr[n] = gcnew array<mechLIB_CPPWrapper::DataPointCPP^>(world->phProps.Counts / step);
		/*pin_ptr<float> pinned1 = &arr[n][0][0];
		std::memcpy(pinned1, &world->rope->Nodes[n].F[0].x, sizeof(pinned1));*/
		//#pragma omp parallel for
		for (size_t t = 0, tout = 0; t < world->time.size() && tout < arr[n]->Length; t += step, tout++)
		{
			arr[n][tout] = gcnew mechLIB_CPPWrapper::DataPointCPP();
			arr[n][tout]->X = world->rope->Nodes[n].F[t].x;
			arr[n][tout]->Y = world->rope->Nodes[n].F[t].y;
			arr[n][tout]->Z = world->rope->Nodes[n].F[t].z;
		}
	}
}
void mechLIB_CPPWrapper::Enviro::GetNodesA(int step, array<array<mechLIB_CPPWrapper::DataPointCPP^>^>^% arr)
{
	arr = gcnew array<array<mechLIB_CPPWrapper::DataPointCPP^>^>(world->rope->NodesSize);
#pragma omp parallel for
	for (int n = 0; n < world->rope->NodesSize; n++)
	{
		arr[n] = gcnew array<mechLIB_CPPWrapper::DataPointCPP^>(world->phProps.Counts / step);
		for (size_t t = 0, tout = 0; t < world->time.size() && tout < arr[n]->Length; t += step, tout++)
		{
			arr[n][tout] = gcnew mechLIB_CPPWrapper::DataPointCPP();
			arr[n][tout]->X = world->rope->Nodes[n].a[t].x;
			arr[n][tout]->Y = world->rope->Nodes[n].a[t].y;
			arr[n][tout]->Z = world->rope->Nodes[n].a[t].z;
		}
	}
}
void mechLIB_CPPWrapper::Enviro::GetNodesV(int step, array<array<mechLIB_CPPWrapper::DataPointCPP^>^>^% arr)
{
	arr = gcnew array<array<mechLIB_CPPWrapper::DataPointCPP^>^>(world->rope->NodesSize);
#pragma omp parallel for
	for (int n = 0; n < world->rope->NodesSize; n++)
	{
		arr[n] = gcnew array<mechLIB_CPPWrapper::DataPointCPP^>(world->phProps.Counts / step);
		for (size_t t = 0, tout = 0; t < world->time.size() && tout < arr[n]->Length; t += step, tout++)
		{
			arr[n][tout] = gcnew mechLIB_CPPWrapper::DataPointCPP();
			arr[n][tout]->X = world->rope->Nodes[n].v[t].x;
			arr[n][tout]->Y = world->rope->Nodes[n].v[t].y;
			arr[n][tout]->Z = world->rope->Nodes[n].v[t].z;
		}
	}
}
void mechLIB_CPPWrapper::Enviro::GetNodesU(int step, array<array<mechLIB_CPPWrapper::DataPointCPP^>^>^% arr)
{
	arr = gcnew array<array<mechLIB_CPPWrapper::DataPointCPP^>^>(world->rope->NodesSize);
#pragma omp parallel for
	for (int n = 0; n < world->rope->NodesSize; n++)
	{
		arr[n] = gcnew array<mechLIB_CPPWrapper::DataPointCPP^>(world->phProps.Counts / step);
		for (size_t t = 0, tout = 0; t < world->time.size() && tout < arr[n]->Length; t += step, tout++)
		{
			arr[n][tout] = gcnew mechLIB_CPPWrapper::DataPointCPP();
			arr[n][tout]->X = world->rope->Nodes[n].u[t].x;
			arr[n][tout]->Y = world->rope->Nodes[n].u[t].y;
			arr[n][tout]->Z = world->rope->Nodes[n].u[t].z;
		}
	}
}
void mechLIB_CPPWrapper::Enviro::GetNodesP(int step, array<array<mechLIB_CPPWrapper::DataPointCPP^>^>^% arr)
{
	arr = gcnew array<array<mechLIB_CPPWrapper::DataPointCPP^>^>(world->rope->NodesSize);
#pragma omp parallel for
	for (int n = 0; n < world->rope->NodesSize; n++)
	{
		arr[n] = gcnew array<mechLIB_CPPWrapper::DataPointCPP^>(world->phProps.Counts / step);
		for (size_t t = 0, tout = 0; t < world->time.size() && tout < arr[n]->Length; t += step, tout++)
		{
			arr[n][tout] = gcnew mechLIB_CPPWrapper::DataPointCPP();
			arr[n][tout]->X = world->rope->Nodes[n].p[t].x;
			arr[n][tout]->Y = world->rope->Nodes[n].p[t].y;
			arr[n][tout]->Z = world->rope->Nodes[n].p[t].z;
		}
	}
}

void mechLIB_CPPWrapper::Enviro::GetTimeArr(int step, array<float>^% arr)
{/*
	arr = gcnew array<float>(world->phProps.Counts);
	pin_ptr<float> pinned1 = &arr[0];
	std::memcpy(pinned1, &world->time[0], (world->phProps.Counts)*sizeof(world->time[0]));*/
	arr = gcnew array<float>(world->phProps.Counts / step);
#pragma omp parallel for
	for (size_t t = 0, tout = 0; t < world->time.size() && tout < arr->Length; t += step, tout++)
	{
		arr[tout] = world->time[t];
	}
}
