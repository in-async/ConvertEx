using System;

namespace InAsync.ConvertExtras.TryParseProviders {

    /// <summary>
    /// 文字列から <see cref="Enum"/> 型への変換デリゲートを提供する <see cref="ITryParseProvider"/> クラス。
    /// </summary>
    public class EnumTryParseProvider : TryParseProvider {
        public static readonly EnumTryParseProvider Default = new EnumTryParseProvider();

        public override TryParseDelegate<T> GetDelegate<T>(IFormatProvider provider) {
            var typeInfo = ConversionTypeInfo.Get(typeof(T));
            if (typeInfo == null) return null;

            return (string input, IFormatProvider p, out T result) => TryParseCore(typeInfo.Value, input, p, out result);
        }

        public override TryParseDelegate<object> GetDelegate(Type conversionType, IFormatProvider provider) {
            var typeInfo = ConversionTypeInfo.Get(conversionType);
            if (typeInfo == null) return null;

            return (string input, IFormatProvider p, out object result) => TryParseCore(typeInfo.Value, input, p, out result);
        }

        /// <summary>
        /// 文字列から <typeparamref name="TResult"/> へ変換します。
        /// </summary>
        /// <typeparam name="TResult">変換後の型。</typeparam>
        /// <param name="typeInfo">変換に必要な型情報。</param>
        /// <param name="input">入力文字列。</param>
        /// <param name="provider">カルチャ固有の書式情報。<c>null</c> の場合は現在のカルチャが使用されます。</param>
        /// <param name="result">変換に成功すれば変換後の値、それ以外なら <typeparamref name="TResult"/> の既定値が返されます。</param>
        /// <returns>変換に成功すれば <c>true</c>、それ以外なら <c>false</c>。</returns>
        private bool TryParseCore<TResult>(ConversionTypeInfo typeInfo, string input, IFormatProvider provider, out TResult result) {
            // 変換先が Nullable の場合は、null から null への変換は正常と見做す。
            if (input == null && typeInfo.IsNullable) {
                result = default(TResult);
                return true;
            }

            // HACK 暫定
            try {
                result = (TResult)Enum.Parse(typeInfo.SimpleType, input, ignoreCase: true);
                return true;
            }
            catch (Exception ex) when (ex is ArgumentException || ex is OverflowException) {
                result = default(TResult);
                return false;
            }
        }

        /// <summary>
        /// 変換に必要な型情報。
        /// </summary>
        private struct ConversionTypeInfo {
            public readonly Type ConversionType;
            public readonly Type SimpleType;
            public readonly bool IsNullable;

            private ConversionTypeInfo(Type conversionType, Type simpleType, bool isNullable) {
                ConversionType = conversionType;
                SimpleType = simpleType;
                IsNullable = isNullable;
            }

            public static ConversionTypeInfo? Get(Type conversionType) {
                // 変換先が Nullable なら基の型を取得。
                var simpleType = Nullable.GetUnderlyingType(conversionType);
                bool isNullable;
                if (simpleType == null) {
                    isNullable = false;
                    simpleType = conversionType;
                }
                else {
                    isNullable = true;
                }
                if (simpleType.IsEnum == false) return null;

                return new ConversionTypeInfo(conversionType, simpleType, isNullable);
            }
        }
    }
}