#pragma once
#include "Rope_t.h"
#include "../mechLIB_CPPWrapper/props_t.h"
#include "../mechLIB_CPPWrapper/C_t.h"
#include <string>
#include <vector>

namespace mechLIB_CPP
{
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
        mechLIB_CPPWrapper::props_t phProps;
        Rope_t* rope;
        std::vector<float> time;
        Enviro(mechLIB_CPPWrapper::props_t _phProps, std::string &_loadFile);
        void GenerateLoad(mechLIB_CPPWrapper::C_t axis);
        ~Enviro()
        {
            delete rope;
        }
        void Run();
    };
}