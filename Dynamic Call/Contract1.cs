using Neo.SmartContract.Framework;
using Neo.SmartContract.Framework.Services.Neo;
using System;
using System.Numerics;

namespace Dynamic_Call
{
    
    public class Contract1 : SmartContract
    {
        delegate object Dyncall(string method, object[] args);

        //0x230cf5ef1e1bd411c7733fa92bb6f9c39714f8f9 µÄÐ¡¶ËÐò
        static byte[] scriptHash = "f9f81497c3f9b62ba93f73c711d41b1eeff50c23".HexToBytes();

        public static object Main(string operation, object[] args)
        {
            if (operation == "name")
            {
                return Contract.Call(scriptHash, "name", new object[0]);
            }
            if (operation == "totalSupply")
            {
                return Contract.Call(scriptHash, "totalSupply", new object[0]);
            }
            return true;
        }
    }
}
