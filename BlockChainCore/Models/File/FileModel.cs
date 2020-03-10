using System;
using System.Collections.Generic;
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
        public string LastEditedBy { get; set; }

    }
}
