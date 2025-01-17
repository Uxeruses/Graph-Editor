﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;

namespace Graph
{
    public class GraphState
    {
        private int _vertexNumber;
        private List<Point> _vertLocation;
        private List<Color> _vertColor;
        private List<List<int>> _edges;
        private int _radius;
        private int _penDiameter;
        private bool _isDirected;
        private int?[,] _weights;

        public int vertexNumber
        {
            get { return _vertexNumber; }
        }
        public GraphState(int radius, int penDiameter, bool isDirected)
        {
            _radius = radius;
            _penDiameter = penDiameter;
            _vertLocation = new List<Point>();
            _vertColor = new List<Color>();
            _edges = new List<List<int>>();
            _vertexNumber = 0;
            _isDirected = isDirected;
            _weights = new int?[0,0];
        }

        public void UpdateDirect(bool isDirected)
        {
            _isDirected = isDirected;
        }
        public void AddEdge(int u, int v, int weight)
        {
            AddEdge(u, v);
            if(!_isDirected)
            {
                _weights[v, u] = weight;
            }
            _weights[u, v] = weight;
        }

        public void AddEdge(int u, int v)
        {
            if (_vertexNumber < u || _vertexNumber < v) throw new IndexOutOfRangeException();
            _edges[u].Add(v);
            if(!_isDirected)
            {
                _edges[v].Add(u);
            }
        }

        public void RemoveEdge(int u, int v)
        {
            if (_vertexNumber < u || _vertexNumber < v) throw new IndexOutOfRangeException();
            _edges[u].Remove(v);
            _edges[v].Remove(u);
            _weights[u, v] = null;
            _weights[v, u] = null;
        }

        public bool HasEdge(int u, int v)
        {
            if (_vertexNumber < u || _vertexNumber < v) throw new IndexOutOfRangeException();

            return _edges[u].Contains(v);
        }

        //return true if new vertex doesn't collide with another vertex 
        public bool AddVertex(Point p, Color c)
        {
            if (Colides(p)) return false;
            _vertLocation.Add(p);
            _vertColor.Add(c);
            _edges.Add(new List<int>());
            IncreaseWeightsSize();
            _vertexNumber++;
            return true;
        }

        private void IncreaseWeightsSize()
        {
            int?[,] tempWeights = new int?[_vertexNumber + 1, _vertexNumber + 1];
            for(int i = 0; i < _vertexNumber; i++)
            {
                for(int j = 0; j < _vertexNumber; j++)
                {
                    tempWeights[i, j] = _weights[i, j];
                }
            }
            _weights = tempWeights;
        }

        private void DecreaseWeightsSize(int u)
        {
            int?[,] tempWeights = new int?[_vertexNumber - 1, _vertexNumber - 1];
            for (int i = 0; i < _vertexNumber; i++)
            {
                if (i == u)
                    continue;
                for (int j = 0; j < _vertexNumber; j++)
                {
                    if (j == u)
                        continue;
                    tempWeights[i > u ? i - 1: i, j > u ? j - 1 : j] = _weights[i, j];
                }
            }
            _weights = tempWeights;
        }

        public void ChangeWeight(int u, int v, int w)
        {
            if (!HasEdge(u, v)) return;
            _weights[u, v] = w;
            if (!_isDirected)
                _weights[v, u] = w;
        }

        public int? GetWeight(int u, int v)
        {
            return _weights[u, v];
        }

        public void RemoveVertex(int u)
        {
            _vertLocation.RemoveAt(u);
            _vertColor.RemoveAt(u);
            var temp = _edges[u].ToArray();
            foreach(var v in temp)
            {
                RemoveEdge(u, v);
            }

            _edges.RemoveAt(u);
            DecreaseWeightsSize(u);
            _vertexNumber--;
            for (int v = 0; v < vertexNumber; v++)
            {
                for (int i = 0; i < _edges[v].Count; i++)
                {
                    if (_edges[v][i] > u)
                        _edges[v][i]--;
                }
            }
        }
        public (Point p, Color c) GetPoint(int u)
        {
            return (_vertLocation[u], _vertColor[u]);
        }

