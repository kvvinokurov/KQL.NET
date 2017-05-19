using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace KQL.NET.Tests
{
    [TestClass]
    public class TestParenthesisMethod
    {
        private const string ParenthesisPattern = "({0})";

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TestNullExpression()
        {
            KQL.Parenthesis(null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void TestEmptyExpression()
        {
            KQL.Parenthesis(string.Empty);
        }

        [TestMethod]
        public void TestWithNormalExpression()
        {
            const string exp = "1";
            var value = KQL.Parenthesis(exp);
            Assert.AreEqual(string.Format(ParenthesisPattern, exp), value);
        }

        [TestMethod]
        public void TestExpressionToTrim()
        {
            const string exp = " 1 ";
            var value = KQL.Parenthesis(exp);
            Assert.AreEqual(string.Format(ParenthesisPattern, exp.Trim()), value);
        }
    }
}