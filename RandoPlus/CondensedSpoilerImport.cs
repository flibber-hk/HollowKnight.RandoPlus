using System;
using System.Collections.Generic;
using MonoMod.ModInterop;
using RandomizerMod.Logging;

namespace RandoPlus
{
    internal static class CondensedSpoilerLogger
    {
        [ModImportName("CondensedSpoilerLogger")]
        private static class CondensedSpoilerLoggerImport
        {
            public static Action<string, Func<LogArguments, bool>, List<string>> AddCategory = null;
        }
        static CondensedSpoilerLogger()
        {
            typeof(CondensedSpoilerLoggerImport).ModInterop();
        }
        /// <summary>
        /// Add a category to the condensed spoiler log.
        /// </summary>
        /// <param name="categoryName">The title to give the category.</param>
        /// <param name="test">Return false to skip adding this category to the log.</param>
        /// <param name="entries">A list of items to log in the category.</param>
        public static void AddCategory(string categoryName, Func<LogArguments, bool> test, List<string> entries)
            => CondensedSpoilerLoggerImport.AddCategory?.Invoke(categoryName, test, entries);
    }
}
