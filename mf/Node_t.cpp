#include "Node_t.h"
#include "maf.h"
#include "dcm_t.h"
#include "integr.h"

Node_t::Node_t(int tCounts, xyz_t coords, xyz_t _radiusPoint, NodeFreedom _freedom, NodeLoad _LoadType, int _ID, std::vector<int> _Neigs)
{
    ID = _ID;
    freedom = _freedom;
    LoadType = _LoadType;
    Neigs = _Neigs;
    F.reserve(tCounts);
    deriv.reserve(tCounts);
    for (int i = 0; i < tCounts; i++)
    {
        F.push_back(xyz_t());
        deriv.push_back(deriv_t());
    }
    deriv[0].p = coords;
    deriv[0].a.y = _g;
    deriv[0].v.y = _g * 5E-06f;
    radiusPoint = _radiusPoint;
}
void Node_t::CalcAccel(int t, float m)
{
    if (LoadType == NodeLoad::none || LoadType == NodeLoad::f)
    {
        deriv[t].a.x = F[t].x / m;
        deriv[t].a.y = F[t].y / m;
        deriv[t].a.z = F[t].z / m;//has to be different
    }

}
void Node_t::GetForces(Rope_t* model, int t, float m, float c)
{
    //xyz_t Fg = new xyz_t();
    //Fg.y = m * maf._g;
    //F[t].Plus(Fg);

    xyz_t Fd;
    Fd.x = 0 - (c * deriv[t - 1].v.x);
    Fd.y = 0 - (c * deriv[t - 1].v.y);
    Fd.z = 0 - (c * deriv[t - 1].v.z);
    F[t].Plus(Fd);
    /*getting element forces*/
    for (auto neigNode : Neigs)
    {
        //getting position of link according base point
        xyz_t LinkPos;
        LinkPos.Minus(deriv[t - 1].p, (model->GetNodeRef(neigNode))->deriv[t - 1].p);
        //getting DCM for this link
        dcm_t* dcm = new dcm_t(LinkPos, radiusPoint);
        xyz_t gFn;
        //convert Fn to global coords and return
        dcm->ToGlob((model->GetElemRef(ID, neigNode))->F[t], gFn);
        //push it to this force pull
        F[t].Plus(gFn);
    }
}
void Node_t::GetPhysicParam(Rope_t* rope, int t, float Re, float& m, float& c)
{
    for (auto neigNode : Neigs)
    {
        float mElem = 0;
        float cElem = 0;
        (rope->GetElemRef(ID, neigNode))->GetPhysicParam(rope, t, Re, mElem, cElem);
        c += cElem;
        m += mElem;
    }
    //m = 1E-04f;
    c /= Neigs.size();
    if (m <= 0)
    {
        throw "Calculated mass of node can't be eaqul to zero";
    }
}
void Node_t::Integrate(int now, int before, float dt)
{
    switch (LoadType)
    {
    case NodeLoad::p:
        Integr::EulerExplBackward(deriv[now], deriv[before], dt);
        break;
    case NodeLoad::none:
        Integr::VerletForward(deriv[now], deriv[before], dt);
        break;
    case NodeLoad::f:
        Integr::VerletForward(deriv[now], deriv[before], dt);
        break;
    default:
        throw "Unexpected load type";
    }
}