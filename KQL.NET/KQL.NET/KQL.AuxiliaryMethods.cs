using System;

namespace KirillVinokurov.SharePoint.Search
{
    public static partial class KQL
    {
        /// <summary>
        ///     Метод преобразования переменной типа DateTime в формат KQL запроса
        /// </summary>
        /// <param name="dateTimeValue">Значение в формате DateTime</param>
        /// <param name="excludeTime">Исключить время</param>
        /// <returns>Время в формате KQL запроса</returns>
        public static string ConvertDateTimeToQueryFormat(DateTime dateTimeValue, bool excludeTime = false)
        {
            var dateTimeInUtc = dateTimeValue.ToUniversalTime();
            return excludeTime ? dateTimeInUtc.Date.ToString("yyyy-MM-dd") : dateTimeInUtc.ToString("O"); //ISO 8601
        }

        /// <summary>
        ///     Метод преобразования переменной типа bool в формат KQL запроса
        /// </summary>
        /// <param name="value">Значение в формате bool</param>
        /// <returns>Bool в формате KQL запроса</returns>
        public static string ConvertBoolToQueryFormat(bool value)
        {
            return value ? Resources.KQL.True : Resources.KQL.False;
        }

        /// <summary>
        ///     Метод включения строки в двойные кавычки
        /// </summary>
        /// <param name="phrase">Фраза</param>
        /// <returns>Фраза в кавычках</returns>
        public static string QuoteString(string phrase)
        {
            if (phrase == null)
                return null;

            if (phrase.IndexOf('"') != -1)
                phrase = phrase.Replace("\"", string.Empty);

            phrase = phrase.Trim();

            return phrase == string.Empty
                ? string.Empty
                : string.Format("\"{0}\"", phrase);
        }

        /// <summary>
        ///     Метод подготовки текстовой фразы для поиска
        /// </summary>
        /// <param name="phrase">Фраза</param>
        /// <returns>Подготовленная фраза</returns>
        public static string PrepareStringPhrase(string phrase)
        {
            if (phrase == null)
                return null;

            if (phrase.IndexOf('"') != -1)
                phrase = phrase.Replace("\"", string.Empty);

            phrase = phrase.Trim();

            if (phrase == string.Empty)
                return string.Empty;

            return phrase.IndexOf(' ') == -1 ? phrase : string.Format("\"{0}\"", phrase);
        }
    }
}