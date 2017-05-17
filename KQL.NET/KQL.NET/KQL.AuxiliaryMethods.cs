using System;

namespace KirillVinokurov.SharePoint.Search
{
    public static partial class KQL
    {
        /// <summary>
        ///     ����� �������������� ���������� ���� DateTime � ������ KQL �������
        /// </summary>
        /// <param name="dateTimeValue">�������� � ������� DateTime</param>
        /// <param name="excludeTime">��������� �����</param>
        /// <returns>����� � ������� KQL �������</returns>
        public static string ConvertDateTimeToQueryFormat(DateTime dateTimeValue, bool excludeTime = false)
        {
            var dateTimeInUtc = dateTimeValue.ToUniversalTime();
            return excludeTime ? dateTimeInUtc.Date.ToString("yyyy-MM-dd") : dateTimeInUtc.ToString("O"); //ISO 8601
        }

        /// <summary>
        ///     ����� �������������� ���������� ���� bool � ������ KQL �������
        /// </summary>
        /// <param name="value">�������� � ������� bool</param>
        /// <returns>Bool � ������� KQL �������</returns>
        public static string ConvertBoolToQueryFormat(bool value)
        {
            return value ? Resources.KQL.True : Resources.KQL.False;
        }

        /// <summary>
        ///     ����� ��������� ������ � ������� �������
        /// </summary>
        /// <param name="phrase">�����</param>
        /// <returns>����� � ��������</returns>
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
        ///     ����� ���������� ��������� ����� ��� ������
        /// </summary>
        /// <param name="phrase">�����</param>
        /// <returns>�������������� �����</returns>
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