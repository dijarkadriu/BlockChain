using BlockChainCore.Helpers;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

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
        /// <summary>
        /// Creates the first block of the blockchain.
        /// </summary>
        /// <returns>Returns the Genesis block</returns>
        private Block CreateGenesisBlock()
        {
            return new Block(DateTime.Now, null);
        }
        /// <summary>
        /// Addes the Genesis block to the blockchain
        /// </summary>
        private void AddGenesisBlock()
        {
            Chain.Add(CreateGenesisBlock());
        }
        /// <summary>
        /// Initializes the chain.
        /// </summary>
        private void InitializeChain()
        {
            Chain = new ObservableCollection<Block>();
          
        }
        /// <summary>
        /// Returns the last block of the chain.
        /// </summary>
        /// <returns></returns>
        private Block GetLatestBlock()
        {
            return Chain[Chain.Count - 1];
        }
        /// <summary>
        /// Addes a block to the blockchain
        /// </summary>
        /// <param name="block">Block to be added</param>
        public void AddBlock(Block block)
        {
            Block latestBlock = GetLatestBlock();
            block.Index = latestBlock.Index + 1;
            block.PreviousHash = latestBlock.Hash;
            block.Hash = block.CalculateHash();
            Chain.Add(block);
        }
        /// <summary>
        /// Populates the block with the initial files.
        /// </summary>
        /// <returns>Return the blockchain</returns>
        public static Blockchain PopulateBlockchain()
        {
            string path;
            string date = DateTime.Now.ToString().Replace(':', '-').Trim();
            List<string> filesPaths = Directory.GetFiles(GlobalVariables.FolderToWatch).ToList();
            filesPaths.RemoveAll(f => f.Contains("~$"));
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
                    CreatedDate = f.CreationTime,
                    LastEdited = System.IO.File.GetLastWriteTime(filesPaths[i]),
                    LastEditedForCheck = System.IO.File.GetLastWriteTime(filesPaths[i]),
                    LastEditedBy = fS.GetOwner(typeof(System.Security.Principal.NTAccount)).ToString(),
                    FileNameForList = f.Name
                };
                path = new Functions().CopyFiles(block.FileName + date, block.FileExtension, block.FullPath);
                block.FullPath = path;
                files.AddBlock(block);


            }
            return files;
        }
    }
}
