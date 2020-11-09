#pragma once
#include <d3d11_1.h>
#include <DirectXMath.h>
#include "SimpleMath.h"
using namespace DirectX::SimpleMath;

class dcm_tLEGACY
{
	float Xx;
	float Yx;
	float Zx;

	float Xy;
	float Yy;
	float Zy;

	float Xz;
	float Yz;
	float Zz;
public:
	dcm_tLEGACY() : Xx(0), Yx(0), Zx(0), Xy(0), Yy(0), Zy(0), Xz(0), Yz(0), Zz(0) {}

	dcm_tLEGACY(Vector3 endPoint, Vector3 radiusPoint)
	{
		float el = endPoint.Length();
		float rl = radiusPoint.Length();
		Xx = endPoint.x / el;
		Yx = endPoint.y / el;
		Zx = endPoint.z / el;

		Xy = radiusPoint.x / rl;
		Yy = radiusPoint.y / rl;
		Zy = radiusPoint.z / rl;

		Xz = Yx * Zy - Zx * Yy;
		Yz = 0 - (Xx * Zy - Zx * Xy);
		Zz = Xx * Yy - Yx * Xy;
		/*if (Zz != 1 && Zz != -1)
		{
			throw "Calculated rotation matrix are wrong";
		}*/
	}

	bool IsZzEqualOne() { return Zz == 1 || Zz == -1; }

	void ToGlob(Vector3 Lp, Vector3& gA)
	{
		gA.x = Xx * Lp.x + Xy * Lp.y + Xz * Lp.z;
		gA.y = Yx * Lp.x + Yy * Lp.y + Yz * Lp.z;
		gA.z = Zx * Lp.x + Zy * Lp.y + Zz * Lp.z;
	}

	void ToLoc(Vector3 Gp, Vector3& lA)
	{
		lA.x = Xx * Gp.x + Yx * Gp.y + Zx * Gp.z;
		lA.y = Xy * Gp.x + Yy * Gp.y + Zy * Gp.z;
		lA.z = Xz * Gp.x + Yz * Gp.y + Zz * Gp.z;
	}
	Vector3 ToGlob(Vector3 Lp)
	{
		Vector3 gA;
		gA.x = Xx * Lp.x + Xy * Lp.y + Xz * Lp.z;
		gA.y = Yx * Lp.x + Yy * Lp.y + Yz * Lp.z;
		gA.z = Zx * Lp.x + Zy * Lp.y + Zz * Lp.z;
		return gA;
	}

	Vector3 ToLoc(Vector3 Gp)
	{
		Vector3 lA;
		lA.x = Xx * Gp.x + Yx * Gp.y + Zx * Gp.z;
		lA.y = Xy * Gp.x + Yy * Gp.y + Zy * Gp.z;
		lA.z = Xz * Gp.x + Yz * Gp.y + Zz * Gp.z;
		return lA;
	}
};
class dcm_t
{
	Matrix matrix;
	Matrix matrixT;
public:
	dcm_t() {}

	dcm_t(const Vector3& endPoint, const Vector3& radiusPoint)
	{
		Vector3 x, y, z;
		endPoint.Normalize(x);
		radiusPoint.Normalize(y);
		x.Cross(y, z);
		matrix = Matrix(x, y, z);
		matrixT = matrix.Transpose();
	}
	bool IsZzEqualOne() { return matrix._33 == 1 || matrix._33 == -1; }

	void ToGlob(const Vector3& src, Vector3& dst)
	{
		Vector3::Transform(src, matrix, dst);
	}

	void ToLoc(const Vector3& src, Vector3& dst)
	{
		Vector3::Transform(src, matrixT, dst);
	}
	Vector3 ToGlob(const Vector3& src)
	{
		return Vector3::Transform(src, matrix);
	}

	Vector3 ToLoc(const Vector3& src)
	{
		return Vector3::Transform(src, matrixT);
	}
};