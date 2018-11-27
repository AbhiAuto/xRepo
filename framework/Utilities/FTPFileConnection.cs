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
        public static string BAI2File, InvBAI2;
        public static bool fileExistsInFTPflag = false;

        //Uploading the generated file to FTP server
        public static void UploadFileToFtp(string BAI2)
        {
            try
            {
                var fileName = "";
                Console.WriteLine("---Uploading File---");
                //Path where test file stored
                string sourcePath = dirPath + @"\Resources\FTPFileGenerated\";
                //Invalid file to upload
                if (BAI2 != "Invalid")
                {
                    fileName = filename + ".txt";
                }
                //Valid file to upload
                else
                {
                    fileName = filename;
                }

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

       
        public static void ValidateFileInFtp(string BAI2)
        {
            try
            {
                //connect to secure FTP Folder
                DirectoryInfo directory = new DirectoryInfo(FTPPath);
                foreach (var file in directory.GetFiles())
                {
                    //search for a BAI2 file in the Folder
                    if (file.ToString().Contains(BAI2))
                    {
                        InvBAI2 = file.ToString();
                        //Delete BAI2 invalid File
                        file.Delete();
                        //InvBAI2 =InvBAI2.Substring(InvBAI2.Length - 3);
                    }
                    
                }
                //If we get a "loaded" text in the string, that means that file is process
                Assert.IsTrue(!InvBAI2.Contains("loaded"), "BAI2files is processed");

            }
            catch (Exception e)
            {
                Assert.Fail("Unable to conect to FTP folder location: " + e);
            }
        }


        internal static void setftpPath(string ftpPath)
        {
            FTPPath = ftpPath;
        }

        //Set a Valid File
        internal static void setFilename()
        {
            filename = GenerateTxtFile.filenameGen;
            BAI2File = "valid";
            UploadFileToFtp(BAI2File);
        }

        //Set an Invalid File
        internal static void setInvFilename()
        {
            filename = "BAI2_Invalid.pdf";
            BAI2File = "Invalid";
            UploadFileToFtp(BAI2File);
        }

        internal static void verifyInvFilename()
        {
            BAI2File =filename;
            ValidateFileInFtp(BAI2File);
        }

        //Validating the flag when the upload is successful
        internal static void valUpload(string ftpval)
        {
            if(fileExistsInFTPflag==true)
            {
                Console.WriteLine("File has uploaded to the FTP location location successfully");
            }
        }

        //Validating the flag when the upload is successful but not process
        internal static void valInvalidBAI2File(string ftpval)
        {
            if (fileExistsInFTPflag == true)
            {
                //needs to add a validation that the
                DirectoryInfo di = new DirectoryInfo("\\sftp.avidxchange.com\\Avidpaytest\\Integration\\FIFTHTHIRD\\BAI2_AZRFSWDVS02");
                FileInfo[] files = di.GetFiles("*.pdf");
                Console.WriteLine("File has uploaded to the FTP location location successfully but not process");
            }
        }


        // validating the file extention after the fileupload is successful
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
