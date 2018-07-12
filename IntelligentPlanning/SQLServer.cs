namespace IntelligentPlanning
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.SqlClient;
    using System.Runtime.InteropServices;
    using System.Threading;
    using System.Windows.Forms;

    internal class SQLServer
    {
        public const string cDONo = "-1";
        public const string cDOYes = "1";
        public const string cSQLCode = "Code";
        public const string cSQLConnection1 = "Data Source={0};Initial Catalog={1};User ID={2};password={3}";
        public const string cSQLConnection2 = "Data Source={0};Initial Catalog={1};Integrated Security=SSPI";
        public const string cSQLExpect = "Expect";
        public const string cSQLID = "ID";
        public static SqlConnection SQLConInfo = null;

        public static void CloseSQLConnection()
        {
            try
            {
                if ((SQLConInfo != null) && (SQLConInfo.State != ConnectionState.Closed))
                {
                    SQLConInfo.Close();
                }
            }
            catch (Exception exception)
            {
                CommFunc.PublicMessageAll(exception.ToString(), true, MessageBoxIcon.Asterisk, "");
            }
        }

        public static string CommandExecuteNonQuery(SqlCommand pCmd, bool pIsActual = false)
        {
            string str = "-1";
            OpenSQLConnection();
            try
            {
                int num = Convert.ToInt32(pCmd.ExecuteNonQuery());
                if (num > 0)
                {
                    if (pIsActual)
                    {
                        str = num.ToString();
                    }
                    else
                    {
                        str = "1";
                    }
                }
            }
            catch
            {
            }
            CloseSQLConnection();
            return str;
        }

        public static string CommandExecuteScalar(SqlCommand pCmd, bool pIsActual = false)
        {
            string str = "-1";
            OpenSQLConnection();
            try
            {
                int num = Convert.ToInt32(pCmd.ExecuteScalar());
                if (num > 0)
                {
                    if (pIsActual)
                    {
                        str = num.ToString();
                    }
                    else
                    {
                        str = "1";
                    }
                }
            }
            catch
            {
            }
            CloseSQLConnection();
            return str;
        }

        public static bool DeleteSQLData(string pName)
        {
            bool flag = false;
            try
            {
                SqlCommand pCmd = new SqlCommand($"truncate table [{pName}]", SQLConInfo);
                CommandExecuteNonQuery(pCmd, false);
                flag = true;
            }
            catch
            {
            }
            return flag;
        }

        public static string GetConnectionString(string pSource, string pInitialCatalog, string pUserID, string passWord)
        {
            if ((pUserID == "") && (passWord == ""))
            {
                return $"Data Source={pSource};Initial Catalog={pInitialCatalog};Integrated Security=SSPI";
            }
            return $"Data Source={pSource};Initial Catalog={pInitialCatalog};User ID={pUserID};password={passWord}";
        }

        public static DataTable GetTableSchema()
        {
            DataTable table = new DataTable();
            DataColumn column = new DataColumn("ID");
            DataColumn column2 = new DataColumn("Expect");
            DataColumn column3 = new DataColumn("Code");
            table.Columns.Add(column);
            table.Columns.Add(column2);
            table.Columns.Add(column3);
            return table;
        }

        public static void OpenSQLConnection()
        {
            try
            {
                bool flag;
                if (SQLConInfo == null)
                {
                    return;
                }
                goto Label_0040;
            Label_0011:
                if (SQLConInfo.State == ConnectionState.Closed)
                {
                    SQLConInfo.Open();
                    return;
                }
                Thread.Sleep(0x3e8);
            Label_0040:
                flag = true;
                goto Label_0011;
            }
            catch (Exception exception)
            {
                CommFunc.PublicMessageAll(exception.ToString(), true, MessageBoxIcon.Asterisk, "");
            }
        }

        public static List<string> ReadSQLData(string pName, int pCount)
        {
            List<string> list = new List<string>();
            OpenSQLConnection();
            try
            {
                string cmdText = "";
                if (pCount == -1)
                {
                    cmdText = $"select * from [{pName}] ";
                }
                else
                {
                    cmdText = string.Format("select top {1} * from [{0}] order by [ID] desc", pName, pCount);
                }
                SqlDataReader reader = new SqlCommand(cmdText, SQLConInfo).ExecuteReader();
                while (reader.Read())
                {
                    list.Add(reader["Expect"].ToString() + "\t" + reader["Code"].ToString());
                }
            }
            catch
            {
            }
            CloseSQLConnection();
            return list;
        }

        public static int ReadSQLRowCount(string pName)
        {
            SqlCommand pCmd = new SqlCommand($"select count(*) from [{pName}] ", SQLConInfo);
            return Convert.ToInt32(CommandExecuteScalar(pCmd, true));
        }

        public static bool SqlBulkCopyInsert(List<string> pDataList, string pName)
        {
            bool flag = false;
            try
            {
                DataTable tableSchema = GetTableSchema();
                for (int i = 0; i < pDataList.Count; i++)
                {
                    string[] strArray = pDataList[i].Split(new char[] { '\t' });
                    DataRow row = tableSchema.NewRow();
                    row[1] = strArray[0];
                    row[2] = strArray[1];
                    tableSchema.Rows.Add(row);
                }
                SqlBulkCopy copy = new SqlBulkCopy(SQLConInfo.ConnectionString) {
                    DestinationTableName = pName,
                    BatchSize = tableSchema.Rows.Count
                };
                if ((tableSchema != null) && (tableSchema.Rows.Count != 0))
                {
                    copy.WriteToServer(tableSchema);
                }
                copy.Close();
                flag = true;
            }
            catch
            {
            }
            return flag;
        }

        public static bool SQLConnection(string pConnectionString)
        {
            bool flag = false;
            try
            {
                SQLConInfo = new SqlConnection(pConnectionString);
                flag = true;
            }
            catch (Exception exception)
            {
                CommFunc.PublicMessageAll(exception.ToString(), true, MessageBoxIcon.Asterisk, "");
            }
            return flag;
        }

        public static bool WriteSQLData(List<string> pDataList, string pName)
        {
            bool flag = false;
            try
            {
                foreach (string str in pDataList)
                {
                    string[] strArray = str.Split(new char[] { '\t' });
                    SqlCommand pCmd = new SqlCommand($"insert into [{pName}]({"Expect"},{"Code"}) values('{strArray[0]}','{strArray[1]}')", SQLConInfo);
                    CommandExecuteNonQuery(pCmd, false);
                }
                flag = true;
            }
            catch
            {
            }
            return flag;
        }
    }
}

