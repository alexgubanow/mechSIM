#pragma once
#include "PhysicalModelEnum.h"

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
	mechLIB::PhysicalModelEnum PhysicalModel;
};