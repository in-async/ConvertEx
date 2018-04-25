using System;
using System.Collections.Generic;
using System.Linq;

namespace InAsync.ConvertExtras.TryParseProviders {

    /// <summary>
    /// 文字列から特定の型への変換デリゲートを提供する <see cref="ITryParseProvider"/> クラス。
    /// </summary>
    /// <remarks>
    /// 変換先としてサポートされている型は以下の通りです。
    /// - <c>Byte</c> / <c>SByte</c>
    /// - <c>Int16</c> / <c>UInt16</c>
    /// - <c>Int32</c> / <c>UInt32</c>
    /// - <c>Int64</c> / <c>UInt64</c>
    /// - 上記構造体の <c>Nullable</c> 型
    /// </remarks>
    public class FastTryParseProvider : TryParseProvider {
        public static readonly FastTryParseProvider Default = new FastTryParseProvider();

        public override TryParseDelegate<T> GetDelegate<T>() => GenericTryParsers<T>.Value;

        public override TryParseDelegate<object> GetDelegate(Type conversionType) => NonGenericTryParsers.GetValue(conversionType);

        /// <summary>
        /// 型パラメーターによって変換デリゲートコレクションを管理するクラス。
        /// </summary>
        /// <typeparam name="T">変換後の型。</typeparam>
        private static class GenericTryParsers<T> {
            public static readonly TryParseDelegate<T> Value;

            static GenericTryParsers() {
                GenericTryParsers<byte>.Value = TryParse;
                GenericTryParsers<byte?>.Value = TryParseToNullable;
                GenericTryParsers<sbyte>.Value = TryParse;
                GenericTryParsers<sbyte?>.Value = TryParseToNullable;
                GenericTryParsers<short>.Value = TryParse;
                GenericTryParsers<short?>.Value = TryParseToNullable;
                GenericTryParsers<ushort>.Value = TryParse;
                GenericTryParsers<ushort?>.Value = TryParseToNullable;
                GenericTryParsers<int>.Value = TryParse;
                GenericTryParsers<int?>.Value = TryParseToNullable;
                GenericTryParsers<uint>.Value = TryParse;
                GenericTryParsers<uint?>.Value = TryParseToNullable;
                GenericTryParsers<long>.Value = TryParse;
                GenericTryParsers<long?>.Value = TryParseToNullable;
                GenericTryParsers<ulong>.Value = TryParse;
                GenericTryParsers<ulong?>.Value = TryParseToNullable;
                GenericTryParsers<float>.Value = TryParse;
                GenericTryParsers<float?>.Value = TryParseToNullable;
                GenericTryParsers<double>.Value = TryParse;
                GenericTryParsers<double?>.Value = TryParseToNullable;
                GenericTryParsers<decimal>.Value = TryParse;
                GenericTryParsers<decimal?>.Value = TryParseToNullable;
                GenericTryParsers<bool>.Value = (string value, IFormatProvider provider, out bool result) => {
                    if (string.IsNullOrEmpty(value) == false) {
                        value = value.Trim();

                        if (value.Equals("true", StringComparison.OrdinalIgnoreCase)) {
                            result = true;
                            return true;
                        }
                        else if (value.Equals("false", StringComparison.OrdinalIgnoreCase)) {
                            result = false;
                            return true;
                        }
                        //switch (value[0]) {
                        //    case 't':
                        //    case 'T':
                        //        if (value.Equals("true", StringComparison.OrdinalIgnoreCase)) {
                        //            result = true;
                        //            return true;
                        //        }
                        //        break;

                        //    case 'f':
                        //    case 'F':
                        //        if (value.Equals("false", StringComparison.OrdinalIgnoreCase)) {
                        //            result = true;
                        //            return true;
                        //        }
                        //        break;
                        //}
                    }
                    result = default(bool);
                    return false;
                };
                GenericTryParsers<bool?>.Value = TryParseToNullable;
                GenericTryParsers<char>.Value = (string value, IFormatProvider provider, out char result) => {
                    if (value?.Length == 1) {
                        result = value[0];
                        return true;
                    }
                    else {
                        result = default(char);
                        return false;
                    }
                };
                GenericTryParsers<char?>.Value = TryParseToNullable;
                //GenericTryParser<DateTime>.Value = (string value, IFormatProvider provider, out DateTime result) => DateTime.TryParse(value, provider, DateTimeStyles.None, out result);
                //GenericTryParser<DateTime?>.Value = TryParseToNullable;
                //GenericTryParser<TimeSpan>.Value = (string value, IFormatProvider provider, out TimeSpan result) => TimeSpan.TryParse(value, provider, out result);
                //GenericTryParser<TimeSpan?>.Value = TryParseToNullable;
                //GenericTryParser<Guid>.Value = (string value, IFormatProvider provider, out Guid result) => Guid.TryParse(value, out result);
                //GenericTryParser<Guid?>.Value = TryParseToNullable;
                GenericTryParsers<string>.Value = (string value, IFormatProvider provider, out string result) => {
                    result = value;
                    return true;
                };
                //GenericTryParser<Version>.Value = (string value, IFormatProvider provider, out Version result) => Version.TryParse(value, out result);
                //GenericTryParser<Uri>.Value = (string value, IFormatProvider provider, out Uri result) => Uri.TryCreate(value, UriKind.Absolute, out result);
            }

