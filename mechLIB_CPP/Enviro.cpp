#include "pch.h"
#include "Enviro.h"
#include "matioWrap.h"
#define _USE_MATH_DEFINES
#include <math.h>
#include "maf.hpp"

namespace mechLIB_CPP
{
	void Enviro::allocateTime(float dt, int Counts)
	{
		time = std::vector<float>(Counts);
		//time = new float[Counts];
		for (int i = 1; i < Counts; i++)
		{
			time[i] = time[i - 1] + dt;
		}
	}
	Enviro::Enviro(props_t _phProps, std::string& _loadFile) : loadFile(_loadFile), phProps(_phProps)
	{
		if (loadFile.size() > 0)
		{
			matioWrap* matioW = NULL;
			matioW = new matioWrap(loadFile);
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
				phProps.Counts = time.size();

				rope = new Rope_t();
				rope->init(&phProps);
				rope->SetupNodesPositions(&phProps, DirectX::SimpleMath::Vector3(pmxq[0], pmyq[0], 0),
					DirectX::SimpleMath::Vector3(plxq[0], plyq[0], 0));
				rope->EvalElements(&phProps);
				int lastN = phProps.nodes - 1;
				for (int t = 0; t < phProps.Counts; t++)
				{
					rope->Nodes[0].p[t].x = pmxq[t];
					rope->Nodes[0].p[t].y = pmyq[t];
					rope->Nodes[0].u[t].x = rope->Nodes[0].p[t].x - rope->Nodes[0].p[0].x;

					rope->Nodes[lastN].p[t].x = plxq[t];
					rope->Nodes[lastN].p[t].y = plyq[t];
					rope->Nodes[lastN].u[t].x = rope->Nodes[lastN].p[t].x - rope->Nodes[lastN].p[0].x;
				}
				//choose load nodes
				rope->Nodes[0].LoadType = NodeLoad::u;
				rope->Nodes[phProps.nodes - 1].LoadType = NodeLoad::u;
				delete matioW;
			}
		}
		else
		{
			allocateTime(phProps.dt, phProps.Counts);
			rope = new Rope_t();
			rope->init(&phProps);
			rope->SetupNodesPositions(&phProps);
			rope->EvalElements(&phProps);
			GenerateLoad(C_t::x);
		}
	}
	void Enviro::GenerateLoad(C_t axis)
	{
		int lastN = phProps.nodes - 1;
		int midN = lastN / 2;
		Re = std::vector<float>(phProps.Counts);
		bloodV = std::vector<float>(phProps.Counts);
		bloodP = std::vector<float>(phProps.Counts);
		float A = (float)M_PI * maf::P2(phProps.D) / 4;
		float maxLoad = ((phProps.E * A) / phProps.L / phProps.nodes) * phProps.MaxU;
		float freq = 1 / (phProps.Counts * phProps.dt);
		rope->Nodes[0].LoadType = NodeLoad::p;
		//rope->Nodes[midN].LoadType = NodeLoad::p;
		rope->Nodes[lastN].LoadType = NodeLoad::u;
		for (int t = 0; t < phProps.Counts; t++)
		{
			rope->Nodes[0].p[t] = rope->Nodes[0].p[0];
			rope->Nodes[lastN].p[t] = rope->Nodes[lastN].p[0];
			/*rope->Nodes[midN].p[t] = rope->Nodes[midN].p[0];
			rope->Nodes[midN].p[t].y = rope->Nodes[midN].p[0].y + (-phProps.MaxU * sinf(2 * (float)M_PI * time[t] * freq / 4));*/

			rope->Nodes[lastN].p[t].x = (phProps.MaxU * sinf(2 * (float)M_PI * time[t] * freq / 2)) + rope->Nodes[lastN].p[0].x;
			rope->Nodes[lastN].u[t] = rope->Nodes[lastN].p[t] - rope->Nodes[lastN].p[0];
		}
	}
	void Enviro::Run(bool NeedToSaveResults)
	{
		for (int t = 1; t < phProps.Counts; t++)
		{
			rope->StepOverElems(t, Re[t - 1], bloodV[t - 1], bloodP[t - 1]);
			rope->StepOverNodes(t, Re[t - 1], phProps.dt);
			for (int i = 0; i < rope->ElementsSize; i++)
			{
				rope->L[t] += rope->Elements[i].L[t];
			}
		}
		if (NeedToSaveResults)
		{
			std::string varName = "L";
			switch (phProps.phMod)
			{
			case PhModels::hook:
				varName += "hook";
				break;
			case PhModels::hookGeomNon:
				varName += "hookGeomNon";
				break;
			case PhModels::mooneyRiv:
				varName += "mooneyRiv";
				break;
			default:
				varName += "";
				break;
			}
			std::string resultsFileName = varName;
			if (loadFile.size() > 0)
			{
				resultsFileName += loadFile.substr(loadFile.size() - 3, 3);
			}
			resultsFileName += ".mat";
			matioWrap::writeFloatArr(resultsFileName.c_str(), varName.c_str(), rope->L);
		}
	}
};