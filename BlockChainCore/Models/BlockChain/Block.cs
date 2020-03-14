using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace BlockChainCore.Models.BlockChain
{
    public class Block
    {
        public int Index { get; set; }
        public DateTime TimeStamp { get; set; }
        public string PreviousHash { get; set; }
        public string Hash { get; set; }
        private string file;
        public string FileExtension { get; set; }
        public string FileName
        {
            get { return file; }
            set
            {
                if (value.Contains(FileExtension))
                    file = value.Substring(0, value.Length - FileExtension.Length);
                else
                    file = value;
            }
        }
        public string FullPath { get; set; }
        public DateTime LastEdited { get; set; }
        public DateTime LastEditedForCheck { get; set; }
        public DateTime CreatedDate { get; set; }
        public string LastEditedBy { get; set; }

        public Block(DateTime timeStamp, string previousHash)
        {
            Index = 0;
            TimeStamp = timeStamp;
            PreviousHash = previousHash;
            Hash = CalculateHash();
        }

        public string CalculateHash()
        {
            SHA256 sha256 = SHA256.Create();

            byte[] inputBytes = Encoding.ASCII.GetBytes($"{TimeStamp}-{PreviousHash ?? ""}-{FileName}");
            byte[] outputBytes = sha256.ComputeHash(inputBytes);

            return Convert.ToBase64String(outputBytes);
        }
    }
}
