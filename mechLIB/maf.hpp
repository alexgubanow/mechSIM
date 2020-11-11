#pragma once
constexpr float _g = -9.80666f;
constexpr float hlf = 1.0f / 2.0f;
constexpr float sxt = 1.0f / 6.0f;
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