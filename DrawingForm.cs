using System.Drawing;
using System;

namespace Graph
{
    public partial class GraphForm : System.Windows.Forms.Form
    {
        private void ResetBackground()
        {
            using Graphics g = Graphics.FromImage(backImage);
            g.Clear(bcgColor);
        }
        private void DrawVertex(int u)
        {
            var temp = _gState.GetPoint(u);
            DrawVertex(temp.p, u, temp.c);
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
            using Graphics g = Graphics.FromImage(backImage);
            Point p1 = _gState.GetPoint(u).p;
            Point p2 = _gState.GetPoint(v).p;
            p1 = new Point(p1.X + _offset.X, p1.Y + _offset.Y);
            p2 = new Point(p2.X + _offset.X, p2.Y + _offset.Y);
            
            g.DrawLine(edgePen, p1, p2);
        
            int? tempWeight = _gState.GetWeight(u, v);

            if (tempWeight == null)
                return;
            using var tempBrush = new SolidBrush(edgePen.Color);

            var med = new Point((p1.X + p2.X) / 2, (p1.Y + p2.Y) / 2);
            Size sizeOfText = System.Windows.Forms.TextRenderer.MeasureText(tempWeight.ToString(), textFont);
            Rectangle rect = new Rectangle(new Point(med.X - sizeOfText.Width / 2, med.Y - sizeOfText.Height / 2), sizeOfText);
            g.FillRectangle(clearBrush, rect);
            g.DrawString(tempWeight.ToString(), textFont, tempBrush, med, format);
        }

        private void DrawDirectedEdge(int u, int v)
        {
            DrawEdge(u, v);
            DrawSingleArrow(u, v);
        }
        private void DrawBiEdge(int u, int v)
        {
            using Graphics g = Graphics.FromImage(backImage);
            Point p1 = _gState.GetPoint(u).p;
            Point p2 = _gState.GetPoint(v).p;
            double off = _bezierMultiply * Math.Sqrt(Math.Pow(p1.X - p2.X, 2) + Math.Pow(p1.Y - p2.Y, 2));
            p1 = new Point(p1.X + _offset.X, p1.Y + _offset.Y);
            p2 = new Point(p2.X + _offset.X, p2.Y + _offset.Y);
            (double X, double Y) aVector = ((p1.X + p2.X) / 2 - p1.X, (p1.Y + p2.Y) / 2 - p1.Y);

            double bx = Math.Sqrt(aVector.Y * aVector.Y * off / (aVector.Y * aVector.Y + aVector.X * aVector.X));
            double by;
            if(aVector.Y == 0)
            {
                by = Math.Sqrt(off);
                bx = 0;
            }
            else
            {
                by = -1 * aVector.X * bx / aVector.Y;
            }

            (double X, double Y) bVector = (bx, by);

            Point pUp = new Point((int)(p1.X + aVector.X + bVector.X), (int)(p1.Y + aVector.Y + bVector.Y));
            Point pDown = new Point((int)(p1.X + aVector.X - bVector.X), (int)(p1.Y + aVector.Y - bVector.Y));

            g.DrawBezier(edgePen, p1, pUp, pUp, p2);
            g.DrawBezier(edgePen, p2, pDown, pDown, p1);
            // Draw uv weight
            int? tempWeight = _gState.GetWeight(u, v);

            if (tempWeight != null)
            {
                using var tempBrush = new SolidBrush(edgePen.Color);

                var med = p1.Y > p2.Y ? pUp : pDown;
                Size sizeOfText = System.Windows.Forms.TextRenderer.MeasureText(tempWeight.ToString(), textFont);
                Rectangle rect = new Rectangle(new Point(med.X - sizeOfText.Width / 2, med.Y - sizeOfText.Height / 2), sizeOfText);
                g.FillRectangle(clearBrush, rect);
                g.DrawString(tempWeight.ToString(), textFont, tempBrush, med, format);
            }


            //Draw vu weight
            tempWeight = _gState.GetWeight(v, u);

            if (tempWeight != null)
            {
                using var tempBrush = new SolidBrush(edgePen.Color);

                var med = p1.Y > p2.Y ? pDown : pUp;
                Size sizeOfText = System.Windows.Forms.TextRenderer.MeasureText(tempWeight.ToString(), textFont);
                Rectangle rect = new Rectangle(new Point(med.X - sizeOfText.Width / 2, med.Y - sizeOfText.Height / 2), sizeOfText);
                g.FillRectangle(clearBrush, rect);
                g.DrawString(tempWeight.ToString(), textFont, tempBrush, med, format);
            }

            DrawSingleArrow(pUp, p1.Y > p2.Y ? p2 : p1, false);
            DrawSingleArrow(pDown, p1.Y > p2.Y ? p1 : p2, false);
        }

