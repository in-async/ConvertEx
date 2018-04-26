using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace InAsync.ConvertExtras.TryParseProviders {

    /// <summary>
    /// カルチャに依存しない（インバリアントな）情報に基づき、文字列から特定の型への変換デリゲートを提供する <see cref="ITryParseProvider"/> クラス。
    /// </summary>
    /// <remarks>
    /// 変換先としてサポートされている型は以下の通りです。
    /// - <c>Byte</c> / <c>SByte</c>
    /// - <c>Int16</c> / <c>UInt16</c>
    /// - <c>Int32</c> / <c>UInt32</c>
    /// - <c>Int64</c> / <c>UInt64</c>
    /// - <c>Single</c>
    /// - <c>Double</c>
    /// - <c>Boolean</c>
    /// - <c>Char</c>
    /// - 上記構造体の <c>Nullable</c> 型
    /// - <c>String</c>
    /// </remarks>
    public class InvariantFastTryParseProvider : TryParseProvider {
        public static readonly InvariantFastTryParseProvider Default = new InvariantFastTryParseProvider();

        public override TryParseDelegate<T> GetDelegate<T>(IFormatProvider provider) {
            if (InvariantNumberFormat.IsInvariant(NumberFormatInfo.GetInstance(provider)) == false) return null;

            return GenericTryParsers<T>.Value;
        }

        public override TryParseDelegate<object> GetDelegate(Type conversionType, IFormatProvider provider) {
            if (InvariantNumberFormat.IsInvariant(NumberFormatInfo.GetInstance(provider)) == false) return null;

            return NonGenericTryParsers.GetValue(conversionType);
        }

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
                //GenericTryParsers<decimal>.Value = TryParse;
                //GenericTryParsers<decimal?>.Value = TryParseToNullable;
                GenericTryParsers<bool>.Value = TryParse;
                GenericTryParsers<bool?>.Value = TryParseToNullable;
                GenericTryParsers<char>.Value = TryParse;
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
                if (TryParseToInteger(value, sbyte.MinValue, sbyte.MaxValue, out var tmp)) {
                    result = (sbyte)tmp;
                    return true;
                }
                result = 0;
                return false;
            }

            private static bool TryParse(string value, IFormatProvider provider, out byte result) {
                if (TryParseToUInteger(value, byte.MaxValue, out var tmp)) {
                    result = (byte)tmp;
                    return true;
                }
                result = 0;
                return false;
            }

            private static bool TryParse(string value, IFormatProvider provider, out short result) {
                if (TryParseToInteger(value, short.MinValue, short.MaxValue, out var tmp)) {
                    result = (short)tmp;
                    return true;
                }
                result = 0;
                return false;
            }

            private static bool TryParse(string value, IFormatProvider provider, out ushort result) {
                if (TryParseToUInteger(value, ushort.MaxValue, out var tmp)) {
                    result = (ushort)tmp;
                    return true;
                }
                result = 0;
                return false;
            }

            private static bool TryParse(string value, IFormatProvider provider, out int result) {
                if (TryParseToInteger(value, int.MinValue, int.MaxValue, out var tmp)) {
                    result = (int)tmp;
                    return true;
                }
                result = 0;
                return false;
            }

            private static bool TryParse(string value, IFormatProvider provider, out uint result) {
                if (TryParseToUInteger(value, uint.MaxValue, out var tmp)) {
                    result = (uint)tmp;
                    return true;
                }
                result = 0;
                return false;
            }

            private static bool TryParse(string value, IFormatProvider provider, out long result) {
                return TryParseToInteger(value, long.MinValue, long.MaxValue, out result);
            }

            private static bool TryParse(string value, IFormatProvider provider, out ulong result) {
                return TryParseToUInteger(value, ulong.MaxValue, out result);
            }

            private static bool TryParseToInteger(string value, long minValue, long maxValue, out long result, int? startIndex = null) {
                if (string.IsNullOrEmpty(value)) {
                    result = 0;
                    return false;
                }

                var offset = 0;
                if (startIndex == null) {
                    value = value.Trim();
                }
                else if (startIndex < value.Length) {
                    offset = startIndex.Value;
                }
                else {
                    result = 0;
                    return false;
                }

                var sign = 1;
                switch (value[offset]) {
                    case InvariantNumberFormat.NumberGroupSeparator:
                        result = 0;
                        return false;

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
                        if (digit > maxValue || digit < prevDigit) {
                            result = 0;
                            return false;
                        }
                    }
                    else {
                        if (digit < minValue || digit > prevDigit) {
                            result = 0;
                            return false;
                        }
                    }
                }

                result = digit;
                return true;
            }

            private static bool TryParseToUInteger(string value, ulong maxValue, out ulong result) {
                if (string.IsNullOrEmpty(value)) {
                    result = 0;
                    return false;
                }

                value = value.Trim();
                var offset = 0;

                switch (value[offset]) {
                    case InvariantNumberFormat.NumberGroupSeparator:
                        result = 0;
                        return false;

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
                    if (digit > maxValue || digit < prevDigit) {
                        result = 0;
                        return false;
                    }
                }

                result = digit;
                return true;
            }

            private static bool TryParse(string value, IFormatProvider provider, out float result) {
                if (TryParseToFloat(value, float.MinValue, float.MaxValue, out var tmp)) {
                    result = (float)tmp;
                    return true;
                }
                result = 0;
                return false;
            }

            private static bool TryParse(string value, IFormatProvider provider, out double result) {
                return TryParseToFloat(value, double.MinValue, double.MaxValue, out result);
            }

            private static bool TryParseToFloat(string value, double minValue, double maxValue, out double result) {
                if (string.IsNullOrEmpty(value)) {
                    result = 0;
                    return false;
                }

                value = value.Trim();
                var offset = 0;

                switch (value) {
                    case InvariantNumberFormat.NaNSymbol:
                        result = double.NaN;
                        return true;

                    case InvariantNumberFormat.PositiveInfinitySymbol:
                        result = double.PositiveInfinity;
                        return true;

                    case InvariantNumberFormat.NegativeInfinitySymbol:
                        result = double.NegativeInfinity;
                        return true;
                }

                var sign = 1;
                switch (value[offset]) {
                    case InvariantNumberFormat.NumberGroupSeparator:
                        result = 0;
                        return false;

                    case InvariantNumberFormat.PositiveSign:
                        sign = 1;
                        offset++;
                        break;

                    case InvariantNumberFormat.NegativeSign:
                        sign = -1;
                        offset++;
                        break;
                }

                var significand = 0d;
                var decimalSeparatorIndex = -1;
                var scale = 0L;
                for (; offset < value.Length; offset++) {
                    var ch = value[offset];
                    if (ch == InvariantNumberFormat.NumberGroupSeparator) continue;
                    if (ch == InvariantNumberFormat.NumberDecimalSeparator) {
                        if (decimalSeparatorIndex >= 0) {
                            result = 0;
                            return false;
                        }
                        decimalSeparatorIndex = offset;
                        continue;
                    }
                    if (ch == 'e' || ch == 'E') {
                        if (TryParseToInteger(value, short.MinValue, short.MaxValue, out scale, startIndex: offset + 1) == false) {
                            result = 0;
                            return false;
                        }
                        break;
                    }
                    if (ch < '0' || '9' < ch) {
                        result = 0;
                        return false;
                    }

                    var prevSignificand = significand;
                    significand = significand * 10 + sign * (ch - '0');
                    if (sign > 0) {
                        if (significand > maxValue || significand < prevSignificand) {
                            result = 0;
                            return false;
                        }
                    }
                    else {
                        if (significand < minValue || significand > prevSignificand) {
                            result = 0;
                            return false;
                        }
                    }
                }

                result = significand;
                if (decimalSeparatorIndex >= 0) {
                    result /= Math.Pow(10, (offset - decimalSeparatorIndex - 1) - scale);
                }
                return true;
            }

            private static bool TryParse(string value, IFormatProvider provider, out decimal result) {
                return TryParseToDecimal(value, decimal.MinValue, decimal.MaxValue, out result);
            }

            private static bool TryParseToDecimal(string value, decimal minValue, decimal maxValue, out decimal result) {
                if (string.IsNullOrEmpty(value)) {
                    result = 0;
                    return false;
                }

                value = value.Trim();
                var offset = 0;

                var sign = 1;
                switch (value[offset]) {
                    case InvariantNumberFormat.NumberGroupSeparator:
                        result = 0;
                        return false;

                    case InvariantNumberFormat.PositiveSign:
                        sign = 1;
                        offset++;
                        break;

                    case InvariantNumberFormat.NegativeSign:
                        sign = -1;
                        offset++;
                        break;
                }

                var significand = 0m;
                var decimalSeparatorIndex = -1;
                try {
                    for (; offset < value.Length; offset++) {
                        var ch = value[offset];
                        if (ch == InvariantNumberFormat.NumberGroupSeparator) continue;
                        if (ch == InvariantNumberFormat.NumberDecimalSeparator) {
                            if (decimalSeparatorIndex >= 0) {
                                result = 0;
                                return false;
                            }
                            decimalSeparatorIndex = offset;
                            continue;
                        }
                        if (ch < '0' || '9' < ch) {
                            result = 0;
                            return false;
                        }

                        var prevSignificand = significand;
                        significand = significand * 10 + sign * (ch - '0');
                        if (sign > 0) {
                            if (significand > maxValue) {
                                result = 0;
                                return false;
                            }
                        }
                        else {
                            if (significand < minValue) {
                                result = 0;
                                return false;
                            }
                        }
                    }
                }
                catch (OverflowException) {
                    result = 0;
                    return false;
                }

                result = significand;
                if (decimalSeparatorIndex >= 0) {
                    result /= (decimal)Math.Pow(10, offset - decimalSeparatorIndex - 1);
                }
                return true;
            }

            private static bool TryParse(string value, IFormatProvider provider, out bool result) {
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
            }

            private static bool TryParse(string value, IFormatProvider provider, out char result) {
                if (value?.Length == 1) {
                    result = value[0];
                    return true;
                }
                else {
                    result = default(char);
                    return false;
                }
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
                //typeof(decimal),
                //typeof(decimal?),
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

        /// <summary>
        /// Invariant な数値フォーマット情報。
        /// </summary>
        private static class InvariantNumberFormat {
            private static readonly NumberFormatInfo InvariantInfo = NumberFormatInfo.InvariantInfo;

            public const char NumberGroupSeparator = ',';   // InvariantInfo.NumberGroupSeparator[0]
            public const char NumberDecimalSeparator = '.'; // InvariantInfo.NumberDecimalSeparator[0]
            public const char NegativeSign = '-';           // InvariantInfo.NegativeSign[0]
            public const char PositiveSign = '+';           // InvariantInfo.PositiveSign[0]
            public const string NaNSymbol = "NaN";          // InvariantInfo.NaNSymbol
            public const string NegativeInfinitySymbol = "-Infinity";   // InvariantInfo.NegativeInfinitySymbol
            public const string PositiveInfinitySymbol = "Infinity";    // InvariantInfo.PositiveInfinitySymbol

            public static bool IsInvariant(NumberFormatInfo info) {
                if (info == null) return false;
                if (info.NumberGroupSeparator[0] != NumberGroupSeparator) return false;
                if (info.NumberDecimalSeparator[0] != NumberDecimalSeparator) return false;
                if (info.NegativeSign[0] != NegativeSign) return false;
                if (info.PositiveSign[0] != PositiveSign) return false;
                if (info.NaNSymbol != NaNSymbol) return false;
                if (info.NegativeInfinitySymbol != NegativeInfinitySymbol) return false;
                if (info.PositiveInfinitySymbol != PositiveInfinitySymbol) return false;

                return true;
            }
        }
    }
}