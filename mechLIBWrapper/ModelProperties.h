#pragma once
#include "../mechLIB/PhysicalModelEnum.h"
#include "../mechLIB/ModelPropertiesNative.h"
#include <corecrt_math_defines.h>

public ref class ModelProperties
{
public:
	float MaxU;
	size_t nodes;
	float E;
	float L;
	float D;
	float ObservationTime;
	int ToBeStoredCounts;
	int Counts() { return dt != 0 ? (int)(ObservationTime / dt) : 0; };
	float dt;
	float ro;
	bool isGravityEnabled;
	mechLIB::PhysicalModelEnum PhysicalModel;
	mechLIB::IntegrationSchemesEnum IntegrationSchema;
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
		tmp.ToBeStoredCounts = ToBeStoredCounts;
		tmp.ro = ro;
		tmp.A = (float)M_PI * D * D / 4;
		tmp.I = (tmp.A * tmp.A * tmp.A) / 12.0f;
		tmp.isGravityEnabled = isGravityEnabled;
		tmp.PhysicalModel = PhysicalModel;
		tmp.IntegrationSchema = IntegrationSchema;
		return tmp;
	}
};