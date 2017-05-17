using System;
using System.Collections.Generic;
using System.Linq;

namespace KirillVinokurov.SharePoint.Search
{
    public static partial class KQL
    {
        /// <summary>
        ///     Метод формирование поискового выражения
        /// </summary>
        /// <param name="propertyName">Название свойства</param>
        /// <param name="operator">Оператор</param>
        /// <param name="value">Значение</param>
        /// <param name="prepareStringPhrase">Подготовить текстовую фразу для поиска</param>
        /// <returns>Выражение</returns>
        private static string Expression(string propertyName, string @operator, string value,
            bool prepareStringPhrase = true)
        {
            if (propertyName == null)
                throw new ArgumentNullException(nameof(propertyName));
            if (propertyName == string.Empty)
                throw new ArgumentException(@"Параметр не может быть пустым", nameof(propertyName));
            if (@operator == null)
                throw new ArgumentNullException(nameof(@operator));
            if (@operator == string.Empty)
                throw new ArgumentException(@"Параметр не может быть пустым", nameof(@operator));
            if (value == null)
                throw new ArgumentNullException(nameof(value));
            if (value == string.Empty)
                throw new ArgumentException(@"Параметр не может быть пустым", nameof(value));

            return string.Format(ConcatPattern, propertyName, @operator,
                prepareStringPhrase ? PrepareStringPhrase(value) : value);
        }

        /// <summary>
        ///     Метод формирование логического поискового выражения
        /// </summary>
        /// <param name="leftExpression">Левое выражение</param>
        /// <param name="operator">Логический оператор</param>
        /// <param name="rightExpression">Правое выражение</param>
        /// <returns>Выражение</returns>
        private static string LogicalExpression(string leftExpression, string @operator, string rightExpression)
        {
            if (string.IsNullOrEmpty(leftExpression) && string.IsNullOrEmpty(rightExpression))
                throw new ArgumentException("Левое и правое выражения не могут быть пустыми");
            if (@operator == null)
                throw new ArgumentNullException(nameof(@operator));
            if (@operator == string.Empty)
                throw new ArgumentException(@"Параметр не может быть пустым", nameof(@operator));

            if (string.IsNullOrEmpty(leftExpression))
                return rightExpression;

            if (string.IsNullOrEmpty(rightExpression))
                return leftExpression;

            return string.Format(WithSpacesPattern, leftExpression, @operator, rightExpression);
        }

        /// <summary>
        ///     Метод формирование логического поискового выражения
        /// </summary>
        /// <param name="operator">Логический оператор</param>
        /// <param name="expressions">Выражения</param>
        /// <param name="addParenthesis">Добавить круглые скобки</param>
        /// <returns>Выражение</returns>
        private static string AggregateLogicalExpression(string @operator, IEnumerable<string> expressions,
            bool addParenthesis = false)
        {
            if (@operator == null)
                throw new ArgumentNullException(nameof(@operator));
            if (@operator == string.Empty)
                throw new ArgumentException(@"Параметр не может быть пустым", nameof(@operator));
            if (expressions == null)
                throw new ArgumentNullException(nameof(expressions));

            var enumerable = expressions as IList<string> ?? expressions.ToList();
            var pureExpressions = enumerable.Where(expression => !string.IsNullOrEmpty(expression)).ToList();

            if (!pureExpressions.Any())
                throw new ArgumentException(@"Параметр не может быть пустым", nameof(expressions));

            if (pureExpressions.Count == 1)
                return pureExpressions.First();

            var aggregate =
                pureExpressions.Aggregate((string1, string2) => LogicalExpression(string1, @operator, string2));
            return addParenthesis ? Parenthesis(aggregate) : aggregate;
        }


        /// <summary>
        ///     Метод формирование логического поискового выражения для одного свойства
        /// </summary>
        /// <param name="propertyName">Нащвание свойства</param>
        /// <param name="comparisonOperator">Оператор сравнения</param>
        /// <param name="values">Значения свойства</param>
        /// <param name="logicalOperator">Логический оператор для сравнений</param>
        /// <returns>Выражение</returns>
        private static string AggregateExpression(string propertyName, string comparisonOperator,
            ICollection<string> values, string logicalOperator)
        {
            if (propertyName == null)
                throw new ArgumentNullException(nameof(propertyName));
            if (propertyName == string.Empty)
                throw new ArgumentException(@"Параметр не может быть пустым", nameof(propertyName));
            if (comparisonOperator == null)
                throw new ArgumentNullException(nameof(comparisonOperator));
            if (comparisonOperator == string.Empty)
                throw new ArgumentException(@"Параметр не может быть пустым", nameof(comparisonOperator));
            if (values == null)
                throw new ArgumentNullException(nameof(values));
            if (!values.Any())
                throw new ArgumentException(@"Параметр не может быть пустым", nameof(values));
            if (logicalOperator == null)
                throw new ArgumentNullException(nameof(logicalOperator));
            if (logicalOperator == string.Empty)
                throw new ArgumentException(@"Параметр не может быть пустым", nameof(logicalOperator));

            var valuesExpression = values.Where(value => !string.IsNullOrEmpty(value))
                .Select(value => Expression(propertyName, comparisonOperator, value));
            return AggregateLogicalExpression(logicalOperator, valuesExpression, true);
        }

