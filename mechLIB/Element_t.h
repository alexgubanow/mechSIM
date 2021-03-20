#pragma once
#include "ModelPropertiesNative.h"
#include <d3d11_1.h>
#include <DirectXMath.h>
#include "SimpleMath.h"

class Rope_t;

class Element_t
{

private:
	ModelPropertiesNative* modelProperties;
public:
	Node_t* n1;
	Node_t* n2;
	std::vector<float> L;
	std::vector<DirectX::SimpleMath::Vector3> F;
	~Element_t() { }
	void init(Node_t* _n1, Node_t* _n2, ModelPropertiesNative* _props);
	void GetForces(size_t t, float Re, float bloodV, float bloodP);
	void GetPhysicParam(size_t t, float Re, float& m);
	float GetOwnLength(size_t t);
	DirectX::SimpleMath::Vector3 GetFn(float oldL, float deltaL);
	DirectX::SimpleMath::Vector3 GetPressureForce(size_t t, float bloodP, float L);
	DirectX::SimpleMath::Vector3 GetDragForce(size_t t, float Re, float v, float L);
};