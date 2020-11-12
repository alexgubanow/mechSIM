#include "EnviroWrapper.h"
#include <msclr\marshal_cppstd.h>
#include "../mechLIB/DerivativesContainer.h"
#include <d3d11_1.h>
#include "SimpleMath.h"
#include <omp.h>

EnviroWrapper::EnviroWrapper()
{
}

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
}

void EnviroWrapper::Run(bool NeedToSaveResults)
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

void EnviroWrapper::GetDerivatives(int step, array<array<mechLIB::DerivativesContainerManaged^>^>^% arr)
{
	arr = gcnew array<array<mechLIB::DerivativesContainerManaged^>^>((int)world->rope->NodesSize);
#pragma omp parallel for
	for (int n = 0; n < world->rope->NodesSize; n++)
	{
		arr[n] = gcnew array<mechLIB::DerivativesContainerManaged^>(world->phProps.Counts / step);
		int tout = 0;
		for (size_t t = 0; t < world->time.size() && tout < arr[n]->Length; t += step, ++tout)
		{
			arr[n][tout] = gcnew mechLIB::DerivativesContainerManaged(&world->rope->Nodes[n].Derivatives[t]);
			int dgsfb = 0;
			int bsdfg = 0;
			/*FillManagedF(arr[n][tout], world->rope->Nodes[n].Derivatives[t]);
			FillManagedA(arr[n][tout], world->rope->Nodes[n].Derivatives[t]);
			FillManagedU(arr[n][tout], world->rope->Nodes[n].Derivatives[t]);
			FillManagedV(arr[n][tout], world->rope->Nodes[n].Derivatives[t]);
			FillManagedP(arr[n][tout], world->rope->Nodes[n].Derivatives[t]);*/
		}
	}
}
//void EnviroWrapper::FillManagedF(mechLIB::DerivativesContainerManaged^% DerivativesManaged, const DerivativesContainer& Derivatives)
//{
//	DerivativesManaged->F.x = Derivatives.F.x;
//	DerivativesManaged->F.y = Derivatives.F.y;
//	DerivativesManaged->F.z = Derivatives.F.z;
//}
//void EnviroWrapper::FillManagedA(mechLIB::DerivativesContainerManaged^% DerivativesManaged, const DerivativesContainer& Derivatives)
//{
//	DerivativesManaged->a.x = Derivatives.a.x;
//	DerivativesManaged->a.y = Derivatives.a.y;
//	DerivativesManaged->a.z = Derivatives.a.z;
//}
//void EnviroWrapper::FillManagedU(mechLIB::DerivativesContainerManaged^% DerivativesManaged, const DerivativesContainer& Derivatives)
//{
//	DerivativesManaged->u.x = Derivatives.u.x;
//	DerivativesManaged->u.y = Derivatives.u.y;
//	DerivativesManaged->u.z = Derivatives.u.z;
//}
//void EnviroWrapper::FillManagedV(mechLIB::DerivativesContainerManaged^% DerivativesManaged, const DerivativesContainer& Derivatives)
//{
//	DerivativesManaged->v.x = Derivatives.v.x;
//	DerivativesManaged->v.y = Derivatives.v.y;
//	DerivativesManaged->v.z = Derivatives.v.z;
//}
//void EnviroWrapper::FillManagedP(mechLIB::DerivativesContainerManaged^% DerivativesManaged, const DerivativesContainer& Derivatives)
//{
//	DerivativesManaged->p.x = Derivatives.p.x;
//	DerivativesManaged->p.y = Derivatives.p.y;
//	DerivativesManaged->p.z = Derivatives.p.z;
//}

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