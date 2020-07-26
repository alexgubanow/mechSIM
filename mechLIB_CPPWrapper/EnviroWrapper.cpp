#include "pch.h"
#include "EnviroWrapper.h"
#include <msclr\marshal_cppstd.h>
#include "Derivatives.h"
#include <d3d11_1.h>
#include <DirectXMath.h>
#include "SimpleMath.h"
#include <omp.h>

mechLIB_CPP::EnviroWrapper::EnviroWrapper()
{
}

void mechLIB_CPP::EnviroWrapper::CreateWorld(float DampRatio, float MaxU, float initDrop, int nodes, float E, float L,
	float D, int Counts, float dt, float ro, mechLIB_CPP::PhModels phMod, System::String^ loadFile)
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
	std::string str = msclr::interop::marshal_as<std::string>(loadFile);
	try
	{
		world = new mechLIB_CPP::Enviro(phProps, str);
	}
	catch (const char* ex)
	{
		throw gcnew System::String(ex);
	}
}

void mechLIB_CPP::EnviroWrapper::Run(bool NeedToSaveResults)
{
	try
	{
		world->Run(NeedToSaveResults);
	}
	catch (const char *ex)
	{
		throw gcnew System::String(ex);
	}
}

void mechLIB_CPP::EnviroWrapper::GetNodesF(int step, array<array<mechLIB_CPP::DataPointCPP^>^>^% arr)
{
	arr = gcnew array<array<mechLIB_CPP::DataPointCPP^>^>((int)world->rope->NodesSize);
#pragma omp parallel for
	for (int n = 0; n < world->rope->NodesSize; n++)
	{
		arr[n] = gcnew array<mechLIB_CPP::DataPointCPP^>(world->phProps.Counts / step);
		/*pin_ptr<float> pinned1 = &arr[n][0][0];
		std::memcpy(pinned1, &world->rope->Nodes[n].F[0].x, sizeof(pinned1));*/
		//#pragma omp parallel for
		size_t t;
		int tout;
		for (t = 0, tout = 0; t < world->time.size() && tout < arr[n]->Length; t += step, tout++)
		{
			arr[n][tout] = gcnew mechLIB_CPP::DataPointCPP();
			arr[n][tout]->X = world->rope->Nodes[n].F[t].x;
			arr[n][tout]->Y = world->rope->Nodes[n].F[t].y;
			arr[n][tout]->Z = world->rope->Nodes[n].F[t].z;
		}
	}
}
void mechLIB_CPP::EnviroWrapper::GetNodesA(int step, array<array<mechLIB_CPP::DataPointCPP^>^>^% arr)
{
	arr = gcnew array<array<mechLIB_CPP::DataPointCPP^>^>((int)world->rope->NodesSize);
#pragma omp parallel for
	for (int n = 0; n < world->rope->NodesSize; n++)
	{
		arr[n] = gcnew array<mechLIB_CPP::DataPointCPP^>(world->phProps.Counts / step);
		size_t t;
		int tout;
		for (t = 0, tout = 0; t < world->time.size() && tout < arr[n]->Length; t += step, tout++)
		{
			arr[n][tout] = gcnew mechLIB_CPP::DataPointCPP();
			arr[n][tout]->X = world->rope->Nodes[n].a[t].x;
			arr[n][tout]->Y = world->rope->Nodes[n].a[t].y;
			arr[n][tout]->Z = world->rope->Nodes[n].a[t].z;
		}
	}
}
void mechLIB_CPP::EnviroWrapper::GetNodesV(int step, array<array<mechLIB_CPP::DataPointCPP^>^>^% arr)
{
	arr = gcnew array<array<mechLIB_CPP::DataPointCPP^>^>((int)world->rope->NodesSize);
#pragma omp parallel for
	for (int n = 0; n < world->rope->NodesSize; n++)
	{
		arr[n] = gcnew array<mechLIB_CPP::DataPointCPP^>(world->phProps.Counts / step);
		size_t t;
		int tout;
		for (t = 0, tout = 0; t < world->time.size() && tout < arr[n]->Length; t += step, tout++)
		{
			arr[n][tout] = gcnew mechLIB_CPP::DataPointCPP();
			arr[n][tout]->X = world->rope->Nodes[n].v[t].x;
			arr[n][tout]->Y = world->rope->Nodes[n].v[t].y;
			arr[n][tout]->Z = world->rope->Nodes[n].v[t].z;
		}
	}
}
void mechLIB_CPP::EnviroWrapper::GetNodesU(int step, array<array<mechLIB_CPP::DataPointCPP^>^>^% arr)
{
	arr = gcnew array<array<mechLIB_CPP::DataPointCPP^>^>((int)world->rope->NodesSize);
#pragma omp parallel for
	for (int n = 0; n < world->rope->NodesSize; n++)
	{
		arr[n] = gcnew array<mechLIB_CPP::DataPointCPP^>(world->phProps.Counts / step);
		size_t t;
		int tout;
		for (t = 0, tout = 0; t < world->time.size() && tout < arr[n]->Length; t += step, tout++)
		{
			arr[n][tout] = gcnew mechLIB_CPP::DataPointCPP();
			arr[n][tout]->X = world->rope->Nodes[n].u[t].x;
			arr[n][tout]->Y = world->rope->Nodes[n].u[t].y;
			arr[n][tout]->Z = world->rope->Nodes[n].u[t].z;
		}
	}
}
void mechLIB_CPP::EnviroWrapper::GetNodesP(int step, array<array<mechLIB_CPP::DataPointCPP^>^>^% arr)
{
	arr = gcnew array<array<mechLIB_CPP::DataPointCPP^>^>((int)world->rope->NodesSize);
#pragma omp parallel for
	for (int n = 0; n < world->rope->NodesSize; n++)
	{
		arr[n] = gcnew array<mechLIB_CPP::DataPointCPP^>(world->phProps.Counts / step);
		size_t t;
		int tout;
		for (t = 0, tout = 0; t < world->time.size() && tout < arr[n]->Length; t += step, tout++)
		{
			arr[n][tout] = gcnew mechLIB_CPP::DataPointCPP();
			arr[n][tout]->X = world->rope->Nodes[n].p[t].x;
			arr[n][tout]->Y = world->rope->Nodes[n].p[t].y;
			arr[n][tout]->Z = world->rope->Nodes[n].p[t].z;
		}
	}
}

void mechLIB_CPP::EnviroWrapper::GetTimeArr(int step, array<float>^% arr)
{/*
	arr = gcnew array<float>(world->phProps.Counts);
	pin_ptr<float> pinned1 = &arr[0];
	std::memcpy(pinned1, &world->time[0], (world->phProps.Counts)*sizeof(world->time[0]));*/
	arr = gcnew array<float>(world->phProps.Counts / step);
	size_t t;
	int tout;
	for (t = 0, tout = 0; t < world->time.size() && tout < arr->Length; t += step, tout++)
	{
		arr[tout] = world->time[t];
	}
}
