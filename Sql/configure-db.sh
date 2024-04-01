#wait for the SQL Server to come up
sleep 30s

# Check if the database exists
if /opt/mssql-tools/bin/sqlcmd -S localhost -U SA -P $MSSQL_SA_PASSWORD -d master -Q "SELECT 1 FROM sys.databases WHERE name = 'PhotoChannel'" | grep -q 1; then
    echo "Database PhotoChannel already exists."
else
    echo "running set up script"
    #run the setup script to create the DB and the schema in the DB
    /opt/mssql-tools/bin/sqlcmd -S localhost -U SA -P $MSSQL_SA_PASSWORD -d master -i CreateDatabase.sql
fi

