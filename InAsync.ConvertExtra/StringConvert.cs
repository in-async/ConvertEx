using InAsync.ConvertExtra.TryParsers;
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Globalization;

namespace InAsync {

    /// <summary>
    /// 文字列を任意の型へ変換するクラス。
    /// </summary>
    /// <remarks>
    /// 変換先としてサポートする型は以下の通りです。
    /// - <c>Byte</c> / <c>SByte</c>
    /// - <c>Int16</c> / <c>UInt16</c>
    /// - <c>Int32</c> / <c>UInt32</c>
    /// - <c>Int64</c> / <c>UInt64</c>
    /// - <c>Single</c>
    /// - <c>Double</c>
    /// - <c>Decimal</c>
    /// - <c>Boolean</c>
    /// - <c>Char</c>
    /// - <c>DateTime</c>
    /// - <c>TimeSpan</c>
    /// - <c>Enum</c>
    /// - <c>Guid</c>
    /// - 上記構造体の <c>Nullable</c> 型
    /// - <c>String</c>
    /// - <c>Version</c>
    /// - <c>Uri</c>
    /// - 文字列からの変換をサポートしている <see cref="TypeConverter"/> を持つ型
    /// </remarks>
    public static partial class StringConvert {

        private static readonly IReadOnlyList<ITryParser> s_TryParsers = new ITryParser[]{
            NativeTryParser.Default,
            EnumTryParser.Default,
            TypeConverterTryParser.Default,
        };

        /// <summary>
        /// 文字列を <typeparamref name="T"/> の型に変換します。
        /// </summary>
        /// <typeparam name="T">変換後の型。</typeparam>
        /// <param name="input">入力文字列。</param>
        /// <param name="defaultValue">変換に失敗した際に返す値。</param>
        /// <returns>変換に成功すれば変換後の値、それ以外なら <paramref name="defaultValue"/>。</returns>
        public static T To<T>(this string input, T defaultValue = default(T))
            => TryParse<T>(input, out var result) ? result : defaultValue;

        /// <summary>
        /// 文字列を <typeparamref name="T"/> の型に変換します。
        /// </summary>
        /// <typeparam name="T">変換後の型。</typeparam>
        /// <param name="input">入力文字列。</param>
        /// <param name="result">変換に成功すれば変換後の値、それ以外なら <typeparamref name="T"/> の既定値が返されます。</param>
        /// <returns>変換に成功すれば <c>true</c>、それ以外なら <c>false</c>。</returns>
        public static bool TryParse<T>(string input, out T result)
            => TryParse<T>(input, CultureInfo.CurrentCulture, out result);

        /// <summary>
        /// 文字列を <typeparamref name="T"/> の型に変換します。
        /// </summary>
        /// <typeparam name="T">変換後の型。</typeparam>
        /// <param name="input">入力文字列。</param>
        /// <param name="provider">カルチャ固有の書式情報。<c>null</c> の場合は現在のカルチャが使用されます。</param>
        /// <param name="result">変換に成功すれば変換後の値、それ以外なら <typeparamref name="T"/> の既定値が返されます。</param>
        /// <returns>変換に成功すれば <c>true</c>、それ以外なら <c>false</c>。</returns>
        public static bool TryParse<T>(string input, IFormatProvider provider, out T result) {
            for (var i = 0; i < s_TryParsers.Count; i++) {
                var tryParser = s_TryParsers[i];

                var parserResult = tryParser.Execute<T>(input, provider);
                if (parserResult.Parsed) {
                    result = parserResult.Value;
                    return parserResult.Success;
                }
            }

            result = default(T);
            return false;
        }

        /// <summary>
        /// 文字列を <paramref name="conversionType"/> の型に変換します。
        /// </summary>
        /// <param name="input">入力文字列。</param>
        /// <param name="conversionType">変換後の型。</param>
        /// <param name="result">変換に成功すれば変換後の値、それ以外なら <c>null</c> が返されます。</param>
        /// <returns>変換に成功すれば <c>true</c>、それ以外なら <c>false</c>。</returns>
        /// <exception cref="ArgumentNullException"><paramref name="conversionType"/> が <c>null</c> の場合に投げられます。</exception>
        public static bool TryParse(string input, Type conversionType, out object result)
            => TryParse(input, conversionType, CultureInfo.CurrentCulture, out result);

        /// <summary>
        /// 文字列を <paramref name="conversionType"/> の型に変換します。
        /// </summary>
        /// <param name="input">入力文字列。</param>
        /// <param name="conversionType">変換後の型。</param>
        /// <param name="provider">カルチャ固有の書式情報。<c>null</c> の場合は現在のカルチャが使用されます。</param>
        /// <param name="result">変換に成功すれば変換後の値、それ以外なら <c>null</c> が返されます。</param>
        /// <returns>変換に成功すれば <c>true</c>、それ以外なら <c>false</c>。</returns>
        /// <exception cref="ArgumentNullException"><paramref name="conversionType"/> が <c>null</c> の場合に投げられます。</exception>
        public static bool TryParse(string input, Type conversionType, IFormatProvider provider, out object result) {
            if (conversionType == null) throw new ArgumentNullException(nameof(conversionType));
            Contract.Ensures(Contract.Result<bool>() || Contract.ValueAtReturn(out result) == null);
            Contract.EndContractBlock();

            for (var i = 0; i < s_TryParsers.Count; i++) {
                var tryParser = s_TryParsers[i];

                var parserResult = tryParser.Execute(conversionType, input, provider);
                if (parserResult.Parsed) {
                    result = parserResult.Value;
                    return parserResult.Success;
                }
            }

            result = null;
            return false;
        }
    }
}