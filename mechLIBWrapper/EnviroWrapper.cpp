#include "EnviroWrapper.h"
#include <msclr\marshal_cppstd.h>
#include "../mechLIB/DerivativesContainer.h"
#include <d3d11_1.h>
#include "SimpleMath.h"
#include <omp.h>

void EnviroWrapper::CreateWorld(ModelProperties^ _props, System::String^ loadFile)
{
	std::string str = msclr::interop::marshal_as<std::string>(loadFile);
	ModelPropertiesNative props = _props->getNative();
	try
	{
		world = new Enviro(props, str);
	}
	catch (const char* ex)
	{
		throw gcnew System::String(ex);
	}
	catch (...)
	{
		throw "Something went very wrong";
	}
}

void EnviroWrapper::Run(bool NeedToSaveResults)
{
	try
	{
		world->Run(NeedToSaveResults);
	}
	catch (const char* ex)
	{
		world->IsRunning = false;
		throw gcnew System::String(ex);
	}
	catch (...)
	{
		world->IsRunning = false;
		throw "Something went very wrong";
	}
}
void EnviroWrapper::Stop()
{
	try
	{
		world->Stop();
	}
	catch (const char* ex)
	{
		throw gcnew System::String(ex);
	}
	catch (...)
	{
		throw "Something went very wrong";
	}
}

void EnviroWrapper::GetDerivatives(int step, array<array<mechLIB::DerivativesContainerManaged^>^>^% arr)
{
	arr = gcnew array<array<mechLIB::DerivativesContainerManaged^>^>((int)world->Nodes.size());
#pragma omp parallel for
	for (int n = 0; n < world->Nodes.size(); n++)
	{
		arr[n] = gcnew array<mechLIB::DerivativesContainerManaged^>(world->phProps.Counts / step);
		int tout = 0;
		for (size_t t = 0; t < world->time.size() && tout < arr[n]->Length; t += step, ++tout)
		{
			arr[n][tout] = gcnew mechLIB::DerivativesContainerManaged(&world->Nodes[n].Derivatives[t]);
		}
	}
}
void EnviroWrapper::GetElementsForce(int step, array<array<mechLIB::Vector3Managed^>^>^% arr)
{
	arr = gcnew array<array<mechLIB::Vector3Managed^>^>((int)world->Elements.size());
#pragma omp parallel for
	for (int element = 0; element < world->Elements.size(); element++)
	{
		arr[element] = gcnew array<mechLIB::Vector3Managed^>(world->phProps.Counts / step);
		int tout = 0;
		for (size_t t = 0; t < world->time.size() && tout < arr[element]->Length; t += step, ++tout)
		{
			arr[element][tout] = gcnew mechLIB::Vector3Managed(world->Elements[element].F[t]);
		}
	}
}

void EnviroWrapper::GetTimeArr(int step, array<float>^% arr)
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