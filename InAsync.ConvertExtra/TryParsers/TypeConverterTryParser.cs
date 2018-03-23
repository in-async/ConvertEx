using System;
using System.ComponentModel;
using System.Globalization;

namespace InAsync.ConvertExtras.TryParsers {

    public class TypeConverterTryParser : ITryParser {
        public static readonly TypeConverterTryParser Default = new TypeConverterTryParser();

        public TryParserResult<T> Execute<T>(string input, IFormatProvider provider) {
            return ExecuteCore<T>(typeof(T), input, provider);
        }

        public TryParserResult<object> Execute(Type conversionType, string input, IFormatProvider provider) {
            return ExecuteCore<object>(conversionType, input, provider);
        }

        private TryParserResult<TReturn> ExecuteCore<TReturn>(Type conversionType, string input, IFormatProvider provider) {
            // TypeConverter で変換。
            // ※ 同じ型の TryParse と挙動が一致しない事もあるので留意する事。
            var converter = TypeDescriptor.GetConverter(conversionType);
            if (converter.CanConvertFrom(typeof(string)) == false) return TryParserResult<TReturn>.Empty;

            var culture = provider as CultureInfo ?? CultureInfo.CurrentCulture;
            try {
                return new TryParserResult<TReturn>(true, (TReturn)converter.ConvertFrom(null, culture, input));
            }
            catch {
                return new TryParserResult<TReturn>(false, default(TReturn));
            }
        }
    }
}