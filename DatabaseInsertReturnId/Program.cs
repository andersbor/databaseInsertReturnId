using System;
using System.Configuration;
using System.Data.SqlClient;
// https://stackoverflow.com/questions/3964305/what-data-type-does-the-sqlcommand-method-executescalar-return
namespace DatabaseInsertReturnId
{
    class Program
    {
        // connectionString is in App.Config
        // Index 1 is used
        // Index 0 is a connection string for an SQLEXPRESS database!
        private static readonly string ConnectionString =
            ConfigurationManager.ConnectionStrings[1].ToString();

        static void Main()
        {
            /*
                        ConnectionStringSettingsCollection con = ConfigurationManager.ConnectionStrings;
                        for (int i = 0;i < con.Count; i++)
                        {
                            ConnectionStringSettings connectionStringSettings = con[i];
                            Console.WriteLine(i + ": " + connectionStringSettings);
                        }*/
            int newId = InsertStudent("Anders", 1);
            Console.WriteLine(newId);
        }

        private static int InsertStudent(string name, int semester)
        {
            using (SqlConnection databaseConnection = new SqlConnection(ConnectionString))
            {
                // https://docs.microsoft.com/en-us/sql/t-sql/functions/ident-current-transact-sql
                const string insertStudent =
                    "insert into student (name, semester) values (@name, @semester); SELECT IDENT_CURRENT('student')";
                databaseConnection.Open();
                using (SqlCommand insertCommand = new SqlCommand(insertStudent, databaseConnection))
                {
                    insertCommand.Parameters.AddWithValue("@name", name);
                    insertCommand.Parameters.AddWithValue("@semester", semester);
                    // ExecuteScalar returns the first column of the first row
                    // Returns an object (in this case we expect it to be an int)
                    // https://msdn.microsoft.com/en-us/library/system.data.sqlclient.sqlcommand.executescalar.aspx
                    object obj = insertCommand.ExecuteScalar();
                    //Console.WriteLine(obj.GetType()); // decimal
                    decimal decimalResult = (decimal)obj;
                    //Console.WriteLine(decimalResult);
                    int theNewId = decimal.ToInt32(decimalResult);
                    //int theNewId = int.Parse(obj.ToString());
                    return theNewId;
                }
            }
        }
    }
}
