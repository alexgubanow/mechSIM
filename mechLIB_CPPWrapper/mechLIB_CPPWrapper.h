#pragma once
#include "props_t.h"
#include "../mechLIB_CPP/enviro.h"
#include <string>

namespace mechLIB_CPPWrapper {
	public ref class Enviro
	{
		mechLIB_CPP::Enviro* world;
	public:
		Enviro();
		void CreateWorld(float DampRatio, float MaxU, float initDrop, int nodes, float E,
			float L, float D, int Counts, float dt, float ro, mechLIB_CPPWrapper::PhModels phMod, System::String^ loadFile);
		void Run();
		void GetNodesDerivs(array<array<array<array<float>^>^>^>^% arr);
		void GetTimeArr(array<float>^% arr);
		void Destroy()
		{
			delete world;
		}
	};
}
