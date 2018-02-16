using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;

namespace InAsync.Tests {

    [TestClass]
    public class StringConvertTest {

        [TestMethod]
        public void ToOrDefault_Test() {
            "a123".ToOrDefault<int>().Is(0);
            "a123".ToOrDefault<int?>().Is(null);
            "a123".ToOrDefault<decimal>().Is(0);
            "a123".ToOrDefault<decimal?>().Is(null);
            "a123".ToOrDefault<DateTimeKind>().Is(default(DateTimeKind));
            "a123".ToOrDefault<DateTime>().Is(default(DateTime));
            "a123".ToOrDefault<DateTime?>().Is(null);
            "a123".ToOrDefault<Guid>().Is(default(Guid));
            "a123".ToOrDefault<Guid?>().Is(null);
            "a123".ToOrDefault<string>().Is("a123");
            "a123".ToOrDefault<Uri>().Is(null);
        }

        [TestMethod]
        public void TryParse_Test() {
            foreach (var item in TestDataSource) {
                StringConvert.TryParse(item.conversionType, item.input, out var actual).Is(item.expectedSuccess, new { item, actual }.ToString());
                actual.Is(item.expectedResult, item.ToString());
            }
        }

        private static IEnumerable<(string input, Type conversionType, bool expectedSuccess, object expectedResult)> TestDataSource = new(string, Type, bool, object)[]{
            // byte
            ("0", typeof(byte), true, (byte)0),
            (byte.MinValue.ToString(), typeof(byte), true, byte.MinValue),
            (byte.MaxValue.ToString(), typeof(byte), true, byte.MaxValue),
            ("+1", typeof(byte), true, (byte)1),
            ("-1", typeof(byte), false, null),
            ("0x10", typeof(byte), false, null),
            ("1,234", typeof(byte), false, null),
            ("1,234.56", typeof(byte), false, null),
            (null, typeof(byte), false, null),

            // TODO byte?

            // sbyte
            ("0", typeof(sbyte), true, (sbyte)0),
            (sbyte.MinValue.ToString(), typeof(sbyte), true, sbyte.MinValue),
            (sbyte.MaxValue.ToString(), typeof(sbyte), true, sbyte.MaxValue),
            ("+1", typeof(sbyte), true, (sbyte)1),
            ("-1", typeof(sbyte), true, (sbyte)-1),
            ("0x10", typeof(sbyte), false, null),
            ("1,234", typeof(sbyte), false, null),
            ("1,234.56", typeof(sbyte), false, null),
            (null, typeof(sbyte), false, null),

            // TODO sbyte?

            // short
            ("0", typeof(short), true, (short)0),
            (short.MinValue.ToString(), typeof(short), true, short.MinValue),
            (short.MaxValue.ToString(), typeof(short), true, short.MaxValue),
            ("+1", typeof(short), true, (short)1),
            ("-1", typeof(short), true, (short)-1),
            ("0x10", typeof(short), false, null),
            ("1,234", typeof(short), true, (short)1234),
            ("1,234.56", typeof(short), false, null),
            (null, typeof(short), false, null),

            // TODO short?

            // ushort
            ("0", typeof(ushort), true, (ushort)0),
            (ushort.MinValue.ToString(), typeof(ushort), true, ushort.MinValue),
            (ushort.MaxValue.ToString(), typeof(ushort), true, ushort.MaxValue),
            ("+1", typeof(ushort), true, (ushort)1),
            ("-1", typeof(ushort), false, null),
            ("0x10", typeof(ushort), false, null),
            ("1,234", typeof(ushort), true, (ushort)1234),
            ("1,234.56", typeof(ushort), false, null),
            (null, typeof(ushort), false, null),

            // TODO ushort?

            // int
            ("0", typeof(int), true, (int)0),
            (int.MinValue.ToString(), typeof(int), true, int.MinValue),
            (int.MaxValue.ToString(), typeof(int), true, int.MaxValue),
            ("+1", typeof(int), true, (int)1),
            ("-1", typeof(int), true, (int)-1),
            ("0x10", typeof(int), false, null),
            ("1,234", typeof(int), true, (int)1234),
            ("1,234.56", typeof(int), false, null),
            (null, typeof(int), false, null),

            // TODO int?

            // uint
            ("0", typeof(uint), true, (uint)0),
            (uint.MinValue.ToString(), typeof(uint), true, uint.MinValue),
            (uint.MaxValue.ToString(), typeof(uint), true, uint.MaxValue),
            ("+1", typeof(uint), true, (uint)1),
            ("-1", typeof(uint), false, null),
            ("0x10", typeof(uint), false, null),
            ("1,234", typeof(uint), true, (uint)1234),
            ("1,234.56", typeof(uint), false, null),
            (null, typeof(uint), false, null),

            // TODO uint?

            // long
            ("0", typeof(long), true, (long)0),
            (long.MinValue.ToString(), typeof(long), true, long.MinValue),
            (long.MaxValue.ToString(), typeof(long), true, long.MaxValue),
            ("+1", typeof(long), true, (long)1),
            ("-1", typeof(long), true, (long)-1),
            ("0x10", typeof(long), false, null),
            ("1,234", typeof(long), true, (long)1234),
            ("1,234.56", typeof(long), false, null),
            (null, typeof(long), false, null),

            // TODO long?

            // ulong
            ("0", typeof(ulong), true, (ulong)0),
            (ulong.MinValue.ToString(), typeof(ulong), true, ulong.MinValue),
            (ulong.MaxValue.ToString(), typeof(ulong), true, ulong.MaxValue),
            ("+1", typeof(ulong), true, (ulong)1),
            ("-1", typeof(ulong), false, null),
            ("0x10", typeof(ulong), false, null),
            ("1,234", typeof(ulong), true, (ulong)1234),
            ("1,234.56", typeof(ulong), false, null),
            (null, typeof(ulong), false, null),

            // TODO ulong?

            // float
            ("0", typeof(float), true, (float)0),
            (float.MinValue.ToString(), typeof(float), true, float.Parse(float.MinValue.ToString())),
            (float.MaxValue.ToString(), typeof(float), true, float.Parse(float.MaxValue.ToString())),
            ("-Åá", typeof(float), true, float.NegativeInfinity),
            ("+Åá", typeof(float), true, float.PositiveInfinity),
            ("Åá", typeof(float), false, null),
            ("NaN (îÒêîíl)", typeof(float), true, float.NaN),
            ("+1", typeof(float), true, (float)1),
            ("-1", typeof(float), true, (float)-1),
            ("0x10", typeof(float), false, null),
            ("1,234", typeof(float), true, (float)1234),
            ("1,234.56", typeof(float), true, (float)1234.56),
            (null, typeof(float), false, null),

            // TODO float?

            // double
            ("0", typeof(double), true, (double)0),
            (float.MinValue.ToString(), typeof(double), true, double.Parse(float.MinValue.ToString())),
            (float.MaxValue.ToString(), typeof(double), true, double.Parse(float.MaxValue.ToString())),
            ("-Åá", typeof(double), true, double.NegativeInfinity),
            ("+Åá", typeof(double), true, double.PositiveInfinity),
            ("Åá", typeof(double), false, null),
            ("NaN (îÒêîíl)", typeof(double), true, double.NaN),
            ("+1", typeof(double), true, (double)1),
            ("-1", typeof(double), true, (double)-1),
            ("0x10", typeof(double), false, null),
            ("1,234", typeof(double), true, (double)1234),
            ("1,234.56", typeof(double), true, (double)1234.56),
            (null, typeof(double), false, null),

            // TODO double?

            // decimal
            ("0", typeof(decimal), true, (decimal)0),
            (decimal.MinValue.ToString(), typeof(decimal), true, decimal.Parse(decimal.MinValue.ToString())),
            (decimal.MaxValue.ToString(), typeof(decimal), true, decimal.Parse(decimal.MaxValue.ToString())),
            (decimal.Zero.ToString(), typeof(decimal), true, decimal.Zero),
            (decimal.One.ToString(), typeof(decimal), true, decimal.One),
            (decimal.MinusOne.ToString(), typeof(decimal), true, decimal.MinusOne),
            ("-Åá", typeof(decimal), false, null),
            ("+Åá", typeof(decimal), false, null),
            ("Åá", typeof(decimal), false, null),
            ("NaN (îÒêîíl)", typeof(decimal), false, null),
            ("+1", typeof(decimal), true, (decimal)1),
            ("-1", typeof(decimal), true, (decimal)-1),
            ("0x10", typeof(decimal), false, null),
            ("1,234", typeof(decimal), true, (decimal)1234),
            ("1,234.56", typeof(decimal), true, (decimal)1234.56),
            (null, typeof(decimal), false, null),

            // TODO decimal?

            // TODO bool
            ("true", typeof(bool), true, true),
            ("false", typeof(bool), true, false),
            ("True", typeof(bool), true, true),
            ("False", typeof(bool), true, false),
            ("TRUE", typeof(bool), true, true),
            ("FALSE", typeof(bool), true, false),
            ("tRue", typeof(bool), true, true),
            ("fAlse", typeof(bool), true, false),
            ("0", typeof(bool), false, null),
            ("+1", typeof(bool), false, null),
            ("-1", typeof(bool), false, null),
            ("0x10", typeof(bool), false, null),
            (null, typeof(bool), false, null),

            // TODO bool?

            // TODO char
            // TODO char?
            // TODO DateTime
            // TODO DateTime?
            // TODO TimeSpan
            // TODO TimeSpan?
            // TODO Enum
            // TODO Enum?
            // TODO Guid
            // TODO Guid?
            // TODO Version
            // TODO Version?
            // TODO string
            // TODO Uri
        };
    }
}