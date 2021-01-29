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
	int ToBeStoredCounts;
	float dt;
	float ro;
	float A;
	float I;
	bool isGravityEnabled;
	mechLIB::PhysicalModelEnum PhysicalModel;
	mechLIB::IntegrationSchemesEnum IntegrationSchema;
};