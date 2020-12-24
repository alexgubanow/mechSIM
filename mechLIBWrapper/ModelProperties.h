#pragma once
#include "../mechLIB/PhysicalModelEnum.h"
#include "../mechLIB/ModelPropertiesNative.h"

public ref class ModelProperties
{
public:
	float MaxU;
	size_t nodes;
	float E;
	float L;
	float D;
	float ObservationTime;
	int Counts() { return dt != 0 ? (int)(ObservationTime / dt) : 0; };
	float dt;
	float ro;
	mechLIB::PhysicalModelEnum PhysicalModel;
	ModelPropertiesNative getNative(void)
	{
		ModelPropertiesNative tmp{};
		tmp.MaxU = MaxU;
		tmp.nodes = nodes;
		tmp.E = E;
		tmp.L = L;
		tmp.D = D;
		tmp.ObservationTime = ObservationTime;
		tmp.dt = dt;
		tmp.Counts = Counts();
		tmp.ro = ro;
		tmp.PhysicalModel = PhysicalModel;
		return tmp;
	}
};