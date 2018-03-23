using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq.Expressions;

namespace InAsync.ConvertExtras.TryParsers {

    public class NativeTryParser : ITryParser {
        public static readonly NativeTryParser Default = new NativeTryParser();

        public bool? TryParse<T>(string input, IFormatProvider provider, out T result) {
            var tryParse = TryParseCache<T>.cache;
            if (tryParse == null) {
                result = default(T);
                return null;
            }

            return tryParse(input, provider, out result);
        }

        public bool? TryParse(Type conversionType, string input, IFormatProvider provider, out object result) {
            if (s_TryParseCaches.TryGetValue(conversionType, out var tryParseLazy) == false) {
                result = null;
                return null;
            }

            return tryParseLazy.Value(input, provider, out result);
        }

        private delegate bool TryParseDelegate<TResult>(string input, IFormatProvider provider, out TResult result);

        private static class TryParseCache<TResult> {
            public static TryParseDelegate<TResult> cache;

            static TryParseCache() {
                TryParseCache<byte>.cache = (string value, IFormatProvider provider, out byte result) => byte.TryParse(value, NumberStyles.Integer, provider, out result);
                TryParseCache<byte?>.cache = (string value, IFormatProvider provider, out byte? result) => {
                    if (value == null) {
                        result = null;
                        return true;
                    }

                    if (TryParseCache<byte>.cache(value, provider, out var tmp)) {
                        result = tmp;
                        return true;
                    }
                    else {
                        result = null;
                        return false;
                    }
                };
                TryParseCache<sbyte>.cache = (string value, IFormatProvider provider, out sbyte result) => sbyte.TryParse(value, NumberStyles.Integer, provider, out result);
                TryParseCache<sbyte?>.cache = (string value, IFormatProvider provider, out sbyte? result) => {
                    if (value == null) {
                        result = null;
                        return true;
                    }

                    if (TryParseCache<sbyte>.cache(value, provider, out var tmp)) {
                        result = tmp;
                        return true;
                    }
                    else {
                        result = null;
                        return false;
                    }
                };
                TryParseCache<short>.cache = (string value, IFormatProvider provider, out short result) => short.TryParse(value, NumberStyles.Integer | NumberStyles.AllowThousands, provider, out result);
                TryParseCache<short?>.cache = (string value, IFormatProvider provider, out short? result) => {
                    if (value == null) {
                        result = null;
                        return true;
                    }

                    if (TryParseCache<short>.cache(value, provider, out var tmp)) {
                        result = tmp;
                        return true;
                    }
                    else {
                        result = null;
                        return false;
                    }
                };
                TryParseCache<ushort>.cache = (string value, IFormatProvider provider, out ushort result) => ushort.TryParse(value, NumberStyles.Integer | NumberStyles.AllowThousands, provider, out result);
                TryParseCache<ushort?>.cache = (string value, IFormatProvider provider, out ushort? result) => {
                    if (value == null) {
                        result = null;
                        return true;
                    }

                    if (TryParseCache<ushort>.cache(value, provider, out var tmp)) {
                        result = tmp;
                        return true;
                    }
                    else {
                        result = null;
                        return false;
                    }
                };
                TryParseCache<int>.cache = (string value, IFormatProvider provider, out int result) => int.TryParse(value, NumberStyles.Integer | NumberStyles.AllowThousands, provider, out result);
                TryParseCache<int?>.cache = (string value, IFormatProvider provider, out int? result) => {
                    if (value == null) {
                        result = null;
                        return true;
                    }

                    if (TryParseCache<int>.cache(value, provider, out var tmp)) {
                        result = tmp;
                        return true;
                    }
                    else {
                        result = null;
                        return false;
                    }
                };
                TryParseCache<uint>.cache = (string value, IFormatProvider provider, out uint result) => uint.TryParse(value, NumberStyles.Integer | NumberStyles.AllowThousands, provider, out result);
                TryParseCache<uint?>.cache = (string value, IFormatProvider provider, out uint? result) => {
                    if (value == null) {
                        result = null;
                        return true;
                    }

                    if (TryParseCache<uint>.cache(value, provider, out var tmp)) {
                        result = tmp;
                        return true;
                    }
                    else {
                        result = null;
                        return false;
                    }
                };
                TryParseCache<long>.cache = (string value, IFormatProvider provider, out long result) => long.TryParse(value, NumberStyles.Integer | NumberStyles.AllowThousands, provider, out result);
                TryParseCache<long?>.cache = (string value, IFormatProvider provider, out long? result) => {
                    if (value == null) {
                        result = null;
                        return true;
                    }

                    if (TryParseCache<long>.cache(value, provider, out var tmp)) {
                        result = tmp;
                        return true;
                    }
                    else {
                        result = null;
                        return false;
                    }
                };
                TryParseCache<ulong>.cache = (string value, IFormatProvider provider, out ulong result) => ulong.TryParse(value, NumberStyles.Integer | NumberStyles.AllowThousands, provider, out result);
                TryParseCache<ulong?>.cache = (string value, IFormatProvider provider, out ulong? result) => {
                    if (value == null) {
                        result = null;
                        return true;
                    }

                    if (TryParseCache<ulong>.cache(value, provider, out var tmp)) {
                        result = tmp;
                        return true;
                    }
                    else {
                        result = null;
                        return false;
                    }
                };
                TryParseCache<float>.cache = (string value, IFormatProvider provider, out float result) => float.TryParse(value, NumberStyles.Float | NumberStyles.AllowThousands, provider, out result);
                TryParseCache<float?>.cache = (string value, IFormatProvider provider, out float? result) => {
                    if (value == null) {
                        result = null;
                        return true;
                    }

                    if (TryParseCache<float>.cache(value, provider, out var tmp)) {
                        result = tmp;
                        return true;
                    }
                    else {
                        result = null;
                        return false;
                    }
                };
                TryParseCache<double>.cache = (string value, IFormatProvider provider, out double result) => double.TryParse(value, NumberStyles.Float | NumberStyles.AllowThousands, provider, out result);
                TryParseCache<double?>.cache = (string value, IFormatProvider provider, out double? result) => {
                    if (value == null) {
                        result = null;
                        return true;
                    }

                    if (TryParseCache<double>.cache(value, provider, out var tmp)) {
                        result = tmp;
                        return true;
                    }
                    else {
                        result = null;
                        return false;
                    }
                };
                TryParseCache<decimal>.cache = (string value, IFormatProvider provider, out decimal result) => decimal.TryParse(value, NumberStyles.Number, provider, out result);
                TryParseCache<decimal?>.cache = (string value, IFormatProvider provider, out decimal? result) => {
                    if (value == null) {
                        result = null;
                        return true;
                    }

                    if (TryParseCache<decimal>.cache(value, provider, out var tmp)) {
                        result = tmp;
                        return true;
                    }
                    else {
                        result = null;
                        return false;
                    }
                };
                TryParseCache<bool>.cache = (string value, IFormatProvider provider, out bool result) => bool.TryParse(value, out result);
                TryParseCache<bool?>.cache = (string value, IFormatProvider provider, out bool? result) => {
                    if (value == null) {
                        result = null;
                        return true;
                    }

                    if (TryParseCache<bool>.cache(value, provider, out var tmp)) {
                        result = tmp;
                        return true;
                    }
                    else {
                        result = null;
                        return false;
                    }
                };
                TryParseCache<char>.cache = (string value, IFormatProvider provider, out char result) => char.TryParse(value, out result);
                TryParseCache<char?>.cache = (string value, IFormatProvider provider, out char? result) => {
                    if (value == null) {
                        result = null;
                        return true;
                    }

                    if (TryParseCache<char>.cache(value, provider, out var tmp)) {
                        result = tmp;
                        return true;
                    }
                    else {
                        result = null;
                        return false;
                    }
                };
                TryParseCache<DateTime>.cache = (string value, IFormatProvider provider, out DateTime result) => DateTime.TryParse(value, provider, DateTimeStyles.None, out result);
                TryParseCache<DateTime?>.cache = (string value, IFormatProvider provider, out DateTime? result) => {
                    if (value == null) {
                        result = null;
                        return true;
                    }

                    if (TryParseCache<DateTime>.cache(value, provider, out var tmp)) {
                        result = tmp;
                        return true;
                    }
                    else {
                        result = null;
                        return false;
                    }
                };
                TryParseCache<TimeSpan>.cache = (string value, IFormatProvider provider, out TimeSpan result) => TimeSpan.TryParse(value, provider, out result);
                TryParseCache<TimeSpan?>.cache = (string value, IFormatProvider provider, out TimeSpan? result) => {
                    if (value == null) {
                        result = null;
                        return true;
                    }

                    if (TryParseCache<TimeSpan>.cache(value, provider, out var tmp)) {
                        result = tmp;
                        return true;
                    }
                    else {
                        result = null;
                        return false;
                    }
                };
                TryParseCache<Guid>.cache = (string value, IFormatProvider provider, out Guid result) => Guid.TryParse(value, out result);
                TryParseCache<Guid?>.cache = (string value, IFormatProvider provider, out Guid? result) => {
                    if (value == null) {
                        result = null;
                        return true;
                    }

                    if (TryParseCache<Guid>.cache(value, provider, out var tmp)) {
                        result = tmp;
                        return true;
                    }
                    else {
                        result = null;
                        return false;
                    }
                };
                TryParseCache<string>.cache = (string value, IFormatProvider provider, out string result) => {
                    result = value;
                    return true;
                };
                TryParseCache<Version>.cache = (string value, IFormatProvider provider, out Version result) => Version.TryParse(value, out result);
                TryParseCache<Uri>.cache = (string value, IFormatProvider provider, out Uri result) => Uri.TryCreate(value, UriKind.Absolute, out result);
            }
        }

