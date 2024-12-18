using Microsoft.Data.SqlClient;
using System.Data;




string server = "SERVERNAME"; //your server
string user = "USERNAME"; //user name for DB connection
string password = "PASSWORD"; //password for DB connection

string timeout = "0"; //timout of the connection in seconds, 0 = none
string tableName = string.Empty; //change to "and table_name = 'YOUR_TABLE'" for a specific table, empty string will query all tables
int maxQueries = 100; //number of queries per table
string rowsSelection = "1000000"; //how many rows selected
string queryAllTables = "SELECT t.name AS TableName from sys.tables t INNER JOIN sys.partitions p ON t.object_id = p.object_id WHERE t.name NOT LIKE 'dt%' and rows > 0 ";
string queryLargeTables = "";
string queryStatement = queryAllTables + tableName;
string sqlConnectionString = "Data Source="+ server + ";Initial Catalog=DbArchive; User ID="+user+"; Password="+password+"; TrustServerCertificate=True; Connection Timeout="+timeout; 
//if you're using azure vaults change this string to string from your vault


DataTable databaseTables = new DataTable("tables");

List<int> queriesList = new List<int>();
for (int i = 0; i < maxQueries; i++)
{
    queriesList.Add(i);
}

SqlConnection conn = new SqlConnection(sqlConnectionString);
using (conn)
{


    using (SqlCommand _cmd = new SqlCommand(queryStatement, conn))
    {


        SqlDataAdapter _dap = new SqlDataAdapter(_cmd);

        conn.Open();
        _dap.Fill(databaseTables);
        conn.Close();

    }
}
foreach (DataRow dataTable in databaseTables.Rows)
{
    //Parallel table querying
    Parallel.ForEach(dataTable.ItemArray, item =>
    {
        //Parallel queries run on the table
        Parallel.ForEach(queriesList, queryNum =>
        {
            DataTable databaseRows = new DataTable("rows");

            SqlConnection conn2 = new SqlConnection(sqlConnectionString);
            using (conn2)
            {
                string queryStatement = "SELECT top " + rowsSelection + " * FROM " + item;

                using (SqlCommand _cmd = new SqlCommand(queryStatement, conn2))
                {


                    SqlDataAdapter _dap2 = new SqlDataAdapter(_cmd);

                    conn2.Open();
                    _dap2.Fill(databaseRows);
                    Console.WriteLine("query run for " + item.ToString() + " no.: "+ queryNum.ToString());
                    conn2.Close();

                }
            }
            


        });


    });



}



