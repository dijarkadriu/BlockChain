﻿using BlockChainCore.Helpers;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Threading;

namespace BlockChainCore.Models.BlockChain
{

    public class Blockchain
    {
        public ObservableCollection<Block> Chain { set; get; }
        private static List<string> currentFiles { get; set; }

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
        //public static Blockchain PopulateBlockchain()
        //{
        //    string path = "";

        //    List<string> filesPaths = Directory.GetFiles(GlobalVariables.FolderToWatch).ToList();
        //    Blockchain files = new Blockchain();
        //    for (int i = 0; i < filesPaths.Count; i++)
        //    {
        //        FileInfo f = new FileInfo(filesPaths[i]);

        //        FileSecurity fS = f.GetAccessControl();
        //        Block block = new Block(DateTime.Now, "")
        //        {
        //            FileExtension = f.Extension,
        //            FileName = f.Name,
        //            FullPath = f.FullName,                   
        //            LastEdited = System.IO.File.GetLastWriteTime(filesPaths[i]),
        //            LastEditedForCheck = System.IO.File.GetLastWriteTime(filesPaths[i]),
        //            LastEditedBy = fS.GetOwner(typeof(System.Security.Principal.NTAccount)).ToString()
        //        };
        //        path = Functions.CopyFiles(block.FileName,block.FileExtension, block.FullPath);
        //        block.FullPath = path;
        //        files.AddBlock(block);


        //    }
        //    return files;
        //}

        public static Blockchain PopulateBlockchain()
        {

            //make main thread
            Thread mainThread=new Thread(new ThreadStart(MainThread_CreateChainCore));

            mainThread.Start();
            

            string path = "";

            List<string> filesPaths = Directory.GetFiles(GlobalVariables.FolderToWatch).ToList();
            Blockchain files = new Blockchain();
            //for (int i = 0; i < filesPaths.Count; i++)
            //{
            //    FileInfo f = new FileInfo(filesPaths[i]);

            //    FileSecurity fS = f.GetAccessControl();
            //    Block block = new Block(DateTime.Now, "")
            //    {
            //        FileExtension = f.Extension,
            //        FileName = f.Name,
            //        FullPath = f.FullName,
            //        LastEdited = System.IO.File.GetLastWriteTime(filesPaths[i]),
            //        LastEditedForCheck = System.IO.File.GetLastWriteTime(filesPaths[i]),
            //        LastEditedBy = fS.GetOwner(typeof(System.Security.Principal.NTAccount)).ToString()
            //    };
            //    path = Functions.CopyFiles(block.FileName, block.FileExtension, block.FullPath);
            //    block.FullPath = path;
            //    files.AddBlock(block);


            //}
            return files;
        }

        /// <summary>
        /// every file in the folder is created as a unique folder to save its history
        /// </summary>
        private static void MainThread_CreateChainCore() {
            //create folders to copy files
            List<string> filesPaths = Directory.GetFiles(GlobalVariables.FolderToWatch).ToList();
            currentFiles = filesPaths;
            foreach (string filePath in filesPaths) 
            {
                GlobalVariables.MakeDirFromFile(filePath);
                createWatchingThread(filePath);
            }

            Thread.Sleep(1000);
            checkForNewFiles();
        }

        private static void createWatchingThread(string filePath)
        {
            string fileToWatch = GlobalVariables.FolderToWatch + "\\" + filePath;
            runThreadWatcherForFile(fileToWatch);
        }

        private static void runThreadWatcherForFile(string fileToWatch)
        {
            new Thread(new ThreadStart(watchFile));//qitu ke met
            throw new NotImplementedException();
        }

        private static void watchFile() {
            //continue
        }

        /// <summary>
        /// check for added files
        /// </summary>
        private static void checkForNewFiles() {
            bool changedState = false;
            while (!changedState) {
                List<string> filesPaths = Directory.GetFiles(GlobalVariables.FolderToWatch).ToList();
                foreach (string fileName in filesPaths) {
                    if (!currentFiles.Contains(fileName)) {
                        changedState = true;
                    }
                }
                Thread.Sleep(1000);
            }

            MainThread_CreateChainCore();
        } 
    }
}