        /// <summary>
        ///     Метод формирования структурных выражений, типа ANY(...), ALL(...), WORDS(...)
        /// </summary>
        /// <param name="operator">Оператор</param>
        /// <param name="words">Коллекция фраз в виде одной строки</param>
        /// <returns>Выражение</returns>
        private static string StructureExpression(string @operator, string words)
        {
            if (@operator == null)
                throw new ArgumentNullException(nameof(@operator));
            if (@operator == string.Empty)
                throw new ArgumentException(@"Параметр не может быть пустым", nameof(@operator));
            if (words == null)
                throw new ArgumentNullException(nameof(words));
            if (words == string.Empty)
                throw new ArgumentException(@"Параметр не может быть пустым", nameof(words));

            return string.Concat(@operator, Parenthesis(words));
        }

        /// <summary>
        ///     Метод формирования структурных выражений, типа ANY(...), ALL(...), WORDS(...)
        /// </summary>
        /// <param name="operator">Оператор</param>
        /// <param name="words">Коллекция фраз</param>
        /// <param name="separator">Разделитель</param>
        /// <returns>Выражение</returns>
        private static string StructureExpression(string @operator, string[] words, string separator = " ")
        {
            if (@operator == null)
                throw new ArgumentNullException(nameof(@operator));
            if (@operator == string.Empty)
                throw new ArgumentException(@"Параметр не может быть пустым", nameof(@operator));
            if (words == null)
                throw new ArgumentNullException(nameof(words));
            if (words.Length == 0)
                throw new ArgumentException(@"Параметр не может быть пустым", nameof(words));
            if (separator == null)
                throw new ArgumentNullException(nameof(separator));
            if (separator == string.Empty)
                throw new ArgumentException(@"Параметр не может быть пустым", nameof(separator));

            var wordsValue = string.Join(separator, words);
            return StructureExpression(@operator, wordsValue);
        }

        /// <summary>
        ///     Метод для формирования выражения для NEAR и ONEAR
        /// </summary>
        /// <param name="leftExpression">Левое выражение</param>
        /// <param name="rightExpression">Правое выражение</param>
        /// <param name="operator">Оператор</param>
        /// <param name="maxTermLength">
        ///     Необязательный параметр, указывающий максимальное расстояние между терминами. (По умолчанию
        ///     значение равно 8, если параметр не задан)
        /// </param>
        /// <returns>Выражение</returns>
        private static string NearOnearInternal(string leftExpression, string rightExpression, string @operator,
            uint? maxTermLength)
        {
            if (leftExpression == null)
                throw new ArgumentNullException(nameof(leftExpression));
            if (leftExpression == string.Empty)
                throw new ArgumentException(@"Параметр не может быть пустым", nameof(leftExpression));
            if (rightExpression == null)
                throw new ArgumentNullException(nameof(rightExpression));
            if (rightExpression == string.Empty)
                throw new ArgumentException(@"Параметр не может быть пустым", nameof(rightExpression));
            if (@operator == null)
                throw new ArgumentNullException(nameof(@operator));
            if (@operator == string.Empty)
                throw new ArgumentException(@"Параметр не может быть пустым", nameof(@operator));

            var operatorExpression = maxTermLength.HasValue
                ? string.Format("{0}({1})", @operator, maxTermLength.Value)
                : @operator;

            return string.Format(WithSpacesPattern, leftExpression, operatorExpression, rightExpression);
        }

        /// <summary>
        ///     Метод для построения выражений с операторами включения и исключения
        /// </summary>
        /// <param name="operator">Оператор</param>
        /// <param name="expression">Фраза</param>
        /// <returns>Выражение</returns>
        private static string PlusMinusInternal(string @operator, string expression)
        {
            if (@operator == null)
                throw new ArgumentNullException(nameof(@operator));
            if (@operator == string.Empty)
                throw new ArgumentException(@"Параметр не может быть пустым", nameof(@operator));
            if (expression == null)
                throw new ArgumentNullException(nameof(expression));
            if (expression == string.Empty)
                throw new ArgumentException(@"Параметр не может быть пустым", nameof(expression));

            return string.Concat(@operator, PrepareStringPhrase(expression));
        }

        /// <summary>
        ///     Метод оборачивания свойства
        /// </summary>
        /// <param name="propertyName">Название свойства</param>
        /// <param name="propertyValue">Значение свойства</param>
        /// <returns>свойство и значение</returns>
        private static string WrapProperty(string propertyName, string propertyValue)
        {
            if (propertyName == null)
                throw new ArgumentNullException(nameof(propertyName));
            if (propertyName == string.Empty)
                throw new ArgumentException(@"Параметр не может быть пустым", nameof(propertyName));

            return string.Format(PropertyPattern, propertyName, propertyValue);
        }
    }
}