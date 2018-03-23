using System;
using System.ComponentModel;
using System.Globalization;

namespace InAsync.ConvertExtras.TryParsers {

    public class TypeConverterTryParser : ITryParser {
        public static readonly TypeConverterTryParser Default = new TypeConverterTryParser();

        public bool? TryParse<T>(string input, IFormatProvider provider, out T result) {
            return TryParseCore<T>(typeof(T), input, provider, out result);
        }

        public bool? TryParse(Type conversionType, string input, IFormatProvider provider, out object result) {
            return TryParseCore<object>(conversionType, input, provider, out result);
        }

        private bool? TryParseCore<TResult>(Type conversionType, string input, IFormatProvider provider, out TResult result) {
            // TypeConverter で変換。
            // ※ 同じ型の TryParse と挙動が一致しない事もあるので留意する事。
            var converter = TypeDescriptor.GetConverter(conversionType);
            if (converter.CanConvertFrom(typeof(string)) == false) {
                result = default(TResult);
                return null;
            }

            var culture = provider as CultureInfo ?? CultureInfo.CurrentCulture;
            try {
                result = (TResult)converter.ConvertFrom(null, culture, input);
                return true;
            }
            catch {
                result = default(TResult);
                return false;
            }
        }
    }
}