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
            g.FillEllipse(clearBrush, e.X - RADIUS, e.Y - RADIUS, 2 * RADIUS, 2 * RADIUS);
            g.DrawEllipse(tempPen, e.X - RADIUS, e.Y - RADIUS, 2 * RADIUS, 2 * RADIUS);
            g.DrawString(no.ToString(), textFont, tempBrush, new Point(e.X, e.Y - RADIUS + penDiam), format);
        }
        private void DeleteVertex(int u)
        {
            var temp = _gState.GetPoint(u);
            using (Graphics g = Graphics.FromImage(backImage))
            {
                g.FillEllipse(Brushes.Transparent, temp.p.X - RADIUS - penDiam, temp.p.Y - RADIUS - penDiam, 2 * (RADIUS + penDiam), 2 * (RADIUS + penDiam));
            }
        }

        private void DrawEdge(int u, int v)
        {
            using (Graphics g = Graphics.FromImage(backImage))
            {
                g.DrawLine(edgePen, _gState.GetPoint(u).p, _gState.GetPoint(v).p);
            }
        }
        private void DeleteEdge(int u, int v)
        {
            using (Graphics g = Graphics.FromImage(backImage))
            {
                g.DrawLine(clearPen, _gState.GetPoint(u).p, _gState.GetPoint(v).p);
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
            using (Graphics g = Graphics.FromImage(backImage))
            {
                Pen dashPen = new Pen(temp.c, penDiam) { DashPattern = new float[] { dashLength, dashLength } };
                g.FillEllipse(clearBrush, temp.p.X - RADIUS, temp.p.Y - RADIUS, 2 * RADIUS, 2 * RADIUS);
                g.DrawEllipse(clearPen, temp.p.X - RADIUS, temp.p.Y - RADIUS, 2 * RADIUS, 2 * RADIUS);
                g.DrawEllipse(dashPen, temp.p.X - RADIUS, temp.p.Y - RADIUS, 2 * RADIUS, 2 * RADIUS);
                g.DrawString((u + 1).ToString(), textFont, new SolidBrush(temp.c), new Point(temp.p.X, temp.p.Y - RADIUS + penDiam), format);
            }
        }
        private void UnCheck(int u)
        {
            if (u == -1) return;
            DrawVertex(u);
        }
    }
}