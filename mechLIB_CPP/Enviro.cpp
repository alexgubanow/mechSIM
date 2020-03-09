#include "pch.h"
#include "enviro.h"
#include "mat.h"
namespace mechLIB_CPP
{
    Enviro::Enviro(mechLIB_CPPWrapper::props_t _phProps, std::string loadFile)
    {
        phProps = _phProps;
        allocateTime(phProps.dt, phProps.Counts);
        rope = new Rope_t(&phProps);
        MATFile* pmat = nullptr;
        pmat = matOpen(loadFile.c_str(), "r");
        if (pmat == NULL) {
            printf("Error reopening file %s\n", loadFile.c_str());
            //return(EXIT_FAILURE);
        }
        GenerateLoad(mechLIB_CPPWrapper::C_t::x);
    }
    void Enviro::GenerateLoad(mechLIB_CPPWrapper::C_t axis)
    {
        /*Re = new float[phProps.Counts];
        bloodV = new float[phProps.Counts];
        bloodP = new float[phProps.Counts];
        float A = (float)Math.PI * maf.P2(phProps.D) / 4;
        float maxLoad = ((phProps.E * A) / phProps.L / phProps.nodes) * phProps.MaxU;
        float freq = 1 / (phProps.Counts * phProps.dt);
        rope.Nodes[0].LoadType = NodeLoad.p;
        rope.Nodes[phProps.nodes - 1].LoadType = NodeLoad.u;
        int lastN = phProps.nodes - 1;
        for (int t = 0; t < phProps.Counts; t++)
        {
        rope.Nodes[0].deriv[t].p = rope.Nodes[0].deriv[0].p;
        rope.Nodes[lastN].deriv[t].p = rope.Nodes[lastN].deriv[0].p;
        //    Re[t] = 0;
        //    bloodV[t] = 0;
        //    bloodP[t] = 0;
        //    rope.Nodes[0].F[t].x = 0 - ((float)Math.Sin(2 * Math.PI * 0.5 * time[t] * freq) * maxLoad);
        //    //model.Nodes[0].deriv[t].p.z = model.Nodes[0].deriv[0].p.z;
        //    //model.Nodes[0].deriv[t].p.y = model.Nodes[0].deriv[0].p.y;
        //    //model.Nodes[0].deriv[t].p.x = 0 - ((time[t] + time[1]) * phProps.MaxU);
        //    //model.Nodes[0].deriv[t].v.x = (model.Nodes[0].deriv[t].p.x - (0 - (time[t] * phProps.MaxU))) / time[1];
        //    int lastN = phProps.nodes - 1;
        //    rope.Nodes[lastN].deriv[t].p.z = rope.Nodes[lastN].deriv[0].p.z;
        //    rope.Nodes[lastN].deriv[t].p.y = rope.Nodes[lastN].deriv[0].p.y;
        //    //model.Nodes[lastN].F[t].x = ((float)Math.Sin(2 * Math.PI * 0.5 * time[t] * freq) * maxLoad);
        //rope.Nodes[lastN].deriv[t].p.X = rope.Nodes[lastN].deriv[0].p.X;
        //rope.Nodes[lastN].deriv[t].p.X = 0 - ((float)Math.Sin(2 * Math.PI * time[t] * freq / 3) * phProps.MaxU) + rope.Nodes[lastN].deriv[0].p.X;
        rope.Nodes[lastN].deriv[t].p.Y = ((float)Math.Sin(2 * Math.PI * time[t] * freq) * phProps.MaxU) + rope.Nodes[lastN].deriv[0].p.Y;
        rope.Nodes[lastN].deriv[t].u = rope.Nodes[lastN].deriv[t].p - rope.Nodes[lastN].deriv[0].p;
        //rope.Nodes[lastN].deriv[t].v.X = ((float)Math.Cos(2 * Math.PI * 0.5 * time[t] * freq) * phProps.MaxU) + rope.Nodes[lastN].deriv[0].v.X;
        }*/
    }
};