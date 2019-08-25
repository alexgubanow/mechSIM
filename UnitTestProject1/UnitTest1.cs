using spring;
using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace spring.Tests
{
    [TestClass()]
    public class UnitTest1
    {
        [TestMethod()]
        public void GetDCMTest1()
        {
            float[] dcm = crds.GetDCM(new float[3] { 1, 0, 0 }, new float[3] { 0, 0, 1 });
            CollectionAssert.AreEqual(new float[9] { 1, 0, 0, 0, 0, 1, 0, -1, 0 }, dcm);
        }
        [TestMethod()]
        public void GetDCMTest2()
        {
            float[] dcm = crds.GetDCM(new float[3] { 0, 1, 0 }, new float[3] { 0, 0, 1 });
            CollectionAssert.AreEqual(new float[9] { 0, 1, 0, 0, 0, 1, 1, 0, 0 }, dcm);
        }
        [TestMethod()]
        public void GetDCMTest3()
        {
            float[] dcm = crds.GetDCM(new float[3] { 0, 0, 1 }, new float[3] { 0, 0, 1 });
            CollectionAssert.AreEqual(new float[9] { 0, 0, 1, 0, 0, 1, 0, 0, 0 }, dcm);
        }
    }
}