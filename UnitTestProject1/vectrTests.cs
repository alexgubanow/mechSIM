using Microsoft.VisualStudio.TestTools.UnitTesting;
using spring;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace spring.Tests
{
    [TestClass()]
    public class vectrTests
    {
        [TestMethod()]
        public void InvertTest()
        {
            float[] vec = new float[1] { 1 };
            vectr.Invert(ref vec);
            CollectionAssert.AreEqual(new float[1] { -1 }, vec);
        }
        [TestMethod()]
        public void PlusNumTest()
        {
            float[] vec = new float[1] { 1 };
            vectr.Plus(ref vec, 1);
            CollectionAssert.AreEqual(new float[1] { 2 }, vec);
        }
        [TestMethod()]
        public void PlusVecTest()
        {
            float[] vec = new float[1] { 1 };
            float[] arg = new float[1] { 1 };
            vectr.Plus(ref vec, arg);
            CollectionAssert.AreEqual(new float[1] { 2 }, vec);
        }
        [TestMethod()]
        public void PlusVecTwoTest()
        {
            float[] vec = new float[1] { 1 };
            float[] arg = new float[1] { 1 };
            float[] res = vectr.Plus(vec, arg);
            CollectionAssert.AreEqual(new float[1] { 2 }, res);
        }
        [TestMethod()]
        public void MinusNumTest()
        {
            float[] vec = new float[1] { 2 };
            vectr.Minus(ref vec, 1);
            CollectionAssert.AreEqual(new float[1] { 1 }, vec);
        }
        [TestMethod()]
        public void MinusVecTest()
        {
            float[] vec = new float[1] { 2 };
            float[] arg = new float[1] { 1 };
            vectr.Minus(ref vec, arg);
            CollectionAssert.AreEqual(new float[1] { 1 }, vec);
        }
        [TestMethod()]
        public void MinusVecTwoTest()
        {
            float[] vec = new float[1] { 2 };
            float[] arg = new float[1] { 1 };
            float[] res = vectr.Minus(vec, arg);
            CollectionAssert.AreEqual(new float[1] { 1 }, res);
        }
    }
}