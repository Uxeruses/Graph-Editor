using ASD.Graphs;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.Windows.Forms;


namespace Graph
{
    public partial class GraphForm : System.Windows.Forms.Form
    {
        private const int RADIUS = 15;
        private const int penDiam = 3;
        private const float dashLength = 2;
        private readonly Font textFont;
        private readonly StringFormat format;
        private Bitmap backImage;
        private Color bcgColor;
        private Color drawColor;
        private Pen edgePen;
        private Pen pen;
        private Pen clearPen;
        private SolidBrush clearBrush;
        private GraphState _gState;
        private int _markedV;
        private Point? _lP;
        private int _comboIndex;
        private List<CursorSelect> _cursors;
        private Point _offset;
        private IGraph<int>? _graph;
        private StartPosition _startPosition;
        private (int x, int y) _graphStartPostion;
        private int _graphGapValue;
        private bool _isDirected;
        private int _bezierMultiply;
        private int _arrowLength;
        private int _arrowWidth;

        public GraphForm(IGraph<double> graph) : this()
        {
            if(graph.Directed)
            {
                var temp = new DiGraph<int>(graph.VertexCount);
                for (int i = 0; i < graph.VertexCount; i++)
                {
                    foreach (var edge in graph.OutEdges(i))
                    {
                        temp.AddEdge(edge.From, edge.To, (int)edge.weight);
                    }
                }
                _graph = temp;
                _isDirected = true;
            }
            else
            {
                var temp = new Graph<int>(graph.VertexCount);
                for (int i = 0; i < graph.VertexCount; i++)
                {
                    foreach (var edge in graph.OutEdges(i))
                    {
                        temp.AddEdge(edge.From, edge.To, (int)edge.weight);
                    }
                }
                _graph = temp;
                _isDirected=false;
            }
            _gState.UpdateDirect(_isDirected);
            SetGraph();
        }


        public GraphForm(IGraph<int> graph) : this()
        {
            _graph = graph;
            _isDirected = graph.Directed;
            _gState.UpdateDirect(_isDirected);
            SetGraph();
        }
        public GraphForm()
        {
            _arrowLength = 10;
            _arrowWidth = 20;
            _bezierMultiply = 3;
            
            _graphStartPostion = (50, 50);
            _graphGapValue = 100;
            _startPosition = Graph.StartPosition.Square;
            _graph = null;
            _cursors = new List<CursorSelect>();
            _cursors.Add(new CursorSelect(Cursors.Cross, 0));
            _cursors.Add(new CursorSelect(Cursors.NoMove2D, 1));

            var culture = new CultureInfo("pl-PL");
            CultureInfo.DefaultThreadCurrentCulture = culture;
            CultureInfo.DefaultThreadCurrentUICulture = culture;
            InitializeComponent();
            backImage = new Bitmap(Canvas.Width, Canvas.Height);

            bcgColor = Color.White;
            drawColor = Color.Black;

            pen = new Pen(drawColor, penDiam);
            clearPen = new Pen(bcgColor, penDiam);
            clearBrush = new SolidBrush(bcgColor);
            edgePen = new Pen(drawColor, penDiam);
            textFont = new Font("Arial", RADIUS, FontStyle.Regular);
            format = new StringFormat() { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center };
            _gState = new GraphState(RADIUS, penDiam, _isDirected);

            _markedV = -1;
            _comboIndex = 0;
            _offset = new Point(0, 0);

            using (Graphics g = Graphics.FromImage(backImage))
            {
                g.Clear(bcgColor);
            }
            ConstructForm();
        }

        private void SetGraph()
        {
            if (_graph == null) return;
            int n = _graph.VertexCount;
            switch (_startPosition)
            {
                case Graph.StartPosition.Square:
                    {
                        Point p = new Point(_graphStartPostion.x, _graphStartPostion.y);

                        int size = (int)Math.Ceiling(Math.Sqrt(n));

                        for (int i = 0; i < n; i++)
                        {
                            _gState.AddVertex(p, drawColor);
                            if ((i + 1) % size == 0)
                            {
                                p.Y += _graphGapValue;
                                p.X = _graphStartPostion.x;
                            }
                            else
                            {
                                p.X += _graphGapValue;
                            }
                        }
                        break;
                    }
            }
            for (int i = 0; i < n; i++)
            {
                foreach (var neigh in _graph.OutNeighbors(i))
                {
                    _gState.AddEdge(i, neigh, _graph.GetEdgeWeight(i, neigh));
                }

            }

            RedrawAll();
        }

