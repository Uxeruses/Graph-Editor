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

        public GraphForm()
        {
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
            format = new StringFormat() { Alignment = StringAlignment.Center };
            _gState = new GraphState(RADIUS, penDiam);
            _markedV = -1;

            using (Graphics g = Graphics.FromImage(backImage))
            {
                g.Clear(bcgColor);
            }
            ConstructForm();
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
        }

        private void LeftButtonControl(MouseEventArgs e)
        {
            if (_markedV != -1)
            {
                int pos = _gState.Check(e.Location, 1);
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

            if (!_gState.AddVertex(e.Location, drawColor)) return;

            DrawVertex(e.Location, _gState.vertexNumber, drawColor);
            Canvas.Refresh();
        }
        private void RightButtonControl(MouseEventArgs e)
        {
            int toCheck = _gState.Check(e.Location, 1);

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
                _gState.Serialize(fileDialog.FileName);
            }
        }

        private void buttonOpen_Click(object sender, EventArgs e)
        {
            OpenFileDialog fileDialog = new OpenFileDialog();
            fileDialog.Filter = "Graph files (*.graph)|*.graph";
            string msg = "", caption = "";
            if (fileDialog.ShowDialog() == DialogResult.OK)
            {
                if (_gState.Deserialize(fileDialog.FileName))
                {
                    _markedV = -1;
                    buttonDelete.Enabled = false;
                    ResetBackground();
                    RedrawEdges();
                    RedrawVertices();
                    Canvas.Refresh();
                    msg = GraphRemake.Properties.MSGBoxResource.MSG_Success_Content;
                    caption = GraphRemake.Properties.MSGBoxResource.MSG_Success_Content;
                    //msg = MyResources.ResourceMSG.MessageSuccessText;
                    //    caption = MyResources.ResourceMSG.MessageSuccessCaption;
                }
                else
                {
                    msg = GraphRemake.Properties.MSGBoxResource.MSG_Failure_Content;
                    caption = GraphRemake.Properties.MSGBoxResource.MSG_Failure_Title;
                    //msg = MyResources.ResourceMSG.MessageFailureText;
                    //caption = MyResources.ResourceMSG.MessageFailureCaption;
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
            if (e.Button != MouseButtons.Middle || _markedV == -1) return;
            if (_lP == null)
            {
                _lP = e.Location;
                return;
            }
            _gState.UpdatePosition(_markedV, e.Location.X - _lP.Value.X, e.Location.Y - _lP.Value.Y);
            _lP = e.Location;
            ResetBackground();
            RedrawEdges();
            RedrawVertices();
            Canvas.Refresh();
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

            // without it keyboard doesn't work :)
            // focus has to be set on any button for keyboard to work
            buttonColor.Focus();

            ConstructForm();
        }
        private void ConstructForm()
        {
            panelColor.BackColor = drawColor;
            Canvas.Image = backImage;
            Canvas.Refresh();

        }

        private void Form_KeyDown(object sender, KeyEventArgs e)
        {
            if ((e.KeyData & Keys.Delete) != 0 && _markedV != -1)
            {
                RemoveVertex();
            }
        }


    }
}
