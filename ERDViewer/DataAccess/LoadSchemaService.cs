using ERDViewer.Models;
using Microsoft.Data.SqlClient;

namespace ERDViewer.DataAccess
{
    public class LoadSchemaService
    {
        private readonly string _connectionString;

        public LoadSchemaService(string connectionString)
        {
            _connectionString = connectionString;
        }
        public List<Table> LoadSchema(string connectionString)
        {
            var tablesDict = new Dictionary<string, Table>();

            string pkQuery = @"
        SELECT 
        t.name AS TableName,
        c.name AS ColumnName
        FROM sys.key_constraints kc
        JOIN sys.tables t ON kc.parent_object_id = t.object_id
        JOIN sys.index_columns ic ON kc.unique_index_id = ic.index_id AND ic.object_id = t.object_id
        JOIN sys.columns c ON ic.object_id = c.object_id AND ic.column_id = c.column_id
        WHERE kc.type = 'PK';
    ";

            string fkQuery = @"
         SELECT
         pt.name AS ParentTable,
         pc.name AS ParentColumn,
         rt.name AS ReferencedTable,
         rc.name AS ReferencedColumn,
         fk.name AS ForeignKeyName,
         i.is_unique AS IsUnique,
         pc.is_nullable AS IsNullable
         FROM sys.foreign_keys fk
         INNER JOIN sys.foreign_key_columns fkc ON fk.object_id = fkc.constraint_object_id
         INNER JOIN sys.tables pt ON fkc.parent_object_id = pt.object_id
         INNER JOIN sys.columns pc ON fkc.parent_object_id = pc.object_id AND fkc.parent_column_id = pc.column_id
         INNER JOIN sys.tables rt ON fkc.referenced_object_id = rt.object_id
         INNER JOIN sys.columns rc ON fkc.referenced_object_id = rc.object_id AND fkc.referenced_column_id = rc.column_id
         INNER JOIN sys.indexes i ON fk.key_index_id = i.index_id AND fk.referenced_object_id = i.object_id
         ORDER BY pt.name, fk.name;
           ";

            using (var connection = new SqlConnection(_connectionString)) 
            { 
                connection.Open();

                using (var pkCommand = new SqlCommand(pkQuery, connection))
                using (var reader = pkCommand.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        string tableName = reader["TableName"].ToString();
                        string columnName = reader["ColumnName"].ToString();

                        if (!tablesDict.ContainsKey(tableName))
                            tablesDict[tableName] = new Table { Name = tableName };

                        tablesDict[tableName].PrimaryKeys.Add(columnName);
                    }
                }

                using (var fkCommand = new SqlCommand(fkQuery, connection))
                using (var reader = fkCommand.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        string parentTable = reader["ParentTable"].ToString();
                        string parentColumn = reader["ParentColumn"].ToString();
                        string referencedTable = reader["ReferencedTable"].ToString();
                        string referencedColumn = reader["ReferencedColumn"].ToString();
                        string foreignKeyName = reader["ForeignKeyName"].ToString();
                        bool isUnique = Convert.ToBoolean(reader["IsUnique"]);
                        bool isNullable = Convert.ToBoolean(reader["IsNullable"]);

                        if (!tablesDict.ContainsKey(parentTable))
                            tablesDict[parentTable] = new Table { Name = parentTable };

                        var fk = new ForeignKey
                        {
                            Name = foreignKeyName,
                            Column = parentColumn,
                            ReferencedTable = referencedTable,
                            ReferencedColumn = referencedColumn,
                            IsUnique = isUnique,
                            IsNullable = isNullable
                        };

                        tablesDict[parentTable].ForeignKeys.Add(fk);
                    }

                }
            }
            

            return tablesDict.Values.ToList();
        }

    }
}
