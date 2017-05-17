namespace KQL.NET
{
    public static partial class KQL
    {
        /// <summary>
        ///     Зарезервированы ключевые слова для работы с датами
        /// </summary>
        public static class DateTokens
        {
            /// <summary>
            ///     Представляет период времени с начала текущего дня до его окончания.
            /// </summary>
            public static string Today => Resources.KQL.Today;

            /// <summary>
            ///     Представляет период времени с начала предшествующего дня до его окончания.
            /// </summary>
            public static string Yesterday => Resources.KQL.Yesterday;

            /// <summary>
            ///     Представляет период времени от начала текущей недели до ее окончания. При определении первого дня недели
            ///     учитываются традиции региона, в котором формируется текст запроса.
            /// </summary>
            public static string ThisWeek => Resources.KQL.ThisWeek;

            /// <summary>
            ///     Представляет период времени с начала текущего месяца до его окончания.
            /// </summary>
            public static string ThisMonth => Resources.KQL.ThisMonth;

            /// <summary>
            ///     Представляет период времени от начала предыдущего месяца до его окончания.
            /// </summary>
            public static string LastMonth => Resources.KQL.LastMonth;

            /// <summary>
            ///     Представляет период времени от начала текущего года до его окончания.
            /// </summary>
            public static string ThisYear => Resources.KQL.ThisYear;

            /// <summary>
            ///     Представляет весь год, предшествующий текущему.
            /// </summary>
            public static string LastYear => Resources.KQL.LastYear;
        }
    }
}