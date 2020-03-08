#pragma once
#include <d3d11_1.h>
#include <DirectXMath.h>
#include "SimpleMath.h"
struct deriv_t
{
	//coordinates
	DirectX::SimpleMath::Vector3 p;
	//displacement
	DirectX::SimpleMath::Vector3 u;
	//velocity
	DirectX::SimpleMath::Vector3 v;
	//acceleration
	DirectX::SimpleMath::Vector3 a;
};