        private static readonly IReadOnlyDictionary<Type, Lazy<TryParseDelegate<object>>> s_TryParseCaches = new Dictionary<Type, Lazy<TryParseDelegate<object>>> {
            [typeof(byte)] = new Lazy<TryParseDelegate<object>>(() => MakeTryParseDeledate(typeof(byte))),
            [typeof(byte?)] = new Lazy<TryParseDelegate<object>>(() => MakeTryParseDeledate(typeof(byte?))),
            [typeof(sbyte)] = new Lazy<TryParseDelegate<object>>(() => MakeTryParseDeledate(typeof(sbyte))),
            [typeof(sbyte?)] = new Lazy<TryParseDelegate<object>>(() => MakeTryParseDeledate(typeof(sbyte?))),
            [typeof(short)] = new Lazy<TryParseDelegate<object>>(() => MakeTryParseDeledate(typeof(short))),
            [typeof(short?)] = new Lazy<TryParseDelegate<object>>(() => MakeTryParseDeledate(typeof(short?))),
            [typeof(ushort)] = new Lazy<TryParseDelegate<object>>(() => MakeTryParseDeledate(typeof(ushort))),
            [typeof(ushort?)] = new Lazy<TryParseDelegate<object>>(() => MakeTryParseDeledate(typeof(ushort?))),
            [typeof(int)] = new Lazy<TryParseDelegate<object>>(() => MakeTryParseDeledate(typeof(int))),
            [typeof(int?)] = new Lazy<TryParseDelegate<object>>(() => MakeTryParseDeledate(typeof(int?))),
            [typeof(uint)] = new Lazy<TryParseDelegate<object>>(() => MakeTryParseDeledate(typeof(uint))),
            [typeof(uint?)] = new Lazy<TryParseDelegate<object>>(() => MakeTryParseDeledate(typeof(uint?))),
            [typeof(long)] = new Lazy<TryParseDelegate<object>>(() => MakeTryParseDeledate(typeof(long))),
            [typeof(long?)] = new Lazy<TryParseDelegate<object>>(() => MakeTryParseDeledate(typeof(long?))),
            [typeof(ulong)] = new Lazy<TryParseDelegate<object>>(() => MakeTryParseDeledate(typeof(ulong))),
            [typeof(ulong?)] = new Lazy<TryParseDelegate<object>>(() => MakeTryParseDeledate(typeof(ulong?))),
            [typeof(float)] = new Lazy<TryParseDelegate<object>>(() => MakeTryParseDeledate(typeof(float))),
            [typeof(float?)] = new Lazy<TryParseDelegate<object>>(() => MakeTryParseDeledate(typeof(float?))),
            [typeof(double)] = new Lazy<TryParseDelegate<object>>(() => MakeTryParseDeledate(typeof(double))),
            [typeof(double?)] = new Lazy<TryParseDelegate<object>>(() => MakeTryParseDeledate(typeof(double?))),
            [typeof(decimal)] = new Lazy<TryParseDelegate<object>>(() => MakeTryParseDeledate(typeof(decimal))),
            [typeof(decimal?)] = new Lazy<TryParseDelegate<object>>(() => MakeTryParseDeledate(typeof(decimal?))),
            [typeof(bool)] = new Lazy<TryParseDelegate<object>>(() => MakeTryParseDeledate(typeof(bool))),
            [typeof(bool?)] = new Lazy<TryParseDelegate<object>>(() => MakeTryParseDeledate(typeof(bool?))),
            [typeof(char)] = new Lazy<TryParseDelegate<object>>(() => MakeTryParseDeledate(typeof(char))),
            [typeof(char?)] = new Lazy<TryParseDelegate<object>>(() => MakeTryParseDeledate(typeof(char?))),
            [typeof(DateTime)] = new Lazy<TryParseDelegate<object>>(() => MakeTryParseDeledate(typeof(DateTime))),
            [typeof(DateTime?)] = new Lazy<TryParseDelegate<object>>(() => MakeTryParseDeledate(typeof(DateTime?))),
            [typeof(TimeSpan)] = new Lazy<TryParseDelegate<object>>(() => MakeTryParseDeledate(typeof(TimeSpan))),
            [typeof(TimeSpan?)] = new Lazy<TryParseDelegate<object>>(() => MakeTryParseDeledate(typeof(TimeSpan?))),
            [typeof(Guid)] = new Lazy<TryParseDelegate<object>>(() => MakeTryParseDeledate(typeof(Guid))),
            [typeof(Guid?)] = new Lazy<TryParseDelegate<object>>(() => MakeTryParseDeledate(typeof(Guid?))),
            [typeof(string)] = new Lazy<TryParseDelegate<object>>(() => MakeTryParseDeledate(typeof(string))),
            [typeof(Version)] = new Lazy<TryParseDelegate<object>>(() => MakeTryParseDeledate(typeof(Version))),
            [typeof(Uri)] = new Lazy<TryParseDelegate<object>>(() => MakeTryParseDeledate(typeof(Uri))),
        };

