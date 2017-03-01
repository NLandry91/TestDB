using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;



namespace TestDB
{
    class Program
    {
        static void Main(string[] args)
        {
            string baseUrl = "http://agent.mtconnect.org";

            MTConnectData.RunMTConnectClient test = new MTConnectData.RunMTConnectClient(baseUrl);
            test.Start();
            System.Threading.Thread.Sleep(10000);
            test.Stop();
            Console.ReadKey();
            //Set up Constructor
            //string serverAddress = "192.168.0.115";
            //string databaseName = "MachineData";
            //string connectionUserID = "db1";
            //string connectionUserPassword = "777nwl/MEH";
            //string tableName = "MachineRuntimeData";

            //List<string> fieldNames = new List<string>();
            //// TimeStamp, MachineName, IPAddress
            //fieldNames.Add("TimeStamp");
            //fieldNames.Add("MachineName");
            //fieldNames.Add("IPAddress");
            //fieldNames.Add("Category");
            //fieldNames.Add("ItemName");
            //fieldNames.Add("Value");


            //List<string> fieldValues = new List<string>();

            //DateTime myDateTime = DateTime.Now;
            //string sqlFormattedDate = myDateTime.ToString("yyyy-MM-dd HH:mm:ss.fff");

            //fieldValues.Add(sqlFormattedDate);
            //fieldValues.Add("MK LP 141");
            //fieldValues.Add("10.2.11.186");


            //DBTools connectDB = new DBTools(serverAddress, databaseName, connectionUserID, connectionUserPassword);

            //connectDB.Insert(tableName, fieldNames, fieldValues);

        }
    }

}
