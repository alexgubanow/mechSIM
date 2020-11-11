#pragma once
#include <string>
#include "ModelProperties.h"
#include "../mechLIB/enviro.h"
#include "Derivatives.h"
#include "DataPointCPP.hpp"

public ref class EnviroWrapper
{
	Enviro* world;
public:
	EnviroWrapper();
	virtual ~EnviroWrapper()
	{
		delete world;
	}
	void CreateWorld(ModelProperties^ _props, System::String^ loadFile);
	void Run(bool NeedToSaveResults);
	void GetNodesF(int step, array<array<DataPointCPP^>^>^% arr);
	void GetNodesA(int step, array<array<DataPointCPP^>^>^% arr);
	void GetNodesV(int step, array<array<DataPointCPP^>^>^% arr);
	void GetNodesU(int step, array<array<DataPointCPP^>^>^% arr);
	void GetNodesP(int step, array<array<DataPointCPP^>^>^% arr);
	void GetTimeArr(int step, array<float>^% arr);
};