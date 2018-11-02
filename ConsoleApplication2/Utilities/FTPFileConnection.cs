using NUnit.Framework;
using System;

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
                //Path where test file stored
                string sourcePath = dirPath + @"\Resources\FTPFile\";
                var fileName = filename + ".txt";

                // Use Path class to manipulate file and directory paths.
                string sourceFile = System.IO.Path.Combine(sourcePath, fileName);
                destFile = System.IO.Path.Combine(FTPPath, fileName);

                // To copy a folder's contents to a new location:
                // Create a new target folder, if necessary.
                if (!System.IO.Directory.Exists(FTPPath))
                {
                    Assert.Fail("Invalid FTP folder location : " + FTPPath);
                }

                // To copy a file to another location and 
                // overwrite the destination file if it already exists.
                System.IO.File.Copy(sourceFile, destFile, true);
                if (System.IO.File.Exists(destFile))
                {
                    fileExistsInFTPflag = true;
                }
                else
                {
                    Assert.Fail("File upload to FTP location is not successful");
                }
            }catch(Exception e)
            {
               Assert.Fail("File is not successfully uploaded into FTP folder location: "+e);
            }
          }

        internal static void setftpPath(string ftpPath)
        {
            FTPPath = ftpPath;
        }
        internal static void setFilename(string ftpval)
        {
            filename = ftpval;
            UploadFileToFtp();
        }

        internal static void valUpload(string ftpval)
        {
            if(fileExistsInFTPflag==true)
            {
                Console.WriteLine("File has uploaded to the FTP location location successfully");
            }
        }
    }
}
