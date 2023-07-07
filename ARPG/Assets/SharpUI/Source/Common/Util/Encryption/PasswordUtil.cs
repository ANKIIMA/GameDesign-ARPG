using System;
using System.Security.Cryptography;

namespace SharpUI.Source.Common.Util.Encryption
{
    /// <summary>
    /// Password encryption utilities.
    /// </summary>
    public class PasswordUtil : IPasswordUtil {
        private const short ByteSize = 128;
        private const int AlgIterations = 1000;

        private readonly IConvertProxy _convertProxy;

        public PasswordUtil()
        {
            _convertProxy = new ConvertProxy();
        }

        public PasswordUtil(IConvertProxy convertProxy)
        {
            _convertProxy = convertProxy;
        }

        /// <summary>
        /// Given the raw password in form of a character sequence, return a hashed password.
        /// </summary>
        /// <param name="password">Raw password</param>
        /// <returns>Hashed password</returns>
        /// <exception cref="PasswordEncryptException">Exception thrown if password is an empty string.</exception>
        public string PasswordHash(string password) {
            string pwdHash;
            var salt = new byte[ByteSize];
            using (var provider = new RNGCryptoServiceProvider()) {
                provider.GetBytes(salt);
                var pbkdf2 = new Rfc2898DeriveBytes(password, salt, AlgIterations);
                var hash = pbkdf2.GetBytes(ByteSize);
                var hashBytes = new byte[ByteSize*2];
                Array.Copy(salt, 0, hashBytes, 0, ByteSize);
                Array.Copy(hash, 0, hashBytes, ByteSize, ByteSize);
                pwdHash = _convertProxy.ToBase64String(hashBytes);
            }

            if (pwdHash.Trim().Length == 0) {
                throw new PasswordEncryptException("ERROR: The PasswordHash() extension is trying to return empty hash value!");
            }

            return pwdHash;
        }

        /// <summary>
        /// Verify if the password matches the hashed password.
        /// </summary>
        /// <param name="password">Raw password</param>
        /// <param name="hashedPassword">Hashed password</param>
        /// <returns>True if password matches the hashed password, otherwise false.</returns>
        public bool IsPasswordValid(string password, string hashedPassword) {
            var valid = true;
            var bytes = Convert.FromBase64String(hashedPassword);
            var salt = new byte[ByteSize];
            Array.Copy(bytes, 0, salt, 0, ByteSize);

            var pbkdf2 = new Rfc2898DeriveBytes(password, salt, AlgIterations);
            var hash = pbkdf2.GetBytes(ByteSize);
            
            for (var i = 0; i < ByteSize; i++) {
                valid &= bytes[i + ByteSize] == hash[i];
            }

            return valid;
        }
    }
}