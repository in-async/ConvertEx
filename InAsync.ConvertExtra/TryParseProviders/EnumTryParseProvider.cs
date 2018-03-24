using System;

namespace InAsync.ConvertExtras.TryParseProviders {

    public class EnumTryParseProvider : TryParseProvider {
        public static readonly EnumTryParseProvider Default = new EnumTryParseProvider();

        public override TryParseDelegate<T> GetDelegate<T>() {
            var typeInfo = GetNullableTypeInfo(typeof(T));
            if (typeInfo == null) return null;

            return (string input, IFormatProvider provider, out T result) => TryParseCore(typeInfo.Value, input, provider, out result);
        }

        public override TryParseDelegate<object> GetDelegate(Type conversionType) {
            var typeInfo = GetNullableTypeInfo(conversionType);
            if (typeInfo == null) return null;

            return (string input, IFormatProvider provider, out object result) => TryParseCore(typeInfo.Value, input, provider, out result);
        }

        private NullableTypeInfo? GetNullableTypeInfo(Type conversionType) {
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

            return new NullableTypeInfo(simpleType, isNullable);
        }

        private bool TryParseCore<T>(NullableTypeInfo typeInfo, string input, IFormatProvider provider, out T result) {
            // 変換先が Nullable の場合は、null から null への変換は正常と見做す。
            if (input == null && typeInfo.IsNullable) {
                result = default(T);
                return true;
            }

            // HACK 暫定
            try {
                result = (T)Enum.Parse(typeInfo.SimpleType, input, ignoreCase: true);
                return true;
            }
            catch (Exception ex) when (ex is ArgumentException || ex is OverflowException) {
                result = default(T);
                return false;
            }
        }

        private struct NullableTypeInfo {
            public readonly Type SimpleType;
            public readonly bool IsNullable;

            public NullableTypeInfo(Type simpleType, bool isNullable) {
                SimpleType = simpleType;
                IsNullable = isNullable;
            }
        }
    }
}