        /// <summary>
        ///
        /// </summary>
        /// <param name="conversionType"></param>
        /// <returns></returns>
        private static TryParseDelegate<object> MakeTryParseDeledate(Type conversionType) {
            var inputParam = Expression.Parameter(typeof(string), "input");
            var providerParam = Expression.Parameter(typeof(IFormatProvider), "provider");
            var resultParam = Expression.Parameter(typeof(object).MakeByRefType(), "result");

            var tmpVar = Expression.Variable(conversionType, "tmp");
            var retVar = Expression.Variable(typeof(bool), "retVal");
            var tryParseCall = Expression.Call(typeof(ConvertExtra), "TryParse", new[] { conversionType }, inputParam, providerParam, tmpVar);
            var bodyExpr = Expression.Block(
                  typeof(bool)
                , new[] { tmpVar, retVar }
                , Expression.Assign(retVar, tryParseCall)
                , Expression.IfThenElse(
                    retVar
                    , Expression.Assign(resultParam, Expression.Convert(tmpVar, typeof(object)))
                    , Expression.Assign(resultParam, Expression.Constant(null))
                )
                , retVar
            );

            var lambdaExpr = Expression.Lambda<TryParseDelegate<object>>(bodyExpr, inputParam, providerParam, resultParam);
            return lambdaExpr.Compile();
        }
    }
}