        public List<(int u, int v, int? weight)> GetEdges()
        {
            List<(int u, int v, int? weight)> edges = new List<(int u, int v, int? weight)>();
            if(!_isDirected)
            {
                for (int u = 0; u < _vertexNumber; u++)
                {
                    foreach (var v in _edges[u])
                    {
                        if (u < v)
                            edges.Add((u, v, _weights[u, v]));
                        else
                            edges.Add((v, u, _weights[u, v]));
                    }
                }
                return edges.Distinct().ToList();
            }
            for (int u = 0; u < _vertexNumber; u++)
            {
                foreach (var v in _edges[u])
                {
                    edges.Add((u, v, _weights[u, v]));
                }
            }
            return edges.Distinct().ToList();

        }
        public List<int> GetNeighbors(int u)
        {
            return _edges[u];
        }

        public void UpdateColor(int u, Color c)
        {
            _vertColor[u] = c;
        }

        public void UpdatePosition(int u, int dx, int dy)
        {
            _vertLocation[u] = new Point(_vertLocation[u].X + dx, _vertLocation[u].Y + dy);
        }
        public void UpdatePosition(int u, Point p)
        {
            _vertLocation[u] = p;
        }
        public void Serialize(string path, Point offset)
        {
            using (StreamWriter sw = File.CreateText(path))
            {
                sw.WriteLine($"{offset.X},{offset.Y}");
                for (int i = 0; i < _vertexNumber; i++)
                {
                    sw.WriteLine($"{_vertLocation[i].X},{_vertLocation[i].Y},{_vertColor[i].ToArgb()}");
                }
                sw.WriteLine(" ");
                foreach (var edge in GetEdges())
                {
                    sw.WriteLine($"{edge.u},{edge.v}");
                }
            }
        }
        public bool Deserialize(string path, out Point offset)
        {
            List<Point> tempLocation = new List<Point>();
            List<Color> tempColor = new List<Color>();
            List<List<int>> tempEdges = new List<List<int>>();
            offset = new Point();
            string[] lines = File.ReadAllLines(path);
            bool processEdges = false;
            bool firstLine = true;
            foreach (var line in lines)
            {
                var temp = line.Split(',');

                if (firstLine)
                {
                    int x, y;
                    if (!(int.TryParse(temp[0], out x) && int.TryParse(temp[1], out y)))
                        return false;
                    offset.X = x;
                    offset.Y = y;
                    firstLine = false;
                    continue;
                }

                if (!processEdges)
                {
                    if (temp.Length != 3)
                    {
                        if (line == " ")
                        {
                            processEdges = true;
                            continue;
                        }
                        return false;
                    }
                    int x, y, argb;
                    if (!(int.TryParse(temp[0], out x) && int.TryParse(temp[1], out y) && int.TryParse(temp[2], out argb)))
                        return false;
                    tempLocation.Add(new Point(x, y));
                    tempColor.Add(Color.FromArgb(argb));
                    tempEdges.Add(new List<int>());
                }
                else
                {
                    if (temp.Length != 2)
                        return false;
                    int u, v;
                    if (!(int.TryParse(temp[0], out u) && int.TryParse(temp[1], out v)))
                        return false;

                    tempEdges[u].Add(v);
                    tempEdges[v].Add(u);
                }

            }
            _vertLocation = tempLocation;
            _vertColor = tempColor;
            _edges = tempEdges;
            _vertexNumber = _vertLocation.Count;
            return true;
        }

        public void Reset()
        {
            _vertexNumber = 0;
            _vertColor.Clear();
            _vertLocation.Clear();
            _edges.Clear();
        }

        public int Check(Point p, int i)
        {
            return ColidesPosition(p, i);
        }

        private int ColidesPosition(Point nP, int mult)
        {
            int delta = mult * _radius + _penDiameter;
            delta *= delta;
            int smallestDist = int.MaxValue;
            int closestVerex = -1;
            for (int i = 0; i < _vertexNumber; i++)
            {
                Point oP = _vertLocation[i];
                int dist = (oP.X - nP.X) * (oP.X - nP.X) + (oP.Y - nP.Y) * (oP.Y - nP.Y);
                if (dist <= delta)
                {
                    if (dist < smallestDist)
                    {
                        smallestDist = dist;
                        closestVerex = i;
                    }
                }
            }
            return closestVerex;
        }
        private bool Colides(Point nP)
        {
            return ColidesPosition(nP, 2) != -1;
        }


    }
}
