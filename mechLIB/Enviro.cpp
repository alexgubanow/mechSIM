#include "Enviro.h"
#include "matioWrap.h"
#define _USE_MATH_DEFINES
#include <math.h>
#include "maf.hpp"
#include <ctime>
#include <chrono>
#include <map>

using namespace mechLIB;

void Enviro::allocateTime(float dt, size_t Counts)
{
	time = std::vector<float>(Counts);
	for (size_t i = 1; i < Counts; i++)
	{
		time[i] = time[i - 1] + dt;
	}
}
Enviro::Enviro(ModelPropertiesNative _phProps, const std::string& _loadFile) : loadFile(_loadFile), phProps(_phProps), rope(nullptr)
{
	if (loadFile.size() > 0)
	{
		auto matioW = new matReader(loadFile.c_str());
		if (matioW)
		{
			matioW->readFloatArr("pmxq", pmxq);
			matioW->readFloatArr("plxq", plxq);
			matioW->readFloatArr("pmyq", pmyq);
			matioW->readFloatArr("plyq", plyq);
			matioW->readFloatArr("tq", time);
			matioW->readFloatArr("req", Re);
			matioW->readFloatArr("bloodVq", bloodV);
			matioW->readFloatArr("abpq", bloodP);
			phProps.Counts = (int)time.size();

			rope = new Rope_t(&phProps);
			rope->SetupNodesPositions(DirectX::SimpleMath::Vector3(pmxq[0], pmyq[0], 0),
				DirectX::SimpleMath::Vector3(plxq[0], plyq[0], 0));
			rope->EvalElements();
			size_t lastN = phProps.nodes - 1;
			for (int t = 0; t < phProps.Counts; t++)
			{
				rope->Nodes[0].Derivatives[t].p.x = pmxq[t];
				rope->Nodes[0].Derivatives[t].p.y = pmyq[t];

				rope->Nodes[lastN].Derivatives[t].p.x = plxq[t];
				rope->Nodes[lastN].Derivatives[t].p.y = plyq[t];
			}
			//choose load nodes
			rope->Nodes[0].LoadType = mechLIB::DerivativesEnum::p;
			rope->Nodes[phProps.nodes - 1].LoadType = mechLIB::DerivativesEnum::p;
			delete matioW;
		}
	}
	else
	{
		allocateTime(phProps.dt, phProps.Counts);
		rope = new Rope_t(&phProps);
		rope->SetupNodesPositions();
		rope->EvalElements();
		GenerateLoad(C_t::x);
	}
}
void Enviro::GenerateLoad(C_t axis)
{
	size_t lastN = phProps.nodes - 1;
	size_t midN = lastN / 2;
	Re = std::vector<float>(phProps.Counts);
	bloodV = std::vector<float>(phProps.Counts);
	bloodP = std::vector<float>(phProps.Counts);
	float A = (float)M_PI * maf::P2(phProps.D) / 4;
	float maxLoad = ((phProps.E * A) / phProps.L / phProps.nodes) * phProps.MaxU;
	float freq = 1 / (phProps.Counts * phProps.dt);
	rope->Nodes[0].LoadType = mechLIB::DerivativesEnum::p;
	//rope->Nodes[midN].LoadType = NodeLoad::u;
	rope->Nodes[lastN].LoadType = mechLIB::DerivativesEnum::p;
	float stepP = phProps.MaxU / phProps.Counts;
	for (size_t t = 1; t < phProps.Counts; t++)
	{
		//rope->Nodes[0].Derivatives[t].p = rope->Nodes[0].Derivatives[0].p;
		/*rope->Nodes[0].p[t].x = (phProps.MaxU * sinf(2 * (float)M_PI * time[t] * freq / 2)) + rope->Nodes[0].p[0].x;
		rope->Nodes[0].u[t] = rope->Nodes[0].p[t] - rope->Nodes[0].p[0];*/

		/*rope->Nodes[midN].p[t] = rope->Nodes[midN].p[0];
		rope->Nodes[midN].p[t].y = (rope->Nodes[midN].p[0].y + (-phProps.MaxU * sinf(2 * (float)M_PI * time[t] * freq)));
		rope->Nodes[midN].u[t] = rope->Nodes[midN].p[t] - rope->Nodes[midN].p[0];*/

		//rope->Nodes[lastN].Derivatives[t].p = rope->Nodes[lastN].Derivatives[0].p;

		//rope->Nodes[lastN].Derivatives[t].F.x = rope->Nodes[lastN].Derivatives[t - 1].F.x + stepP;
		rope->Nodes[lastN].Derivatives[t].p.x = rope->Nodes[lastN].Derivatives[t - 1].p.x + stepP;
		//rope->Nodes[lastN].Derivatives[t].p.y = rope->Nodes[lastN].Derivatives[t - 1].p.y + stepP;
		//rope->Nodes[lastN].Derivatives[t].F.x = (phProps.MaxU * sinf(2 * (float)M_PI * time[t] * freq / 2));
		/*rope->Nodes[lastN].Derivatives[t].p.x = (phProps.MaxU * sinf(2 * (float)M_PI * time[t] * freq / 2)) +
			rope->Nodes[lastN].Derivatives[0].p.x;*/
			/*rope->Nodes[lastN].Derivatives[t].p.x = (phProps.MaxU * sinf(2 * (float)M_PI * time[t] * freq * 3)) +
				rope->Nodes[lastN].Derivatives[0].p.x;*/

				/*rope->Nodes[lastN].p[t].x = 0 - ((phProps.MaxU * sinf(2 * (float)M_PI * time[t] * freq / 2)) + rope->Nodes[lastN].p[0].x);
				rope->Nodes[lastN].u[t] = rope->Nodes[lastN].p[t] - rope->Nodes[lastN].p[0];*/
	}
}
std::string getDataTimeNow()
{
	std::chrono::system_clock::time_point now = std::chrono::system_clock::now();
	std::time_t now_c = std::chrono::system_clock::to_time_t(now);
	std::tm now_tm;
	localtime_s(&now_tm, &now_c);
	char buffer[80];
	strftime(buffer, sizeof(buffer), "%d-%m-%Y_%H-%M-%S", &now_tm);
	return buffer;
}
void Enviro::Run(bool NeedToSaveResults)
{
	IsRunning = true;
	for (size_t t = 2; t < phProps.Counts; t++)
	{
		rope->StepOverElements(t, Re[t - 1]);
		rope->StepOverNodes(t, Re[t - 1], phProps.dt);
		for (int i = 0; i < rope->Elements.size(); i++)
		{
			rope->L[t] += rope->Elements[i].GetOwnLength(t - 1);
			if (!ableToRun)
			{
				break;
			}
		}
		if (!ableToRun)
		{
			break;
		}
	}
	if (NeedToSaveResults && ableToRun)
	{
		saveResultsToTXT();
	}
	IsRunning = false;
}
std::map<PhysicalModelEnum, const char*> PhysicalModelEnumSTR
{
	{PhysicalModelEnum::hook,"Linear Hook"},
	{PhysicalModelEnum::hookGeomNon,"Nonlinear Hook" },
	{PhysicalModelEnum::mooneyRiv,"Mooney-Rivlin"}
};

