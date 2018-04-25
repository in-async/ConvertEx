using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Microsoft.VisualStudio.TestTools.UnitTesting {

    public static class ObjectExtensions {

        public static void Is<T>(this T actual, T expected, object error = null) {
            if (typeof(T) != typeof(string) && typeof(IEnumerable).IsAssignableFrom(typeof(T))) {
                ((IEnumerable)actual).Cast<object>().Is(((IEnumerable)expected).Cast<object>(), error);
                return;
            }

            Assert.AreEqual(expected, actual, $"{new { expected, actual }}\n{error}");
        }

        public static void Is<T>(this IEnumerable<T> actual, IEnumerable<T> expected, object error = null) {
            CollectionAssert.AreEqual(expected.ToList(), actual.ToList(), $"{new { expected, actual }}\n{error}");
        }
    }
}