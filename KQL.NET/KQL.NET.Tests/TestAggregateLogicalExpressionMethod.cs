using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace KQL.NET.Tests
{
    [TestClass]
    public class TestAggregateLogicalExpressionMethod
    {
        private const string MethodName = "AggregateLogicalExpression";
        private const string Operator = "Operator";
        private readonly string[] _testExpressions = {"One", "Two", "Three"};

        private PrivateType _privateType;

        [TestInitialize]
        public void Initialize()
        {
            _privateType = new PrivateType(typeof(KQL));
        }

        [TestCleanup]
        public void Cleanup()
        {
            _privateType = null;
        }

        [Ignore]
        private object InvokeMethodWithObjectResult(IEnumerable<string> expressions, string @operator = Operator,
            bool addParenthesis = false)
        {
            var result = _privateType.InvokeStatic(MethodName, @operator, expressions, addParenthesis);
            return result;
        }

        [Ignore]
        private string InvokeMethod(IEnumerable<string> expressions, string @operator = Operator,
            bool addParenthesis = false)
        {
            var result = (string) InvokeMethodWithObjectResult(expressions, @operator, addParenthesis);
            return result;
        }

        [TestMethod]
        public void TestReturnType()
        {
            var result = InvokeMethodWithObjectResult(_testExpressions);
            Assert.IsInstanceOfType(result, typeof(string));
        }

        [TestMethod]
        public void TestNormalWithoutParenthesis()
        {
            var result = InvokeMethod(_testExpressions);
            var correctResult = _testExpressions.Aggregate(
                (string1, string2) => string.Format("{0} {1} {2}", string1, Operator, string2));
            Assert.AreEqual(correctResult, result);
        }

        [TestMethod]
        public void TestNormalWithParenthesis()
        {
            var result = InvokeMethod(_testExpressions, addParenthesis: true);
            var correctResult = string.Format("({0})", _testExpressions.Aggregate(
                (string1, string2) => string.Format("{0} {1} {2}", string1, Operator, string2)));
            Assert.AreEqual(correctResult, result);
        }

        [TestMethod]
        public void TestOneElementInExpressionWithoutParenthesis()
        {
            var result = InvokeMethod(new[] {Operator});
            Assert.AreEqual(Operator, result);
        }

        [TestMethod]
        public void TestOneElementInExpressionWithParenthesis()
        {
            var result = InvokeMethod(new[] {Operator}, addParenthesis: true);
            Assert.AreEqual(Operator, result);
        }

        [TestMethod]
        public void TestOperatorIsNull()
        {
            try
            {
                InvokeMethodWithObjectResult(_testExpressions, null);
            }
            catch (ArgumentNullException exception)
            {
                Assert.AreEqual("operator", exception.ParamName);
            }
        }

        [TestMethod]
        public void TestOperatorIsEmpty()
        {
            try
            {
                InvokeMethodWithObjectResult(_testExpressions, string.Empty);
            }
            catch (ArgumentException exception)
            {
                Assert.AreEqual("operator", exception.ParamName);
            }
        }

        [TestMethod]
        public void TestExpressionsIsNull()
        {
            try
            {
                InvokeMethodWithObjectResult(null);
            }
            catch (ArgumentNullException exception)
            {
                Assert.AreEqual("expressions", exception.ParamName);
            }
        }

        [TestMethod]
        public void TestExpressionsWithNullElements()
        {
            try
            {
                InvokeMethodWithObjectResult(new string[] {null, null, null});
            }
            catch (ArgumentException exception)
            {
                Assert.AreEqual("expressions", exception.ParamName);
            }
        }

        [TestMethod]
        public void TestExpressionsWithEmptyElements()
        {
            try
            {
                InvokeMethodWithObjectResult(new[] {string.Empty, string.Empty, string.Empty});
            }
            catch (ArgumentException exception)
            {
                Assert.AreEqual("expressions", exception.ParamName);
            }
        }

        [TestMethod]
        public void TestExpressionsWithMixedElements()
        {
            var mixedExpressions = new List<string>();
            mixedExpressions.AddRange(_testExpressions);
            mixedExpressions.Insert(1, null);
            mixedExpressions.Insert(3, string.Empty);

            var result = InvokeMethod(mixedExpressions);
            var correctResult = mixedExpressions
                .Where(item => !string.IsNullOrEmpty(item))
                .Aggregate(
                    (string1, string2) => string.Format("{0} {1} {2}", string1, Operator, string2));
            Assert.AreEqual(correctResult, result);
        }
    }
}