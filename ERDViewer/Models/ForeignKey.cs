namespace ERDViewer.Models
{
    public class ForeignKey
    {
        public required string Name { get; set; }
        public required string ReferencedTable { get; set; }
        public required string ReferencedColumn { get; set; }
        public required string Column { get; set; }

        public bool IsUnique { get; set; }
        public bool IsNullable { get; set; }

        public ForeignKey() { }

        public ForeignKey(string name, string referencedTable, string referencedColumn, string column, bool isUnique, bool isNullable)
        {
            Name = name;
            ReferencedTable = referencedTable;
            ReferencedColumn = referencedColumn;
            Column = column;
            IsUnique = isUnique;
            IsNullable = isNullable;
        }
    }
}
