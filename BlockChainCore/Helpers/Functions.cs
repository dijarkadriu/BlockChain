using BlockChainCore.Models.BlockChain;
using BlockChainCore.Models.File;
using BlockChainCore.Models.FTP;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;

namespace BlockChainCore.Helpers
{
    public static class Functions
    {
        public static string CopyFiles(string name, string extension, string fullPath)
        {
            FtpClient fTPClient = new FtpClient();
            string fileName = name + extension;
            string path = GlobalVariables.CopiedFilePath + fileName;
            File.Copy(fullPath, path);


            // fTPClient.upload(fileName, path);
            return path;
        }
        public static string ReturnPathOfLastFile(string fileName, string extensions)
        {
            List<string> filesPaths = Directory.GetFiles(GlobalVariables.CopiedFilePath).ToList();
            filesPaths = filesPaths.Where(c => c.Contains(fileName) && c.Contains(extensions) && !c.Contains("~$")).OrderByDescending(c => c).ToList();
            return filesPaths[0];

        }
        public static bool FileEquals(string lastFilePath, string newFilePath)
        {
            byte[] lastFile = File.ReadAllBytes(lastFilePath);
            byte[] newFile = File.ReadAllBytes(newFilePath);
            if (lastFile.Length == newFile.Length)
            {
                for (int i = 0; i < lastFile.Length; i++)
                {
                    if (lastFile[i] != newFile[i])
                    {
                        return false;
                    }
                }
                return true;
            }
            return false;
        }

        public static Task Watch(Blockchain chain)
        {
            string path = "";
            while (true)
            {
                string date = DateTime.Now.ToString().Replace('-', ' ').Replace(':', ' ').Trim();
                List<FileModel> newFiles = FileModel.PopulateFilesList();
                //remove files that are in use 
                newFiles.RemoveAll(f => f.FileName.StartsWith("~$"));
                for (int i = 0; i < newFiles.Count; i++)
                {
                    if (!FileModel.IsFileinUse(new FileInfo(newFiles[i].FullPath)))
                    {
                        if (!chain.Chain.Exists(f => f.FileName == newFiles[i].FileName && f.FileExtension == newFiles[i].FileExtension))
                        {
                            path = CopyFiles(newFiles[i].FileName + date, newFiles[i].FileExtension, newFiles[i].FullPath);
                            Block block = new  Block(DateTime.Now, "")
                            {
                                FileExtension = newFiles[i].FileExtension,
                                FileName = newFiles[i].FileName,
                                FullPath = path,                                
                                LastEdited = newFiles[i].LastEdited,
                                LastEditedBy = newFiles[i].LastEditedBy,
                                LastEditedForCheck = newFiles[i].LastEdited
                            };
                            chain.AddBlock(block);                                                      
                        }
                        else
                        {
                            var block = chain.Chain.SingleOrDefault(f => f.FileName == newFiles[i].FileName && f.FileExtension == newFiles[i].FileExtension);

                            if (block.LastEditedForCheck != newFiles[i].LastEdited)
                            {
                                string lastFilePath = ReturnPathOfLastFile(newFiles[i].FileName, newFiles[i].FileExtension);
                                if (!FileEquals(lastFilePath,newFiles[i].FullPath))
                                {
                                    block.LastEditedForCheck = newFiles[i].LastEdited;

                                    path = CopyFiles(newFiles[i].FileName + date, newFiles[i].FileExtension, newFiles[i].FullPath);
                                    Block blockToAdd = new Block(DateTime.Now, "")
                                    {
                                        FileExtension = newFiles[i].FileExtension,
                                        FileName = newFiles[i].FileName,
                                        FullPath = path,                                       
                                        LastEdited = newFiles[i].LastEdited,
                                        LastEditedBy = newFiles[i].LastEditedBy,
                                        LastEditedForCheck = newFiles[i].LastEdited
                                    };
                                    chain.AddBlock(blockToAdd);
                                }
                            }
                        }
                    }
                }

            }
        }
    }
}