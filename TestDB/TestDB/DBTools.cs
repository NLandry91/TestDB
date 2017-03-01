using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using System.Windows.Forms;
using System.Diagnostics;
using System.IO;


namespace DBTools
{
    class SQLTools
    {
        private MySqlConnection sqlConnection;
        private string serverAddress;
        private string databaseName;
        private string connectionUserID;
        private string connectionUserPassword;

        //Constructor
        public SQLTools(string serverAddress, string databaseName, string connectionUserID, string connectionUserPassword)
        {
            this.serverAddress = serverAddress;
            this.databaseName = databaseName;
            this.connectionUserID = connectionUserID;
            this.connectionUserPassword = connectionUserPassword;
            Initialize();
        }
        
        //Initialize values
        private void Initialize()
        {
            string connectionString;
            connectionString = "SERVER=" + serverAddress + ";" + "DATABASE=" +
            databaseName + ";" + "UID=" + connectionUserID + ";" + "PASSWORD=" + connectionUserPassword + ";";

            sqlConnection = new MySqlConnection(connectionString);
        }

            //open connection to database
        private bool OpenConnection()
        {
            try
            {
                sqlConnection.Open();
                return true;
            }
            catch (MySqlException ex)
            {
                //When handling errors, you can your application's response based 
                //on the error number.
                //The two most common error numbers when connecting are as follows:
                //0: Cannot connect to server.
                //1045: Invalid user name and/or password.
                switch (ex.Number)
                {
                    case 0:
                        MessageBox.Show("Cannot connect to server.  Contact administrator");
                        break;

                    case 1045:
                        MessageBox.Show("Invalid username/password, please try again");
                        break;
                }
                return false;
            }
        }

            //Close connection
        private bool CloseConnection()
        {
            try
            {
                sqlConnection.Close();
                return true;
            }
            catch (MySqlException ex)
            {
                MessageBox.Show(ex.Message);
                return false;
            }
        }

            //Insert statement
        public void Insert(string tableName, List<string> fieldNames, List<string> fieldValues)
        {
            if (fieldNames.Count > 0 && fieldValues.Count > 0 && fieldNames.Count == fieldValues.Count)
            {
                string fieldNameList = FormatFieldNameInsert(fieldNames);
                string fieldValueList = FormatFieldValueInsert(fieldValues);
                

                string query = "INSERT INTO " + tableName + " (" + fieldNameList + ") VALUES(" + fieldValueList + ")";

                //open connection
                if (this.OpenConnection() == true)
                {
                    //create command and assign the query and connection from the constructor
                    MySqlCommand cmd = new MySqlCommand(query, sqlConnection);

                    cmd.CommandText = query;
                    //Assign the connection using Connection
                    cmd.Connection = sqlConnection;
                    try
                    {
                        //Execute command
                        cmd.ExecuteNonQuery();
                    }
                    catch (MySqlException ex)
                    {
                        MessageBox.Show(ex.Number.ToString());
                    }

                    //close connection
                    this.CloseConnection();
                }
            }
        }

            //Update statement
        public void Update()
        {
            string query = "UPDATE tableinfo SET name='Joe', age='22' WHERE name='John Smith'";

            //Open connection
            if (this.OpenConnection() == true)
            {
                //create mysql command
                MySqlCommand cmd = new MySqlCommand();
                //Assign the query using CommandText
                cmd.CommandText = query;
                //Assign the connection using Connection
                cmd.Connection = sqlConnection;

                //Execute query
                cmd.ExecuteNonQuery();

                //close connection
                this.CloseConnection();
            }
        }

            //Delete statement
        public void Delete()
        {
            string query = "DELETE FROM tableinfo WHERE name='John Smith'";

            if (this.OpenConnection() == true)
            {
                MySqlCommand cmd = new MySqlCommand(query, sqlConnection);
                cmd.ExecuteNonQuery();
                this.CloseConnection();
            }
        }

