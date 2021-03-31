#pragma once
#include <d3d11_1.h>
#include <SimpleMath.h>
#include <msclr\marshal.h>

namespace mechLIB {
	public ref class Vector3Managed
	{
	public:
		float x;
		float y;
		float z;
		Vector3Managed(const DirectX::SimpleMath::Vector3& src)
		{
			x = src.x;
			y = src.y;
			z = src.z;
		}
	};
};