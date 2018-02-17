using System;
using System.ComponentModel;
using System.Diagnostics.Contracts;
using System.Globalization;

namespace InAsync.Converter {

    /// <summary>
    /// オブジェクトを任意の型へ変換するクラス。
    /// </summary>
    public static class ObjectConvert {

        /// <summary>
        /// オブジェクトを任意の型に変換します。
        /// </summary>
        /// <typeparam name="T">変換後の型。</typeparam>
        /// <param name="input">入力値。</param>
        /// <param name="defaultValue">変換に失敗した場合の既定値。</param>
        /// <returns></returns>
        public static T ToOrDefault<T>(this object input, T defaultValue = default(T)) {
            if (TryConvert(input, typeof(T), CultureInfo.CurrentCulture, out var result)) {
                return (T)result;
            }
            else {
                return defaultValue;
            }
        }

        /// <summary>
        /// オブジェクトを任意の型に変換します。
        /// </summary>
        /// <param name="input">入力値。</param>
        /// <param name="conversionType">変換後の型。</param>
        /// <param name="provider"></param>
        /// <param name="result"></param>
        /// <returns></returns>
        public static bool TryConvert(object input, Type conversionType, IFormatProvider provider, out object result) {
            if (conversionType == null) throw new ArgumentNullException(nameof(conversionType));
            Contract.Ensures(Contract.Result<bool>() || Contract.ValueAtReturn(out result) == null);
            Contract.EndContractBlock();

            // 変換先が Nullable なら、その基になる型を実際の変換先とする。
            var simpleType = Nullable.GetUnderlyingType(conversionType);
            if (simpleType != null) {
                conversionType = simpleType;
            }

            // 入力が null の場合、変換先が null を許容するか否かを戻り値とする。
            if (input == null) {
                result = null;
                return conversionType.IsValueType == false || simpleType != null;
            }
            var inputType = input.GetType();

            // 入力と変換先の型が同一なら、そのまま返す。
            if (inputType == conversionType) {
                result = input;
                return true;
            }

            // 入力の型が string なら、TryParse に丸投げ。
            if (inputType == typeof(string)) {
                return StringConvert.TryParse((string)input, conversionType, provider, out result);
            }

            // TypeConverter が入力の型をサポートしていたら、丸投げ。
            var converter = TypeDescriptor.GetConverter(conversionType);
            if (converter.CanConvertFrom(inputType)) {
                var culture = provider as CultureInfo ?? CultureInfo.CurrentCulture;

                try {
                    result = converter.ConvertFrom(null, culture, input);
                    return true;
                }
                catch {
                    result = null;
                    return false;
                }
            }

            // 入力の型が IConvertible を実装していたら、Convert.ChangeType に丸投げ。
            if (input is IConvertible) {
                try {
                    result = Convert.ChangeType(input, conversionType, provider);
                    return true;
                }
                catch {
                    result = null;
                    return false;
                }
            }

            result = null;
            return false;
        }
    }
}