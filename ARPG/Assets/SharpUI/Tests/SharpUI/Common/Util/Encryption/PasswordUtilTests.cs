using NSubstitute;
using NUnit.Framework;
using SharpUI.Source.Common.Util.Encryption;

namespace SharpUI.Tests.SharpUI.Common.Util.Encryption
{
    public class PasswordUtilTests
    {
        private const string Password = "MyRandomPassword#1234";
        private IConvertProxy _convertProxy;
        private IPasswordUtil _passwordUtil;

        [SetUp]
        public void SetUp()
        {
            _passwordUtil = new PasswordUtil();
        }

        [Test]
        public void PasswordHash_WillHaveCorrectLength()
        {
            var hashedPassword = _passwordUtil.PasswordHash(Password);
            
            Assert.AreEqual(hashedPassword.Length, 344);
        }

        [Test]
        public void IsPasswordValid_WillWorkCorrectly()
        {
            var hashedPassword = _passwordUtil.PasswordHash(Password);

            var isValid = _passwordUtil.IsPasswordValid(Password, hashedPassword);
            
            Assert.IsTrue(isValid);
        }

        [Test]
        public void IsPasswordValid_WillFailCorrectly()
        {
            var hashedPassword = _passwordUtil.PasswordHash(Password);

            var isValid = _passwordUtil.IsPasswordValid("MyRand", hashedPassword);
            
            Assert.IsFalse(isValid);
        }

        [Test]
        public void PasswordHash_WithEmptyHash_WillThrowException()
        {
            _convertProxy = Substitute.For<IConvertProxy>();
            _convertProxy.ToBase64String(Arg.Any<byte[]>()).Returns("");
            _passwordUtil = new PasswordUtil(_convertProxy);

            Assert.Throws<PasswordEncryptException>(() => _passwordUtil.PasswordHash(Password));
        }
    }
}