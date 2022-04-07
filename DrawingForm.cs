using System;
using System.Drawing;
using System.Windows.Forms;
using System.Diagnostics;
using System.Globalization;
using System.Resources;

namespace Graph
{
    public partial class GraphForm : System.Windows.Forms.Form
    {
        private void ResetBackground()
        {
            using (Graphics g = Graphics.FromImage(backImage))
            {
                g.Clear(bcgColor);
            }
        }
        private void ResetVertexImage()
        {
            using (Graphics g = Graphics.FromImage(backImage))
            {
                g.Clear(Color.Transparent);
            }
        }
        private void DrawVertex(int u)
        {
            var temp = _gState.GetPoint(u);
            DrawVertex(temp.p, u + 1, temp.c);
        }

        private void DrawVertex(Point e, int no, Color color)
        {
            using Graphics g = Graphics.FromImage(backImage);
            using var tempPen = new Pen(color, penDiam);
            using var tempBrush = new SolidBrush(color);
            int xCord = e.X - RADIUS + _offset.X;
            int yCord = e.Y - RADIUS + _offset.Y;
            g.FillEllipse(clearBrush, xCord, yCord, 2 * RADIUS, 2 * RADIUS);
            g.DrawEllipse(tempPen, xCord, yCord, 2 * RADIUS, 2 * RADIUS);
            g.DrawString(no.ToString(), textFont, tempBrush, new Point(e.X + _offset.X, e.Y + _offset.Y), format);
        }
        private void DrawEdge(int u, int v)
        {
            using (Graphics g = Graphics.FromImage(backImage))
            {
                Point p1 = _gState.GetPoint(u).p;
                Point p2 = _gState.GetPoint(v).p;
                p1 = new Point(p1.X + _offset.X, p1.Y + _offset.Y);
                p2 = new Point(p2.X + _offset.X, p2.Y + _offset.Y);
                g.DrawLine(edgePen, p1, p2);
            }
        }

        private void RedrawEdges()
        {
            var temp = _gState.GetEdges();
            foreach (var edge in temp)
            {
                DrawEdge(edge.u, edge.v);
            }
        }


        private void RedrawVertices()
        {
            for (int i = 0; i < _gState.vertexNumber; i++)
            {
                if (i == _markedV)
                {
                    continue;
                }
                DrawVertex(i);
            }
            Check(_markedV);
        }


        private void Check(int u)
        {
            if (u == -1)
                return;
            var temp = _gState.GetPoint(u);
            using Pen dashPen = new Pen(temp.c, penDiam) { DashPattern = new float[] { dashLength, dashLength } };
            using SolidBrush tempBrush = new SolidBrush(temp.c);
            using Graphics g = Graphics.FromImage(backImage);
            int xCord = temp.p.X - RADIUS + _offset.X;
            int yCord = temp.p.Y - RADIUS + _offset.Y;
            g.FillEllipse(clearBrush, xCord, yCord, 2 * RADIUS, 2 * RADIUS);
            g.DrawEllipse(clearPen, xCord, yCord, 2 * RADIUS, 2 * RADIUS);
            g.DrawEllipse(dashPen, xCord, yCord, 2 * RADIUS, 2 * RADIUS);
            g.DrawString((u + 1).ToString(), textFont, tempBrush, new Point(temp.p.X + _offset.X, temp.p.Y + _offset.Y), format);
        }
        private void UnCheck(int u)
        {
            if (u == -1) return;
            DrawVertex(u);
        }

        private void RedrawAll()
        {
            ResetBackground();
            RedrawEdges();
            RedrawVertices();
            Canvas.Refresh();
        }
    }
}