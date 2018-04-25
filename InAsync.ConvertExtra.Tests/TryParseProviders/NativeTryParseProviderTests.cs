using System;
using InAsync.Tests.TestHelpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace InAsync.ConvertExtras.TryParseProviders.Tests {

    [TestClass]
    public class NativeTryParseProviderTests {

        [TestMethod] public void GetDelegate_Byte() => InternalGetDelegate_Supported<Byte>();

        [TestMethod] public void GetDelegate_NByte() => InternalGetDelegate_Supported<Byte?>();

        [TestMethod] public void GetDelegate_SByte() => InternalGetDelegate_Supported<SByte>();

        [TestMethod] public void GetDelegate_NSByte() => InternalGetDelegate_Supported<SByte?>();

        [TestMethod] public void GetDelegate_Int16() => InternalGetDelegate_Supported<Int16>();

        [TestMethod] public void GetDelegate_NInt16() => InternalGetDelegate_Supported<Int16?>();

        [TestMethod] public void GetDelegate_UInt16() => InternalGetDelegate_Supported<UInt16>();

        [TestMethod] public void GetDelegate_NUInt16() => InternalGetDelegate_Supported<UInt16?>();

        [TestMethod] public void GetDelegate_Int32() => InternalGetDelegate_Supported<Int32>();

        [TestMethod] public void GetDelegate_NInt32() => InternalGetDelegate_Supported<Int32?>();

        [TestMethod] public void GetDelegate_UInt32() => InternalGetDelegate_Supported<UInt32>();

        [TestMethod] public void GetDelegate_NUInt32() => InternalGetDelegate_Supported<UInt32?>();

        [TestMethod] public void GetDelegate_Int64() => InternalGetDelegate_Supported<Int64>();

        [TestMethod] public void GetDelegate_NInt64() => InternalGetDelegate_Supported<Int64?>();

        [TestMethod] public void GetDelegate_UInt64() => InternalGetDelegate_Supported<UInt64>();

        [TestMethod] public void GetDelegate_NUInt64() => InternalGetDelegate_Supported<UInt64?>();

        [TestMethod] public void GetDelegate_Single() => InternalGetDelegate_Supported<Single>();

        [TestMethod] public void GetDelegate_Double() => InternalGetDelegate_Supported<Double>();

        [TestMethod] public void GetDelegate_Decimal() => InternalGetDelegate_Supported<Decimal>();

        [TestMethod] public void GetDelegate_Boolean() => InternalGetDelegate_Supported<Boolean>();

        [TestMethod] public void GetDelegate_Char() => InternalGetDelegate_Supported<Char>();

        [TestMethod] public void GetDelegate_DateTime() => InternalGetDelegate_Supported<DateTime>();

        [TestMethod] public void GetDelegate_TimeSpan() => InternalGetDelegate_Supported<TimeSpan>();

        [TestMethod] public void GetDelegate_Guid() => InternalGetDelegate_Supported<Guid>();

        [TestMethod] public void GetDelegate_NSingle() => InternalGetDelegate_Supported<Single?>();

        [TestMethod] public void GetDelegate_NDouble() => InternalGetDelegate_Supported<Double?>();

        [TestMethod] public void GetDelegate_NDecimal() => InternalGetDelegate_Supported<Decimal?>();

        [TestMethod] public void GetDelegate_NBoolean() => InternalGetDelegate_Supported<Boolean?>();

        [TestMethod] public void GetDelegate_NChar() => InternalGetDelegate_Supported<Char?>();

        [TestMethod] public void GetDelegate_NDateTime() => InternalGetDelegate_Supported<DateTime?>();

        [TestMethod] public void GetDelegate_NTimeSpan() => InternalGetDelegate_Supported<TimeSpan?>();

        [TestMethod] public void GetDelegate_NGuid() => InternalGetDelegate_Supported<Guid?>();

        [TestMethod] public void GetDelegate_String() => InternalGetDelegate_Supported<String>();

        [TestMethod] public void GetDelegate_Version() => InternalGetDelegate_Supported<Version>();

        [TestMethod] public void GetDelegate_Uri() => InternalGetDelegate_Supported<Uri>();

        private void InternalGetDelegate_Supported<TConversionType>() {
            var target = NativeTryParseProvider.Default;

            foreach (var item in TryParseTestCaseStore.Query<TConversionType>()) {
                (target.GetDelegate<TConversionType>()(item.input, item.provider, out var actualResult), actualResult).Is((item.expected, item.expectedResult), $"No.{item.testNumber}");
            }

            foreach (var item in TryParseTestCaseStore.Query(typeof(TConversionType))) {
                (target.GetDelegate(item.conversionType)(item.input, item.provider, out var actualResult), actualResult).Is((item.expected, item.expectedResult), $"No.{item.testNumber}");
            }
        }

        [TestMethod] public void GetDelegate_CustomClass() => InternalGetDelegate_NotSupported<CustomClass>();

        [TestMethod] public void GetDelegate_Enum() => InternalGetDelegate_NotSupported<TestByteEnum>();

        private void InternalGetDelegate_NotSupported<TConversionType>() {
            var target = NativeTryParseProvider.Default;

            target.GetDelegate<TConversionType>().Is(null);

            target.GetDelegate(typeof(TConversionType)).Is(null);
        }
    }
}