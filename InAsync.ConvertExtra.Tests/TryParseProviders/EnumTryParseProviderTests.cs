using System;
using InAsync.Tests.TestHelpers;
using InAsync.Tests.TestHelpers.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace InAsync.ConvertExtras.TryParseProviders.Tests {

    [TestClass]
    public class EnumTryParseProviderTests {

        private static EnumTryParseProvider TargetProvider() => EnumTryParseProvider.Default;

        [TestMethod] public void GetDelegate_ByteEnum() => InternalGetDelegate_Supported<ByteEnum>();

        [TestMethod] public void GetDelegate_ByteEnumN() => InternalGetDelegate_Supported<ByteEnum?>();

        [TestMethod] public void GetDelegate_IntEnum() => InternalGetDelegate_Supported<IntEnum>();

        [TestMethod] public void GetDelegate_IntEnumN() => InternalGetDelegate_Supported<IntEnum?>();

        private void InternalGetDelegate_Supported<TConversionType>() {
            foreach (var item in TryParseTestCaseStore.Query<TConversionType>()) {
                (TargetProvider().GetDelegate<TConversionType>(item.provider)(item.input, item.provider, out var actualResult), actualResult).Is((item.expected, item.expectedResult), $"No.{item.testNumber}");
            }

            foreach (var item in TryParseTestCaseStore.Query(typeof(TConversionType))) {
                (TargetProvider().GetDelegate(item.conversionType, item.provider)(item.input, item.provider, out var actualResult), actualResult).Is((item.expected, item.expectedResult), $"No.{item.testNumber}");
            }
        }

        [TestMethod] public void GetDelegate_Byte() => InternalGetDelegate_NotSupported<Byte>();

        [TestMethod] public void GetDelegate_ByteN() => InternalGetDelegate_NotSupported<Byte?>();

        [TestMethod] public void GetDelegate_SByte() => InternalGetDelegate_NotSupported<SByte>();

        [TestMethod] public void GetDelegate_SByteN() => InternalGetDelegate_NotSupported<SByte?>();

        [TestMethod] public void GetDelegate_Int16() => InternalGetDelegate_NotSupported<Int16>();

        [TestMethod] public void GetDelegate_Int16N() => InternalGetDelegate_NotSupported<Int16?>();

        [TestMethod] public void GetDelegate_UInt16() => InternalGetDelegate_NotSupported<UInt16>();

        [TestMethod] public void GetDelegate_UInt16N() => InternalGetDelegate_NotSupported<UInt16?>();

        [TestMethod] public void GetDelegate_Int32() => InternalGetDelegate_NotSupported<Int32>();

        [TestMethod] public void GetDelegate_Int32N() => InternalGetDelegate_NotSupported<Int32?>();

        [TestMethod] public void GetDelegate_UInt32() => InternalGetDelegate_NotSupported<UInt32>();

        [TestMethod] public void GetDelegate_UInt32N() => InternalGetDelegate_NotSupported<UInt32?>();

        [TestMethod] public void GetDelegate_Int64() => InternalGetDelegate_NotSupported<Int64>();

        [TestMethod] public void GetDelegate_Int64N() => InternalGetDelegate_NotSupported<Int64?>();

        [TestMethod] public void GetDelegate_UInt64() => InternalGetDelegate_NotSupported<UInt64>();

        [TestMethod] public void GetDelegate_UInt64N() => InternalGetDelegate_NotSupported<UInt64?>();

        [TestMethod] public void GetDelegate_Single() => InternalGetDelegate_NotSupported<Single>();

        [TestMethod] public void GetDelegate_SingleN() => InternalGetDelegate_NotSupported<Single?>();

        [TestMethod] public void GetDelegate_Double() => InternalGetDelegate_NotSupported<Double>();

        [TestMethod] public void GetDelegate_DoubleN() => InternalGetDelegate_NotSupported<Double?>();

        [TestMethod] public void GetDelegate_Decimal() => InternalGetDelegate_NotSupported<Decimal>();

        [TestMethod] public void GetDelegate_DecimalN() => InternalGetDelegate_NotSupported<Decimal?>();

        [TestMethod] public void GetDelegate_Boolean() => InternalGetDelegate_NotSupported<Boolean>();

        [TestMethod] public void GetDelegate_BooleanN() => InternalGetDelegate_NotSupported<Boolean?>();

        [TestMethod] public void GetDelegate_Char() => InternalGetDelegate_NotSupported<Char>();

        [TestMethod] public void GetDelegate_CharN() => InternalGetDelegate_NotSupported<Char?>();

        [TestMethod] public void GetDelegate_DateTime() => InternalGetDelegate_NotSupported<DateTime>();

        [TestMethod] public void GetDelegate_DateTimeN() => InternalGetDelegate_NotSupported<DateTime?>();

        [TestMethod] public void GetDelegate_TimeSpan() => InternalGetDelegate_NotSupported<TimeSpan>();

        [TestMethod] public void GetDelegate_TimeSpanN() => InternalGetDelegate_NotSupported<TimeSpan?>();

        [TestMethod] public void GetDelegate_Guid() => InternalGetDelegate_NotSupported<Guid>();

        [TestMethod] public void GetDelegate_GuidN() => InternalGetDelegate_NotSupported<Guid?>();

        [TestMethod] public void GetDelegate_String() => InternalGetDelegate_NotSupported<String>();

        [TestMethod] public void GetDelegate_Version() => InternalGetDelegate_NotSupported<Version>();

        [TestMethod] public void GetDelegate_Uri() => InternalGetDelegate_NotSupported<Uri>();

        [TestMethod] public void GetDelegate_TypeConvertableClass() => InternalGetDelegate_NotSupported<TypeConvertableClass>();

        [TestMethod] public void GetDelegate_VanillaClass() => InternalGetDelegate_NotSupported<VanillaClass>();

        private void InternalGetDelegate_NotSupported<TConversionType>() {
            foreach (var item in TryParseTestCaseStore.Query<TConversionType>()) {
                TargetProvider().GetDelegate<TConversionType>(item.provider).Is(null);
            }

            foreach (var item in TryParseTestCaseStore.Query(typeof(TConversionType))) {
                TargetProvider().GetDelegate(typeof(TConversionType), item.provider).Is(null);
            }
        }
    }
}