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
	void allocateTime(float dt, size_t Counts);
	std::string loadFile;
	bool ableToRun = true;
	void writeFloatArr(FILE* file, const char* varName, std::vector<Node_t>& arr, size_t step);
public:
	std::vector<Node_t> Nodes;
	std::vector<Element_t> Elements;
	bool IsRunning = false;
	ModelPropertiesNative phProps;
	Rope_t* rope = nullptr;
	std::vector<float> time;
	Enviro(ModelPropertiesNative _phProps, const std::string& _loadFile);
	void GenerateLoad(C_t axis);
	~Enviro()
	{
		while (IsRunning)
		{

		}
		if (rope)
		{
			delete rope;
		}
	}
	void Run(bool NeedToSaveResults);
	void StepOverNodes(size_t t, float Re);
	void Integrate(size_t t, float dt);
	void StepOverElements(size_t t, float Re);
	void saveResultsToMAT();
	void saveResultsToTXT();
	void Stop() { ableToRun = false; };
};