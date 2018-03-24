using System;
using System.ComponentModel;
using System.Globalization;

namespace InAsync.ConvertExtras.TryParseProviders {

    public class TypeConverterTryParserProvider : TryParseProvider {
        public static readonly TypeConverterTryParserProvider Default = new TypeConverterTryParserProvider();

        public override TryParseDelegate<T> GetDelegate<T>() {
            var converter = GetConverter(typeof(T));
            if (converter == null) return null;

            return (string input, IFormatProvider provider, out T result) => TryParseCore(converter, input, provider, out result);
        }

        public override TryParseDelegate<object> GetDelegate(Type conversionType) {
            var converter = GetConverter(conversionType);
            if (converter == null) return null;

            return (string input, IFormatProvider provider, out object result) => TryParseCore(converter, input, provider, out result);
        }

        private TypeConverter GetConverter(Type conversionType) {
            var converter = TypeDescriptor.GetConverter(conversionType);
            if (converter.CanConvertFrom(typeof(string)) == false) {
                return null;
            }
            return converter;
        }

        /// <summary>
        /// TypeConverter で変換。
        /// </summary>
        /// <remarks>
        /// 同じ型の TryParse と挙動が一致しない事もあるので留意する事。
        /// </remarks>
        /// <typeparam name="T"></typeparam>
        /// <param name="converter"></param>
        /// <param name="input"></param>
        /// <param name="provider"></param>
        /// <param name="result"></param>
        /// <returns></returns>
        private bool TryParseCore<T>(TypeConverter converter, string input, IFormatProvider provider, out T result) {
            var culture = provider as CultureInfo ?? CultureInfo.CurrentCulture;
            try {
                result = (T)converter.ConvertFrom(null, culture, input);
                return true;
            }
            catch {
                result = default(T);
                return false;
            }
        }
    }
}