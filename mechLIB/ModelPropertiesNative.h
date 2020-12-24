#pragma once
#include "PhysicalModelEnum.h"

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
	mechLIB::PhysicalModelEnum PhysicalModel;
};