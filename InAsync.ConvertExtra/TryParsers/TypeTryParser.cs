using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace InAsync.ConvertExtras.TryParsers {

    public abstract class TypeTryParser : ITryParser {

        public bool? TryParse<T>(string input, IFormatProvider provider, out T result) {
            var tryParse = GetTryParseDelegate<T>();
            if (tryParse == null) {
                result = default(T);
                return null;
            }

            return tryParse(input, provider, out result);
        }

        public bool? TryParse(Type conversionType, string input, IFormatProvider provider, out object result) {
            var tryParse = GetTryParseDelegate(conversionType);
            if (tryParse == null) {
                result = null;
                return null;
            }

            return tryParse(input, provider, out result);
        }

        protected abstract TryParseDelegate<T> GetTryParseDelegate<T>();

        protected abstract TryParseDelegate<object> GetTryParseDelegate(Type conversionType);

        protected delegate bool TryParseDelegate<TResult>(string input, IFormatProvider provider, out TResult result);

        protected abstract class NonGenericTryParsersBase {
            private readonly IReadOnlyDictionary<Type, Lazy<TryParseDelegate<object>>> _values;

            public NonGenericTryParsersBase(TypeTryParser tryParser) {
                _values = SupportedTypes.ToDictionary(t => t, t => new Lazy<TryParseDelegate<object>>(() => MakeNonGenericTryParse(tryParser, t)));
            }

            protected abstract IReadOnlyList<Type> SupportedTypes { get; }

            public TryParseDelegate<object> GetValue(Type conversionType) {
                return _values.TryGetValue(conversionType, out var valueLazy) ? valueLazy.Value : null;
            }

            /// <summary>
            /// <code>
            /// bool TryParseDelegate<object>(string input, IFormatProvider provider, ref object result) {
            ///     bool retVal = (Default.TryParse<ConversionType>(input, provider, out ConversionType tmp).GetValueOrDefault(true));
            ///     if (retVal) {
            ///         result = (object)tmp;
            ///     }
            ///     else {
            ///         result = null;
            ///     }
            ///     return retVal;
            /// }
            /// </code>
            /// </summary>
            /// <param name="conversionType"></param>
            /// <returns></returns>
            private static TryParseDelegate<object> MakeNonGenericTryParse(ITryParser tryParser, Type conversionType) {
                var inputParam = Expression.Parameter(typeof(string), "input");
                var providerParam = Expression.Parameter(typeof(IFormatProvider), "provider");
                var resultParam = Expression.Parameter(typeof(object).MakeByRefType(), "result");

                var tmpVar = Expression.Variable(conversionType, "tmp");
                var retVar = Expression.Variable(typeof(bool), "retVal");
                var tryParseCall = Expression.Call(Expression.Constant(tryParser), "TryParse", new[] { conversionType }, inputParam, providerParam, tmpVar);
                var bodyExpr = Expression.Block(
                      typeof(bool)
                    , new[] { tmpVar, retVar }
                    , Expression.Assign(retVar, Expression.Call(tryParseCall, "GetValueOrDefault", null, Expression.Constant(true)))
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
}