            /// <summary>
            /// <typeparamref name="TStruct"/> の <see cref="Nullable"/> 型へ変換します。
            /// </summary>
            /// <typeparam name="TStruct"><see cref="Nullable"/> の基となる構造体型。</typeparam>
            /// <param name="value">変換対象の文字列。</param>
            /// <param name="provider">カルチャ固有の書式情報。</param>
            /// <param name="result">変換に成功すれば変換後の値、それ以外なら <c>null</c> が返されます。</param>
            /// <returns>変換に成功すれば <c>true</c>、それ以外なら <c>false</c>。</returns>
            private static bool TryParseToNullable<TStruct>(string value, IFormatProvider provider, out TStruct? result) where TStruct : struct {
                if (value == null) {
                    result = null;
                    return true;
                }

                if (GenericTryParsers<TStruct>.Value(value, provider, out var tmp)) {
                    result = tmp;
                    return true;
                }
                else {
                    result = null;
                    return false;
                }
            }

            private static bool TryParse(string value, IFormatProvider provider, out sbyte result) {
                if (TryParseToInteger(value, provider, out var tmp)) {
                    result = (sbyte)tmp;
                    if (result == tmp) return true;
                }
                result = 0;
                return false;
            }

            private static bool TryParse(string value, IFormatProvider provider, out byte result) {
                if (TryParseToUInteger(value, provider, out var tmp)) {
                    result = (byte)tmp;
                    if (result == tmp) return true;
                }
                result = 0;
                return false;
            }

            private static bool TryParse(string value, IFormatProvider provider, out short result) {
                if (TryParseToInteger(value, provider, out var tmp)) {
                    result = (short)tmp;
                    if (result == tmp) return true;
                }
                result = 0;
                return false;
            }

            private static bool TryParse(string value, IFormatProvider provider, out ushort result) {
                if (TryParseToUInteger(value, provider, out var tmp)) {
                    result = (ushort)tmp;
                    if (result == tmp) return true;
                }
                result = 0;
                return false;
            }

            private static bool TryParse(string value, IFormatProvider provider, out int result) {
                if (TryParseToInteger(value, provider, out var tmp)) {
                    result = (int)tmp;
                    if (result == tmp) return true;
                }
                result = 0;
                return false;
            }

            private static bool TryParse(string value, IFormatProvider provider, out uint result) {
                if (TryParseToUInteger(value, provider, out var tmp)) {
                    result = (uint)tmp;
                    if (result == tmp) return true;
                }
                result = 0;
                return false;
            }

            private static bool TryParse(string value, IFormatProvider provider, out long result) {
                return TryParseToInteger(value, provider, out result);
            }

            private static bool TryParse(string value, IFormatProvider provider, out ulong result) {
                return TryParseToUInteger(value, provider, out result);
            }

            private static bool TryParseToInteger(string value, IFormatProvider provider, out long result) {
                if (string.IsNullOrEmpty(value)) {
                    result = 0;
                    return false;
                }

                value = value.Trim();
                var offset = 0;

                var sign = 1;
                switch (value[offset]) {
                    case InvariantNumberFormat.PositiveSign:
                        sign = 1;
                        offset++;
                        break;

                    case InvariantNumberFormat.NegativeSign:
                        sign = -1;
                        offset++;
                        break;
                }

                var digit = 0L;
                for (; offset < value.Length; offset++) {
                    var ch = value[offset];
                    if (ch == InvariantNumberFormat.NumberGroupSeparator) continue;
                    if (ch < '0' || '9' < ch) {
                        result = 0;
                        return false;
                    }

                    var prevDigit = digit;
                    digit = digit * 10 + sign * (ch - '0');
                    if (sign > 0) {
                        if (digit < prevDigit) {
                            result = 0;
                            return false;
                        }
                    }
                    else {
                        if (digit > prevDigit) {
                            result = 0;
                            return false;
                        }
                    }
                }

                result = digit;
                return true;
            }

