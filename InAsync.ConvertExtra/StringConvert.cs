using System;
using System.Collections.Generic;
using System.ComponentModel;
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

        private static readonly IReadOnlyDictionary<Type, Func<string, Type, IFormatProvider, (bool Success, object Result)>> _tryParsers = new Dictionary<Type, Func<string, Type, IFormatProvider, (bool, object)>> {
            [typeof(byte)] = (value, conversionType, provider) => (byte.TryParse(value, NumberStyles.Integer, provider, out var tmp), (object)tmp),
            [typeof(short)] = (value, conversionType, provider) => (short.TryParse(value, NumberStyles.Integer | NumberStyles.AllowThousands, provider, out var tmp), (object)tmp),
            [typeof(long)] = (value, conversionType, provider) => (long.TryParse(value, NumberStyles.Integer | NumberStyles.AllowThousands, provider, out var tmp), (object)tmp),
            [typeof(int)] = (value, conversionType, provider) => (int.TryParse(value, NumberStyles.Integer | NumberStyles.AllowThousands, provider, out var tmp), (object)tmp),
            [typeof(sbyte)] = (value, conversionType, provider) => (sbyte.TryParse(value, NumberStyles.Integer | NumberStyles.AllowThousands, provider, out var tmp), (object)tmp),
            [typeof(ushort)] = (value, conversionType, provider) => (ushort.TryParse(value, NumberStyles.Integer | NumberStyles.AllowThousands, provider, out var tmp), (object)tmp),
            [typeof(uint)] = (value, conversionType, provider) => (uint.TryParse(value, NumberStyles.Integer | NumberStyles.AllowThousands, provider, out var tmp), (object)tmp),
            [typeof(ulong)] = (value, conversionType, provider) => (ulong.TryParse(value, NumberStyles.Integer | NumberStyles.AllowThousands, provider, out var tmp), (object)tmp),
            [typeof(float)] = (value, conversionType, provider) => (float.TryParse(value, NumberStyles.Float | NumberStyles.AllowThousands, provider, out var tmp), (object)tmp),
            [typeof(double)] = (value, conversionType, provider) => (double.TryParse(value, NumberStyles.Float | NumberStyles.AllowThousands, provider, out var tmp), (object)tmp),
            [typeof(decimal)] = (value, conversionType, provider) => (decimal.TryParse(value, NumberStyles.Number, provider, out var tmp), (object)tmp),
            [typeof(bool)] = (value, conversionType, provider) => (bool.TryParse(value, out var tmp), (object)tmp),
            [typeof(char)] = (value, conversionType, provider) => (char.TryParse(value, out var tmp), (object)tmp),
            [typeof(DateTime)] = (value, conversionType, provider) => (DateTime.TryParse(value, provider, DateTimeStyles.None, out var tmp), (object)tmp),
            [typeof(TimeSpan)] = (value, conversionType, provider) => (TimeSpan.TryParse(value, provider, out var tmp), (object)tmp),
            [typeof(Enum)] = (value, conversionType, provider) => {
                // HACK 暫定
                try {
                    return (true, Enum.Parse(conversionType, value, ignoreCase: true));
                }
                catch (Exception ex) when (ex is ArgumentException || ex is OverflowException) {
                    return (false, (object)null);
                }
            },
            [typeof(Guid)] = (value, conversionType, provider) => (Guid.TryParse(value, out var tmp), (object)tmp),
            [typeof(string)] = (value, conversionType, provider) => (true, (object)value),
            [typeof(Version)] = (value, conversionType, provider) => (Version.TryParse(value, out var tmp), (object)tmp),
            [typeof(Uri)] = (value, conversionType, provider) => (Uri.TryCreate(value, UriKind.Absolute, out var tmp), (object)tmp),
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
            if (TryParse(input, typeof(T), provider, out var resultObj)) {
                result = (T)resultObj;
                return true;
            }
            else {
                result = default(T);
                return false;
            }
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

            // 変換先が Nullable なら、その基になる型を実際の変換先とする。
            var simpleType = Nullable.GetUnderlyingType(conversionType);
            if (simpleType != null) {
                conversionType = simpleType;
            }

            // 入力が null の場合、変換先が null を許容するか否かを戻り値とする。
            if (input == null) {
                result = null;
                return conversionType.IsValueType == false || simpleType != null;
            }

            // 辞書から変換関数を検索して変換。
            if (_tryParsers.TryGetValue(conversionType.IsEnum ? typeof(Enum) : conversionType, out var parser)) {
                var parsed = parser(input, conversionType, provider);
                if (parsed.Success) {
                    result = parsed.Result;
                    return true;
                }
                else {
                    result = null;
                    return false;
                }
            }

            // TypeConverter で変換。
            // ※ 同じ型の TryParse と挙動が一致しない事もあるので留意する事。
            var converter = TypeDescriptor.GetConverter(conversionType);
            if (converter.CanConvertFrom(typeof(string))) {
                var culture = provider as CultureInfo ?? CultureInfo.CurrentCulture;

                try {
                    result = converter.ConvertFrom(null, culture, input);
                    return true;
                }
                catch {
                    result = null;
                    return false;
                }
            }

            // 重いし、ここまで必要か疑問なので、コメントアウト。
            //// TryParse メソッド（リフレクション）で変換
            //var method = conversionType.GetMethod("TryParse", BindingFlags.Public | BindingFlags.Static, null, new[] { typeof(string), conversionType }, null);
            //if (method != null) {
            //    var args = new[] { input, null };
            //    if ((bool)method.Invoke(null, args)) {
            //        result = args[1];
            //        return true;
            //    }
            //    else {
            //        result = null;
            //        return false;
            //    }
            //}

            result = null;
            return false;
        }
    }
}