using System;
using System.Diagnostics.Contracts;
using System.Globalization;

namespace InAsync {

    public static partial class StringConvert {

        /// <summary>
        /// byte 配列を 16 進文字列に変換します。
        /// </summary>
        /// <param name="data"></param>
        /// <param name="toUpper"></param>
        /// <returns></returns>
        public static string ToHexString(this byte[] data, bool toUpper = false) {
            if (data == null) throw new ArgumentNullException(nameof(data));
            Contract.Ensures(Contract.Result<string>() != null);
            Contract.Ensures(Contract.Result<String>().Length == data.Length * 2);

            var format = toUpper ? "X2" : "x2";
            var numberFormatInfo = CultureInfo.InvariantCulture.NumberFormat;
            var chars = new Char[data.Length * 2];
            for (int i = 0, ci = 0; i < data.Length; i++, ci += 2) {
                var str = data[i].ToString(format, numberFormatInfo);
                Contract.Assume(str.Length == 2);

                chars[ci] = str[0];
                chars[ci + 1] = str[1];
            }
            return new String(chars);
        }

        //public static String ToHexString(this byte[] data) {
        //    if (data == null) throw new ArgumentNullException(nameof(data));
        //    Contract.Ensures(Contract.Result<System.String>() != null);
        //    Contract.Ensures(Contract.Result<System.String>().Length == data.Length * 2);

        //    var bldr = new StringBuilder(data.Length * 2);
        //    for (int i = 0; i < data.Length; i++) {
        //        bldr.Append(data[i].ToString("x2"));
        //    }
        //    var result = bldr.ToString();
        //    if (result.Length != data.Length * 2) throw new InvalidOperationException();

        //    return result;
        //}

        /// <summary>
        /// 16進文字列を byte 配列に変換します。
        /// </summary>
        /// <param name="hexString"></param>
        /// <returns></returns>
        public static byte[] FromHexString(string hexString) {
            if (hexString == null) throw new ArgumentNullException(nameof(hexString));
            if (hexString.Length % 2 == 1) throw new ArgumentOutOfRangeException(nameof(hexString), hexString, "16進文字列の長さが無効です。");
            Contract.Ensures(Contract.Result<byte[]>() != null);
            Contract.Ensures(Contract.Result<byte[]>().Length == hexString.Length / 2);

            var bin = new byte[hexString.Length / 2];
            for (int i = 0; i < bin.Length; i++) {
                bin[i] = Convert.ToByte(hexString.Substring(i * 2, 2), 16);
            }
            return bin;
        }
    }
}