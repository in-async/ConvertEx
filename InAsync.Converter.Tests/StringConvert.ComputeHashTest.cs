using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;

namespace InAsync.Tests {

    public partial class StringConvertTest {

        // static フィールド変数は宣言順に初期化されるので、_sjisLazy は SJIS フィールドよりも上に記述する必要がある。
        // https://docs.microsoft.com/ja-jp/dotnet/csharp/language-reference/language-specification/classes#static-field-initialization
        private static readonly Lazy<Encoding> _sjisLazy = new Lazy<Encoding>(() => {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            return Encoding.GetEncoding("sjis");
        });

        private static Encoding UTF8 = Encoding.UTF8;
        private static Encoding SJIS = _sjisLazy.Value;

        [TestMethod]
        public void MD5_Test() {
            foreach (var item in MD5_TestSource) {
                if (!AssertException.TryExecute(() => StringConvert.MD5(item.message, item.encoding), item.expectedExceptionType, out var actual)) {
                    continue;
                }

                actual.Is<byte>(item.expected, item);
            }
        }

        private static IEnumerable<(string message, Encoding encoding, byte[] expected, Type expectedExceptionType)> MD5_TestSource = new(string, Encoding, byte[], Type)[]{
            (null  , null, null                                                           , typeof(ArgumentNullException)),
            ("foo" , null, StringConvert.FromHexString("acbd18db4cc2f85cedef654fccc4a4d8"), null),
            ("ほげ", null, StringConvert.FromHexString("63e6f3f3326ea60afe9f7b037cc1d732"), null),
            ("ほげ", UTF8, StringConvert.FromHexString("63e6f3f3326ea60afe9f7b037cc1d732"), null),
            ("ほげ", SJIS, StringConvert.FromHexString("9e6c4b30163d973d011f10143b25efd8"), null),
        };

        [TestMethod]
        public void SHA1_Test() {
            foreach (var item in SHA1_TestSource) {
                if (!AssertException.TryExecute(() => StringConvert.SHA1(item.message, item.encoding), item.expectedExceptionType, out var actual)) {
                    continue;
                }

                actual.Is<byte>(item.expected, item);
            }
        }

        private static IEnumerable<(string message, Encoding encoding, byte[] expected, Type expectedExceptionType)> SHA1_TestSource = new(string, Encoding, byte[], Type)[]{
            (null, null, null, typeof(ArgumentNullException)),

            ("foo" , null, StringConvert.FromHexString("0beec7b5ea3f0fdbc95d0dd47f3c5bc275da8a33"), null),
            ("ほげ", null, StringConvert.FromHexString("eb3a35081005182d70ff64c9856e14186c03e197"), null),
            ("ほげ", UTF8, StringConvert.FromHexString("eb3a35081005182d70ff64c9856e14186c03e197"), null),
            ("ほげ", SJIS, StringConvert.FromHexString("cba533c92ab3949601a520508faf080cc796aa4d"), null),
        };
    }
}