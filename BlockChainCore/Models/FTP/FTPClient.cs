﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;

namespace BlockChainCore.Models.FTP
{

    public class FtpClient
    {
        private string host = "ftp://irvinboy123-001@ftp.site4now.net/site1/";
        private string user = "irvinboy123-001";
        private string pass = "1q2w3ecc";
        private FtpWebRequest ftpRequest = null;
        private FtpWebResponse ftpResponse = null;
        private Stream ftpStream = null;
        private int bufferSize = 2048;

        /// <summary>
        /// Uploads the file to the ftp server
        /// </summary>
        /// <param name="fileName">The name of the file to be uploaded.</param>
        /// <param name="localFile">Path of the file.</param>
        public void upload(string fileName, string localFile)
        {
            try
            {

                ftpRequest = (FtpWebRequest)FtpWebRequest.Create(host + "/" + fileName);

                ftpRequest.Credentials = new NetworkCredential(user, pass);

                ftpRequest.UseBinary = true;
                ftpRequest.UsePassive = true;
                ftpRequest.KeepAlive = true;
                ftpRequest.Method = WebRequestMethods.Ftp.UploadFile;

                using (var fileStream = System.IO.File.OpenRead(localFile))
                {
                    using (var requestStream = ftpRequest.GetRequestStream())
                    {
                        fileStream.CopyTo(requestStream);
                        requestStream.Close();
                    }
                }

                var response = (FtpWebResponse)ftpRequest.GetResponse();

                response.Close();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}