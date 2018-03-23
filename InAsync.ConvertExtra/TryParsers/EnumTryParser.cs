using System;

namespace InAsync.ConvertExtras.TryParsers {

    public class EnumTryParser : ITryParser {
        public static readonly EnumTryParser Default = new EnumTryParser();

        public bool? TryParse<T>(string input, IFormatProvider provider, out T result) {
            return TryParseCore<T>(typeof(T), input, provider, out result);
        }

        public bool? TryParse(Type conversionType, string input, IFormatProvider provider, out object result) {
            return TryParseCore<object>(conversionType, input, provider, out result);
        }

        private bool? TryParseCore<TResult>(Type conversionType, string input, IFormatProvider provider, out TResult result) {
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
            if (simpleType.IsEnum == false) {
                result = default(TResult);
                return null;
            }

            // 変換先が Nullable の場合は、null から null への変換は正常と見做す。
            if (input == null && isNullable) {
                result = default(TResult);
                return true;
            }

            // HACK 暫定
            try {
                result = (TResult)Enum.Parse(simpleType, input, ignoreCase: true);
                return true;
            }
            catch (Exception ex) when (ex is ArgumentException || ex is OverflowException) {
                result = default(TResult);
                return false;
            }
        }
    }
}