using System;
using System.ComponentModel;
using System.Globalization;

namespace InAsync.Tests.TestHelpers.Models {

    [TypeConverter(typeof(Converter))]
    public class TypeConvertableClass {

        public TypeConvertableClass(string value) {
            Value = value ?? throw new ArgumentNullException(nameof(value));
        }

        public string Value { get; }

        private class Converter : TypeConverter {

            public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType) {
                if (sourceType == typeof(string)) return true;

                return base.CanConvertFrom(context, sourceType);
            }

            public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value) {
                if (value is string source) {
                    if (source is null) throw new FormatException();

                    return new TypeConvertableClass(source);
                }

                return base.ConvertFrom(context, culture, value);
            }
        }
    }
}