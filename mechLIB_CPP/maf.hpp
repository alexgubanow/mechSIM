#pragma once
constexpr auto _g = -9.80666f;
constexpr auto hlf = 1 / 2;
constexpr auto sxt = 1 / 6;
class maf
{
public:

	static inline float P2(float value)
	{
		return value * value;
	}

	static inline float P3(float value)
	{
		return value * value * value;
	}

	static inline float P4(float value)
	{
		return value * value * value * value;
	}
};