#pragma once
#include <string>
#include "ModelProperties.h"
#include "../mechLIB/enviro.h"
#include "DerivativesContainerManaged.h"

public ref class EnviroWrapper
{
	Enviro* world = nullptr;
public:
	~EnviroWrapper()
	{
		if (world)
		{
			delete world;
		}
	}
	void CreateWorld(ModelProperties^ _props, System::String^ loadFile);
	void Run(bool NeedToSaveResults);
	void Stop();
	void GetDerivatives(int step, array<array<mechLIB::DerivativesContainerManaged^>^>^% arr);
	void GetTimeArr(int step, array<float>^% arr);
	bool IsRunning() {
		if (world)
		{
			return world->IsRunning;
		}
		else
		{
			return false;
		}
	};
};