        private void Form_SizeChanged(object sender, EventArgs e)
        {
            Size size = tableLayoutMain.GetControlFromPosition(0, 0).Size;
            Bitmap newBitmap = new Bitmap(size.Width, size.Height);
            using (Graphics g = Graphics.FromImage(newBitmap))
            {
                g.Clear(bcgColor);
            }
            Canvas.Image = newBitmap;
            backImage.Dispose();
            backImage = newBitmap;
            ResetBackground();
            RedrawEdges();
            RedrawVertices();
            Canvas.Refresh();
        }
        private void Canvas_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            Canvas_MouseClick(sender, e);
        }
        private void Canvas_MouseClick(object sender, MouseEventArgs e)
        {
            switch (_comboIndex)
            {
                case 0:
                    {
                        switch (e.Button)
                        {
                            case MouseButtons.Left:
                                {
                                    LeftButtonControl(e);
                                    break;
                                }
                            case MouseButtons.Right:
                                {
                                    RightButtonControl(e);
                                    break;
                                }
                            case MouseButtons.Middle:
                                {
                                    MiddleButtonControl(e);
                                    break;
                                }
                        }
                        break;
                    }
                case 1:
                    {
                        switch (e.Button)
                        {
                            case MouseButtons.Left:
                                {
                                    MoveBitMap(e);
                                    break;
                                }
                        }
                        break;
                    }
            }

        }

        private void MoveBitMap(MouseEventArgs e)
        {
        }

        private void LeftButtonControl(MouseEventArgs e)
        {
            Point calibrated = new Point(e.Location.X - _offset.X, e.Location.Y - _offset.Y);

            if (_markedV != -1)
            {
                int pos = _gState.Check(calibrated, 1);
                if (pos != -1 && pos != _markedV)
                {
                    if (_gState.HasEdge(_markedV, pos))
                    {
                        _gState.RemoveEdge(_markedV, pos);

                    }
                    else
                    {
                        _gState.AddEdge(_markedV, pos);
                    }
                    ResetBackground();
                    RedrawEdges();
                    RedrawVertices();
                    Canvas.Refresh();
                    return;
                }
            }

            if (!_gState.AddVertex(calibrated, drawColor)) return;

            DrawVertex(_gState.vertexNumber - 1);
            Canvas.Refresh();
        }
        private void RightButtonControl(MouseEventArgs e)
        {
            Point calibrated = new Point(e.Location.X - _offset.X, e.Location.Y - _offset.Y);
            int toCheck = _gState.Check(calibrated, 1);

            if (toCheck == -1)
            {
                UnCheck(_markedV);
                _markedV = -1;
            }
            else
            {
                if (_markedV != toCheck)
                {
                    UnCheck(_markedV);
                    _markedV = toCheck;
                    Check(_markedV);
                }
            }
            Canvas.Refresh();
            if (_markedV == -1)
                buttonDelete.Enabled = false;
            else
                buttonDelete.Enabled = true;
        }
        private void MiddleButtonControl(MouseEventArgs e)
        {
        }

        private void button1_Click(object sender, EventArgs e)
        {
            ColorDialog colorDialog = new ColorDialog();
            if (colorDialog.ShowDialog() == DialogResult.OK)
            {
                drawColor = colorDialog.Color;
                panelColor.BackColor = drawColor;
                pen.Dispose();
                pen = new Pen(drawColor, penDiam);

                if (_markedV != -1)
                {
                    _gState.UpdateColor(_markedV, drawColor);
                    Check(_markedV);
                    Canvas.Refresh();
                }
            }

        }

        private void buttonSave_Click(object sender, EventArgs e)
        {
            SaveFileDialog fileDialog = new SaveFileDialog();
            fileDialog.Filter = "Graph files (*.graph)|*.graph";
            if (fileDialog.ShowDialog() == DialogResult.OK)
            {
                _gState.Serialize(fileDialog.FileName, _offset);
            }
        }

        private void buttonOpen_Click(object sender, EventArgs e)
        {
            OpenFileDialog fileDialog = new OpenFileDialog();
            fileDialog.Filter = "Graph files (*.graph)|*.graph";
            string msg = "", caption = "";
            if (fileDialog.ShowDialog() == DialogResult.OK)
            {

                Point temp;
                if (_gState.Deserialize(fileDialog.FileName, out temp))
                {
                    _offset = temp;
                    _markedV = -1;
                    buttonDelete.Enabled = false;
                    ResetBackground();
                    RedrawEdges();
                    RedrawVertices();
                    Canvas.Refresh();
                    msg = GraphRemake.Properties.MSGBoxResource.MSG_Success_Content;
                    caption = GraphRemake.Properties.MSGBoxResource.MSG_Success_Title;
                }
                else
                {
                    msg = GraphRemake.Properties.MSGBoxResource.MSG_Failure_Content;
                    caption = GraphRemake.Properties.MSGBoxResource.MSG_Failure_Title;
                }
                MessageBoxButtons btn = MessageBoxButtons.OK;
                MessageBox.Show(msg, caption, btn);
            }
        }

