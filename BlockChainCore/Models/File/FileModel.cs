using System;
using System.Collections.Generic;
using System.IO;
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
        public string LastEditedBy { get; set; }

        public long Data { get; set; }

       public static bool IsFileinUse(FileInfo file)
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

    }
}
