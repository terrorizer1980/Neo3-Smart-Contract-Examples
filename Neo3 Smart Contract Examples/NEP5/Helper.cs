using Neo.SmartContract.Framework;
using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace NEP5
{
    public static class Helper
    {
        public static BigInteger TryToBigInteger(this byte[] value)
        {
            return value?.ToBigInteger() ?? 0;
        }
    }
}
