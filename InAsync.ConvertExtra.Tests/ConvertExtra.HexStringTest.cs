using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;

namespace InAsync.Tests {

    public partial class ConvertExtraTest {

        [TestMethod]
        public void ToHexString_Test() {
            foreach (var item in ToHexString_TestSource) {
                if (!AssertException.TryExecute(() => item.data.ToHexString(item.toUpper), item.expectedExceptionType, out var actual)) {
                    continue;
                }

                actual.Is(item.expected, item);
            }
        }

        private static IEnumerable<(byte[] data, bool toUpper, string expected, Type expectedExceptionType)> ToHexString_TestSource = new(byte[], bool, string, Type)[]{
            (null                                    , false, null              , typeof(ArgumentNullException)),
            (new byte[]{0x01,0x23}                   , false, "0123"            , null),
            (BitConverter.GetBytes(0x1)              , false, "01000000"        , null),
            (BitConverter.GetBytes(-1)               , false, "ffffffff"        , null),
            (BitConverter.GetBytes(0x12)             , false, "12000000"        , null),
            (BitConverter.GetBytes(0x123)            , false, "23010000"        , null),
            (BitConverter.GetBytes(0x123456789abcdef), false, "efcdab8967452301", null),
            (BitConverter.GetBytes(0x123)            , true , "23010000"        , null),
            (BitConverter.GetBytes(0x123456789abcdef), true , "EFCDAB8967452301", null),
            (new byte[]{0xac,0xbd,0x18,0xdb,0x4c,0xc2,0xf8,0x5c,0xed,0xef,0x65,0x4f,0xcc,0xc4,0xa4,0xd8 }, false, "acbd18db4cc2f85cedef654fccc4a4d8", null),
            (new byte[]{0xac,0xbd,0x18,0xdb,0x4c,0xc2,0xf8,0x5c,0xed,0xef,0x65,0x4f,0xcc,0xc4,0xa4,0xd8 }, true , "ACBD18DB4CC2F85CEDEF654FCCC4A4D8", null),
        };

        [TestMethod]
        public void FromHexString_Test() {
            foreach (var item in FromHexString_TestSource) {
                if (!AssertException.TryExecute(() => ConvertExtra.FromHexString(item.hexString), item.expectedExceptionType, out var actual)) {
                    continue;
                }

                actual.Is<byte>(item.expected, item);
            }
        }

        private static IEnumerable<(string hexString, byte[] expected, Type expectedExceptionType)> FromHexString_TestSource = new(string, byte[], Type)[]{
            (null              , null                                    , typeof(ArgumentNullException)),
            ("123"             , null                                    , typeof(ArgumentOutOfRangeException)),
            ("wxyz"            , null                                    , typeof(FormatException)),
            ("0123"            , new byte[]{0x01,0x23}                   , null),
            ("01000000"        , BitConverter.GetBytes(0x1)              , null),
            ("ffffffff"        , BitConverter.GetBytes(-1)               , null),
            ("12000000"        , BitConverter.GetBytes(0x12)             , null),
            ("23010000"        , BitConverter.GetBytes(0x123)            , null),
            ("efcdab8967452301", BitConverter.GetBytes(0x123456789abcdef), null),
            ("23010000"        , BitConverter.GetBytes(0x123)            , null),
            ("EFCDAB8967452301", BitConverter.GetBytes(0x123456789abcdef), null),
            ("acbd18db4cc2f85cedef654fccc4a4d8", new byte[]{0xac,0xbd,0x18,0xdb,0x4c,0xc2,0xf8,0x5c,0xed,0xef,0x65,0x4f,0xcc,0xc4,0xa4,0xd8 }, null),
            ("ACBD18DB4CC2F85CEDEF654FCCC4A4D8", new byte[]{0xac,0xbd,0x18,0xdb,0x4c,0xc2,0xf8,0x5c,0xed,0xef,0x65,0x4f,0xcc,0xc4,0xa4,0xd8 }, null),
        };
    }
}