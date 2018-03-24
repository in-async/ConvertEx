using System;
using System.Collections.Generic;

namespace InAsync.ConvertExtras.TryParsers {

    public class FastTryParser : TypeTryParser {
        public static readonly FastTryParser Default = new FastTryParser();

        protected override TryParseDelegate<T> GetTryParseDelegate<T>() => GenericTryParsers<T>.Value;

        protected override TryParseDelegate<object> GetTryParseDelegate(Type conversionType) => NonGenericTryParsers.GetValue(conversionType);

        private static class GenericTryParsers<TResult> {
            public static readonly TryParseDelegate<TResult> Value;

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
                //GenericTryParser<float>.Value = (string value, IFormatProvider provider, out float result) => float.TryParse(value, NumberStyles.Float | NumberStyles.AllowThousands, provider, out result);
                //GenericTryParser<float?>.Value = TryParseToNullable;
                //GenericTryParser<double>.Value = (string value, IFormatProvider provider, out double result) => double.TryParse(value, NumberStyles.Float | NumberStyles.AllowThousands, provider, out result);
                //GenericTryParser<double?>.Value = TryParseToNullable;
                //GenericTryParser<decimal>.Value = (string value, IFormatProvider provider, out decimal result) => decimal.TryParse(value, NumberStyles.Number, provider, out result);
                //GenericTryParser<decimal?>.Value = TryParseToNullable;
                //GenericTryParser<bool>.Value = (string value, IFormatProvider provider, out bool result) => bool.TryParse(value, out result);
                //GenericTryParser<bool?>.Value = (TryParseToNullable;
                //GenericTryParser<char>.Value = (string value, IFormatProvider provider, out char result) => char.TryParse(value, out result);
                //GenericTryParser<char?>.Value = TryParseToNullable;
                //GenericTryParser<DateTime>.Value = (string value, IFormatProvider provider, out DateTime result) => DateTime.TryParse(value, provider, DateTimeStyles.None, out result);
                //GenericTryParser<DateTime?>.Value = TryParseToNullable;
                //GenericTryParser<TimeSpan>.Value = (string value, IFormatProvider provider, out TimeSpan result) => TimeSpan.TryParse(value, provider, out result);
                //GenericTryParser<TimeSpan?>.Value = TryParseToNullable;
                //GenericTryParser<Guid>.Value = (string value, IFormatProvider provider, out Guid result) => Guid.TryParse(value, out result);
                //GenericTryParser<Guid?>.Value = TryParseToNullable;
                //GenericTryParser<string>.Value = (string value, IFormatProvider provider, out string result) => {
                //    result = value;
                //    return true;
                //};
                //GenericTryParser<Version>.Value = (string value, IFormatProvider provider, out Version result) => Version.TryParse(value, out result);
                //GenericTryParser<Uri>.Value = (string value, IFormatProvider provider, out Uri result) => Uri.TryCreate(value, UriKind.Absolute, out result);
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

            private static bool TryParse(string value, IFormatProvider provider, out sbyte result) {
                var success = TryParseToInteger(value, provider, out var tmp);
                result = (sbyte)tmp;
                return success && (result == tmp);
            }

            private static bool TryParse(string value, IFormatProvider provider, out byte result) {
                var success = TryParseToUInteger(value, provider, out var tmp);
                result = (byte)tmp;
                return success && (result == tmp);
            }

            private static bool TryParse(string value, IFormatProvider provider, out short result) {
                var success = TryParseToInteger(value, provider, out var tmp);
                result = (short)tmp;
                return success && (result == tmp);
            }

            private static bool TryParse(string value, IFormatProvider provider, out ushort result) {
                var success = TryParseToUInteger(value, provider, out var tmp);
                result = (ushort)tmp;
                return success && (result == tmp);
            }

            private static bool TryParse(string value, IFormatProvider provider, out int result) {
                var success = TryParseToInteger(value, provider, out var tmp);
                result = (int)tmp;
                return success && (result == tmp);
            }

            private static bool TryParse(string value, IFormatProvider provider, out uint result) {
                var success = TryParseToUInteger(value, provider, out var tmp);
                result = (uint)tmp;
                return success && (result == tmp);
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

            private static class InvariantNumberFormat {
                public const char NumberGroupSeparator = ',';
                public const char NegativeSign = '-';
                public const char PositiveSign = '+';
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
                //typeof(float),
                //typeof(float?),
                //typeof(double),
                //typeof(double?),
                //typeof(decimal),
                //typeof(decimal?),
                //typeof(bool),
                //typeof(bool?),
                //typeof(char),
                //typeof(char?),
                //typeof(DateTime),
                //typeof(DateTime?),
                //typeof(TimeSpan),
                //typeof(TimeSpan?),
                //typeof(Guid),
                //typeof(Guid?),
                //typeof(string),
                //typeof(Version),
                //typeof(Uri),
            };
        }
    }
}