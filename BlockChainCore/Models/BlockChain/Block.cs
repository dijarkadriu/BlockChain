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
        public string Data { get; set; }
        private string file;
        public string FileExtension { get; set; }
        public string FileName
        {
            get { return file; }
            set
            {
                file = value.Substring(0, value.Length - FileExtension.Length);
            }
        }
        public string FullPath { get; set; }
        public DateTime LastEdited { get; set; }
        public string LastEditedBy { get; set; }

        public Block(DateTime timeStamp, string previousHash, string fileName, string fileExtension, string fullPath, DateTime lastEdited, string lastEditedBy)
        {
            Index = 0;
            TimeStamp = timeStamp;
            PreviousHash = previousHash;
            FileName = fileName;
            FileExtension = fileExtension;
            FullPath = fullPath;
            LastEdited = lastEdited;
            LastEditedBy = lastEditedBy;
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
