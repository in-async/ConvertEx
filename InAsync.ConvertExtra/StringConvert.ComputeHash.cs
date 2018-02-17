using System;
using System.Diagnostics.Contracts;
using System.Security.Cryptography;
using System.Text;

namespace InAsync {

    public static partial class StringConvert {

        /// <summary>
        /// 文字列の <c>MD5</c> ハッシュ値を算出します。
        /// </summary>
        /// <param name="message">ハッシュ化する文字列。</param>
        /// <param name="encoding">文字列をハッシュ化する際のエンコーディング。<c>null</c> の場合は <see cref="Encoding.UTF8"/> が使用されます。</param>
        /// <returns>ハッシュを表す <c>Byte</c> の配列。</returns>
        /// <exception cref="ArgumentNullException"><paramref name="message"/> が <c>null</c> の場合に投げられます。</exception>
        /// <exception cref="InvalidOperationException"><c>MD5</c> ハッシュアルゴリズムが定義されていない場合に投げられます。</exception>
        public static byte[] MD5(this string message, Encoding encoding = null) {
            if (message == null) throw new ArgumentNullException(nameof(message));
            Contract.Ensures(Contract.Result<byte[]>() != null);

            using (var algorithm = System.Security.Cryptography.MD5.Create()) {
                if (algorithm == null) throw new InvalidOperationException("MD5 hash algorithm is undefined.");

                return ComputeHash(algorithm, message, encoding);
            }
        }

        /// <summary>
        /// 文字列の <c>SHA1</c> ハッシュ値を算出します。
        /// </summary>
        /// <param name="message">ハッシュ化する文字列。</param>
        /// <param name="encoding">文字列をハッシュ化する際のエンコーディング。<c>null</c> の場合は <see cref="Encoding.UTF8"/> が使用されます。</param>
        /// <returns>ハッシュを表す <c>Byte</c> の配列。</returns>
        /// <exception cref="ArgumentNullException"><paramref name="message"/> が <c>null</c> の場合に投げられます。</exception>
        /// <exception cref="InvalidOperationException"><c>SHA1</c> ハッシュアルゴリズムが定義されていない場合に投げられます。</exception>
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
        /// <param name="algorithm">ハッシュ化アルゴリズム。</param>
        /// <param name="message">ハッシュ化する文字列。</param>
        /// <param name="encoding">文字列をハッシュ化する際のエンコーディング。<c>null</c> の場合は <see cref="Encoding.UTF8"/> が使用されます。</param>
        /// <returns>ハッシュを表す <c>Byte</c> の配列。</returns>
        private static byte[] ComputeHash(HashAlgorithm algorithm, string message, Encoding encoding = null) {
            Contract.Requires(algorithm != null);
            Contract.Requires(message != null);
            Contract.Ensures(Contract.Result<byte[]>() != null);

            if (encoding == null) {
                encoding = Encoding.UTF8;
            }

            return algorithm.ComputeHash(encoding.GetBytes(message));
        }
    }
}