﻿using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AvidxBDDFramework.Utilities
{
    //Generating text file using the data from the backend
    class GenerateTxtFile
    {
        public static string dirPath = WebUtilities.getDirPath();
        public static string sqlQuery;
        public static string customerName;
        public static string checkNo;
        public static string netInvoice;
        public static bool flag = false;
        public static string filenameGen, genFilePath;
        public static string paymentID;
        internal static void generateBAI2file()
        {
            flag = false;
            string[] textLines = System.IO.File.ReadAllLines(dirPath + @"\Resources\PaymentID.txt");
            int i = textLines.Count();
            for (int j = 0; j <= i; j++)
            {
                sqlQuery = "SELECT ppi.PaymentProcessingStatusTypeID from AvidPayTransaction.trn.tPayment p"
                        + " inner join AvidPayTransaction.trn.tPaymentProcessingItem ppi on p.PaymentID = ppi.PaymentID"
                        + " inner join AvidPayTransaction.pfl.tUser u on u.UserID = p.UserID"
                        + " inner join AvidPayTransaction.pfl.tOrganization o on o.OrganizationID = u.OrganizationID"
                        + " where p.PaymentID in (" + textLines[j] + ");";
                string val = DBConnection.sqlConnect(sqlQuery);
                if (val.Equals("14"))
                {
                    sqlQuery = "SELECT o.Name from AvidPayTransaction.trn.tPayment p"
                        + " inner join AvidPayTransaction.trn.tPaymentProcessingItem ppi on p.PaymentID = ppi.PaymentID"
                        + " inner join AvidPayTransaction.pfl.tUser u on u.UserID = p.UserID"
                        + " inner join AvidPayTransaction.pfl.tOrganization o on o.OrganizationID = u.OrganizationID"
                        + " where p.PaymentID in (" + textLines[j] + " );";
                    customerName = DBConnection.sqlConnect(sqlQuery);

                    sqlQuery = "SELECT p.ONSTnumber from AvidPayTransaction.trn.tPayment p"
                        + " inner join AvidPayTransaction.trn.tPaymentProcessingItem ppi on p.PaymentID = ppi.PaymentID"
                        + " inner join AvidPayTransaction.pfl.tUser u on u.UserID = p.UserID"
                        + " inner join AvidPayTransaction.pfl.tOrganization o on o.OrganizationID = u.OrganizationID"
                        + " where p.PaymentID in (" + textLines[j] + ");";
                    checkNo = DBConnection.sqlConnect(sqlQuery);

                    sqlQuery = "SELECT SUM(InvoiceNet) FROM [AvidPayTransaction].[trn].tPaymentDetail where PaymentID in (" + textLines[j] + ");";
                    netInvoice = DBConnection.sqlConnect(sqlQuery);
                    netInvoice = netInvoice.Remove(netInvoice.Length - 2);
                    string[] splitStr = netInvoice.Split('.');
                    netInvoice = splitStr[0] + splitStr[1];
                    flag = true;
                    paymentID = textLines[j];
                }
                if (flag)
                {
                    break;
                }
            }
            if (!flag)
            {
                Assert.Fail("Payment Processing Status for all the provided payment ID is 14, no file is generated");
            }
            string repoTime = DateTime.Now.ToString("yyyyMMdd_HHmmss");
            filenameGen = "BAI2_Avid_AutoGen_" + repoTime;
            string strText = "";
            if (System.IO.File.Exists(@"E:\genrocket-3.5.16\output\data1\Test1.txt"))
            {
                //Reading file generated by genrocket
                strText = System.IO.File.ReadAllText(@"E:\genrocket-3.5.16\output\data1\Test1.txt");
            }
            else
            {
                Assert.Fail(@"Genrocket generated BAI2 file is not existing in the location : E:\genrocket-3.5.16\output\data1");
            }


            ////Reading file generated by framework template
            //string strText = System.IO.File.ReadAllText(dirPath + @"\Resources\FTPFileTemplate\BAI2_Avid_Template.txt");
            //strText = strText.Replace("invoicenet", netInvoice);
            //strText = strText.Replace("checkno", checkNo);

            genFilePath = dirPath + @"\Resources\FTPFileGenerated\" + filenameGen + ".txt";
            if (!System.IO.File.Exists(genFilePath))
            {
                System.IO.File.WriteAllLines(genFilePath, new string[0]);
            }

            System.IO.File.WriteAllText(genFilePath, strText);
        }

        //Fetching value from the DB for the payment id and storing the values into variables
        internal static string Fetchdetailsfromdb()
        {
            flag = false;
            string[] textLines = System.IO.File.ReadAllLines(dirPath + @"\Resources\PaymentID.txt");
            int i = textLines.Count();
            for (int j = 0; j <= i; j++)
            {
                sqlQuery = "SELECT o.Name from AvidPayTransaction.trn.tPayment p"
                        + " inner join AvidPayTransaction.trn.tPaymentProcessingItem ppi on p.PaymentID = ppi.PaymentID"
                        + " inner join AvidPayTransaction.pfl.tUser u on u.UserID = p.UserID"
                        + " inner join AvidPayTransaction.pfl.tOrganization o on o.OrganizationID = u.OrganizationID"
                        + " where p.PaymentID in (" + textLines[j] + " );";
                customerName = DBConnection.sqlConnect(sqlQuery);

                sqlQuery = "SELECT ppi.PaymentProcessingStatusTypeID from AvidPayTransaction.trn.tPayment p"
                        + " inner join AvidPayTransaction.trn.tPaymentProcessingItem ppi on p.PaymentID = ppi.PaymentID"
                        + " inner join AvidPayTransaction.pfl.tUser u on u.UserID = p.UserID"
                        + " inner join AvidPayTransaction.pfl.tOrganization o on o.OrganizationID = u.OrganizationID"
                        + " where p.PaymentID in (" + textLines[j] + ");";
                string val = DBConnection.sqlConnect(sqlQuery);
                if (val.Equals("14"))
                {
                    sqlQuery = "SELECT p.ONSTnumber from AvidPayTransaction.trn.tPayment p"
                        + " inner join AvidPayTransaction.trn.tPaymentProcessingItem ppi on p.PaymentID = ppi.PaymentID"
                        + " inner join AvidPayTransaction.pfl.tUser u on u.UserID = p.UserID"
                        + " inner join AvidPayTransaction.pfl.tOrganization o on o.OrganizationID = u.OrganizationID"
                        + " where p.PaymentID in (" + textLines[j] + ");";
                    checkNo = DBConnection.sqlConnect(sqlQuery);

                    sqlQuery = "SELECT SUM(InvoiceNet) FROM [AvidPayTransaction].[trn].tPaymentDetail where PaymentID in (" + textLines[j] + ");";
                    netInvoice = DBConnection.sqlConnect(sqlQuery);
                    netInvoice = netInvoice.Remove(netInvoice.Length - 2);
                    flag = true;
                    paymentID = textLines[j];
                }
                if (flag)
                {
                    break;
                }
            }
            if (!flag)
            {
                Assert.Fail("Payment Processing Status for all the provided payment ID is 14");
            }
            string strReq = "{\"" + checkNo + "\":" + netInvoice + "}";
            return strReq;
        }

    }
}
