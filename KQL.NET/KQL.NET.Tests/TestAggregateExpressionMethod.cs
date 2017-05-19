using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace KQL.NET.Tests
{
    [TestClass]
    public class TestAggregateExpressionMethod
    {
        private const string MethodName = "AggregateExpression";
        private const string PropertyName = "SomeProperty";
        private const string Operator = "=";
        private const string LogicalOperator = "OR";
        private readonly string[] _testValues = {"One", "Two", "Three"};

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
        private object InvokeMethodWithObjectResult(ICollection<string> values, string propertyName = PropertyName,
            string comparisonOperator = Operator,
            string logicalOperator = LogicalOperator)
        {
            var result = _privateType.InvokeStatic(MethodName, propertyName, comparisonOperator, values,
                logicalOperator);
            return result;
        }

        [Ignore]
        private string InvokeMethod(ICollection<string> values, string propertyName = PropertyName,
            string comparisonOperator = Operator,
            string logicalOperator = LogicalOperator)
        {
            var result =
                (string) InvokeMethodWithObjectResult(values, propertyName, comparisonOperator, logicalOperator);
            return result;
        }

        [TestMethod]
        public void TestReturnType()
        {
            var result = InvokeMethodWithObjectResult(_testValues);
            Assert.IsInstanceOfType(result, typeof(string));
        }

        [TestMethod]
        public void TestNormal()
        {
            var result = InvokeMethod(_testValues);
            //var correctResult = string.Format("({0})",
            //    _testValues
            //        .Select(value => string.Concat(PropertyName, Operator, value))
            //        .Aggregate((string1, string2) => string.Format("{0} {1} {2}", string1, LogicalOperator, string2)));
            var values = new List<object>
            {
                PropertyName, //0
                Operator, //1
                LogicalOperator //2
            };
            values.AddRange(_testValues); //3, 4, 5
            var correctResult = string.Format("({0}{1}{3} {2} {0}{1}{4} {2} {0}{1}{5})", values.ToArray());
            Assert.AreEqual(correctResult, result);
        }

        [TestMethod]
        public void TestMixedValues()
        {
            var mixedExpressions = new List<string>();
            mixedExpressions.AddRange(_testValues);
            mixedExpressions.Insert(1, null);
            mixedExpressions.Insert(3, string.Empty);

            var result = InvokeMethod(mixedExpressions);

            //var correctResult = string.Format("({0})",
            //    _testValues
            //        .Select(value => string.Concat(PropertyName, Operator, value))
            //        .Aggregate((string1, string2) => string.Format("{0} {1} {2}", string1, LogicalOperator, string2)));

            var values = new List<object>
            {
                PropertyName, //0
                Operator, //1
                LogicalOperator //2
            };
            values.AddRange(_testValues); //3, 4, 5
            var correctResult = string.Format("({0}{1}{3} {2} {0}{1}{4} {2} {0}{1}{5})", values.ToArray());
            Assert.AreEqual(correctResult, result);
        }

        [TestMethod]
        public void TestPropertyIsNull()
        {
            try
            {
                InvokeMethodWithObjectResult(_testValues, null);
            }
            catch (ArgumentNullException exception)
            {
                Assert.AreEqual("propertyName", exception.ParamName);
            }
        }

        [TestMethod]
        public void TestPropertyIsEmpty()
        {
            try
            {
                InvokeMethodWithObjectResult(_testValues, string.Empty);
            }
            catch (ArgumentException exception)
            {
                Assert.AreEqual("propertyName", exception.ParamName);
            }
        }

        [TestMethod]
        public void TestOperatorIsNull()
        {
            try
            {
                InvokeMethodWithObjectResult(_testValues, comparisonOperator: null);
            }
            catch (ArgumentNullException exception)
            {
                Assert.AreEqual("comparisonOperator", exception.ParamName);
            }
        }

        [TestMethod]
        public void TestOperatorIsEmpty()
        {
            try
            {
                InvokeMethodWithObjectResult(_testValues, comparisonOperator: string.Empty);
            }
            catch (ArgumentException exception)
            {
                Assert.AreEqual("comparisonOperator", exception.ParamName);
            }
        }

        [TestMethod]
        public void TestValuesIsNull()
        {
            try
            {
                InvokeMethodWithObjectResult(null);
            }
            catch (ArgumentNullException exception)
            {
                Assert.AreEqual("values", exception.ParamName);
            }
        }

        [TestMethod]
        public void TestEmptyValues()
        {
            try
            {
                InvokeMethodWithObjectResult(new List<string>());
            }
            catch (ArgumentException exception)
            {
                Assert.AreEqual("values", exception.ParamName);
            }
        }

        [TestMethod]
        public void TestLogicalOperatorIsNull()
        {
            try
            {
                InvokeMethodWithObjectResult(_testValues, logicalOperator: null);
            }
            catch (ArgumentNullException exception)
            {
                Assert.AreEqual("logicalOperator", exception.ParamName);
            }
        }

        [TestMethod]
        public void TestLogicalOperatorIsEmpty()
        {
            try
            {
                InvokeMethodWithObjectResult(_testValues, logicalOperator: string.Empty);
            }
            catch (ArgumentException exception)
            {
                Assert.AreEqual("logicalOperator", exception.ParamName);
            }
        }
    }
}