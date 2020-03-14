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
    public class Functions
    {
        /// <summary>
        /// Copy the files to the specifed url.
        /// </summary>
        /// <param name="name">Name of the file to copy</param>
        /// <param name="extension">Extension of the file</param>
        /// <param name="fullPath">Actual path of the file</param>
        /// <returns></returns>
        public string CopyFiles(string name, string extension, string fullPath)
        {
            string path = CalculatePath(name, extension);
            File.Copy(fullPath, path);

            //send to archive
            TransferFilesViaFTP();

            return path;
        }
        /// <summary>
        /// Transfer files to the ftp server.
        /// </summary>
        private void TransferFilesViaFTP()
        {
            FtpClient fTPClient = new FtpClient();
            // fTPClient.upload(fileName, path);
        }
        /// <summary>
        /// Returns the path were the files should be copied. 
        /// </summary>
        /// <param name="name">Name of the file</param>
        /// <param name="extension">Extension of the file</param>
        /// <returns></returns>
        private string CalculatePath(string name, string extension)
        {
            return GlobalVariables.CopiedFilePath + name + extension;
        }
        /// <summary>
        ///  Return the path of the last file that was edited.
        /// </summary>
        /// <param name="name">Name of the file</param>
        /// <param name="extension">Extension of the file</param>
        /// <returns>Returns the path</returns>
        private string ReturnPathOfLastFile(string fileName, string extensions)
        {
            List<string> filesPaths = Directory.GetFiles(GlobalVariables.CopiedFilePath).ToList();
            string file = filesPaths.Where(c => c.Contains(fileName) && c.Contains(extensions) && !c.Contains("~$")).OrderByDescending(c => c).First();
            return file;
        }

        /// <summary>
        ///   Check the bytes of the files if they are equal.
        /// </summary>
        /// <param name="lastFilePath">The file of the previous version.</param>
        /// <param name="newFilePath">The file of actual version</param>
        /// <returns>Returns true if they are equal, false if they are not.</returns>
        private bool FileEquals(string lastFilePath, string newFilePath)
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
        /// <summary>
        /// Watches over a folder if there are any changes made.
        /// </summary>
        /// <param name="chain">The blockchain where to add the changes.</param>
        /// <returns></returns>
        public async Task Watch(Blockchain chain)
        {
            string path = "";
            FileModel fm;
            while (true)
            {
                fm = new FileModel();
                string date = DateTime.Now.ToString().Replace(':', '-').Trim();

                List<FileModel> newFiles = fm.PopulateFilesList();
                //remove files that are in use 

                newFiles.RemoveAll(f => f.FileName.StartsWith("~$"));
                for (int i = 0; i < newFiles.Count; i++)
                {
                    if (!fm.IsFileinUse(new FileInfo(newFiles[i].FullPath)))
                    {
                        if (!chain.Chain.Any(f => f.FileName == newFiles[i].FileName && f.FileExtension == newFiles[i].FileExtension))
                        {
                            path = CopyFiles(newFiles[i].FileName + date, newFiles[i].FileExtension, newFiles[i].FullPath);
                            Block block = new Block(DateTime.Now, "")
                            {
                                FileExtension = newFiles[i].FileExtension,
                                FileName = newFiles[i].FileName,
                                FullPath = path,
                                LastEdited = newFiles[i].LastEdited,
                                LastEditedBy = newFiles[i].LastEditedBy,
                                LastEditedForCheck = newFiles[i].LastEdited,
                                FileNameForList = newFiles[i].FileName + date
                            };
                            System.Windows.Application.Current.Dispatcher.Invoke((System.Action)delegate
                            {
                                chain.AddBlock(block);
                            });
                        }
                        else
                        {
                            var block = chain.Chain.SingleOrDefault(f => f.FileName == newFiles[i].FileName && f.FileExtension == newFiles[i].FileExtension);

                            if (block.LastEditedForCheck != newFiles[i].LastEdited)
                            {
                                string lastFilePath = ReturnPathOfLastFile(newFiles[i].FileName, newFiles[i].FileExtension);
                                if (!FileEquals(lastFilePath, newFiles[i].FullPath))
                                {
                                    block.LastEditedForCheck = newFiles[i].LastEdited;

                                    path = CopyFiles(newFiles[i].FileName + date, newFiles[i].FileExtension, newFiles[i].FullPath);
                                    Block blockToAdd = new Block(DateTime.Now, "")
                                    {
                                        FileExtension = newFiles[i].FileExtension,
                                        FileName = newFiles[i].FileName + date,
                                        FullPath = path,
                                        LastEdited = newFiles[i].LastEdited,
                                        LastEditedBy = newFiles[i].LastEditedBy,
                                        LastEditedForCheck = newFiles[i].LastEdited,
                                        FileNameForList = newFiles[i].FileName + date
                                    };
                                    System.Windows.Application.Current.Dispatcher.Invoke((System.Action)delegate
                                    {
                                        chain.AddBlock(blockToAdd);
                                    });
                                    
                                }
                            }
                        }
                    }
                }
            }
        }
    }
}