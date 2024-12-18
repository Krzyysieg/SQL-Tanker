# StressTestSQL

# Update your connection:


``` string server = "SERVERNAME"; //your server
string user = "USERNAME"; //user name for DB connection
string password = "PASSWORD"; //password for DB connection
```
# Set this stuff to your liking
``` string timeout = "0"; //timout of the connection in seconds, 0 = none
string tableName = string.Empty; //change to "and table_name = 'YOUR_TABLE'" for a specific table, empty string will query all tables
int maxQueries = 100; //number of queries per table
string rowsSelection = "1000000"; //how many rows selected
string queryAllTables = "SELECT t.name AS TableName from sys.tables t INNER JOIN sys.partitions p ON t.object_id = p.object_id WHERE t.name NOT LIKE 'dt%' and rows > 0 ";
string queryLargeTables = "";
string queryStatement = queryAllTables + tableName;
string sqlConnectionString = "Data Source="+ server + ";Initial Catalog=DbArchive; User ID="+user+"; Password="+password+"; TrustServerCertificate=True; Connection Timeout="+timeout; 
//if you're using azure vaults change this string to string from your vault
```

And run the guy 🚀
