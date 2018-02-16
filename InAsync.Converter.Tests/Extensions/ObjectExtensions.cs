using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace InAsync.Tests {

    internal static class ObjectExtensions {

        public static void Is<T>(this T actual, T expected, string error = null) {
            Assert.AreEqual(expected, actual, $"{new { actual, expected }}\n{error}");
        }
    }
}