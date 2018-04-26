using InAsync.Tests.TestHelpers;
using InAsync.Tests.TestHelpers.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace InAsync.ConvertExtras.TryParseProviders.Tests {

    [TestClass]
    public class TypeConverterTryParseProviderTests {

        private static TypeConverterTryParseProvider TargetProvider() => TypeConverterTryParseProvider.Default;

        [TestMethod] public void GetDelegate_TypeConvertableClass() => InternalGetDelegate_Supported<TypeConvertableClass>();

        private void InternalGetDelegate_Supported<TConversionType>() {
            foreach (var item in TryParseTestCaseStore.Query<TConversionType>()) {
                (TargetProvider().GetDelegate<TConversionType>(item.provider)(item.input, item.provider, out var actualResult), actualResult).Is((item.expected, item.expectedResult), $"No.{item.testNumber}");
            }

            foreach (var item in TryParseTestCaseStore.Query(typeof(TConversionType))) {
                (TargetProvider().GetDelegate(item.conversionType, item.provider)(item.input, item.provider, out var actualResult), actualResult).Is((item.expected, item.expectedResult), $"No.{item.testNumber}");
            }
        }

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