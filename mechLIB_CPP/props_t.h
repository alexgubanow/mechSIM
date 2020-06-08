#pragma once
#include "PhModels.h"

namespace mechLIB_CPP {

	struct props_t
	
	{
		float DampRatio;
		float MaxU;
		float initDrop;
		int nodes;
		float E;
		float L;
		float D;
		int Counts;
		float dt;
		float ro;
		PhModels phMod;
	};
}