            //Select statement
        public List<string>[] Select()
        {
            string query = "SELECT * FROM tableinfo";

            //Create a list to store the result
            List<string>[] list = new List<string>[3];
            list[0] = new List<string>();
            list[1] = new List<string>();
            list[2] = new List<string>();

            //Open connection
            if (this.OpenConnection() == true)
            {
                //Create Command
                MySqlCommand cmd = new MySqlCommand(query, sqlConnection);
                //Create a data reader and Execute the command
                MySqlDataReader dataReader = cmd.ExecuteReader();

                //Read the data and store them in the list
                while (dataReader.Read())
                {
                    list[0].Add(dataReader["id"] + "");
                    list[1].Add(dataReader["name"] + "");
                    list[2].Add(dataReader["age"] + "");
                }

                //close Data Reader
                dataReader.Close();

                //close Connection
                this.CloseConnection();

                //return list to be displayed
                return list;
            }
            else
            {
                return list;
            }
        }

            //Count statement
        public int Count()
        {
            string query = "SELECT Count(*) FROM tableinfo";
            int Count = -1;

            //Open Connection
            if (this.OpenConnection() == true)
            {
                //Create Mysql Command
                MySqlCommand cmd = new MySqlCommand(query, sqlConnection);

                //ExecuteScalar will return one value
                Count = int.Parse(cmd.ExecuteScalar() + "");

                //close Connection
                this.CloseConnection();

                return Count;
            }
            else
            {
                return Count;
            }
        }

            //Backup
        public void Backup()
        {
            //try
            //{
            //    DateTime Time = DateTime.Now;
            //    int year = Time.Year;
            //    int month = Time.Month;
            //    int day = Time.Day;
            //    int hour = Time.Hour;
            //    int minute = Time.Minute;
            //    int second = Time.Second;
            //    int millisecond = Time.Millisecond;

            //    //Save file to C:\ with the current date as a filename
            //    string path;
            //    path = "C:\\MySqlBackup" + year + "-" + month + "-" + day +
            //"-" + hour + "-" + minute + "-" + second + "-" + millisecond + ".sql";
            //    StreamWriter file = new StreamWriter(path);


            //    ProcessStartInfo psi = new ProcessStartInfo();
            //    psi.FileName = "mysqldump";
            //    psi.RedirectStandardInput = false;
            //    psi.RedirectStandardOutput = true;
            //    psi.Arguments = string.Format(@"-u{0} -p{1} -h{2} {3}",
            //        uid, password, server, database);
            //    psi.UseShellExecute = false;

            //    Process process = Process.Start(psi);

            //    string output;
            //    output = process.StandardOutput.ReadToEnd();
            //    file.WriteLine(output);
            //    process.WaitForExit();
            //    file.Close();
            //    process.Close();
            //}
            //catch (IOException ex)
            //{
            //    MessageBox.Show("Error , unable to backup!");
            //}
        }

            //Restore
        public void Restore()
        {
        //    try
        //    {
        //        //Read file from C:\
        //        string path;
        //        path = "C:\\MySqlBackup.sql";
        //        StreamReader file = new StreamReader(path);
        //        string input = file.ReadToEnd();
        //        file.Close();

        //        ProcessStartInfo psi = new ProcessStartInfo();
        //        psi.FileName = "mysql";
        //        psi.RedirectStandardInput = true;
        //        psi.RedirectStandardOutput = false;
        //        psi.Arguments = string.Format(@"-u{0} -p{1} -h{2} {3}",
        //            uid, password, server, database);
        //        psi.UseShellExecute = false;


        //        Process process = Process.Start(psi);
        //        process.StandardInput.WriteLine(input);
        //        process.StandardInput.Close();
        //        process.WaitForExit();
        //        process.Close();
        //    }
        //    catch (IOException)
        //    {
        //        MessageBox.Show("Error , unable to Restore!");
        //    }
        }

        private string FormatFieldNameInsert(List<string> fieldNames)
        {
            string fieldNameList = "";
            for (int i = 0; i < fieldNames.Count - 1; i++)
                fieldNameList += fieldNames[i] + ", ";
            
            fieldNameList += fieldNames[fieldNames.Count - 1];

            return fieldNameList;
        }

        private string FormatFieldValueInsert(List<string> fieldValues)
        {
            string fieldValueList = "";
            for (int i = 0; i < fieldValues.Count - 1; i++)
                fieldValueList += "'" + fieldValues[i] + "', ";

            fieldValueList += "'" + fieldValues[fieldValues.Count - 1] + "'";

            return fieldValueList;
        }
    }
}
