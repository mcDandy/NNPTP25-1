﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using NNPTPZ1.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NNPTPZ1;

namespace NNPTPZ1.Mathematics.Tests
{
    [TestClass()]
    public class CplxTests
    {

        [TestMethod()]
        public void AddTest()
        {
            Cplx a = new Cplx()
            {
                Real = 10,
                Imaginary = 20
            };
            Cplx b = new Cplx()
            {
                Real = 1,
                Imaginary = 2
            };

            Cplx actual = a.Add(b);
            Cplx shouldBe = new Cplx()
            {
                Real = 11,
                Imaginary = 22
            };

            Assert.AreEqual(shouldBe, actual);

            string e2 = "(10 + 20i)";
            string r2 = a.ToString();
            Assert.AreEqual(e2, r2);
            e2 = "(1 + 2i)";
            r2 = b.ToString();
            Assert.AreEqual(e2, r2);

            a = new Cplx()
            {
                Real = 1,
                Imaginary = -1
            };
            b = new Cplx() { Real = 0, Imaginary = 0 };
            shouldBe = new Cplx() { Real = 1, Imaginary = -1 };
            actual = a.Add(b);
            Assert.AreEqual(shouldBe, actual);

            e2 = "(1 + -1i)";
            r2 = a.ToString();
            Assert.AreEqual(e2, r2);

            e2 = "(0 + 0i)";
            r2 = b.ToString();
            Assert.AreEqual(e2, r2);
        }

        [TestMethod()]
        public void AddTestPolynome()
        {
            Poly poly = new Mathematics.Poly();
            poly.Coefficients.Add(new Cplx() { Real = 1, Imaginary = 0 });
            poly.Coefficients.Add(new Cplx() { Real = 0, Imaginary = 0 });
            poly.Coefficients.Add(new Cplx() { Real = 1, Imaginary = 0 });
            Cplx result = poly.Eval(new Cplx() { Real = 0, Imaginary = 0 });
            Cplx expected = new Cplx() { Real = 1, Imaginary = 0 };
            Assert.AreEqual(expected, result);
            result = poly.Eval(new Cplx() { Real = 1, Imaginary = 0 });
            expected = new Cplx() { Real = 2, Imaginary = 0 };
            Assert.AreEqual(expected, result);
            result = poly.Eval(new Cplx() { Real = 2, Imaginary = 0 });
            expected = new Cplx() { Real = 5.0000000000, Imaginary = 0 };
            Assert.AreEqual(expected, result);

            string r2 = poly.ToString();
            string e2 = "(1 + 0i) + (0 + 0i)x + (1 + 0i)xx";
            Assert.AreEqual(e2, r2);
        }
    }
}


