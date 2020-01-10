#include "pch.h"
#include "mf.h"
#include <corecrt_math.h>

float _getTotL(float x1, float y1, float z1, float x2, float y2, float z2)
{
	return sqrtf(powf(x2 - x1, 2) + powf(y2 - y1, 2) + powf(z2 - z1, 2));
}

float _getTotL(float x, float y, float z)
{
	return sqrtf(powf(x, 2) + powf(y, 2) + powf(z, 2));
}
