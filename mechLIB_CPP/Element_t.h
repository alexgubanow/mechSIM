#pragma once
#include "props_t.h"
#include <d3d11_1.h>
#include <DirectXMath.h>
#include "SimpleMath.h"

class Rope_t;

class Element_t
{

private:
	mechLIB_CPP::props_t* props;
	float A;
	float I;
public:
	Node_t* n1;
	Node_t* n2;
	std::vector<float> L;
	std::vector<DirectX::SimpleMath::Vector3> F;
	Element_t() : props(nullptr), A(0), I(0), n1(0), n2(0), L(0), F(0) { }
	~Element_t() { }
	void init(Node_t* _n1, Node_t* _n2, mechLIB_CPP::props_t* _props);
	void CalcForce(Node_t* baseNode, size_t t, float Re, float bloodV, float bloodP);
	void GetForceForNode(size_t t, Node_t* baseP, DirectX::SimpleMath::Vector3& force);
	void GetPhysicParam(size_t t, float Re, float& m, float& c);
	float GetOwnLength(size_t t);
	void GetFn(size_t t, DirectX::SimpleMath::Vector3 deltaL, DirectX::SimpleMath::Vector3& force);
	void calcHookFn(DirectX::SimpleMath::Vector3& Fn, float oldL, DirectX::SimpleMath::Vector3 deltaL);
	void calcMooneyRivlinFn(DirectX::SimpleMath::Vector3& Fn, float oldL, DirectX::SimpleMath::Vector3 deltaL);
	void GetPressureForce(size_t t, float bloodP, float L);
	void GetDragForce(size_t t, float Re, float v, float L, DirectX::SimpleMath::Vector3& force);
};