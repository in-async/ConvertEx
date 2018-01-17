using System;
using System.Diagnostics.Contracts;
using System.Globalization;

namespace InAsync {

    public static partial class ConvertEx {

        #region Hex String

        /// <summary>
        /// byte 配列を 16 進文字列に変換します。
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static string ToHexString(this byte[] data, StringCase stringCase = StringCase.Lower) {
            if (data == null) throw new ArgumentNullException(nameof(data));
            Contract.Ensures(Contract.Result<string>() != null);
            Contract.Ensures(Contract.Result<String>().Length == data.Length * 2);

            string format = stringCase == StringCase.Lower ? "x2" : "X2";
            var chars = new Char[data.Length * 2];
            for (int i = 0, ci = 0; i < data.Length; i++, ci += 2) {
                var str = data[i].ToString(format, CultureInfo.InvariantCulture.NumberFormat);
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

        public enum StringCase {
            Lower = 0,
            Upper = 1,
        }

        #endregion Hex String
    }
}