#pragma once
#include "Rope_t.h"
#include "../mechLIB_CPPWrapper/props_t.h"
#include "../mechLIB_CPPWrapper/C_t.h"
#include <string>

namespace mechLIB_CPP
{
    class Enviro
    {
    private:
        /*float[] Re;
        float[] bloodV;
        float[] bloodP;*/
        void allocateTime(float dt, int Counts)
        {
            time = new float[Counts];
            for (int i = 1; i < Counts; i++)
            {
                time[i] = time[i - 1] + dt;
            }
        }

    public:
        mechLIB_CPPWrapper::props_t phProps;
        Rope_t* rope;
        float* time;
        Enviro(mechLIB_CPPWrapper::props_t _phProps, std::string loadFile);
        void GenerateLoad(mechLIB_CPPWrapper::C_t axis);
        ~Enviro()
        {
            delete rope;
            delete time;
        }
        void Run()
        {
            /*for (int t = 1; t < time.Length; t++)
            {
                rope.StepOverElems(t, Re[t - 1], bloodV[t - 1], bloodP[t - 1]);
                rope.StepOverNodes(t, Re[t - 1], phProps.dt);
                foreach(var elem in rope.Elements)
                {
                    rope.L[t] += elem.L[t];
                }
            }*/
        }
    };
}