using System;
using System.Linq.Expressions;

namespace InAsync.ConvertExtras.TryParseProviders {

    /// <summary>
    /// 文字列から任意の型への変換デリゲートを提供する <see cref="ITryParseProvider"/> の基底クラス。
    /// </summary>
    public abstract class TryParseProvider : ITryParseProvider {

        /// <summary>
        /// 文字列から <typeparamref name="T"/> に変換するデリゲートを返します。
        /// </summary>
        /// <typeparam name="T">変換後の型。</typeparam>
        /// <param name="provider">カルチャ固有の書式情報。</param>
        /// <returns><typeparamref name="T"/> への変換がサポートされていれば <see cref="TryParseDelegate{T}"/> デリゲートを、それ以外なら <c>null</c> を返します。</returns>
        public abstract TryParseDelegate<T> GetDelegate<T>(IFormatProvider provider);

        /// <summary>
        /// 文字列から <paramref name="conversionType"/> に変換するデリゲートを返します。
        /// </summary>
        /// <param name="conversionType">変換後の型。</param>
        /// <param name="provider">カルチャ固有の書式情報。</param>
        /// <returns><paramref name="conversionType"/> への変換がサポートされていれば <see cref="TryParseDelegate{object}"/> デリゲートを、それ以外なら <c>null</c> を返します。</returns>
        public abstract TryParseDelegate<object> GetDelegate(Type conversionType, IFormatProvider provider);

        /// <summary>
        /// <paramref name="conversionType"/> を使用して <see cref="ITryParseProvider.GetDelegate{T}"/> からデリゲートを取得し実行する為の、
        /// 非ジェネリックデリゲートを生成します。
        /// </summary>
        /// <remarks>
        /// 返されるデリゲートの擬似コードは下記になります。
        /// <code>
        /// (string input, IFormatProvider provider, out object result) => {
        ///     var success = GetDelegate&lt;conversionType&gt;()(input, provider, out var tmp);
        ///     if (success) {
        ///         result = (object)tmp;
        ///     }
        ///     else {
        ///         result = null;
        ///     }
        ///     return success;
        /// }
        /// </code>
        /// </remarks>
        /// <param name="conversionType">変換後の型。</param>
        /// <returns>文字列から <paramref name="conversionType"/> へ変換する非ジェネリックデリゲート。</returns>
        protected TryParseDelegate<object> MakeNonGenericTryParse(Type conversionType) {
            if (conversionType == null) throw new ArgumentNullException(nameof(conversionType));

            var inputParam = Expression.Parameter(typeof(string), "input");
            var providerParam = Expression.Parameter(typeof(IFormatProvider), "provider");
            var resultParam = Expression.Parameter(typeof(object).MakeByRefType(), "result");

            var tmpVar = Expression.Variable(conversionType, "tmp");
            var successVar = Expression.Variable(typeof(bool), "success");
            var getDelegateCall = Expression.Call(Expression.Constant(this), nameof(GetDelegate), new[] { conversionType }, providerParam);
            var tryParseCall = Expression.Invoke(getDelegateCall, inputParam, providerParam, tmpVar);
            var bodyExpr = Expression.Block(
                  typeof(bool)
                , new[] { tmpVar, successVar }
                , Expression.Assign(successVar, tryParseCall)
                , Expression.IfThenElse(
                      successVar
                    , Expression.Assign(resultParam, Expression.Convert(tmpVar, typeof(object)))
                    , Expression.Assign(resultParam, Expression.Constant(null))
                )
                , successVar
            );

            var lambdaExpr = Expression.Lambda<TryParseDelegate<object>>(bodyExpr, inputParam, providerParam, resultParam);
            return lambdaExpr.Compile();
        }
    }
}