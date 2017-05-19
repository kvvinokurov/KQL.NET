using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace KQL.NET.Tests
{
    [TestClass]
    public class TestLogicalExpressionMethod
    {
        private const string MethodName = "LogicalExpression";
        private const string Left = "Left";
        private const string Right = "Right";
        private const string Operator = "Operator";
        private readonly string _correctResult = string.Format("{0} {1} {2}", Left, Operator, Right);

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

        private object InvokeMethodWithObjectResult(string leftExpression = Left, string @operator = Operator,
            string rightExpression = Right)
        {
            var result = _privateType.InvokeStatic(MethodName, leftExpression, @operator, rightExpression);
            return result;
        }

        private string InvokeMethod(string leftExpression = Left, string @operator = Operator,
            string rightExpression = Right)
        {
            var result = (string) InvokeMethodWithObjectResult(leftExpression, @operator, rightExpression);
            return result;
        }

        [TestMethod]
        public void TestReturnType()
        {
            var result = InvokeMethodWithObjectResult();
            Assert.IsInstanceOfType(result, typeof(string));
        }

        [TestMethod]
        public void TestNormal()
        {
            var result = InvokeMethod();
            Assert.AreEqual(_correctResult, result);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void TestLeftAndRightExpressionsIsNull()
        {
            InvokeMethodWithObjectResult(null, rightExpression: null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void TestLeftAndRightExpressionsIsEmpty()
        {
            InvokeMethodWithObjectResult(string.Empty, rightExpression: string.Empty);
        }

        [TestMethod]
        public void TestNullOperator()
        {
            try
            {
                InvokeMethodWithObjectResult(@operator: null);
            }
            catch (ArgumentNullException exception)
            {
                Assert.AreEqual("operator", exception.ParamName);
            }
        }

        [TestMethod]
        public void TestEmptyOperator()
        {
            try
            {
                InvokeMethodWithObjectResult(@operator: string.Empty);
            }
            catch (ArgumentException exception)
            {
                Assert.AreEqual("operator", exception.ParamName);
            }
        }

        [TestMethod]
        public void TestIfLeftExpressionIsNull()
        {
            var result = InvokeMethod(null);
            Assert.AreEqual(Right, result);
        }

        [TestMethod]
        public void TestIfLeftExpressionIsEmpty()
        {
            var result = InvokeMethod(string.Empty);
            Assert.AreEqual(Right, result);
        }

        [TestMethod]
        public void TestIfRightExpressionIsNull()
        {
            var result = InvokeMethod(rightExpression: null);
            Assert.AreEqual(Left, result);
        }

        [TestMethod]
        public void TestIfRightExpressionIsEmpty()
        {
            var result = InvokeMethod(rightExpression: string.Empty);
            Assert.AreEqual(Left, result);
        }
    }
}