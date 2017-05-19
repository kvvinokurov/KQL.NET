namespace KQL.NET
{
    public static partial class KQL
    {
        /// <summary>
        ///  Шаблон с пробелами на 3 замены
        /// </summary>
        private const string WithSpacesPattern = "{0} {1} {2}";
        /// <summary>
        /// Шаблон для построения выражения в скобках
        /// </summary>
        private const string ParenthesisPattern = "({0})";
        /// <summary>
        /// Шаблон для аргументов на 2 замены
        /// </summary>
        private const string PropertyPattern = "{0}={1}";
        /// <summary>
        /// Шаблон для XRANK на 4 замены
        /// </summary>
        private const string XrankPattern = "{0} {1}({2}) {3}";
    }
}