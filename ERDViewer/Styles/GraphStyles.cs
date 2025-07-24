using Microsoft.Msagl.Drawing;

namespace ERDViewer.Styles
{
    public static class GraphStyles
    {
        public static void ApplyNodeStyle(Node node)
        {
            node.Attr.Shape = Shape.Box;
            node.Attr.FillColor = Theme.NodeBackground;
            node.Label.FontName = Theme.FontName;
            node.Label.FontSize = Theme.FontSize;
            node.Label.FontColor = Theme.FontColor;
            node.Attr.LineWidth = 1.2;
            node.Attr.Color = Theme.NodeBorder;
        }

        public static void ApplyEdgeStyle(Edge edge, bool isNullable)
        {
            edge.Attr.ArrowheadAtTarget = ArrowStyle.Normal;
            edge.Attr.ArrowheadAtSource = isNullable ? ArrowStyle.Diamond : ArrowStyle.None;
            edge.Attr.Color = Theme.EdgeColor;
            edge.Attr.LineWidth = 1.5;
        }
    }

    public static class Theme
    {
        public static Color NodeBackground => new Color(255, 255, 224);
        public static Color NodeBorder => new Color(100, 100, 100);
        public static Color FontColor => Color.Black;
        public static string FontName => "Segoe UI";
        public static double FontSize => 10;
        public static Color EdgeColor => Color.DarkSlateGray;
    }
}
