using System;
using System.Collections.Generic;
using System.IO;
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
    }
}
