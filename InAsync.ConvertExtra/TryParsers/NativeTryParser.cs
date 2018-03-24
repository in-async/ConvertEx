using System;
using System.Collections.Generic;
using System.Globalization;

namespace InAsync.ConvertExtras.TryParsers {

    public class NativeTryParser : TypeTryParser {
        public static readonly NativeTryParser Default = new NativeTryParser();

        protected override TryParseDelegate<T> GetTryParseDelegate<T>() => GenericTryParsers<T>.Value;

        protected override TryParseDelegate<object> GetTryParseDelegate(Type conversionType) => NonGenericTryParsers.GetValue(conversionType);

        private static class GenericTryParsers<TResult> {
            public static readonly TryParseDelegate<TResult> Value;

            static GenericTryParsers() {
                GenericTryParsers<byte>.Value = (string value, IFormatProvider provider, out byte result) => byte.TryParse(value, NumberStyles.Integer, provider, out result);
                GenericTryParsers<byte?>.Value = TryParseToNullable;
                GenericTryParsers<sbyte>.Value = (string value, IFormatProvider provider, out sbyte result) => sbyte.TryParse(value, NumberStyles.Integer, provider, out result);
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

        private static readonly NonGenericTryParsersImpl NonGenericTryParsers = new NonGenericTryParsersImpl(Default);
        private class NonGenericTryParsersImpl : NonGenericTryParsersBase {
            public NonGenericTryParsersImpl(TypeTryParser tryParser) : base(tryParser) {
            }

            protected override IReadOnlyList<Type> SupportedTypes { get; } = new[]{
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
            };
        }
    }
}