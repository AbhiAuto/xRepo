using NUnit.Framework;
using Renci.SshNet;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;

namespace AvidxBDDFramework.Utilities
{
    class ftpFileTransfer
    {
        public static string dirPath = WebUtilities.getDirPath();
        public static string FTPPath;
        public static string filename;
        public static string destFile;
        public static bool fileExistsInFTPflag = false;
        public static void UploadFileToFtp()
        {
            try
            {
                Console.WriteLine("---Uploading File---");
                //Path where test file stored
                string sourcePath = dirPath + @"\Resources\FTPFileGenerated\";
                var fileName = filename + ".txt";

                // Use Path class to manipulate file and directory paths.
                string sourceFile = System.IO.Path.Combine(sourcePath, fileName);
                destFile = System.IO.Path.Combine(FTPPath, fileName);

                //FileStream for holding the file
                FileStream fStream = new FileStream(destFile, FileMode.Create);

                //connect to the server
                FileWebRequest fileRequest = (FileWebRequest)FtpWebRequest.Create(new Uri(sourceFile));

                //set the protocol for the request
                fileRequest.Method = WebRequestMethods.Ftp.DownloadFile;

                string username = WebUtilities.fetchParamValFromConfig("username");
                string password = WebUtilities.fetchParamValFromConfig("password");
                string dePassword = WebUtilities.decryptString(password);

                //provide username and password                
                fileRequest.Credentials = new NetworkCredential(username, dePassword);

                //get the servers response
                WebResponse response = fileRequest.GetResponse();

                //retrieve the response stream
                Stream stream = response.GetResponseStream();

                //create byte buffer
                byte[] buffer = new byte[1024];
                long size = 0;

                //determine how much has been read
                int totalRead = stream.Read(buffer, 0, buffer.Length);

                //loop through the total size of the file
                while (totalRead > 0)
                {
                    size += totalRead;

                    //write to the stream
                    fStream.Write(buffer, 0, totalRead);

                    //get remaining size
                    totalRead = stream.Read(buffer, 0, 1024);
                }

                // Close the streams.
                fStream.Close();
                stream.Close();
                fileExistsInFTPflag = true;

            }
            catch(Exception e)
            {
               Assert.Fail("File is not successfully uploaded into FTP folder location: "+e);
            }
          }

        internal static void setftpPath(string ftpPath)
        {
            FTPPath = ftpPath;
        }
        internal static void setFilename()
        {
            filename = GenerateTxtFile.filenameGen;
            UploadFileToFtp();
        }

        internal static void valUpload(string ftpval)
        {
            if(fileExistsInFTPflag==true)
            {
                Console.WriteLine("File has uploaded to the FTP location location successfully");
            }
        }

        internal static void validateFileExt(string ftpval)
        {
            bool flag = false;
            string[] loadedfiles = Directory.GetFiles(FTPPath, "*.loaded").Select(Path.GetFileName).ToArray();
            for(int i =0;i<loadedfiles.Length;i++)
            {
                if(loadedfiles[i].Contains(filename))
                {
                    Console.WriteLine("File successfully loaded by RabbitMQ Queue");
                    flag = true;
                    break;
                }
            }
            if(!flag)
            {
                Assert.Fail("File not successfully loaded by RabbitMQ Queue");
            }
        }
    }
}
