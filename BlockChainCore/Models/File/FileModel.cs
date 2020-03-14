using BlockChainCore.Helpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.AccessControl;
using System.Text;

namespace BlockChainCore.Models.File
{
    public class FileModel
    {
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
        public DateTime LastEditedForCheck { get; set; }
        public DateTime CreatedDate { get; set; }
        public string LastEditedBy { get; set; }
        public bool hasThread { get; set; }


        /// <summary>
        /// Determinates if a file is beign used.
        /// </summary>
        /// <param name="file">The file to check.</param>
        /// <returns>Returns true if its beign used, false if not.</returns>
        public bool IsFileinUse(FileInfo file)
        {
            FileStream stream = null;

            try
            {
                stream = file.Open(FileMode.Open, FileAccess.ReadWrite, FileShare.None);
            }
            catch (IOException)
            {
                //the file is unavailable because it is:
                //still being written to
                //or being processed by another thread
                //or does not exist (has already been processed)
                return true;
            }
            finally
            {
                if (stream != null)
                    stream.Close();
            }
            return false;
        }
        /// <summary>
        /// Populates the list with the files to check.
        /// </summary>
        /// <returns>Return the list</returns>
        public List<FileModel> PopulateFilesList()
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
                    CreatedDate = f.CreationTime,
                    LastEdited = System.IO.File.GetLastWriteTime(filesPaths[i]),
                    LastEditedForCheck = System.IO.File.GetLastWriteTime(filesPaths[i]),
                    LastEditedBy = fS.GetOwner(typeof(System.Security.Principal.NTAccount)).ToString(),
                    hasThread = false
                });
            }
            return files;
        }

    }
}
