#pragma once
#include "coords.hpp"
#include <d3d11_1.h>
#include <DirectXMath.h>
#include "SimpleMath.h"

class dcm_t
{
public:
	float Xx;
	float Yx;
	float Zx;

	float Xy;
	float Yy;
	float Zy;

	float Xz;
	float Yz;
	float Zz; 
	dcm_t(DirectX::SimpleMath::Vector3 endPoint, DirectX::SimpleMath::Vector3 radiusPoint)
	{
		float lax = crds::GetTotL(endPoint);
		float lby = crds::GetTotL(radiusPoint);
		Xx = endPoint.x / lax;
		Yx = endPoint.y / lax;
		Zx = endPoint.z / lax;
		Xy = radiusPoint.x / lby;
		Yy = radiusPoint.y / lby;
		Zy = radiusPoint.z / lby;
		Xz = Yx * Zy - Zx * Yy;
		Yz = 0 - (Xx * Zy - Zx * Xy);
		Zz = Xx * Yy - Yx * Xy;
	}

	inline void ToGlob(DirectX::SimpleMath::Vector3 Lp, DirectX::SimpleMath::Vector3& gA)
	{
		gA.x = Xx * Lp.x + Xy * Lp.y + Xz * Lp.z;
		gA.y = Yx * Lp.x + Yy * Lp.y + Yz * Lp.z;
		gA.z = Zx * Lp.x + Zy * Lp.y + Zz * Lp.z;
	}

	inline void ToLoc(DirectX::SimpleMath::Vector3 Gp, DirectX::SimpleMath::Vector3& lA)
	{
		lA.x = Xx * Gp.x + Yx * Gp.y + Zx * Gp.z;
		lA.y = Xy * Gp.x + Yy * Gp.y + Zy * Gp.z;
		lA.z = Xz * Gp.x + Yz * Gp.y + Zz * Gp.z;
	}
};