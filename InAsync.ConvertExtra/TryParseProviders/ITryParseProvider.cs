using System;

namespace InAsync.ConvertExtras.TryParseProviders {

    /// <summary>
    /// 文字列から任意の型への変換デリゲートを提供するプロバイダー。
    /// </summary>
    public interface ITryParseProvider {

        /// <summary>
        /// 文字列から <typeparamref name="T"/> に変換するデリゲートを返します。
        /// </summary>
        /// <typeparam name="T">変換後の型。</typeparam>
        /// <returns><typeparamref name="T"/> への変換がサポートされていれば <see cref="TryParseDelegate{T}"/> デリゲートを、それ以外なら <c>null</c> を返します。</returns>
        TryParseDelegate<T> GetDelegate<T>();

        /// <summary>
        /// 文字列から <paramref name="conversionType"/> に変換するデリゲートを返します。
        /// </summary>
        /// <param name="conversionType">変換後の型。</param>
        /// <returns><paramref name="conversionType"/> への変換がサポートされていれば <see cref="TryParseDelegate{object}"/> デリゲートを、それ以外なら <c>null</c> を返します。</returns>
        TryParseDelegate<object> GetDelegate(Type conversionType);
    }

    /// <summary>
    /// 文字列から <typeparamref name="T"/> に変換する為のデリゲート型。
    /// </summary>
    /// <typeparam name="T">変換後の型。</typeparam>
    /// <param name="input">入力文字列。</param>
    /// <param name="provider">カルチャ固有の書式情報。</param>
    /// <param name="result">変換に成功すれば変換後の値、それ以外なら <typeparamref name="T"/> の既定値が返されます。</param>
    /// <returns>変換に成功すれば <c>true</c>、失敗すれば <c>false</c>、<typeparamref name="T"/> が変換のサポートされない型なら <c>null</c> が返されます。</returns>
    public delegate bool TryParseDelegate<T>(string input, IFormatProvider provider, out T result);
}