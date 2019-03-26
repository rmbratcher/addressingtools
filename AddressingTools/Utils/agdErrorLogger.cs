using System;
using System.Collections.Generic;

namespace AddressingTools.Utils
{
    public class agdErrorLogger
    {

        //private string serverurl;
        //private MongoServer mServer;
        //private MongoDatabase ErrorLogs;
        //private MongoCollection Logs;
        //private bool ConnectionTried = false;


        /// <summary>
        /// The name or address of the server ex.. example.com or 120.120.2.200
        /// </summary>
        /// <param name="ServerURL"></param>
        public agdErrorLogger(string ServerURL)
        {
            //serverurl = ServerURL;
            //ConnectToServer();
        }

        //private void ConnectToServer()
        //{
        //    try
        //    {

        //        string connectionString = String.Format("mongodb://{0}", serverurl);
        //        //MongoCredentials mCredentials = new MongoCredentials("mark", "Bogue!23");
        //        mServer = MongoServer.Create(connectionString);
        //        mServer.Connect(new TimeSpan(0, 0, 5));

        //        ErrorLogs = mServer.GetDatabase("ErrorLogs");//, mCredentials);
        //        Logs = ErrorLogs.GetCollection("Logs");
        //    }
        //    catch(Exception ex)
        //    {
        //        System.Diagnostics.EventLog.WriteEntry("agdErrorLogger", ex.StackTrace, System.Diagnostics.EventLogEntryType.Error);
        //    }
        //}


        //public void WriteError(Exception ex, string Product, string UserName,Dictionary<string, string> AdditionalItems)
        //{
        //    if (mServer.State == MongoServerState.Connected)
        //    {

        //        try
        //        {
        //            BsonDocument doc = new BsonDocument();

        //            doc.Add("ErrorMessage", ex.Message);
        //            doc.Add("StackTrace", ex.StackTrace);
        //            doc.Add("Product", Product);
        //            doc.Add("UserName", UserName);
        //            doc.Add("ClassName", ex.Source);
        //            doc.Add("TimeStamp", new BsonDateTime(DateTime.UtcNow));

        //            if (AdditionalItems != null)
        //            {
        //                if (AdditionalItems.Count > 0)
        //                {
        //                    doc.Add(AdditionalItems);
        //                }
        //            }

        //            Logs.Insert(doc);
        //        }
        //        catch (Exception ex2)
        //        {
        //            if (!ConnectionTried)
        //            {
        //                ConnectionTried = true;
        //                ConnectToServer();
        //            }
        //            else
        //            {
        //                try
        //                {
        //                    mServer.Disconnect();
        //                }
        //                catch{}
        //            }
        //        }
        //    }

        //    else
        //    {
        //        System.Diagnostics.EventLog.WriteEntry(Product, ex.StackTrace, System.Diagnostics.EventLogEntryType.Error);

        //        if (!ConnectionTried)
        //        {
        //            ConnectionTried = true;
        //            ConnectToServer();
        //        }
        //    }
        //}


        public void WriteError(Exception ex, string Product, string UserName, Dictionary<string, string> AdditionalItems)
        {
            System.Diagnostics.EventLog.WriteEntry(Product, ex.StackTrace, System.Diagnostics.EventLogEntryType.Error);
        }
    }
}
