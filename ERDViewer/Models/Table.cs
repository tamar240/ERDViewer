namespace ERDViewer.Models
{
    public class Table
    {
        public required string Name { get; set; }
        public List<string> PrimaryKeys { get; set; } = new();
        public List<ForeignKey> ForeignKeys { get; set; } = new();

        public Table() { }

        public Table(string name)
        {
            Name = name;
        }
    }
}
