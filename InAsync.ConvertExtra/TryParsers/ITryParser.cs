using System;

namespace InAsync.ConvertExtra.TryParsers {

    public interface ITryParser {

        TryParserResult<T> Execute<T>(string input, IFormatProvider provider);

        TryParserResult<object> Execute(Type conversionType, string input, IFormatProvider provider);
    }

    public struct TryParserResult<T> {
        public static TryParserResult<T> Empty = new TryParserResult<T>();

        public TryParserResult(bool success, T value) {
            Parsed = true;
            Success = success;
            Value = value;
        }

        public bool Parsed { get; }
        public bool Success { get; }
        public T Value { get; }
    }
}