            private static bool TryParseToUInteger(string value, IFormatProvider provider, out ulong result) {
                if (string.IsNullOrEmpty(value)) {
                    result = 0;
                    return false;
                }

                value = value.Trim();
                var offset = 0;

                switch (value[offset]) {
                    case InvariantNumberFormat.PositiveSign:
                        offset++;
                        break;
                }

                var digit = 0UL;
                for (; offset < value.Length; offset++) {
                    var ch = value[offset];
                    if (ch == InvariantNumberFormat.NumberGroupSeparator) continue;
                    if (ch < '0' || '9' < ch) {
                        result = 0;
                        return false;
                    }

                    var prevDigit = digit;
                    digit = digit * 10 + (ulong)(ch - '0');
                    if (digit < prevDigit) {
                        result = 0;
                        return false;
                    }
                }

                result = digit;
                return true;
            }

            private static bool TryParse(string value, IFormatProvider provider, out float result) {
                if (TryParseToFloat(value, provider, out var tmp)) {
                    result = (float)tmp;
                    if (result == tmp) return true;
                }
                result = 0;
                return false;
            }

            private static bool TryParse(string value, IFormatProvider provider, out double result) {
                return TryParseToFloat(value, provider, out result);
            }

            private static bool TryParseToFloat(string value, IFormatProvider provider, out double result) {
                throw new NotImplementedException();
            }

            private static bool TryParse(string value, IFormatProvider provider, out decimal result) {
                return TryParseToDecimal(value, provider, out result);
            }

            private static bool TryParseToDecimal(string value, IFormatProvider provider, out decimal result) {
                throw new NotImplementedException();
            }

            /// <summary>
            /// Invariant な数値フォーマット情報。
            /// </summary>
            private static class InvariantNumberFormat {
                public const char NumberGroupSeparator = ',';
                public const char NumberDecimalSeparator = '.';
                public const char NegativeSign = '-';
                public const char PositiveSign = '+';
            }
        }

        /// <summary>
        /// <see cref="Type"/> によって変換デリゲートコレクションを管理するクラス。
        /// </summary>
        private static class NonGenericTryParsers {

            private static readonly IReadOnlyDictionary<Type, Lazy<TryParseDelegate<object>>> _values = new[]{
                typeof(byte),
                typeof(byte?),
                typeof(sbyte),
                typeof(sbyte?),
                typeof(short),
                typeof(short?),
                typeof(ushort),
                typeof(ushort?),
                typeof(int),
                typeof(int?),
                typeof(uint),
                typeof(uint?),
                typeof(long),
                typeof(long?),
                typeof(ulong),
                typeof(ulong?),
                typeof(float),
                typeof(float?),
                typeof(double),
                typeof(double?),
                typeof(decimal),
                typeof(decimal?),
                typeof(bool),
                typeof(bool?),
                typeof(char),
                typeof(char?),
                //typeof(DateTime),
                //typeof(DateTime?),
                //typeof(TimeSpan),
                //typeof(TimeSpan?),
                //typeof(Guid),
                //typeof(Guid?),
                typeof(string),
                //typeof(Version),
                //typeof(Uri),
            }.ToDictionary(
                  type => type
                , type => new Lazy<TryParseDelegate<object>>(() => Default.MakeNonGenericTryParse(type))
            );

            /// <summary>
            /// 文字列を <paramref name="conversionType"/> に変換するデリゲートを返します。
            /// </summary>
            /// <param name="conversionType">変換後の型。</param>
            /// <returns><paramref name="conversionType"/> がサポートされていれば変換デリゲートが返され、それ以外の場合は <c>null</c> が返ります。</returns>
            public static TryParseDelegate<object> GetValue(Type conversionType) {
                return _values.TryGetValue(conversionType, out var valueLazy) ? valueLazy.Value : null;
            }
        }
    }
}