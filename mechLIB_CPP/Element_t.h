#pragma once
#include "../mechLIB_CPPWrapper/props_t.h"
#include <d3d11_1.h>
#include <DirectXMath.h>
#include "SimpleMath.h"

class Rope_t;

class Element_t
{

private:
	mechLIB_CPPWrapper::props_t* props;
	float A;
	float I;
public:
	int n1;
	int n2;
	int ID;
	std::vector<float> L;
	std::vector<DirectX::SimpleMath::Vector3> F;
	DirectX::SimpleMath::Vector3 radiusPoint;
	Element_t() : props(nullptr), A(0), I(0), n1(0), n2(0), ID(), L(0), F(0), radiusPoint() { }
	~Element_t() { }
	void init(int _n1, int _n2, int Counts, int _ID, mechLIB_CPPWrapper::props_t* _props);
	inline bool IsMyNode(int id) { return (n1 == id || n2 == id) ? true : false; };
	void CalcForce(Rope_t* rope, int t, float Re, float bloodV, float bloodP);
	void GetPhysicParam(Rope_t* rope, int t, float Re, float& m, float& c);
	float GetOwnLength(Rope_t* rope, int t);
	void GetFn(int t, DirectX::SimpleMath::Vector3 deltaL, DirectX::SimpleMath::Vector3& force);
	void calcHookFn(DirectX::SimpleMath::Vector3& Fn, float oldL, DirectX::SimpleMath::Vector3 deltaL);
	void calcMooneyRivlinFn(DirectX::SimpleMath::Vector3& Fn, float oldL, DirectX::SimpleMath::Vector3 deltaL);
	void GetPressureForce(int t, float bloodP, float L);
	void GetDragForce(int t, float Re, float v, float L, DirectX::SimpleMath::Vector3& force);
};