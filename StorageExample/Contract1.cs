using Neo.SmartContract.Framework;
using Neo.SmartContract.Framework.Services.Neo;
using Neo.SmartContract.Framework.Services.System;
using System;
using System.ComponentModel;
using System.Numerics;

namespace StorageExample
{
    [Features(ContractFeatures.HasStorage)]
    public class Contract1 : SmartContract
    {
        [DisplayName("saved")]
        public static event Action<byte[], uint> Saved;

        /// <summary>
        /// Hash:
        /// Key: hash
        /// Value: block index/height
        /// </summary>
        readonly static StorageMap Hash = Storage.CurrentContext.CreateMap(nameof(Hash));

        public static object Main(string operation, object[] args)
        {
            if (Runtime.Trigger == TriggerType.Application)
            {
                if (operation == "save") return Save((byte[])args[0]);

                if (operation == "getSavedBlock") return GetSavedBlock((byte[])args[0]);

                if (operation == "isSaved") return IsSaved((byte[])args[0]);

                if (operation == "isSavedBefore") return IsSavedBefore((byte[])args[0], (uint)args[1]);
            }
            return true;
        }

        [DisplayName("save")]
        public static bool Save(byte[] sha256)
        {
            if (sha256.Length != 32) return false;
            if (IsSaved(sha256)) return false;

            var blockChainHeight = Blockchain.GetHeight();
            Hash.Put(sha256, blockChainHeight);
            Saved(sha256, blockChainHeight);

            return true;
        }

        [DisplayName("getSavedBlock")]
        public static uint GetSavedBlock(byte[] sha256)
        {
            if (sha256.Length != 32) throw new ArgumentException();
            if (!IsSaved(sha256)) throw new ArgumentException();
            return (uint)Hash.Get(sha256).TryToBigInteger();
        }

        [DisplayName("isSaved")]
        public static bool IsSaved(byte[] sha256)
        {
            if (sha256.Length != 32) return false;
            return Hash.Get(sha256) != null;
        }

        [DisplayName("isSavedBefore")]
        public static bool IsSavedBefore(byte[] sha256, uint blockHeight)
        {
            return GetSavedBlock(sha256) < blockHeight;
        }
    }

    public static class Helper
    {
        public static BigInteger TryToBigInteger(this byte[] value)
        {
            return value?.ToBigInteger() ?? 0;
        }
    }
}
