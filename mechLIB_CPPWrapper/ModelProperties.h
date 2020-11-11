#pragma once
#include "PhysicalModelEnum.h"

namespace mechLIB_CPP {
	class ModelPropertiesNative
	{
	public:
		float DampRatio;
		float MaxU;
		float initDrop;
		size_t nodes;
		float E;
		float L;
		float D;
		int Counts;
		float dt;
		float ro;
		PhysicalModelEnum PhysicalModel;
	};
#if (_MANAGED == 1) || (_M_CEE == 1)
	public ref class ModelProperties
	{
	public:
		float DampRatio;
		float MaxU;
		float initDrop;
		size_t nodes;
		float E;
		float L;
		float D;
		int Counts;
		float dt;
		float ro;
		PhysicalModelEnum PhysicalModel;
		ModelPropertiesNative getNative(void)
		{
			ModelPropertiesNative tmp{};
			tmp.DampRatio = DampRatio;
			tmp.MaxU = MaxU;
			tmp.initDrop = initDrop;
			tmp.nodes = nodes;
			tmp.E = E;
			tmp.L = L;
			tmp.D = D;
			tmp.Counts = Counts;
			tmp.dt = dt;
			tmp.ro = ro;
			tmp.PhysicalModel = PhysicalModel;
			return tmp;
		}
	};
#endif	
}
