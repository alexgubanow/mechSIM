#pragma once
#include <string>
#include "ModelProperties.h"
#include "../mechLIB_CPP/enviro.h"
#include "Derivatives.h"
#include "DataPointCPP.hpp"

namespace mechLIB_CPP {
	public ref class EnviroWrapper
	{
		mechLIB_CPP::Enviro* world;
	public:
		EnviroWrapper();
		virtual ~EnviroWrapper()
		{
			delete world;
		}
		void CreateWorld(ModelProperties^ _props, System::String^ loadFile);
		void Run(bool NeedToSaveResults);
		void GetNodesF(int step, array<array<mechLIB_CPP::DataPointCPP^>^>^% arr);
		void GetNodesA(int step, array<array<mechLIB_CPP::DataPointCPP^>^>^% arr);
		void GetNodesV(int step, array<array<mechLIB_CPP::DataPointCPP^>^>^% arr);
		void GetNodesU(int step, array<array<mechLIB_CPP::DataPointCPP^>^>^% arr);
		void GetNodesP(int step, array<array<mechLIB_CPP::DataPointCPP^>^>^% arr);
		void GetTimeArr(int step, array<float>^% arr);
	};
}
