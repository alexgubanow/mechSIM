#pragma once
#include "PhysicalModelEnum.h"
#include "IntegrationSchemesEnum.h"

class ModelPropertiesNative
{
public:
	float MaxU;
	size_t nodes;
	float E;
	float L;
	float D;
	float ObservationTime;
	int Counts;
	float dt;
	float ro;
	float A;
	float I;
	mechLIB::PhysicalModelEnum PhysicalModel;
	mechLIB::IntegrationSchemesEnum IntegrationSchema;
};