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

	public ref class DerivativesContainerManaged
	{
		Vector3Managed^ _p;
		Vector3Managed^ _v;
		Vector3Managed^ _a;
		Vector3Managed^ _f;
	public:
		DerivativesContainerManaged(DerivativesContainer* _NativeContainerPtr)
		{
			p = gcnew Vector3Managed(_NativeContainerPtr->p);
			v = gcnew Vector3Managed(_NativeContainerPtr->v);
			a = gcnew Vector3Managed(_NativeContainerPtr->a);
			f = gcnew Vector3Managed(_NativeContainerPtr->F);
		}
		property Vector3Managed^ p { Vector3Managed^ get() { return _p; } void set(Vector3Managed^ value) { _p = value; } }
		property Vector3Managed^ v { Vector3Managed^ get() { return _v; } void set(Vector3Managed^ value) { _v = value; } }
		property Vector3Managed^ a { Vector3Managed^ get() { return _a; } void set(Vector3Managed^ value) { _a = value; } }
		property Vector3Managed^ f { Vector3Managed^ get() { return _f; } void set(Vector3Managed^ value) { _f = value; } }
	};
};