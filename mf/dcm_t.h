#pragma once
#include "xyz_t.h"

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
	dcm_t(xyz_t endPoint, xyz_t radiusPoint);
	void ToGlob(xyz_t Lp, xyz_t& gA);
	void ToLoc(xyz_t Gp, xyz_t& lA);
};