#pragma once
#include <string>
#include "ModelProperties.h"
#include "../mechLIB/enviro.h"
#include "DerivativesContainerManaged.h"

public ref class EnviroWrapper
{
	Enviro* world;
	/*static void FillManagedF(mechLIB::DerivativesContainerManaged^% DerivativesManaged, const DerivativesContainer& Derivatives);
	static void FillManagedA(mechLIB::DerivativesContainerManaged^% DerivativesManaged, const DerivativesContainer& Derivatives);
	static void FillManagedU(mechLIB::DerivativesContainerManaged^% DerivativesManaged, const DerivativesContainer& Derivatives);
	static void FillManagedV(mechLIB::DerivativesContainerManaged^% DerivativesManaged, const DerivativesContainer& Derivatives);
	static void FillManagedP(mechLIB::DerivativesContainerManaged^% DerivativesManaged, const DerivativesContainer& Derivatives);*/
public:
	EnviroWrapper();
	virtual ~EnviroWrapper()
	{
		delete world;
	}
	void CreateWorld(ModelProperties^ _props, System::String^ loadFile);
	void Run(bool NeedToSaveResults);
	void GetDerivatives(int step, array<array<mechLIB::DerivativesContainerManaged^>^>^% arr);
	void GetTimeArr(int step, array<float>^% arr);
};