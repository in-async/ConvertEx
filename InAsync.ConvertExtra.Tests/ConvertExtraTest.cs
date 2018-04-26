using System;
using System.Globalization;
using InAsync.Tests.TestHelpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace InAsync.Tests {

    [TestClass]
    public partial class ConvertExtraTest {

        [TestMethod]
        public void To_Test() {
            "123.45".To<int>().Is(0);
            "123.45".To<int>(1).Is(1);
            "123.45".To<float>().Is(123.45f);
        }

        [TestMethod]
        public void TryParse_T_input_result_Test() {
            try {
                CultureInfo.CurrentCulture = InvariantCulture;
                { (ConvertExtra.TryParse<float>("-Infinity", out var result), result).Is((true, float.NegativeInfinity)); }

                CultureInfo.CurrentCulture = StubJPCulture;
                { (ConvertExtra.TryParse<float>("-Infinity", out var result), result).Is((false, 0)); }
            }
            finally {
                CultureInfo.CurrentCulture = CurrentCulture;
            }
        }

        [TestMethod]
        public void TryParse_T_input_provider_result_Test() {
            { (ConvertExtra.TryParse<int>("123.45", null, out var result), result).Is((false, 0)); }
            { (ConvertExtra.TryParse<float>("123.45", null, out var result), result).Is((true, 123.45f)); }
            { (ConvertExtra.TryParse<int>("123.45", InvariantCulture, out var result), result).Is((false, 0)); }
            { (ConvertExtra.TryParse<float>("123.45", InvariantCulture, out var result), result).Is((true, 123.45f)); }
        }

        [TestMethod]
        public void TryParse_input_conversionType_result_Test() {
            try {
                CultureInfo.CurrentCulture = InvariantCulture;
                { (ConvertExtra.TryParse("-Infinity", typeof(float), out var result), result).Is((true, float.NegativeInfinity)); }

                CultureInfo.CurrentCulture = StubJPCulture;
                { (ConvertExtra.TryParse("-Infinity", typeof(float), out var result), result).Is((false, null)); }
            }
            finally {
                CultureInfo.CurrentCulture = CurrentCulture;
            }
        }

        [TestMethod]
        public void TryParse_input_conversionType_provider_result_Test() {
            foreach (var item in TryParseTestCaseStore.Query()) {
                (ConvertExtra.TryParse(item.input, item.conversionType, item.provider, out var actual), actual).Is((item.expected, item.expectedResult), item);
            }
        }

        #region TestData

        private static readonly CultureInfo InvariantCulture = CultureInfo.InvariantCulture;
        private static readonly CultureInfo CurrentCulture = CultureInfo.CurrentCulture;

        private static readonly CultureInfo StubJPCulture = ((Func<CultureInfo>)(() => {
            var stubCulture = new CultureInfo("ja-JP");
            stubCulture.NumberFormat.PositiveInfinitySymbol = "+∞";
            stubCulture.NumberFormat.NegativeInfinitySymbol = "-∞";
            stubCulture.NumberFormat.NaNSymbol = "NaN (非数値)";
            return stubCulture;
        }))();

        #endregion TestData
    }
}