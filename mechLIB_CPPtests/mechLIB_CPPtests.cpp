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
			try
			{
				dcm_tLEGACY dcm(Vector3(75.02f, 25.09f, 0), Vector3(-4.4871f, 13.4166f, 0));
			}
			catch (const std::exception& ex)
			{
				cout << ex.what() << endl;
			}
		}
		TEST_METHOD(dcm_tNEW_ctorTest)
		{
			try
			{
				dcm_t dcm(Vector3(75.02f, 25.09f, 0), Vector3(-4.4871f, 13.4166f, 0));
			}
			catch (const std::exception& ex)
			{
				cout << ex.what() << endl;
			}
		}
		TEST_METHOD(dcm_tNEW_and_dcm_tGivesSameResult)
		{
			try
			{
				dcm_tLEGACY dcm(Vector3(75.02f, 25.09f, 0), Vector3(-4.4871f, 13.4166f, 0));
				dcm_t dcmNEW(Vector3(75.02f, 25.09f, 0), Vector3(-4.4871f, 13.4166f, 0));
				Vector3 inGlob = dcm.ToGlob(Vector3(50, 0, 0));
				Vector3 inGlobNEW = dcmNEW.ToGlob(Vector3(50, 0, 0));
				Assert::AreEqual(inGlob.x, inGlobNEW.x);
				Assert::AreEqual(inGlob.y, inGlobNEW.y);
				Assert::AreEqual(inGlob.z, inGlobNEW.z);
			}
			catch (const std::exception& ex)
			{
				cout << ex.what() << endl;
			}
		}
	};
}
