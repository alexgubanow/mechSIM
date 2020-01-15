#pragma once
#include "xyz_t.h"
#include <corecrt_math.h>
#include "maf.h"
class crds
{
public:
	static float GetTotL(xyz_t zeroP, xyz_t targetP)
	{
		//return _getTotL(zeroP.x, zeroP.y, zeroP.z, targetP.x, targetP.y, targetP.z);
		return sqrtf(maf::P2(targetP.x - zeroP.x) + maf::P2(targetP.y - zeroP.y) + maf::P2(targetP.z - zeroP.z));
	};
	static float GetTotL(float x1, float y1, float z1, float x2, float y2, float z2)
	{
		return sqrtf(maf::P2(x2 - x1) + maf::P2(y2 - y1) + maf::P2(z2 - z1));
	};
	static float GetTotL(xyz_t targetP)
	{
		//return _getTotL(targetP.x, targetP.y, targetP.z);
		return sqrtf(maf::P2(targetP.x) + maf::P2(targetP.y) + maf::P2(targetP.z));
	};
};