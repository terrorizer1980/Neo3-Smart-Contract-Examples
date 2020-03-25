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
        [DisplayName("documentSaved")]
        public static event Action<byte[], uint> DocumentSaved;

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
            DocumentSaved(sha256, blockChainHeight);

            return true;
        }

        [DisplayName("bulkIssue")]
        public static bool BulkIssue(byte[][] documents)
        {
            foreach (var document in documents)
            {
                if (document.Length != 32) return false;
                if (IsSaved(document)) return false;
            }

            var blockChainHeight = Blockchain.GetHeight();
            foreach (var document in documents)
            {
                Hash.Put(document, blockChainHeight);
                DocumentSaved(document, blockChainHeight);
            }
            return true;
        }

        [DisplayName("getSavedBlock")]
        public static uint GetSavedBlock(byte[] document)
        {
            if (document.Length != 32) throw new ArgumentException();
            if (!IsSaved(document)) throw new ArgumentException();
            return (uint)Hash.Get(document).AsBigInteger();
        }

        [DisplayName("isSaved")]
        public static bool IsSaved(byte[] document)
        {
            if (document.Length != 32) return false;
            return Hash.Get(document) != null;
        }

        [DisplayName("isSavedBefore")]
        public static bool IsSavedBefore(byte[] document, uint blockHeight)
        {
            return GetSavedBlock(document) < blockHeight;
        }
    }
}
