using System;

namespace InAsync.ConvertExtras.TryParsers {

    public class EnumTryParser : ITryParser {
        public static readonly EnumTryParser Default = new EnumTryParser();

        public TryParserResult<T> Execute<T>(string input, IFormatProvider provider) {
            return ExecuteCore<T>(typeof(T), input, provider);
        }

        public TryParserResult<object> Execute(Type conversionType, string input, IFormatProvider provider) {
            return ExecuteCore<object>(conversionType, input, provider);
        }

        private TryParserResult<TReturn> ExecuteCore<TReturn>(Type conversionType, string input, IFormatProvider provider) {
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
            if (simpleType.IsEnum == false) return TryParserResult<TReturn>.Empty;

            // 変換先が Nullable の場合は、null から null への変換は正常と見做す。
            if (input == null && isNullable) {
                return new TryParserResult<TReturn>(true, default(TReturn));
            }

            // HACK 暫定
            try {
                return new TryParserResult<TReturn>(true, (TReturn)Enum.Parse(simpleType, input, ignoreCase: true));
            }
            catch (Exception ex) when (ex is ArgumentException || ex is OverflowException) {
                return new TryParserResult<TReturn>(false, default(TReturn));
            }
        }
    }
}