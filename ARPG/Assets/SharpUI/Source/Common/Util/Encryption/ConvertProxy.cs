using System;

namespace SharpUI.Source.Common.Util.Encryption
{
    public class ConvertProxy : IConvertProxy
    {
        public string ToBase64String(byte[] inArray)
        {
            return Convert.ToBase64String(inArray);
        }
    }
}