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
    Enviro::Enviro(mechLIB_CPPWrapper::props_t _phProps, std::string &_loadFile) : loadFile(_loadFile), phProps(_phProps)
    {
        if (loadFile.size() > 0)
        {
            matioWrap* matioW = NULL;
            matioW = new matioWrap(loadFile);
            if (matioW)
            {
                matioW->readFloatArrFromMAT("pmxq", pmxq);
                matioW->readFloatArrFromMAT("plxq", plxq);
                matioW->readFloatArrFromMAT("pmyq", pmyq);
                matioW->readFloatArrFromMAT("plyq", plyq);
                matioW->readFloatArrFromMAT("tq", time);
                matioW->readFloatArrFromMAT("req", Re);
                matioW->readFloatArrFromMAT("bloodVq", bloodV);
                matioW->readFloatArrFromMAT("abpq", bloodP);

                rope = new Rope_t();
                rope->init(&phProps);
                rope->SetupNodesPositions(&phProps, DirectX::SimpleMath::Vector3(pmxq[0], pmyq[0], 0),
                    DirectX::SimpleMath::Vector3(plxq[0], plyq[0], 0));
                rope->EvalElements(&phProps);

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
            GenerateLoad(mechLIB_CPPWrapper::C_t::x);
        }
    }
    void Enviro::GenerateLoad(mechLIB_CPPWrapper::C_t axis)
    {
        Re = std::vector<float>(phProps.Counts);
        bloodV = std::vector<float>(phProps.Counts);
        bloodP = std::vector<float>(phProps.Counts);
        float A = (float)M_PI * maf::P2(phProps.D) / 4;
        float maxLoad = ((phProps.E * A) / phProps.L / phProps.nodes) * phProps.MaxU;
        float freq = 1 / (phProps.Counts * phProps.dt);
        rope->Nodes[0].LoadType = mechLIB_CPPWrapper::NodeLoad::p;
        rope->Nodes[phProps.nodes - 1].LoadType = mechLIB_CPPWrapper::NodeLoad::u;
        int lastN = phProps.nodes - 1;
        for (int t = 0; t < phProps.Counts; t++)
        {
        rope->Nodes[0].p[t] = rope->Nodes[0].p[0];
        rope->Nodes[lastN].p[t] = rope->Nodes[lastN].p[0];
        //    Re[t] = 0;
        //    bloodV[t] = 0;
        //    bloodP[t] = 0;
        //    rope->Nodes[0].F[t].x = 0 - ((float)Math.Sin(2 * Math.PI * 0.5 * time[t] * freq) * maxLoad);
        //    //model.Nodes[0].deriv[t].p.z = model.Nodes[0].deriv[0].p.z;
        //    //model.Nodes[0].deriv[t].p.y = model.Nodes[0].deriv[0].p.y;
        //    //model.Nodes[0].deriv[t].p.x = 0 - ((time[t] + time[1]) * phProps.MaxU);
        //    //model.Nodes[0].deriv[t].v.x = (model.Nodes[0].deriv[t].p.x - (0 - (time[t] * phProps.MaxU))) / time[1];
        //    int lastN = phProps.nodes - 1;
        //    rope->Nodes[lastN].deriv[t].p.z = rope->Nodes[lastN].deriv[0].p.z;
        //    rope->Nodes[lastN].deriv[t].p.y = rope->Nodes[lastN].deriv[0].p.y;
        //    //model.Nodes[lastN].F[t].x = ((float)Math.Sin(2 * Math.PI * 0.5 * time[t] * freq) * maxLoad);
        //rope->Nodes[lastN].deriv[t].p.X = rope->Nodes[lastN].deriv[0].p.X;
        //rope->Nodes[lastN].deriv[t].p.X = 0 - ((float)Math.Sin(2 * Math.PI * time[t] * freq / 3) * phProps.MaxU) + rope->Nodes[lastN].deriv[0].p.X;



        rope->Nodes[lastN].p[t].x = (phProps.MaxU * sinf(2 * (float)M_PI * time[t] * freq / 2)) + rope->Nodes[lastN].p[0].x;
        rope->Nodes[lastN].u[t] = rope->Nodes[lastN].p[t] - rope->Nodes[lastN].p[0];


        //rope->Nodes[lastN].deriv[t].v.X = ((float)Math.Cos(2 * Math.PI * 0.5 * time[t] * freq) * phProps.MaxU) + rope->Nodes[lastN].deriv[0].v.X;
        }
    }
    void Enviro::Run()
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
    }
};