        private void buttonClear_Click(object sender, EventArgs e)
        {
            using (Graphics g = Graphics.FromImage(backImage))
            {
                g.Clear(bcgColor);
            }
            _offset = new Point(0, 0);
            _markedV = -1;
            buttonDelete.Enabled = false;
            _gState.Reset();
            Canvas.Refresh();
        }

        private void buttonDelete_Click(object sender, EventArgs e)
        {
            RemoveVertex();
        }

        private void RemoveVertex()
        {
            if (_markedV == -1) return;
            _gState.RemoveVertex(_markedV);
            _markedV = -1;
            ResetBackground();
            RedrawEdges();
            RedrawVertices();
            Canvas.Refresh();

            buttonDelete.Enabled = false;
        }
        private void Canvas_MouseUp(object sender, MouseEventArgs e)
        {
            _lP = null;
        }

        //TODO: Repair case when user starts middleclick outside vertex
        private void Canvas_MouseMove(object sender, MouseEventArgs e)
        {
            switch (_comboIndex)
            {
                case 0:
                    {
                        MouseMoveSelect(e);
                        break;
                    }
                case 1:
                    {
                        MouseMoveMove(e);
                        break;
                    }
            }
        }

        private void MouseMoveSelect(MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Middle || _markedV == -1) return;
            if (_lP == null)
            {
                _lP = e.Location;
                return;
            }
            _gState.UpdatePosition(_markedV, e.Location.X - _lP.Value.X, e.Location.Y - _lP.Value.Y);
            _lP = e.Location;
            RedrawAll();
        }
        private void MouseMoveMove(MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Left) return;
            if (_lP == null)
            {
                _lP = e.Location;
                return;
            }
            int dx = e.Location.X - _lP.Value.X;
            int dy = e.Location.Y - _lP.Value.Y;
            _offset = new Point(_offset.X + dx, _offset.Y + dy);
            _lP = e.Location;
            RedrawAll();
        }

        private void buttonPolish_Click(object sender, EventArgs e)
        {
            ChangeLanguage("pl-PL");
        }

        private void buttonEnglish_Click(object sender, EventArgs e)
        {
            ChangeLanguage("en-US");
        }

        private void ChangeLanguage(string code)
        {
            var culture = new CultureInfo(code);
            CultureInfo.DefaultThreadCurrentCulture = culture;
            CultureInfo.DefaultThreadCurrentUICulture = culture;
            this.Controls.Clear();
            var oldSize = Size;
            InitializeComponent();
            Size = oldSize;
            this.KeyPreview = true;
            buttonDelete.Enabled = _markedV != -1;
            Canvas.Cursor = _cursors[_comboIndex].Cursor;

            // without it keyboard doesn't work :)
            // focus has to be set on any button for keyboard to work
            buttonColor.Focus();

            ConstructForm();
        }
        private void ConstructForm()
        {
            comboBoxCursors.DataSource = _cursors;
            comboBoxCursors.SelectedIndex = _comboIndex;
            comboBoxCursors.DisplayMember = "Name";
            comboBoxCursors.ValueMember = "Cursor";
            comboBoxCursors.Text = _cursors[_comboIndex].Name;

            panelColor.BackColor = drawColor;
            Canvas.Image = backImage;
            Canvas.Refresh();

        }

        private void Form_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete && _markedV != -1)
            {
                RemoveVertex();
            }

        }

        private void GraphForm_Load(object sender, EventArgs e)
        {
        }

        private class CursorSelect
        {
            private Cursor _cursor;
            public Cursor Cursor
            {
                get { return _cursor; }
            }
            private int _id;
            public string Name
            {
                get
                {
                    switch (_id)
                    {
                        case 0:
                            return GraphRemake.Properties.MSGBoxResource.SEL_Cursor_Selecet;
                        case 1:
                            return GraphRemake.Properties.MSGBoxResource.SEL_Cursor_Move;
                    }
                    return string.Empty;
                }
            }
            public CursorSelect(Cursor cursor, int id)
            {
                _cursor = cursor;
                _id = id;
            }
        }

        private void comboBoxCursors_SelectedIndexChanged(object sender, EventArgs e)
        {
            var cursor = (CursorSelect)comboBoxCursors.SelectedItem;
            Canvas.Cursor = cursor.Cursor;
            _comboIndex = comboBoxCursors.SelectedIndex;
        }
    }
}
