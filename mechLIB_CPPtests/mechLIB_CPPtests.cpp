#include "pch.h"
#include "CppUnitTest.h"
#include "../mechLIB_CPP/dcm_t.hpp"
#include <iostream>

using namespace Microsoft::VisualStudio::CppUnitTestFramework;
using namespace DirectX::SimpleMath;
using namespace std;

namespace mechLIBCPPtests
{
	TEST_CLASS(dcm_ttests)
	{
	public:

		TEST_METHOD(dcm_t_ctorTest)
		{
			dcm_tLEGACY dcm(Vector3(75.02f, 25.09f, 0), Vector3(-4.4871f, 13.4166f, 0));
			Assert::IsTrue(dcm.IsZzEqualOne());
		}
		TEST_METHOD(dcm_tNEW_ctorTest)
		{
			dcm_t dcm(Vector3(75.02f, 25.09f, 0), Vector3(-4.4871f, 13.4166f, 0));
			Assert::IsTrue(dcm.IsZzEqualOne());
		}
		TEST_METHOD(dcm_tNEW_and_dcm_tGivesSameResult)
		{
			dcm_tLEGACY dcm(Vector3(75.02f, 25.09f, 0), Vector3(-4.4871f, 13.4166f, 0));
			Assert::IsTrue(dcm.IsZzEqualOne());
			dcm_t dcmNEW(Vector3(75.02f, 25.09f, 0), Vector3(-4.4871f, 13.4166f, 0));
			Assert::IsTrue(dcmNEW.IsZzEqualOne());
			Vector3 inGlob = dcm.ToGlob(Vector3(50, 0, 0));
			Vector3 inGlobNEW = dcmNEW.ToGlob(Vector3(50, 0, 0));
			Assert::AreEqual(inGlob.x, inGlobNEW.x);
			Assert::AreEqual(inGlob.y, inGlobNEW.y);
			Assert::AreEqual(inGlob.z, inGlobNEW.z);
		}
	};
}
