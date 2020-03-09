#include "pch.h"

#include "mechLIB_CPPWrapper.h"
#include <msclr\marshal_cppstd.h>

mechLIB_CPPWrapper::Enviro::Enviro()
{
}

void mechLIB_CPPWrapper::Enviro::CreateWorld(float DampRatio, float MaxU, float initDrop, int nodes, float E, float L, 
	float D, int Counts, float dt, float ro, mechLIB_CPPWrapper::PhModels phMod, System::String^ loadFile)
{
	props_t phProps;
	phProps.DampRatio = DampRatio;
	phProps.MaxU = MaxU;
	phProps.initDrop = initDrop;
	phProps.nodes = nodes;
	phProps.E = E;
	phProps.L = L;
	phProps.D = D;
	phProps.Counts = Counts;
	phProps.dt = dt;
	phProps.ro = ro;
	phProps.phMod = phMod;
	world = new mechLIB_CPP::Enviro(phProps, msclr::interop::marshal_as<std::string>(loadFile));
}

void mechLIB_CPPWrapper::Enviro::Run()
{
	world->Run();
}

void mechLIB_CPPWrapper::Enviro::GetRopeL(array<float>^% L)
{
	float sxs = world->rope->Elements[0].F[10].x;
	float dsv = world->rope->L[1];
	float gbf = world->rope->L[2];
	float we = world->rope->L[3];
	L = gcnew array<float>(world->phProps.Counts);
	pin_ptr<float> pinned1 = &L[0];
	pin_ptr<float> pinned2 = &world->rope->L[0];
	std::memcpy(pinned1, pinned2, world->phProps.Counts);
}
