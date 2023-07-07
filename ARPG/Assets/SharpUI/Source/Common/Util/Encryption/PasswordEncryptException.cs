using System;

namespace SharpUI.Source.Common.Util.Encryption
{
    public class PasswordEncryptException : Exception
    {
        public PasswordEncryptException(string message) : base(message) { }
    }
}