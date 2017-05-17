using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace KirillVinokurov.SharePoint.Search
{
    /// <summary>
    ///     Keyword Query Language (KQL)
    /// </summary>
    public static partial class KQL
    {
        #region Parenthesis

        /// <summary>
        ///     Круглые скобки
        /// </summary>
        /// <param name="expression">Выражение</param>
        /// <returns>Выражение в круглых скобках</returns>
        public static string Parenthesis(string expression)
        {
            if (expression == null)
                throw new ArgumentNullException(nameof(expression));
            if (expression == string.Empty)
                throw new ArgumentException(@"Параметр не может быть пустым", nameof(expression));

            return string.Format(ParenthesisPattern, expression.Trim());
        }

        #endregion

        #region XRANK

        /// <summary>
        ///     Оператор динамического ранжирования. Применяйте оператор XRANK, чтобы повысить динамический рейтинг элементов в
        ///     зависимости от повторений определенных терминов в match expression, не меняя состав элементов, удовлетворяющих
        ///     критериям запроса.
        /// </summary>
        /// <param name="matchExpressions">
        ///     Параметр Match expressions может быть любым допустимым выражением KQL, включая вложенные
        ///     выражения XRANK.
        /// </param>
        /// <param name="rankExpressions">
        ///     Параметр Rank expressions может быть любым допустимым выражением в языке KQL, кроме
        ///     выражений XRANK.
        /// </param>
        /// <param name="n">
        ///     Указывает количество результатов для вычисления статистики. Этот параметр не влияет на количество
        ///     результатов, на которые влияет динамический рейтинг. С его помощью из вычисления статистики просто исключаются
        ///     ненужные элементы.
        /// </param>
        /// <param name="nb">
        ///     Параметр относится к нормированному увеличению. Этот параметр определяет коэффициент, на который
        ///     умножается произведение дисперсии и среднего арифметического значений рейтингов в наборе результатов.
        /// </param>
        /// <param name="cb">Параметр увеличивает рейтинг на постоянную величину.</param>
        /// <param name="stdb">Параметр увеличивает рейтинг на величину стандартного отклонения.</param>
        /// <param name="avgb">Параметр увеличивает рейтинг на среднее значение.</param>
        /// <param name="rb">
        ///     Параметр увеличивает рейтинг в диапазоне. Этот коэффициент умножается на диапазон значений рейтингов в
        ///     наборе результатов.
        /// </param>
        /// <param name="pb">
        ///     Параметр задает увеличение в процентах. Этот коэффициент умножается на собственный рейтинг элемента в
        ///     сравнении с минимальным значением в наборе.
        /// </param>
        /// <returns>Выражение</returns>
        /// <remarks>Более подробно читайте на странице: https://msdn.microsoft.com/ru-ru/library/office/ee558911.aspx </remarks>
        public static string Xrank(string matchExpressions, string rankExpressions, int? n, float? nb, float? cb,
            float? stdb, float? avgb, float? rb, float? pb)
        {
            if (matchExpressions == null)
                throw new ArgumentNullException(nameof(matchExpressions));
            if (matchExpressions == string.Empty)
                throw new ArgumentException(@"Параметр не может быть пустым", nameof(matchExpressions));
            if (rankExpressions == null)
                throw new ArgumentNullException(nameof(rankExpressions));
            if (rankExpressions == string.Empty)
                throw new ArgumentException(@"Параметр не может быть пустым", nameof(rankExpressions));
            if (rankExpressions.Contains(Resources.KQL.Xrank))
                throw new ArgumentException(
                    @"Параметр Rank expressions может быть любым допустимым выражением в языке KQL, кроме выражений XRANK.",
                    nameof(rankExpressions));
            if (!nb.HasValue && !cb.HasValue && !stdb.HasValue && !avgb.HasValue && !rb.HasValue && !pb.HasValue)
                throw new ArgumentException(
                    "Должен быть задан хотя бы один параметр (за исключением n), чтобы выражение XRANK было допустимым.");

            string arguments;
            {
                var listOfArguments = new List<string>();
                if (n.HasValue)
                    listOfArguments.Add(WrapProperty(Resources.KQL.XrankN,
                        n.Value.ToString(CultureInfo.InvariantCulture)));

                if (nb.HasValue)
                    listOfArguments.Add(WrapProperty(Resources.KQL.XrankNB,
                        nb.Value.ToString(CultureInfo.InvariantCulture)));

                if (cb.HasValue)
                    listOfArguments.Add(WrapProperty(Resources.KQL.XrankCB,
                        cb.Value.ToString(CultureInfo.InvariantCulture)));

                if (cb.HasValue)
                    listOfArguments.Add(WrapProperty(Resources.KQL.XrankCB,
                        cb.Value.ToString(CultureInfo.InvariantCulture)));

                if (stdb.HasValue)
                    listOfArguments.Add(WrapProperty(Resources.KQL.XrankSTDB,
                        stdb.Value.ToString(CultureInfo.InvariantCulture)));

                if (avgb.HasValue)
                    listOfArguments.Add(WrapProperty(Resources.KQL.XrankAVGB,
                        avgb.Value.ToString(CultureInfo.InvariantCulture)));

                if (rb.HasValue)
                    listOfArguments.Add(WrapProperty(Resources.KQL.XrankRB,
                        rb.Value.ToString(CultureInfo.InvariantCulture)));

                if (pb.HasValue)
                    listOfArguments.Add(WrapProperty(Resources.KQL.XrankPB,
                        pb.Value.ToString(CultureInfo.InvariantCulture)));


#if NET35
                arguments = string.Join(", ", listOfArguments.ToArray());      
#else
                arguments = string.Join(", ", listOfArguments);
#endif
            }

            return string.Format(XrankPattern, matchExpressions, Resources.KQL.Xrank, arguments,
                rankExpressions);
        }

        #endregion

        #region Eq

        /// <summary>
        ///     Возвращает результаты, в которых указанное в ограничении свойства значение равно значению свойства, хранящегося в
        ///     базе данных хранилища свойств, или совпадает с отдельными терминами в значении свойства, хранящегося в
        ///     полнотекстовом индексе.
        /// </summary>
        /// <param name="propertyName">Название свойства</param>
        /// <param name="value">Значение</param>
        /// <returns>Выражение</returns>
        public static string Eq(string propertyName, string value)
        {
            return Expression(propertyName, Resources.KQL.Eq, value);
        }

        /// <summary>
        ///     Возвращает результаты, в которых указанное в ограничении свойства значение равно значению свойства, хранящегося в
        ///     базе данных хранилища свойств, или совпадает с отдельными терминами в значении свойства, хранящегося в
        ///     полнотекстовом индексе.
        /// </summary>
        /// <param name="propertyName">Название свойства</param>
        /// <param name="value">Значение</param>
        /// <returns>Выражение</returns>
        public static string Eq(string propertyName, int value)
        {
            return Eq(propertyName, value.ToString(CultureInfo.InvariantCulture));
        }

        /// <summary>
        ///     Возвращает результаты, в которых указанное в ограничении свойства значение равно значению свойства, хранящегося в
        ///     базе данных хранилища свойств, или совпадает с отдельными терминами в значении свойства, хранящегося в
        ///     полнотекстовом индексе.
        /// </summary>
        /// <param name="propertyName">Название свойства</param>
        /// <param name="value">Значение</param>
        /// <returns>Выражение</returns>
        public static string Eq(string propertyName, long value)
        {
            return Eq(propertyName, value.ToString(CultureInfo.InvariantCulture));
        }

        /// <summary>
        ///     Возвращает результаты, в которых указанное в ограничении свойства значение равно значению свойства, хранящегося в
        ///     базе данных хранилища свойств, или совпадает с отдельными терминами в значении свойства, хранящегося в
        ///     полнотекстовом индексе.
        /// </summary>
        /// <param name="propertyName">Название свойства</param>
        /// <param name="value">Значение</param>
        /// <returns>Выражение</returns>
        public static string Eq(string propertyName, decimal value)
        {
            return Eq(propertyName, value.ToString(CultureInfo.InvariantCulture));
        }

        /// <summary>
        ///     Возвращает результаты, в которых указанное в ограничении свойства значение равно значению свойства, хранящегося в
        ///     базе данных хранилища свойств, или совпадает с отдельными терминами в значении свойства, хранящегося в
        ///     полнотекстовом индексе.
        /// </summary>
        /// <param name="propertyName">Название свойства</param>
        /// <param name="value">Значение</param>
        /// <returns>Выражение</returns>
        public static string Eq(string propertyName, double value)
        {
            return Eq(propertyName, value.ToString(CultureInfo.InvariantCulture));
        }

        /// <summary>
        ///     Возвращает результаты, в которых указанное в ограничении свойства значение равно значению свойства, хранящегося в
        ///     базе данных хранилища свойств, или совпадает с отдельными терминами в значении свойства, хранящегося в
        ///     полнотекстовом индексе.
        /// </summary>
        /// <param name="propertyName">Название свойства</param>
        /// <param name="value">Значение</param>
        /// <returns>Выражение</returns>
        public static string Eq(string propertyName, bool value)
        {
            return Eq(propertyName, ConvertBoolToQueryFormat(value));
        }

        /// <summary>
        ///     Возвращает результаты, в которых указанное в ограничении свойства значение равно значению свойства, хранящегося в
        ///     базе данных хранилища свойств, или совпадает с отдельными терминами в значении свойства, хранящегося в
        ///     полнотекстовом индексе.
        /// </summary>
        /// <param name="propertyName">Название свойства</param>
        /// <param name="value">Значение</param>
        /// <param name="excludeTime">Исключить время</param>
        /// <returns>Выражение</returns>
        public static string Eq(string propertyName, DateTime value, bool excludeTime = false)
        {
            return Eq(propertyName, ConvertDateTimeToQueryFormat(value, excludeTime));
        }

        #endregion

        #region StrongEq

        /// <summary>
        ///     Возвращает результаты со значениями свойств, равными значению, указанному в ограничении свойств.
        /// </summary>
        /// <param name="propertyName">Название свойства</param>
        /// <param name="value">Значение</param>
        /// <returns>Выражение</returns>
        /// <remarks>Не рекомендуется комбинировать значение со знаком звездочки (*), если нужно найти точное совпадение.</remarks>
        public static string StrongEq(string propertyName, string value)
        {
            return Expression(propertyName, Resources.KQL.StrongEg, value);
        }

        /// <summary>
        ///     Возвращает результаты со значениями свойств, равными значению, указанному в ограничении свойств.
        /// </summary>
        /// <param name="propertyName">Название свойства</param>
        /// <param name="value">Значение</param>
        /// <returns>Выражение</returns>
        public static string StrongEq(string propertyName, int value)
        {
            return StrongEq(propertyName, value.ToString(CultureInfo.InvariantCulture));
        }

        /// <summary>
        ///     Возвращает результаты со значениями свойств, равными значению, указанному в ограничении свойств.
        /// </summary>
        /// <param name="propertyName">Название свойства</param>
        /// <param name="value">Значение</param>
        /// <returns>Выражение</returns>
        public static string StrongEq(string propertyName, long value)
        {
            return StrongEq(propertyName, value.ToString(CultureInfo.InvariantCulture));
        }

        /// <summary>
        ///     Возвращает результаты со значениями свойств, равными значению, указанному в ограничении свойств.
        /// </summary>
        /// <param name="propertyName">Название свойства</param>
        /// <param name="value">Значение</param>
        /// <returns>Выражение</returns>
        public static string StrongEq(string propertyName, decimal value)
        {
            return StrongEq(propertyName, value.ToString(CultureInfo.InvariantCulture));
        }

        /// <summary>
        ///     Возвращает результаты со значениями свойств, равными значению, указанному в ограничении свойств.
        /// </summary>
        /// <param name="propertyName">Название свойства</param>
        /// <param name="value">Значение</param>
        /// <returns>Выражение</returns>
        public static string StrongEq(string propertyName, double value)
        {
            return StrongEq(propertyName, value.ToString(CultureInfo.InvariantCulture));
        }

        /// <summary>
        ///     Возвращает результаты со значениями свойств, равными значению, указанному в ограничении свойств.
        /// </summary>
        /// <param name="propertyName">Название свойства</param>
        /// <param name="value">Значение</param>
        /// <returns>Выражение</returns>
        public static string StrongEq(string propertyName, bool value)
        {
            return StrongEq(propertyName, ConvertBoolToQueryFormat(value));
        }

        /// <summary>
        ///     Возвращает результаты со значениями свойств, равными значению, указанному в ограничении свойств.
        /// </summary>
        /// <param name="propertyName">Название свойства</param>
        /// <param name="value">Значение</param>
        /// <param name="excludeTime">Исключить время</param>
        /// <returns>Выражение</returns>
        public static string StrongEq(string propertyName, DateTime value, bool excludeTime = false)
        {
            return StrongEq(propertyName, ConvertDateTimeToQueryFormat(value, excludeTime));
        }

        #endregion

        #region Lt

        /// <summary>
        ///     Возвращает результаты, в которых значение свойства меньше значения, указанного в ограничении свойства.
        /// </summary>
        /// <param name="propertyName">Название свойства</param>
        /// <param name="value">Значение</param>
        /// <param name="excludeTime">Исключить время</param>
        /// <returns>Выражение</returns>
        public static string Lt(string propertyName, DateTime value, bool excludeTime = false)
        {
            return Expression(propertyName, Resources.KQL.Lt, ConvertDateTimeToQueryFormat(value, excludeTime));
        }

        /// <summary>
        ///     Возвращает результаты, в которых значение свойства меньше значения, указанного в ограничении свойства.
        /// </summary>
        /// <param name="propertyName">Название свойства</param>
        /// <param name="value">Значение</param>
        /// <returns>Выражение</returns>
        public static string Lt(string propertyName, int value)
        {
            return Expression(propertyName, Resources.KQL.Lt, value.ToString(CultureInfo.InvariantCulture));
        }

        /// <summary>
        ///     Возвращает результаты, в которых значение свойства меньше значения, указанного в ограничении свойства.
        /// </summary>
        /// <param name="propertyName">Название свойства</param>
        /// <param name="value">Значение</param>
        /// <returns>Выражение</returns>
        public static string Lt(string propertyName, long value)
        {
            return Expression(propertyName, Resources.KQL.Lt, value.ToString(CultureInfo.InvariantCulture));
        }

        /// <summary>
        ///     Возвращает результаты, в которых значение свойства меньше значения, указанного в ограничении свойства.
        /// </summary>
        /// <param name="propertyName">Название свойства</param>
        /// <param name="value">Значение</param>
        /// <returns>Выражение</returns>
        public static string Lt(string propertyName, decimal value)
        {
            return Expression(propertyName, Resources.KQL.Lt, value.ToString(CultureInfo.InvariantCulture));
        }

        /// <summary>
        ///     Возвращает результаты, в которых значение свойства меньше значения, указанного в ограничении свойства.
        /// </summary>
        /// <param name="propertyName">Название свойства</param>
        /// <param name="value">Значение</param>
        /// <returns>Выражение</returns>
        public static string Lt(string propertyName, double value)
        {
            return Expression(propertyName, Resources.KQL.Lt, value.ToString(CultureInfo.InvariantCulture));
        }

        #endregion

        #region Gt

        /// <summary>
        ///     Возвращает результаты, в которых значение свойства больше значения, указанного в ограничении свойства.
        /// </summary>
        /// <param name="propertyName">Название свойства</param>
        /// <param name="value">Значение</param>
        /// <param name="excludeTime">Исключить время</param>
        /// <returns>Выражение</returns>
        public static string Gt(string propertyName, DateTime value, bool excludeTime = false)
        {
            return Expression(propertyName, Resources.KQL.Gt, ConvertDateTimeToQueryFormat(value, excludeTime));
        }

        /// <summary>
        ///     Возвращает результаты, в которых значение свойства меньше значения, указанного в ограничении свойства.
        /// </summary>
        /// <param name="propertyName">Название свойства</param>
        /// <param name="value">Значение</param>
        /// <returns>Выражение</returns>
        public static string Gt(string propertyName, int value)
        {
            return Expression(propertyName, Resources.KQL.Gt, value.ToString(CultureInfo.InvariantCulture));
        }

        /// <summary>
        ///     Возвращает результаты, в которых значение свойства меньше значения, указанного в ограничении свойства.
        /// </summary>
        /// <param name="propertyName">Название свойства</param>
        /// <param name="value">Значение</param>
        /// <returns>Выражение</returns>
        public static string Gt(string propertyName, long value)
        {
            return Expression(propertyName, Resources.KQL.Gt, value.ToString(CultureInfo.InvariantCulture));
        }

        /// <summary>
        ///     Возвращает результаты, в которых значение свойства меньше значения, указанного в ограничении свойства.
        /// </summary>
        /// <param name="propertyName">Название свойства</param>
        /// <param name="value">Значение</param>
        /// <returns>Выражение</returns>
        public static string Gt(string propertyName, decimal value)
        {
            return Expression(propertyName, Resources.KQL.Gt, value.ToString(CultureInfo.InvariantCulture));
        }

        /// <summary>
        ///     Возвращает результаты, в которых значение свойства меньше значения, указанного в ограничении свойства.
        /// </summary>
        /// <param name="propertyName">Название свойства</param>
        /// <param name="value">Значение</param>
        /// <returns>Выражение</returns>
        public static string Gt(string propertyName, double value)
        {
            return Expression(propertyName, Resources.KQL.Gt, value.ToString(CultureInfo.InvariantCulture));
        }

        #endregion

        #region Leq

        /// <summary>
        ///     Возвращает результаты со значением свойства, меньшим или равным значению, указанному в ограничении свойства.
        /// </summary>
        /// <param name="propertyName">Название свойства</param>
        /// <param name="value">Значение</param>
        /// <param name="excludeTime">Исключить время</param>
        /// <returns>Выражение</returns>
        public static string Leq(string propertyName, DateTime value, bool excludeTime = false)
        {
            return Expression(propertyName, Resources.KQL.Leq, ConvertDateTimeToQueryFormat(value, excludeTime));
        }

        /// <summary>
        ///     Возвращает результаты со значением свойства, меньшим или равным значению, указанному в ограничении свойства.
        /// </summary>
        /// <param name="propertyName">Название свойства</param>
        /// <param name="value">Значение</param>
        /// <returns>Выражение</returns>
        public static string Leq(string propertyName, int value)
        {
            return Expression(propertyName, Resources.KQL.Leq, value.ToString(CultureInfo.InvariantCulture));
        }

        /// <summary>
        ///     Возвращает результаты со значением свойства, меньшим или равным значению, указанному в ограничении свойства.
        /// </summary>
        /// <param name="propertyName">Название свойства</param>
        /// <param name="value">Значение</param>
        /// <returns>Выражение</returns>
        public static string Leq(string propertyName, long value)
        {
            return Expression(propertyName, Resources.KQL.Leq, value.ToString(CultureInfo.InvariantCulture));
        }

        /// <summary>
        ///     Возвращает результаты со значением свойства, меньшим или равным значению, указанному в ограничении свойства.
        /// </summary>
        /// <param name="propertyName">Название свойства</param>
        /// <param name="value">Значение</param>
        /// <returns>Выражение</returns>
        public static string Leq(string propertyName, decimal value)
        {
            return Expression(propertyName, Resources.KQL.Leq, value.ToString(CultureInfo.InvariantCulture));
        }

        /// <summary>
        ///     Возвращает результаты со значением свойства, меньшим или равным значению, указанному в ограничении свойства.
        /// </summary>
        /// <param name="propertyName">Название свойства</param>
        /// <param name="value">Значение</param>
        /// <returns>Выражение</returns>
        public static string Leq(string propertyName, double value)
        {
            return Expression(propertyName, Resources.KQL.Leq, value.ToString(CultureInfo.InvariantCulture));
        }

        #endregion

        #region Geq

        /// <summary>
        ///     Возвращает результаты со значением свойства, большим или равным значению, указанному в ограничении свойства.
        /// </summary>
        /// <param name="propertyName">Название свойства</param>
        /// <param name="value">Значение</param>
        /// <param name="excludeTime">Исключить время</param>
        /// <returns>Выражение</returns>
        public static string Geq(string propertyName, DateTime value, bool excludeTime = false)
        {
            return Expression(propertyName, Resources.KQL.Geq, ConvertDateTimeToQueryFormat(value, excludeTime));
        }

        /// <summary>
        ///     Возвращает результаты со значением свойства, большим или равным значению, указанному в ограничении свойства.
        /// </summary>
        /// <param name="propertyName">Название свойства</param>
        /// <param name="value">Значение</param>
        /// <returns>Выражение</returns>
        public static string Geq(string propertyName, int value)
        {
            return Expression(propertyName, Resources.KQL.Geq, value.ToString(CultureInfo.InvariantCulture));
        }

        /// <summary>
        ///     Возвращает результаты со значением свойства, большим или равным значению, указанному в ограничении свойства.
        /// </summary>
        /// <param name="propertyName">Название свойства</param>
        /// <param name="value">Значение</param>
        /// <returns>Выражение</returns>
        public static string Geq(string propertyName, long value)
        {
            return Expression(propertyName, Resources.KQL.Geq, value.ToString(CultureInfo.InvariantCulture));
        }

        /// <summary>
        ///     Возвращает результаты со значением свойства, большим или равным значению, указанному в ограничении свойства.
        /// </summary>
        /// <param name="propertyName">Название свойства</param>
        /// <param name="value">Значение</param>
        /// <returns>Выражение</returns>
        public static string Geq(string propertyName, decimal value)
        {
            return Expression(propertyName, Resources.KQL.Geq, value.ToString(CultureInfo.InvariantCulture));
        }

        /// <summary>
        ///     Возвращает результаты со значением свойства, большим или равным значению, указанному в ограничении свойства.
        /// </summary>
        /// <param name="propertyName">Название свойства</param>
        /// <param name="value">Значение</param>
        /// <returns>Выражение</returns>
        public static string Geq(string propertyName, double value)
        {
            return Expression(propertyName, Resources.KQL.Geq, value.ToString(CultureInfo.InvariantCulture));
        }

        #endregion

        #region Neq

        /// <summary>
        ///     Возвращает результаты со значением свойства, не равным значению, указанному в ограничении свойства.
        /// </summary>
        /// <param name="propertyName">Название свойства</param>
        /// <param name="value">Значение</param>
        /// <returns>Выражение</returns>
        public static string Neq(string propertyName, string value)
        {
            return Expression(propertyName, Resources.KQL.Neq, value);
        }

        /// <summary>
        ///     Возвращает результаты со значением свойства, не равным значению, указанному в ограничении свойства.
        /// </summary>
        /// <param name="propertyName">Название свойства</param>
        /// <param name="value">Значение</param>
        /// <returns>Выражение</returns>
        public static string Neq(string propertyName, int value)
        {
            return Neq(propertyName, value.ToString(CultureInfo.InvariantCulture));
        }

        /// <summary>
        ///     Возвращает результаты со значением свойства, не равным значению, указанному в ограничении свойства.
        /// </summary>
        /// <param name="propertyName">Название свойства</param>
        /// <param name="value">Значение</param>
        /// <returns>Выражение</returns>
        public static string Neq(string propertyName, long value)
        {
            return Neq(propertyName, value.ToString(CultureInfo.InvariantCulture));
        }

        /// <summary>
        ///     Возвращает результаты со значением свойства, не равным значению, указанному в ограничении свойства.
        /// </summary>
        /// <param name="propertyName">Название свойства</param>
        /// <param name="value">Значение</param>
        /// <returns>Выражение</returns>
        public static string Neq(string propertyName, decimal value)
        {
            return Neq(propertyName, value.ToString(CultureInfo.InvariantCulture));
        }

        /// <summary>
        ///     Возвращает результаты со значением свойства, не равным значению, указанному в ограничении свойства.
        /// </summary>
        /// <param name="propertyName">Название свойства</param>
        /// <param name="value">Значение</param>
        /// <returns>Выражение</returns>
        public static string Neq(string propertyName, double value)
        {
            return Neq(propertyName, value.ToString(CultureInfo.InvariantCulture));
        }

        /// <summary>
        ///     Возвращает результаты со значением свойства, не равным значению, указанному в ограничении свойства.
        /// </summary>
        /// <param name="propertyName">Название свойства</param>
        /// <param name="value">Значение</param>
        /// <returns>Выражение</returns>
        public static string Neq(string propertyName, bool value)
        {
            return Neq(propertyName, ConvertBoolToQueryFormat(value));
        }

        /// <summary>
        ///     Возвращает результаты со значением свойства, не равным значению, указанному в ограничении свойства.
        /// </summary>
        /// <param name="propertyName">Название свойства</param>
        /// <param name="value">Значение</param>
        /// <param name="excludeTime">Исключить время</param>
        /// <returns>Выражение</returns>
        public static string Neq(string propertyName, DateTime value, bool excludeTime = false)
        {
            return Neq(propertyName, ConvertDateTimeToQueryFormat(value, excludeTime));
        }

        #endregion

        #region Range

        /// <summary>
        ///     Возвращает результаты, в которых значение свойства включено в диапазон, указанный в ограничении свойства.
        /// </summary>
        /// <param name="propertyName">Название свойства</param>
        /// <param name="startValue">Начальное значение</param>
        /// <param name="endValue">Конечное значение</param>
        /// <returns>Выражение</returns>
        private static string Range(string propertyName, string startValue, string endValue)
        {
            if (startValue == null)
                throw new ArgumentNullException(nameof(startValue));
            if (startValue == string.Empty)
                throw new ArgumentException(@"Параметр не может быть пустым", nameof(startValue));
            if (endValue == null)
                throw new ArgumentNullException(nameof(endValue));
            if (endValue == string.Empty)
                throw new ArgumentException(@"Параметр не может быть пустым", nameof(endValue));

            var valueExpression = string.Format(ConcatPattern, startValue, Resources.KQL.Range, endValue);

            return StrongEq(propertyName, valueExpression);
        }

        /// <summary>
        ///     Возвращает результаты, в которых значение свойства включено в диапазон, указанный в ограничении свойства.
        /// </summary>
        /// <param name="propertyName">Название свойства</param>
        /// <param name="startValue">Начальное значение</param>
        /// <param name="endValue">Конечное значение</param>
        /// <param name="excludeTime">Исключить время</param>
        /// <returns>Выражение</returns>
        public static string Range(string propertyName, DateTime startValue, DateTime endValue,
            bool excludeTime = false)
        {
            return Range(propertyName, ConvertDateTimeToQueryFormat(startValue, excludeTime),
                ConvertDateTimeToQueryFormat(endValue, excludeTime));
        }

        /// <summary>
        ///     Возвращает результаты, в которых значение свойства включено в диапазон, указанный в ограничении свойства.
        /// </summary>
        /// <param name="propertyName">Название свойства</param>
        /// <param name="startValue">Начальное значение</param>
        /// <param name="endValue">Конечное значение</param>
        /// <returns>Выражение</returns>
        public static string Range(string propertyName, int startValue, int endValue)
        {
            return Range(propertyName, startValue.ToString(CultureInfo.InvariantCulture),
                endValue.ToString(CultureInfo.InvariantCulture));
        }

        /// <summary>
        ///     Возвращает результаты, в которых значение свойства включено в диапазон, указанный в ограничении свойства.
        /// </summary>
        /// <param name="propertyName">Название свойства</param>
        /// <param name="startValue">Начальное значение</param>
        /// <param name="endValue">Конечное значение</param>
        /// <returns>Выражение</returns>
        public static string Range(string propertyName, long startValue, long endValue)
        {
            return Range(propertyName, startValue.ToString(CultureInfo.InvariantCulture),
                endValue.ToString(CultureInfo.InvariantCulture));
        }

        /// <summary>
        ///     Возвращает результаты, в которых значение свойства включено в диапазон, указанный в ограничении свойства.
        /// </summary>
        /// <param name="propertyName">Название свойства</param>
        /// <param name="startValue">Начальное значение</param>
        /// <param name="endValue">Конечное значение</param>
        /// <returns>Выражение</returns>
        public static string Range(string propertyName, decimal startValue, decimal endValue)
        {
            return Range(propertyName, startValue.ToString(CultureInfo.InvariantCulture),
                endValue.ToString(CultureInfo.InvariantCulture));
        }

        /// <summary>
        ///     Возвращает результаты, в которых значение свойства включено в диапазон, указанный в ограничении свойства.
        /// </summary>
        /// <param name="propertyName">Название свойства</param>
        /// <param name="startValue">Начальное значение</param>
        /// <param name="endValue">Конечное значение</param>
        /// <returns>Выражение</returns>
        public static string Range(string propertyName, double startValue, double endValue)
        {
            return Range(propertyName, startValue.ToString(CultureInfo.InvariantCulture),
                endValue.ToString(CultureInfo.InvariantCulture));
        }

        #endregion

        #region Логические выражения

        /// <summary>
        ///     Возвращает результаты поиска, содержащие все произвольные выражения или ограничения свойств, заданные оператором
        ///     AND.
        /// </summary>
        /// <param name="leftExpression">Левое выражение</param>
        /// <param name="rightExpression">Правое выражение</param>
        /// <returns>Выражение</returns>
        public static string And(string leftExpression, string rightExpression)
        {
            return LogicalExpression(leftExpression, Resources.KQL.And, rightExpression);
        }

        /// <summary>
        ///     Возвращает результаты поиска, содержащие все произвольные выражения или ограничения свойств, заданные оператором
        ///     AND.
        /// </summary>
        /// <param name="expressions">Выражения</param>
        /// <param name="addParenthesis">Добавить круглые скобки</param>
        /// <returns>Выражение</returns>
        public static string And(IList<string> expressions, bool addParenthesis = false)
        {
            return AggregateLogicalExpression(Resources.KQL.And, expressions, addParenthesis);
        }

        /// <summary>
        ///     Возвращает результаты поиска, содержащие одно или несколько заданных произвольных выражений или ограничений
        ///     свойств.
        /// </summary>
        /// <param name="leftExpression">Левое выражение</param>
        /// <param name="rightExpression">Правое выражение</param>
        /// <returns>Выражение</returns>
        public static string Or(string leftExpression, string rightExpression)
        {
            return LogicalExpression(leftExpression, Resources.KQL.Or, rightExpression);
        }

        /// <summary>
        ///     Возвращает результаты поиска, содержащие все произвольные выражения или ограничения свойств, заданные оператором
        ///     AND.
        /// </summary>
        /// <param name="expressions">Выражения</param>
        /// <param name="addParenthesis">Добавить круглые скобки</param>
        /// <returns>Выражение</returns>
        public static string Or(IList<string> expressions, bool addParenthesis = false)
        {
            return AggregateLogicalExpression(Resources.KQL.Or, expressions, addParenthesis);
        }

        /// <summary>
        ///     Строит запрос по мягкому сравнению (Eq, ":") значению и объединяет логическим оператором AND
        /// </summary>
        /// <param name="propertyName">Название свойства</param>
        /// <param name="values">Значения</param>
        /// <returns>Выражение</returns>
        public static string AndEq(string propertyName, IList<string> values)
        {
            return AggregateExpression(propertyName, Resources.KQL.Eq, values, Resources.KQL.And);
        }

        /// <summary>
        ///     Строит запрос по точному сравнению (StrongEq, "=") значению и объединяет логическим оператором AND
        /// </summary>
        /// <param name="propertyName">Название свойства</param>
        /// <param name="values">Значения</param>
        /// <returns>Выражение</returns>
        public static string AndStrongEq(string propertyName, IList<string> values)
        {
            return AggregateExpression(propertyName, Resources.KQL.StrongEg, values, Resources.KQL.And);
        }

        /// <summary>
        ///     Строит запрос по точному сравнению (StrongEq, "=") значению и объединяет логическим оператором OR
        /// </summary>
        /// <param name="propertyName">Название свойства</param>
        /// <param name="values">Значения</param>
        /// <returns>Выражение</returns>
        public static string OrStrongEq(string propertyName, IList<string> values)
        {
            return AggregateExpression(propertyName, Resources.KQL.StrongEg, values, Resources.KQL.Or);
        }

        /// <summary>
        ///     Возвращает результаты поиска, не содержащие заданных произвольных выражений или ограничений свойств. Укажите
        ///     допустимое произвольное текстовое выражение и (или) допустимое ограничение свойств после оператора NOT.
        /// </summary>
        /// <param name="leftExpression">Левое выражение</param>
        /// <param name="rightExpression">Правое выражение</param>
        /// <returns>Выражение</returns>
        public static string Not(string leftExpression, string rightExpression)
        {
            return LogicalExpression(leftExpression, Resources.KQL.Not, rightExpression);
        }

        #endregion

        #region Структурные выражения

        /// <summary>
        ///     Возвращает результаты поиска, содержищие хотябы одно слово
        /// </summary>
        /// <param name="words">Коллекция слов</param>
        /// <returns>Выражение</returns>
        public static string Any(params string[] words)
        {
            return StructureExpression(Resources.KQL.Any, words);
        }

        /// <summary>
        ///     Возвращает результаты поиска, содержищие хотябы одно слово
        /// </summary>
        /// <param name="words">Коллекция слов в виде одной строки (в качестве разделителя должен быть один пробел)</param>
        /// <returns>Выражение</returns>
        public static string Any(string words)
        {
            return StructureExpression(Resources.KQL.Any, words);
        }

        /// <summary>
        ///     Возвращает результаты поиска, содержищие одновременно все слова
        /// </summary>
        /// <param name="words">Коллекция слов</param>
        /// <returns>Выражение</returns>
        public static string All(params string[] words)
        {
            return StructureExpression(Resources.KQL.All, words);
        }

        /// <summary>
        ///     Возвращает результаты поиска, содержищие одновременно все слова
        /// </summary>
        /// <param name="words">Коллекция слов в виде одной строки (в качестве разделителя должен быть один пробел)</param>
        /// <returns>Выражение</returns>
        public static string All(string words)
        {
            return StructureExpression(Resources.KQL.All, words);
        }

        /// <summary>
        ///     Поиск одновременно несколько слов. Замена выражения: слово1 OR слово2 OR слово3
        /// </summary>
        /// <param name="words">Коллекция слов</param>
        /// <returns>Выражение</returns>
        public static string Words(params string[] words)
        {
            var escapetWords = words.Where(word => !string.IsNullOrEmpty(word)).Select(PrepareStringPhrase);
            return StructureExpression(Resources.KQL.Words, escapetWords.ToArray(), ", ");
        }

        #endregion

        #region Near/Onear

        /// <summary>
        ///     Оператор NEAR возвращает совпадения, в которых заданные термины поиска находятся в непосредственной близости друг
        ///     от друга (при этом не учитывается порядок следования терминов).
        /// </summary>
        /// <param name="leftExpression">Левое выражение</param>
        /// <param name="rightExpression">Правое выражение</param>
        /// <param name="maxTermLength">Максимальное расстояние между терминами</param>
        /// <returns>Выражение</returns>
        public static string Near(string leftExpression, string rightExpression, uint maxTermLength)
        {
            return NearOnearInternal(leftExpression, rightExpression, Resources.KQL.Near, maxTermLength);
        }

        /// <summary>
        ///     Оператор NEAR возвращает совпадения, в которых заданные термины поиска находятся в непосредственной близости друг
        ///     от друга (при этом не учитывается порядок следования терминов).
        /// </summary>
        /// <param name="leftExpression">Левое выражение</param>
        /// <param name="rightExpression">Правое выражение</param>
        /// <returns>Выражение</returns>
        public static string Near(string leftExpression, string rightExpression)
        {
            return NearOnearInternal(leftExpression, rightExpression, Resources.KQL.Near, null);
        }

        /// <summary>
        ///     Оператор ONEAR возвращает совпадения, в которых заданные термины поиска находятся в непосредственной близости друг
        ///     от друга, при этом сохраняется порядок их следования.
        /// </summary>
        /// <param name="leftExpression">Левое выражение</param>
        /// <param name="rightExpression">Правое выражение</param>
        /// <param name="maxTermLength">Максимальное расстояние между терминами</param>
        /// <returns>Выражение</returns>
        public static string Onear(string leftExpression, string rightExpression, uint maxTermLength)
        {
            return NearOnearInternal(leftExpression, rightExpression, Resources.KQL.Onear, maxTermLength);
        }

        /// <summary>
        ///     Оператор ONEAR возвращает совпадения, в которых заданные термины поиска находятся в непосредственной близости друг
        ///     от друга, при этом сохраняется порядок их следования.
        /// </summary>
        /// <param name="leftExpression">Левое выражение</param>
        /// <param name="rightExpression">Правое выражение</param>
        /// <returns>Выражение</returns>
        public static string Onear(string leftExpression, string rightExpression)
        {
            return NearOnearInternal(leftExpression, rightExpression, Resources.KQL.Onear, null);
        }

        #endregion

        #region Wildcard

        /// <summary>
        ///     Оператор подстановочного знака слева. Вы можете указать в запросе конечную часть слова после подстановочного знака.
        /// </summary>
        /// <param name="wordPart">Часть слова</param>
        /// <returns>Выражение</returns>
        public static string WildcardLeft(string wordPart)
        {
            if (wordPart == null)
                throw new ArgumentNullException(nameof(wordPart));
            if (wordPart == string.Empty)
                throw new ArgumentException(@"Параметр не может быть пустым", nameof(wordPart));

            return string.Format("*{0}", wordPart.TrimStart('*').Trim());
        }

        /// <summary>
        ///     Оператор подстановочного знака справа. Вы можете указать в запросе начальную часть слова, поставив далее оператор
        ///     подстановочного знака.
        /// </summary>
        /// <param name="wordPart">Часть слова</param>
        /// <returns>Выражение</returns>
        public static string WildcardRight(string wordPart)
        {
            if (wordPart == null)
                throw new ArgumentNullException(nameof(wordPart));
            if (wordPart == string.Empty)
                throw new ArgumentException(@"Параметр не может быть пустым", nameof(wordPart));

            return string.Format("{0}*", wordPart.TrimEnd('*').Trim());
        }

        /// <summary>
        ///     Оператор подстановочного знака c двух сторон. Вы можете указать в запросе часть слова.
        /// </summary>
        /// <param name="wordPart">Часть слова</param>
        /// <returns>Выражение</returns>
        public static string WildcardBoth(string wordPart)
        {
            if (wordPart == null)
                throw new ArgumentNullException(nameof(wordPart));
            if (wordPart == string.Empty)
                throw new ArgumentException(@"Параметр не может быть пустым", nameof(wordPart));

            return string.Format("*{0}*", wordPart.Trim(' ', '*'));
        }

        #endregion

        #region Операторы включения и исключения

        /// <summary>
        ///     Включает в результаты контент со значениями, совпадающими с теми, которые нужно включить.
        /// </summary>
        /// <param name="word">Фраза</param>
        /// <returns>Выражение</returns>
        /// <remarks>
        ///     Это поведение по умолчанию, если символы не указаны. Аналогичный результат будет при использовании оператора
        ///     AND.
        /// </remarks>
        public static string Plus(string word)
        {
            return PlusMinusInternal(Resources.KQL.Plus, word);
        }

        /// <summary>
        ///     Исключает контент со значениями, совпадающими с теми, которые нужно исключить.
        /// </summary>
        /// <param name="word">Фраза</param>
        /// <returns>Выражение</returns>
        /// <remarks>Аналогичный результат будет при использовании оператора NOT.</remarks>
        public static string Minus(string word)
        {
            return PlusMinusInternal(Resources.KQL.Minus, word);
        }

        #endregion
    }
}