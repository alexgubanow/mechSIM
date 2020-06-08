#pragma once
#include "../mechLIB_CPP/props_t.h"
#include "../mechLIB_CPP/enviro.h"
#include <string>
#include "Derivatives.h"
#include "DataPointCPP.hpp"

namespace mechLIB_CPP {
	public ref class EnviroWrapper
	{
		mechLIB_CPP::Enviro* world;
	public:
		EnviroWrapper();
		void CreateWorld(float DampRatio, float MaxU, float initDrop, int nodes, float E,
			float L, float D, int Counts, float dt, float ro, mechLIB_CPP::PhModels phMod, System::String^ loadFile);
		void Run(bool NeedToSaveResults);
		void GetNodesF(int step, array<array<mechLIB_CPP::DataPointCPP^>^>^% arr);
		void GetNodesA(int step, array<array<mechLIB_CPP::DataPointCPP^>^>^% arr);
		void GetNodesV(int step, array<array<mechLIB_CPP::DataPointCPP^>^>^% arr);
		void GetNodesU(int step, array<array<mechLIB_CPP::DataPointCPP^>^>^% arr);
		void GetNodesP(int step, array<array<mechLIB_CPP::DataPointCPP^>^>^% arr);
		void GetTimeArr(int step, array<float>^% arr);
		void Destroy()
		{
			delete world;
		}
	};
}
