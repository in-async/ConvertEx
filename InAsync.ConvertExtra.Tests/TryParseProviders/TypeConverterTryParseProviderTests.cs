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
                (TargetProvider().GetDelegate<TConversionType>()(item.input, item.provider, out var actualResult), actualResult).Is((item.expected, item.expectedResult), $"No.{item.testNumber}");
            }

            foreach (var item in TryParseTestCaseStore.Query(typeof(TConversionType))) {
                (TargetProvider().GetDelegate(item.conversionType)(item.input, item.provider, out var actualResult), actualResult).Is((item.expected, item.expectedResult), $"No.{item.testNumber}");
            }
        }

        [TestMethod] public void GetDelegate_VanillaClass() => InternalGetDelegate_NotSupported<VanillaClass>();

        private void InternalGetDelegate_NotSupported<TConversionType>() {
            TargetProvider().GetDelegate<TConversionType>().Is(null);

            TargetProvider().GetDelegate(typeof(TConversionType)).Is(null);
        }
    }
}