using spring;
using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace spring.Tests
{
    [TestClass()]
    public class ToLocUnitTest1
    {
        [TestMethod()]
        public void ToLocTest110_10()
        {
            float[] dcm = crds.GetDCM(new float[3] { 1, 1, 0 }, new float[3] { 0, 0, 1 });
            float[] lBpUx = crds.ToLoc(dcm, new float[3] { 10, 0, 0 });

            CollectionAssert.AreEqual(new float[3] { 7.071068f, 0, 7.071068f }, lBpUx);
        }
    }
}