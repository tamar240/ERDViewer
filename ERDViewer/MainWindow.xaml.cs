using ERDViewer.DataAccess;
using ERDViewer.Models;
using ERDViewer.Styles;
using Microsoft.Msagl.Drawing;
using Microsoft.Msagl.GraphViewerGdi;
using System.Text;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Forms.Integration;

namespace ERDViewer
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            var connectionString = @"Data Source=localhost;Initial Catalog=AdventureWorks2019;Integrated Security=True;TrustServerCertificate=True";

            var service = new LoadSchemaService(connectionString);
            var tables = service.LoadSchema(connectionString);

            DrawGraph(tables);
        }

        private void DrawGraph(List<Table> tables)
        {
            if (tables == null)
                return;

            var viewer = new GViewer();
            var graph = new Graph("ERD");

            var tableNameNodes = new Dictionary<string, Node>();

          
            foreach (var table in tables)
            {
                var sb = new StringBuilder();
                sb.AppendLine("╔════════════╗");
                sb.AppendLine($"{table.Name}");
                sb.AppendLine("╚════════════╝");
            

                table.PrimaryKeys.ForEach(pk => sb.AppendLine($"{pk}"));


                foreach (var fk in table.ForeignKeys)
                    if (!table.PrimaryKeys.Contains(fk.Column))
                        sb.AppendLine($"{fk.Column}");

                var node = graph.AddNode(table.Name);
                node.LabelText = sb.ToString().Trim();
                GraphStyles.ApplyNodeStyle(node);
                
                tableNameNodes[table.Name] =  node;
            }

            foreach (var table in tables)
            {
                foreach (var fk in table.ForeignKeys)
                {
                    if (!tableNameNodes.ContainsKey(fk.ReferencedTable)) continue;

                    var edge = graph.AddEdge(fk.ReferencedTable, table.Name);
                    GraphStyles.ApplyEdgeStyle(edge, fk.IsNullable);

                    var cardinality = (!fk.IsNullable && fk.IsUnique) ? "has a single" : "may have multiple";
                    edge.UserData = $"each {fk.ReferencedTable} {cardinality} {table.Name}";
                }
            }

            graph.LayoutAlgorithmSettings = new Microsoft.Msagl.Layout.Layered.SugiyamaLayoutSettings
            {
                LayerSeparation = 60,
                NodeSeparation = 60
            };
            viewer.Graph = graph;
            
            WindowsFormsHost.Child = viewer;

            var tooltip = new ToolTip();
            viewer.MouseMove += (sender, e) =>
            {
                var hit = viewer.ObjectUnderMouseCursor;
                if (hit?.DrawingObject is Edge edge && edge.UserData is string text)
                    viewer.SetToolTip(tooltip, text);
                else
                    viewer.SetToolTip(tooltip, "");
            };
        }
    }
}
