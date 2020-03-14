using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace BlockChainCore.Helpers
{
    public class GlobalVariables
    {
        public static string CopiedFilePath = @"C:\Users\ddija\Desktop\CopiedFiles\";
        //public static string CopiedFilePath = @"C:\Users\arkad\Desktop\new block\";
        public static string FolderToWatch = "";

        //public static void fixVariables(string path) {
        //    FolderToWatch = path;
        //    fixCopiedFilePath(path);
        //    MakeDir(CopiedFilePath);
        //}

        //private static string fixCopiedFilePath(string path) {
        //    var temp = path.Split('\\');
        //    temp[temp.Length - 1] = temp[temp.Length - 1] + "DirHistory";
        //    CopiedFilePath = "";
        //    foreach (var f in temp)
        //    {
        //        CopiedFilePath += f + "\\";
        //    }

        //    CopiedFilePath = CopiedFilePath.Remove(CopiedFilePath.Length - 1);
        //    return CopiedFilePath;
        //}

        //private static void MakeDir(string path)
        //{
        //    if (!Directory.Exists(path))
        //    {
        //        Directory.CreateDirectory(path);
        //    }
        //}

    }
}
