using spring;
using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace spring.Tests
{
    [TestClass()]
    public class UnitTest1
    {
        [TestMethod()]
        public void GetDCMTest()
        {
            float[] dcm = crds.GetDCM(new float[3] { 1,0,0 }, new float[3] { 0, 0, 1 });
            CollectionAssert.AreEqual(new float[9] { 1, 0, 0, 0, 0, 1, 0, -1, 0 }, dcm);
        }
    }
}