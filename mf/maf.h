#pragma once
constexpr auto _g = -9.80666f;
constexpr auto hlf = 1 / 2;
constexpr auto sxt = 1 / 6;
class maf
{
public:

	static float P2(float value)
	{
		return value * value;
	}

	static float P3(float value)
	{
		return value * value * value;
	}

	static float P4(float value)
	{
		return value * value * value * value;
	}
};