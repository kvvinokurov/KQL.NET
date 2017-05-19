using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace KQL.NET.Tests
{
    [TestClass]
    public class TestStructureExpressionMethods
    {
        private const string MethodName = "StructureExpression";
        private const string Operator = "OPERATOR";
        private const string Separator = " ";
        private readonly string[] _wordsAsArray = {"One", "Two", "Three"};
        private readonly string _wordsAsString = "One Two Three";

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
        private object InvokeSingleStringMethodWithObjectResult(string words, string @operator = Operator)
        {
            var result = _privateType.InvokeStatic(MethodName, @operator, words);
            return result;
        }

        [Ignore]
        private string InvokeSingleStringMethod(string words, string @operator = Operator)
        {
            var result =
                (string) InvokeSingleStringMethodWithObjectResult(words, @operator);
            return result;
        }

        [Ignore]
        private object InvokeArrayOfStringsMethodWithObjectResult(string[] words, string @operator = Operator,
            string separator = Separator)
        {
            var result = _privateType.InvokeStatic(MethodName, @operator, words, separator);
            return result;
        }

        [Ignore]
        private string InvokeArrayOfStringsMethod(string[] words, string @operator = Operator,
            string separator = Separator)
        {
            var result =
                (string) InvokeArrayOfStringsMethodWithObjectResult(words, @operator, separator);
            return result;
        }

        [TestMethod]
        public void TestReturnTypeSingle()
        {
            var result = InvokeSingleStringMethodWithObjectResult(_wordsAsString);
            Assert.IsInstanceOfType(result, typeof(string));
        }

        [TestMethod]
        public void TestReturnTypeMultiple()
        {
            var result = InvokeArrayOfStringsMethodWithObjectResult(_wordsAsArray);
            Assert.IsInstanceOfType(result, typeof(string));
        }

        [TestMethod]
        public void TestNormalSingle()
        {
            var result = InvokeSingleStringMethod(_wordsAsString);
            var correctResult = string.Format("{0}({1})", Operator, _wordsAsString);
            Assert.AreEqual(correctResult, result);
        }

        [TestMethod]
        public void TestMultipleSpacesSingle()
        {
            var result = InvokeSingleStringMethod("One  Two   Three");
            var correctResult = string.Concat(Operator, "(One Two Three)");
            Assert.AreEqual(correctResult, result);
        }

        [TestMethod]
        public void TestNormalMiltiple()
        {
            var result = InvokeArrayOfStringsMethod(_wordsAsArray);
            var correctResult = string.Concat(Operator, "(One Two Three)");
            Assert.AreEqual(correctResult, result);
        }

        [TestMethod]
        public void TestMixedMiltiple()
        {
            var mixedExpressions = new List<string>();
            mixedExpressions.AddRange(_wordsAsArray);
            mixedExpressions.Insert(1, null);
            mixedExpressions.Insert(3, string.Empty);

            var result = InvokeArrayOfStringsMethod(mixedExpressions.ToArray());
            var correctResult = string.Concat(Operator, "(One Two Three)");
            Assert.AreEqual(correctResult, result);
        }

        [TestMethod]
        public void TestNormalMiltipleWithDiferentSeparator()
        {
            var result = InvokeArrayOfStringsMethod(_wordsAsArray, separator: ",");
            var correctResult = string.Concat(Operator, "(One,Two,Three)");
            Assert.AreEqual(correctResult, result);
        }

        [TestMethod]
        public void TestWordsIsNullWithSingleString()
        {
            try
            {
                InvokeSingleStringMethodWithObjectResult(null);
            }
            catch (ArgumentNullException exception)
            {
                Assert.AreEqual("words", exception.ParamName);
            }
        }

        [TestMethod]
        public void TestWordsIsEmptyWithSingleString()
        {
            try
            {
                InvokeSingleStringMethodWithObjectResult(string.Empty);
            }
            catch (ArgumentException exception)
            {
                Assert.AreEqual("words", exception.ParamName);
            }
        }

        [TestMethod]
        public void TestOperatorIsNullWithSingleString()
        {
            try
            {
                InvokeSingleStringMethodWithObjectResult(_wordsAsString, null);
            }
            catch (ArgumentNullException exception)
            {
                Assert.AreEqual("operator", exception.ParamName);
            }
        }

        [TestMethod]
        public void TestOperatorIsEmptyWithSingleString()
        {
            try
            {
                InvokeSingleStringMethodWithObjectResult(_wordsAsString, string.Empty);
            }
            catch (ArgumentException exception)
            {
                Assert.AreEqual("operator", exception.ParamName);
            }
        }

        [TestMethod]
        public void TestWordsIsNullWithMultipleString()
        {
            try
            {
                InvokeArrayOfStringsMethodWithObjectResult(null);
            }
            catch (ArgumentNullException exception)
            {
                Assert.AreEqual("words", exception.ParamName);
            }
        }

        [TestMethod]
        public void TestWordsIsEmptyWithMultipleString()
        {
            try
            {
                InvokeArrayOfStringsMethodWithObjectResult(new string[] { });
            }
            catch (ArgumentException exception)
            {
                Assert.AreEqual("words", exception.ParamName);
            }
        }

        [TestMethod]
        public void TestOperatorIsNullWithMultipleString()
        {
            try
            {
                InvokeArrayOfStringsMethodWithObjectResult(_wordsAsArray, null);
            }
            catch (ArgumentNullException exception)
            {
                Assert.AreEqual("operator", exception.ParamName);
            }
        }

        [TestMethod]
        public void TestOperatorIsEmptyWithMultipleString()
        {
            try
            {
                InvokeArrayOfStringsMethodWithObjectResult(_wordsAsArray, string.Empty);
            }
            catch (ArgumentException exception)
            {
                Assert.AreEqual("operator", exception.ParamName);
            }
        }

        [TestMethod]
        public void TestSeparatorIsNullWithMultipleString()
        {
            try
            {
                InvokeArrayOfStringsMethodWithObjectResult(_wordsAsArray, separator: null);
            }
            catch (ArgumentNullException exception)
            {
                Assert.AreEqual("separator", exception.ParamName);
            }
        }

        [TestMethod]
        public void TestSeparatorIsEmptyWithMultipleString()
        {
            try
            {
                InvokeArrayOfStringsMethodWithObjectResult(_wordsAsArray, separator: string.Empty);
            }
            catch (ArgumentException exception)
            {
                Assert.AreEqual("separator", exception.ParamName);
            }
        }
    }
}