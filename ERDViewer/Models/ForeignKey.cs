//namespace ERDViewer.Models
//{
//    //public enum CardinalityDirection
//    //{
//    //    OneToOne,     
//    //    OneToMany,  
//    //    ManyToOne,    
//    //    Unknown
//    //}



//    public class ForeignKey
//    {
//        public string Name { get; set; }
//        public string ReferencedTable { get; set; }
//        public string ReferencedColumn { get; set; }
//        public string Column { get; set; }
//        public bool IsUnique { get; set; }
//        public bool IsNullable { get; set; }

//    }
//}
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
