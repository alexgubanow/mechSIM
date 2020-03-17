#pragma once
#include "props_t.h"
#include "../mechLIB_CPP/enviro.h"
#include <string>
#include "Derivatives.h"
#include "DataPointCPP.hpp"

namespace mechLIB_CPPWrapper {
	public ref class Enviro
	{
		mechLIB_CPP::Enviro* world;
	public:
		Enviro();
		void CreateWorld(float DampRatio, float MaxU, float initDrop, int nodes, float E,
			float L, float D, int Counts, float dt, float ro, mechLIB_CPPWrapper::PhModels phMod, System::String^ loadFile);
		void Run();
		void GetNodesF(int step, array<array<mechLIB_CPPWrapper::DataPointCPP^>^>^% arr);
		void GetNodesA(int step, array<array<mechLIB_CPPWrapper::DataPointCPP^>^>^% arr);
		void GetNodesV(int step, array<array<mechLIB_CPPWrapper::DataPointCPP^>^>^% arr);
		void GetNodesU(int step, array<array<mechLIB_CPPWrapper::DataPointCPP^>^>^% arr);
		void GetNodesP(int step, array<array<mechLIB_CPPWrapper::DataPointCPP^>^>^% arr);
		void GetTimeArr(int step, array<float>^% arr);
		void Destroy()
		{
			delete world;
		}
	};
}
