#pragma once
#include "Rope_t.h"
#include "ModelPropertiesNative.h"
#include "C_t.h"
#include <string>
#include <vector>

class Enviro
{
private:
	std::vector<float> pmxq;
	std::vector<float> plxq;
	std::vector<float> pmyq;
	std::vector<float> plyq;
	std::vector<float> Re;
	std::vector<float> bloodV;
	std::vector<float> bloodP;
	void allocateTime(float dt, int Counts);
	std::string loadFile;
public:
	ModelPropertiesNative phProps;
	Rope_t* rope;
	std::vector<float> time;
	Enviro(ModelPropertiesNative _phProps, const std::string& _loadFile);
	void GenerateLoad(C_t axis);
	~Enviro()
	{
		delete rope;
	}
	void Run(bool NeedToSaveResults);
};