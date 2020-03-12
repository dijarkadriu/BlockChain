using BlockChainCore.Helpers;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Security.AccessControl;
using System.Text;

namespace BlockChainCore.Models.BlockChain
{

    public class Blockchain
    {
        public ObservableCollection<Block> Chain { set; get; }

        public Blockchain()
        {
            InitializeChain();
            AddGenesisBlock();
        }
        public Block CreateGenesisBlock()
        {
            return new Block(DateTime.Now, null);
        }

        public void AddGenesisBlock()
        {
            Chain.Add(CreateGenesisBlock());
        }

        public void InitializeChain()
        {
            Chain = new ObservableCollection<Block>();
        }
        public Block GetLatestBlock()
        {
            return Chain[Chain.Count - 1];
        }

        public void AddBlock(Block block)
        {
            Block latestBlock = GetLatestBlock();
            block.Index = latestBlock.Index + 1;
            block.PreviousHash = latestBlock.Hash;
            block.Hash = block.CalculateHash();
            Chain.Add(block);
        }
        public static Blockchain PopulateBlockchain()
        {
            string path = "";
         
            List<string> filesPaths = Directory.GetFiles(GlobalVariables.FolderToWatch).ToList();
            Blockchain files = new Blockchain();
            for (int i = 0; i < filesPaths.Count; i++)
            {
                FileInfo f = new FileInfo(filesPaths[i]);
               
                FileSecurity fS = f.GetAccessControl();
                Block block = new Block(DateTime.Now, "")
                {
                    FileExtension = f.Extension,
                    FileName = f.Name,
                    FullPath = f.FullName,                   
                    LastEdited = System.IO.File.GetLastWriteTime(filesPaths[i]),
                    LastEditedForCheck = System.IO.File.GetLastWriteTime(filesPaths[i]),
                    LastEditedBy = fS.GetOwner(typeof(System.Security.Principal.NTAccount)).ToString()
                };
                path = Functions.CopyFiles(block.FileName,block.FileExtension, block.FullPath);
                block.FullPath = path;
                files.AddBlock(block);


            }
            return files;
        }
    }
}
