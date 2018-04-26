using System;
using System.Collections.Generic;
using System.Globalization;
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
    /// - <c>Single</c>
    /// - <c>Double</c>
    /// - <c>Decimal</c>
    /// - <c>Boolean</c>
    /// - <c>Char</c>
    /// - <c>DateTime</c>
    /// - <c>TimeSpan</c>
    /// - <c>Guid</c>
    /// - 上記構造体の <c>Nullable</c> 型
    /// - <c>String</c>
    /// - <c>Version</c>
    /// - <c>Uri</c>
    /// </remarks>
    public class NativeTryParseProvider : TryParseProvider {
        public static readonly NativeTryParseProvider Default = new NativeTryParseProvider();

        public override TryParseDelegate<T> GetDelegate<T>() => GenericTryParsers<T>.Value;

        public override TryParseDelegate<object> GetDelegate(Type conversionType) => NonGenericTryParsers.GetValue(conversionType);

        /// <summary>
        /// 型パラメーターによって変換デリゲートコレクションを管理するクラス。
        /// </summary>
        /// <typeparam name="T">変換後の型。</typeparam>
        private static class GenericTryParsers<T> {
            public static readonly TryParseDelegate<T> Value;

            static GenericTryParsers() {
                GenericTryParsers<byte>.Value = (string value, IFormatProvider provider, out byte result) => byte.TryParse(value, NumberStyles.Integer | NumberStyles.AllowThousands, provider, out result);
                GenericTryParsers<byte?>.Value = TryParseToNullable;
                GenericTryParsers<sbyte>.Value = (string value, IFormatProvider provider, out sbyte result) => sbyte.TryParse(value, NumberStyles.Integer | NumberStyles.AllowThousands, provider, out result);
                GenericTryParsers<sbyte?>.Value = TryParseToNullable;
                GenericTryParsers<short>.Value = (string value, IFormatProvider provider, out short result) => short.TryParse(value, NumberStyles.Integer | NumberStyles.AllowThousands, provider, out result);
                GenericTryParsers<short?>.Value = TryParseToNullable;
                GenericTryParsers<ushort>.Value = (string value, IFormatProvider provider, out ushort result) => ushort.TryParse(value, NumberStyles.Integer | NumberStyles.AllowThousands, provider, out result);
                GenericTryParsers<ushort?>.Value = TryParseToNullable;
                GenericTryParsers<int>.Value = (string value, IFormatProvider provider, out int result) => int.TryParse(value, NumberStyles.Integer | NumberStyles.AllowThousands, provider, out result);
                GenericTryParsers<int?>.Value = TryParseToNullable;
                GenericTryParsers<uint>.Value = (string value, IFormatProvider provider, out uint result) => uint.TryParse(value, NumberStyles.Integer | NumberStyles.AllowThousands, provider, out result);
                GenericTryParsers<uint?>.Value = TryParseToNullable;
                GenericTryParsers<long>.Value = (string value, IFormatProvider provider, out long result) => long.TryParse(value, NumberStyles.Integer | NumberStyles.AllowThousands, provider, out result);
                GenericTryParsers<long?>.Value = TryParseToNullable;
                GenericTryParsers<ulong>.Value = (string value, IFormatProvider provider, out ulong result) => ulong.TryParse(value, NumberStyles.Integer | NumberStyles.AllowThousands, provider, out result);
                GenericTryParsers<ulong?>.Value = TryParseToNullable;
                GenericTryParsers<float>.Value = (string value, IFormatProvider provider, out float result) => float.TryParse(value, NumberStyles.Float | NumberStyles.AllowThousands, provider, out result);
                GenericTryParsers<float?>.Value = TryParseToNullable;
                GenericTryParsers<double>.Value = (string value, IFormatProvider provider, out double result) => double.TryParse(value, NumberStyles.Float | NumberStyles.AllowThousands, provider, out result);
                GenericTryParsers<double?>.Value = TryParseToNullable;
                GenericTryParsers<decimal>.Value = (string value, IFormatProvider provider, out decimal result) => decimal.TryParse(value, NumberStyles.Number, provider, out result);
                GenericTryParsers<decimal?>.Value = TryParseToNullable;
                GenericTryParsers<bool>.Value = (string value, IFormatProvider provider, out bool result) => bool.TryParse(value, out result);
                GenericTryParsers<bool?>.Value = TryParseToNullable;
                GenericTryParsers<char>.Value = (string value, IFormatProvider provider, out char result) => char.TryParse(value, out result);
                GenericTryParsers<char?>.Value = TryParseToNullable;
                GenericTryParsers<DateTime>.Value = (string value, IFormatProvider provider, out DateTime result) => DateTime.TryParse(value, provider, DateTimeStyles.None, out result);
                GenericTryParsers<DateTime?>.Value = TryParseToNullable;
                GenericTryParsers<TimeSpan>.Value = (string value, IFormatProvider provider, out TimeSpan result) => TimeSpan.TryParse(value, provider, out result);
                GenericTryParsers<TimeSpan?>.Value = TryParseToNullable;
                GenericTryParsers<Guid>.Value = (string value, IFormatProvider provider, out Guid result) => Guid.TryParse(value, out result);
                GenericTryParsers<Guid?>.Value = TryParseToNullable;
                GenericTryParsers<string>.Value = (string value, IFormatProvider provider, out string result) => {
                    result = value;
                    return true;
                };
                GenericTryParsers<Version>.Value = (string value, IFormatProvider provider, out Version result) => Version.TryParse(value, out result);
                GenericTryParsers<Uri>.Value = (string value, IFormatProvider provider, out Uri result) => Uri.TryCreate(value, UriKind.Absolute, out result);
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
                typeof(DateTime),
                typeof(DateTime?),
                typeof(TimeSpan),
                typeof(TimeSpan?),
                typeof(Guid),
                typeof(Guid?),
                typeof(string),
                typeof(Version),
                typeof(Uri),
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