void Enviro::saveResultsToMAT()
{
	std::string resultsFileName = "expr_";
	/*if (loadFile.size() > 0)
	{
	resultsFileName += loadFile.substr(loadFile.size() - 3, 3);
	}*/
	resultsFileName += getDataTimeNow();
	resultsFileName += ".mat";
	auto mat = new matWriter(resultsFileName.c_str());
	mat->writeString("PhysicalModel", PhysicalModelEnumSTR[phProps.PhysicalModel]);
	mat->writeFloat("dt", phProps.dt);
	mat->writeUINT64("nodes", phProps.nodes);
	mat->writeFloat("E", phProps.E);
	mat->writeFloat("ro", phProps.ro);
	mat->writeFloat("D", phProps.D);
	size_t step = rope->L.size() / phProps.ToBeStoredCounts;
	size_t arraySize = rope->L.size() / phProps.ToBeStoredCounts;
	if (phProps.ToBeStoredCounts == -1)
	{
		step = 1;
		arraySize = rope->L.size();
	}

	std::vector<float> LToMat = std::vector<float>(arraySize);
	for (size_t srcIdx = 0, dstIdx = 0; srcIdx < rope->L.size(); srcIdx += step, dstIdx++)
	{
		LToMat[dstIdx] = rope->L[srcIdx];
	}
	mat->writeFloatArr("L", LToMat);
	delete mat;
}
void Enviro::writeFloatArr(FILE* file, const char* varName, std::vector<Node_t>& arr, size_t step)
{
	fprintf_s(file, "%s\n", varName);
	for (size_t t = 0; t < phProps.Counts; t += step)
	{
		for (size_t node = 0; node < arr.size(); node++)
		{
			if (!strcmp(varName, "a"))
			{
				fprintf_s(file, "%E\t", arr[node].Derivatives[t].a.x);
			}
			else if (!strcmp(varName, "f"))
			{
				fprintf_s(file, "%E\t", arr[node].Derivatives[t].F.x);
			}
			else if (!strcmp(varName, "v"))
			{
				fprintf_s(file, "%E\t", arr[node].Derivatives[t].v.x);
			}
			else if (!strcmp(varName, "p"))
			{
				fprintf_s(file, "%E\t", arr[node].Derivatives[t].p.x);
			}
		}
		fprintf_s(file, "\n");
	}
	fprintf_s(file, "\n");
}
void Enviro::saveResultsToTXT()
{
	std::string resultsFileName = "expr_";
	resultsFileName += getDataTimeNow();
	resultsFileName += ".txt";
	FILE* fileStream;
	if (fopen_s(&fileStream, resultsFileName.c_str(), "w+"))
	{
		char errBuff[2048];
		snprintf(errBuff, sizeof(errBuff), "Failed to create file: \"%s\"", resultsFileName.c_str());
		throw (const char*)errBuff;
	}
	fprintf_s(fileStream, "PhysicalModel: %s\t", PhysicalModelEnumSTR[phProps.PhysicalModel]);
	fprintf_s(fileStream, "Total time: %E\t", phProps.Counts * (double)phProps.dt);
	fprintf_s(fileStream, "dt: %E\t", phProps.dt);
	fprintf_s(fileStream, "nodes: %i\t", (int)phProps.nodes);
	fprintf_s(fileStream, "E: %E\t", phProps.E);
	fprintf_s(fileStream, "ro: %E\t", phProps.ro);
	fprintf_s(fileStream, "D: %E\t", phProps.D);
	fprintf_s(fileStream, "L: %E\t", phProps.L);
	fprintf_s(fileStream, "\n");
	size_t step = rope->L.size() / phProps.ToBeStoredCounts;
	if (phProps.ToBeStoredCounts == -1)
	{
		step = 1;
	}
	writeFloatArr(fileStream, "f", rope->Nodes, step);
	writeFloatArr(fileStream, "a", rope->Nodes, step);
	writeFloatArr(fileStream, "v", rope->Nodes, step);
	writeFloatArr(fileStream, "p", rope->Nodes, step);
	/*fprintf_s(fileStream, "%s\n", "f for each element");
	for (size_t t = 0; t < phProps.Counts; t += step)
	{
		for (size_t Element = 0; Element < rope->Elements.size(); Element++)
		{
			fprintf_s(fileStream, "%E\t", rope->Elements[Element].F[t].x);
		}
		fprintf_s(fileStream, "\n");
	}
	fprintf_s(fileStream, "\n");*/
	fclose(fileStream);
}
