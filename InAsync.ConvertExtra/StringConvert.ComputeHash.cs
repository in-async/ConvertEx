using System;
using System.Diagnostics.Contracts;
using System.Security.Cryptography;
using System.Text;

namespace InAsync {

    public static partial class StringConvert {

        /// <summary>
        /// 文字列の MD5 ハッシュ値を算出します。
        /// </summary>
        /// <param name="message">ハッシュ化する文字列</param>
        /// <param name="encoding">文字列ををハッシュ化する際のエンコーディング</param>
        /// <returns></returns>
        public static byte[] MD5(this string message, Encoding encoding = null) {
            if (message == null) throw new ArgumentNullException(nameof(message));
            Contract.Ensures(Contract.Result<byte[]>() != null);

            using (var algorithm = System.Security.Cryptography.MD5.Create()) {
                return ComputeHash(algorithm, message, encoding);
            }
        }

        /// <summary>
        /// 文字列の SHA1 ハッシュ値を算出します。
        /// </summary>
        /// <param name="message">ハッシュ化する文字列</param>
        /// <param name="encoding">文字列ををハッシュ化する際のエンコーディング</param>
        /// <returns></returns>
        public static byte[] SHA1(this string message, Encoding encoding = null) {
            if (message == null) throw new ArgumentNullException(nameof(message));
            Contract.Ensures(Contract.Result<byte[]>() != null);

            using (var algorithm = System.Security.Cryptography.SHA1.Create()) {
                if (algorithm == null) throw new InvalidOperationException("SHA1 hash algorithm is undefined.");
                return ComputeHash(algorithm, message, encoding);
            }
        }

        /// <summary>
        /// 文字列のハッシュ値を算出します。
        /// </summary>
        /// <param name="algorithm">ハッシュ化アルゴリズム</param>
        /// <param name="message">ハッシュ化する文字列</param>
        /// <param name="encoding">文字列ををハッシュ化する際のエンコーディング</param>
        /// <returns></returns>
        private static byte[] ComputeHash(HashAlgorithm algorithm, string message, Encoding encoding = null) {
            Contract.Requires(algorithm != null);
            Contract.Requires(message != null);
            Contract.Ensures(Contract.Result<byte[]>() != null);

            encoding = encoding ?? Encoding.UTF8;
            return algorithm.ComputeHash(encoding.GetBytes(message));
        }
    }
}