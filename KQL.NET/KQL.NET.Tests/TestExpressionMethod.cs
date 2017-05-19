using System;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace KQL.NET.Tests
{
    [TestClass]
    public class TestExpressionMethod
    {
        private const string MethodName = "Expression";
        private const string TestValue = "Test";

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

        private object InvokeMethodWithObjectResult(string propertyName = TestValue,
            string @operator = TestValue,
            string value = TestValue, bool prepare = false)
        {
            var result = _privateType.InvokeStatic(MethodName, propertyName, @operator, value, prepare);
            return result;
        }

        private string InvokeMethod(string propertyName = TestValue, string @operator = TestValue,
            string value = TestValue, bool prepare = false)
        {
            var result = (string) InvokeMethodWithObjectResult(propertyName, @operator, value, prepare);
            return result;
        }

        private static string GetMultipleValue(string value, int count = 3)
        {
            var sb = new StringBuilder();
            for (var i = 0; i < count; i++)
                sb.Append(value);
            return sb.ToString();
        }

        [TestMethod]
        public void TestReturnType()
        {
            var result = InvokeMethodWithObjectResult();
            Assert.IsInstanceOfType(result, typeof(string));
        }

        [TestMethod]
        public void TestNormalWithoutPrepare()
        {
            var result = InvokeMethod();
            Assert.AreEqual(GetMultipleValue(TestValue), result);
        }

        [TestMethod]
        public void TestNormalWithPrepare()
        {
            var result = InvokeMethod(value: string.Format(@"""{0}""", TestValue), prepare: true);
            Assert.AreEqual(GetMultipleValue(TestValue), result);
        }

        #region Null

        [TestMethod]
        public void TestNullPropertyName()
        {
            try
            {
                InvokeMethod(null);
            }
            catch (ArgumentNullException exception)
            {
                Assert.AreEqual("propertyName", exception.ParamName);
            }
        }

        [TestMethod]
        public void TestNullOperator()
        {
            try
            {
                InvokeMethod(@operator: null);
            }
            catch (ArgumentNullException exception)
            {
                Assert.AreEqual("operator", exception.ParamName);
            }
        }

        [TestMethod]
        public void TestNullValue()
        {
            try
            {
                InvokeMethod(value: null);
            }
            catch (ArgumentNullException exception)
            {
                Assert.AreEqual("value", exception.ParamName);
            }
        }

        #endregion

        #region Empty

        [TestMethod]
        public void TestEmptyPropertyName()
        {
            try
            {
                InvokeMethod(string.Empty);
            }
            catch (ArgumentException exception)
            {
                Assert.AreEqual("propertyName", exception.ParamName);
            }
        }

        [TestMethod]
        public void TestEmptyOperator()
        {
            try
            {
                InvokeMethod(@operator: string.Empty);
            }
            catch (ArgumentException exception)
            {
                Assert.AreEqual("operator", exception.ParamName);
            }
        }

        [TestMethod]
        public void TestEmptyValue()
        {
            try
            {
                InvokeMethod(value: string.Empty);
            }
            catch (ArgumentException exception)
            {
                Assert.AreEqual("value", exception.ParamName);
            }
        }

        #endregion
    }
}