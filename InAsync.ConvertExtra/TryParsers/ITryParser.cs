using System;

namespace InAsync.ConvertExtras.TryParsers {

    public interface ITryParser {

        bool? TryParse<T>(string input, IFormatProvider provider, out T result);

        bool? TryParse(Type conversionType, string input, IFormatProvider provider, out object result);
    }
}