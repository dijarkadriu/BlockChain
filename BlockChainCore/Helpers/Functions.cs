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
        public static void CopyFiles(string name, string extension, string fullPath)
        {
            FtpClient fTPClient = new FtpClient();           
            string fileName = name  + extension;
            string path = GlobalVariables.CopiedFilePath + fileName;
            File.Copy(fullPath, path);

            fTPClient.upload(fileName, path);
        }
        public static List<FileModel> PopulateFilesList()
        {
            List<string> filesPaths = Directory.GetFiles(GlobalVariables.FolderToWatch).ToList();
            List<FileModel> files = new List<FileModel>();
            for (int i = 0; i < filesPaths.Count; i++)
            {
                FileInfo f = new FileInfo(filesPaths[i]);

                FileSecurity fS = f.GetAccessControl();
                files.Add(new FileModel()
                {
                    FileExtension = f.Extension,
                    FileName = f.Name,
                    FullPath = f.FullName,
                    LastEdited = File.GetLastWriteTime(filesPaths[i]),
                    LastEditedBy = fS.GetOwner(typeof(System.Security.Principal.NTAccount)).ToString()
                });
            }
            return files;
        }
        public static Blockchain PopulateBlockchain()
        {
            List<string> filesPaths = Directory.GetFiles(GlobalVariables.FolderToWatch).ToList();
            Blockchain files = new Blockchain();
            for (int i = 0; i < filesPaths.Count; i++)
            {
                FileInfo f = new FileInfo(filesPaths[i]);

                FileSecurity fS = f.GetAccessControl();
                files.AddBlock(new Block(DateTime.Now, "")
                {
                    FileExtension = f.Extension,
                    FileName = f.Name,
                    FullPath = f.FullName,
                    LastEdited = File.GetLastWriteTime(filesPaths[i]),
                    LastEditedBy = fS.GetOwner(typeof(System.Security.Principal.NTAccount)).ToString()
                });
            }
            return files;
        }
        public static Task Watch( Blockchain chain)
        {

            while (true)
            {
                List<FileModel> newFiles = PopulateFilesList();
                //remove files that are in use 
                newFiles.RemoveAll(f => f.FileName.StartsWith("~$"));
                for (int i = 0; i < newFiles.Count; i++)
                {
                    if (!FileModel.IsFileinUse(new FileInfo(newFiles[i].FullPath)))
                    {
                        if (!chain.Chain.Exists(f => f.FileName == newFiles[i].FileName && f.FileExtension == newFiles[i].FileExtension))
                        {
                          
                            chain.AddBlock(new Block(DateTime.Now, "")
                            {
                                FileExtension = newFiles[i].FileExtension + DateTime.Now.ToString().Replace('-', ' ').Replace(':', ' ').Trim(),
                                FileName = newFiles[i].FileName,
                                FullPath = newFiles[i].FullPath,
                                LastEdited = newFiles[i].LastEdited,
                                LastEditedBy = newFiles[i].LastEditedBy
                            });
                            CopyFiles(newFiles[i].FileName, newFiles[i].FileExtension, newFiles[i].FullPath);
                        }
                        else
                        {
                            if ((chain.Chain.SingleOrDefault(f => f.FileName == newFiles[i].FileName && f.FileExtension == newFiles[i].FileExtension)).LastEdited != newFiles[i].LastEdited)
                            {
                                chain.AddBlock(new Block(DateTime.Now, "")
                                {
                                    FileExtension = newFiles[i].FileExtension + DateTime.Now.ToString().Replace('-', ' ').Replace(':', ' ').Trim(),
                                    FileName = newFiles[i].FileName,
                                    FullPath = newFiles[i].FullPath,
                                    LastEdited = newFiles[i].LastEdited,
                                    LastEditedBy = newFiles[i].LastEditedBy
                                });
                                CopyFiles(newFiles[i].FileName, newFiles[i].FileExtension, newFiles[i].FullPath);

                            }
                        }
                    }
                }

            }
        }
    }
}