using System;
using System.ComponentModel;
using System.Globalization;

namespace InAsync.ConvertExtras.TryParseProviders {

    /// <summary>
    /// 文字列から、<see cref="TypeConverter"/> を実装している型への変換デリゲートを提供する <see cref="ITryParseProvider"/> クラス。
    /// </summary>
    public class TypeConverterTryParseProvider : TryParseProvider {
        public static readonly TypeConverterTryParseProvider Default = new TypeConverterTryParseProvider();

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

        /// <summary>
        /// <paramref name="conversionType"/> に紐付いている <see cref="TypeConverter"/> を返します。
        /// </summary>
        /// <param name="conversionType">変換後の型。</param>
        /// <returns><see cref="TypeConverter"/> が文字列からの変換をサポートしていれば <see cref="TypeConverter"/> インスタンス、それ以外の場合は <c>null</c>。</returns>
        private TypeConverter GetConverter(Type conversionType) {
            var converter = TypeDescriptor.GetConverter(conversionType);
            if (converter.CanConvertFrom(typeof(string)) == false) {
                return null;
            }
            return converter;
        }

        /// <summary>
        /// <paramref name="converter"/> を使用して文字列を変換します。
        /// </summary>
        /// <remarks>
        /// 同じ型の <c>TryParse</c> と挙動が一致しない事もあるので留意する事。
        /// </remarks>
        /// <typeparam name="TResult">戻り値の型。</typeparam>
        /// <param name="converter">変換に使用される <see cref="TypeConverter"/>。</param>
        /// <param name="input">入力文字列。</param>
        /// <param name="provider">カルチャ固有の書式情報。<c>null</c> の場合は現在のカルチャが使用されます。</param>
        /// <param name="result">変換に成功すれば変換後の値、それ以外なら <typeparamref name="TResult"/> の既定値が返されます。</param>
        /// <returns>変換に成功すれば <c>true</c>、それ以外なら <c>false</c>。</returns>
        private bool TryParseCore<TResult>(TypeConverter converter, string input, IFormatProvider provider, out TResult result) {
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