#pragma once

#include <d3d11_1.h>
#include "SimpleMath.h"
#include <vector>

struct DerivativesContainer
{
	DirectX::SimpleMath::Vector3 p;
	DirectX::SimpleMath::Vector3 u;
	DirectX::SimpleMath::Vector3 v;
	DirectX::SimpleMath::Vector3 a;
	DirectX::SimpleMath::Vector3 F;
};

