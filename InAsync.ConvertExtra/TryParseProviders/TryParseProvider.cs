using System;
using System.Linq.Expressions;

namespace InAsync.ConvertExtras.TryParseProviders {

    /// <summary>
    /// 文字列を任意の型へ変換するデリゲートのプロバイダー。
    /// </summary>
    public abstract class TryParseProvider : ITryParseProvider {

        /// <summary>
        /// 文字列を <typeparamref name="T"/> に変換する為のデリゲートを返します。
        /// </summary>
        /// <typeparam name="T">変換後の型。</typeparam>
        /// <returns><typeparamref name="T"/> への変換がサポートされていれば <see cref="TryParseDelegate{T}"/> デリゲートを、それ以外なら <c>null</c> を返します。</returns>
        public abstract TryParseDelegate<T> GetDelegate<T>();

        /// <summary>
        /// 文字列を <paramref name="conversionType"/> に変換する為のデリゲートを返します。
        /// </summary>
        /// <param name="conversionType">変換後の型。</param>
        /// <returns><paramref name="conversionType"/> への変換がサポートされていれば <see cref="TryParseDelegate{object}"/> デリゲートを、それ以外なら <c>null</c> を返します。</returns>
        public abstract TryParseDelegate<object> GetDelegate(Type conversionType);

        /// <summary>
        /// <paramref name="conversionType"/> を使用して、<see cref="ITryParser.TryParse{T}(string, IFormatProvider, out T)"/>
        /// メソッドの非ジェネリックデリゲートを生成します。
        /// </summary>
        /// <remarks>
        /// 返されるデリゲートの擬似コードは下記のようになります。
        /// <code>
        /// bool TryParseDelegate&lt;object&gt;(string input, IFormatProvider provider, ref object result) {
        ///     bool retVal = (GetDelegate&lt;ConversionType&gt;()(input, provider, out ConversionType tmp).GetValueOrDefault(true));
        ///     if (retVal) {
        ///         result = (object)tmp;
        ///     }
        ///     else {
        ///         result = null;
        ///     }
        ///     return retVal;
        /// }
        /// </code>
        /// </remarks>
        /// <param name="tryParser">非ジェネリック化したい <see cref="ITryParser.TryParse{T}(string, IFormatProvider, out T)"/>
        /// メソッドが定義されている型のインスタンス。</param>
        /// <param name="conversionType">変換後の型。</param>
        /// <returns>非ジェネリック化された <see cref="ITryParser.TryParse{T}(string, IFormatProvider, out T)"/> メソッドのデリゲート。</returns>
        protected TryParseDelegate<object> MakeNonGenericTryParse(Type conversionType) {
            if (conversionType == null) throw new ArgumentNullException(nameof(conversionType));

            var inputParam = Expression.Parameter(typeof(string), "input");
            var providerParam = Expression.Parameter(typeof(IFormatProvider), "provider");
            var resultParam = Expression.Parameter(typeof(object).MakeByRefType(), "result");

            var tmpVar = Expression.Variable(conversionType, "tmp");
            var retVar = Expression.Variable(typeof(bool), "retVal");
            var getDelegateCall = Expression.Call(Expression.Constant(this), nameof(GetDelegate), new[] { conversionType });
            var tryParseCall = Expression.Invoke(getDelegateCall, inputParam, providerParam, tmpVar);
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