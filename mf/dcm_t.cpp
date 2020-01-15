#include "dcm_t.h"
#include "coords.h"

dcm_t::dcm_t(xyz_t endPoint, xyz_t radiusPoint)
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

inline void dcm_t::ToGlob(xyz_t Lp, xyz_t& gA)
{
	gA.x = Xx * Lp.x + Xy * Lp.y + Xz * Lp.z;
	gA.y = Yx * Lp.x + Yy * Lp.y + Yz * Lp.z;
	gA.z = Zx * Lp.x + Zy * Lp.y + Zz * Lp.z;
}

inline void dcm_t::ToLoc(xyz_t Gp, xyz_t& lA)
{
	lA.x = Xx * Gp.x + Yx * Gp.y + Zx * Gp.z;
	lA.y = Xy * Gp.x + Yy * Gp.y + Zy * Gp.z;
	lA.z = Xz * Gp.x + Yz * Gp.y + Zz * Gp.z;
}