        private void DrawSingleArrow(int u, int v)
        {
            DrawSingleArrow(_gState.GetPoint(u).p, _gState.GetPoint(v).p, true);
        }

        private void DrawSingleArrow(Point u, Point v, bool includeOffset)
        {
            using Graphics g = Graphics.FromImage(backImage);
            Point p1 = new Point(u.X + (includeOffset ? _offset.X : 0), u.Y + (includeOffset ? _offset.Y : 0));
            Point p2 = new Point(v.X + (includeOffset ? _offset.X : 0), v.Y + (includeOffset ? _offset.Y : 0));

            double dx = Math.Abs(p1.X - p2.X);
            double dy = Math.Abs(p1.Y - p2.Y);
            double D = Math.Sqrt(dx * dx + dy * dy);

            double m = (RADIUS + penDiam) * dx / D;
            double n = (RADIUS + penDiam) * dy / D;

            double mt = (RADIUS + penDiam + _arrowLength) * dx / D;
            double nt = (RADIUS + penDiam + _arrowLength) * dy / D;

            Point q3 = new Point(p2.X + (int)m * (p2.X < p1.X ? 1 : -1), p2.Y + (int)n * (p2.Y < p1.Y ? 1 : -1));
            Point t = new Point(p2.X + (int)mt * (p2.X < p1.X ? 1 : -1), p2.Y + (int)nt * (p2.Y < p1.Y ? 1 : -1));

            (double X, double Y) aVector = (t.X - q3.X, t.Y - q3.Y);

            double bx = Math.Sqrt(aVector.Y * aVector.Y * _arrowWidth / (aVector.Y * aVector.Y + aVector.X * aVector.X));
            double by;
            if (aVector.Y == 0)
            {
                by = Math.Sqrt(_arrowWidth);
                bx = 0;
            }
            else
            {
                by = -1 * aVector.X * bx / aVector.Y;
            }

            (double X, double Y) bVector = (bx, by);

            Point q1 = new Point((int)(t.X + bVector.X), (int)(t.Y + bVector.Y));
            Point q2 = new Point((int)(t.X - bVector.X), (int)(t.Y - bVector.Y));
            Point[] pts = { q1, q2, q3 };
            g.DrawPolygon(edgePen, pts);
            using SolidBrush tempBrush = new SolidBrush(edgePen.Color);
            g.FillPolygon(tempBrush, pts);
        }

        private void RedrawEdges()
        {
            var temp = _gState.GetEdges();
            foreach (var edge in temp)
            {
                if(!_isDirected)
                    DrawEdge(edge.u, edge.v);
                else
                {
                    if(temp.FindIndex(x => x.u == edge.v && x.v == edge.u) == -1)
                    {
                        DrawDirectedEdge(edge.u, edge.v);
                    }
                    else if(edge.u < edge.v)
                    {
                        DrawBiEdge(edge.u, edge.v);
                    }
                }
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


        private void Check(int? u)
        {
            if (u == -1 || u == null)
                return;
            var temp = _gState.GetPoint((int)u);
            using Pen dashPen = new Pen(temp.c, penDiam) { DashPattern = new float[] { dashLength, dashLength } };
            using SolidBrush tempBrush = new SolidBrush(temp.c);
            using Graphics g = Graphics.FromImage(backImage);
            int xCord = temp.p.X - RADIUS + _offset.X;
            int yCord = temp.p.Y - RADIUS + _offset.Y;
            g.FillEllipse(clearBrush, xCord, yCord, 2 * RADIUS, 2 * RADIUS);
            g.DrawEllipse(clearPen, xCord, yCord, 2 * RADIUS, 2 * RADIUS);
            g.DrawEllipse(dashPen, xCord, yCord, 2 * RADIUS, 2 * RADIUS);
            g.DrawString(u.ToString(), textFont, tempBrush, new Point(temp.p.X + _offset.X, temp.p.Y + _offset.Y), format);
        }
        private void UnCheck(int? u)
        {
            if (u == -1 || u == null) return;
            DrawVertex((int)u);
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