#pragma once
#include <corecrt_math.h>
#include "maf.hpp"
#include <d3d11_1.h>
#include <DirectXMath.h>
#include "SimpleMath.h"

class crds
{
public:
	static inline float GetTotL(DirectX::SimpleMath::Vector3 zeroP, DirectX::SimpleMath::Vector3 targetP)
	{
		return sqrtf(maf::P2(targetP.x - zeroP.x) + maf::P2(targetP.y - zeroP.y) + maf::P2(targetP.z - zeroP.z));
	};
	static inline float GetTotL(float x1, float y1, float z1, float x2, float y2, float z2)
	{
		return sqrtf(maf::P2(x2 - x1) + maf::P2(y2 - y1) + maf::P2(z2 - z1));
	};
	static inline float GetTotL(DirectX::SimpleMath::Vector3 targetP)
	{
		return sqrtf(maf::P2(targetP.x) + maf::P2(targetP.y) + maf::P2(targetP.z));
	};
};