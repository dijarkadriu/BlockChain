using BlockChainCore.Models.File;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.AccessControl;
using System.Text;

namespace BlockChainCore.Helpers
{
    public static class Functions
    {
        public static void CopyFiles(string name, string extension, string fullPath)
        {
            string date = DateTime.Now.ToString().Replace('-', ' ').Replace(':', ' ').Trim();
            string path = GlobalVariables.CopiedFilePath + name + date + extension;
            File.Copy(fullPath, path);
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
    }
}
