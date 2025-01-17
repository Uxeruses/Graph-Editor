﻿namespace Graph
{
    partial class GraphForm
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(GraphForm));
            this.tableLayoutMain = new System.Windows.Forms.TableLayoutPanel();
            this.tableLayoutRight = new System.Windows.Forms.TableLayoutPanel();
            this.groupBoxEdit = new System.Windows.Forms.GroupBox();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.weightNumBox = new System.Windows.Forms.NumericUpDown();
            this.confirmEdgeWeight = new System.Windows.Forms.Button();
            this.comboBoxCursors = new System.Windows.Forms.ComboBox();
            this.buttonClear = new System.Windows.Forms.Button();
            this.buttonDelete = new System.Windows.Forms.Button();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.panelColor = new System.Windows.Forms.Panel();
            this.buttonColor = new System.Windows.Forms.Button();
            this.groupBoxLang = new System.Windows.Forms.GroupBox();
            this.buttonEnglish = new System.Windows.Forms.Button();
            this.buttonPolish = new System.Windows.Forms.Button();
            this.groupBoxIE = new System.Windows.Forms.GroupBox();
            this.buttonOpen = new System.Windows.Forms.Button();
            this.buttonSave = new System.Windows.Forms.Button();
            this.Canvas = new System.Windows.Forms.PictureBox();
            this.tableLayoutMain.SuspendLayout();
            this.tableLayoutRight.SuspendLayout();
            this.groupBoxEdit.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.weightNumBox)).BeginInit();
            this.tableLayoutPanel1.SuspendLayout();
            this.groupBoxLang.SuspendLayout();
            this.groupBoxIE.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Canvas)).BeginInit();
            this.SuspendLayout();
            // 
            // tableLayoutMain
            // 
            resources.ApplyResources(this.tableLayoutMain, "tableLayoutMain");
            this.tableLayoutMain.Controls.Add(this.tableLayoutRight, 1, 0);
            this.tableLayoutMain.Controls.Add(this.Canvas, 0, 0);
            this.tableLayoutMain.Name = "tableLayoutMain";
            // 
            // tableLayoutRight
            // 
            resources.ApplyResources(this.tableLayoutRight, "tableLayoutRight");
            this.tableLayoutRight.Controls.Add(this.groupBoxEdit, 0, 0);
            this.tableLayoutRight.Controls.Add(this.groupBoxLang, 0, 1);
            this.tableLayoutRight.Controls.Add(this.groupBoxIE, 0, 2);
            this.tableLayoutRight.Name = "tableLayoutRight";
            // 
            // groupBoxEdit
            // 
            resources.ApplyResources(this.groupBoxEdit, "groupBoxEdit");
            this.groupBoxEdit.Controls.Add(this.tableLayoutPanel2);
            this.groupBoxEdit.Controls.Add(this.comboBoxCursors);
            this.groupBoxEdit.Controls.Add(this.buttonClear);
            this.groupBoxEdit.Controls.Add(this.buttonDelete);
            this.groupBoxEdit.Controls.Add(this.tableLayoutPanel1);
            this.groupBoxEdit.Name = "groupBoxEdit";
            this.groupBoxEdit.TabStop = false;
            // 
            // tableLayoutPanel2
            // 
            resources.ApplyResources(this.tableLayoutPanel2, "tableLayoutPanel2");
            this.tableLayoutPanel2.Controls.Add(this.weightNumBox, 0, 0);
            this.tableLayoutPanel2.Controls.Add(this.confirmEdgeWeight, 1, 0);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            // 
            // weightNumBox
            // 
            resources.ApplyResources(this.weightNumBox, "weightNumBox");
            this.weightNumBox.Maximum = new decimal(new int[] {
            2147483647,
            0,
            0,
            0});
            this.weightNumBox.Minimum = new decimal(new int[] {
            -2147483648,
            0,
            0,
            -2147483648});
            this.weightNumBox.Name = "weightNumBox";
            // 
            // confirmEdgeWeight
            // 
            resources.ApplyResources(this.confirmEdgeWeight, "confirmEdgeWeight");
            this.confirmEdgeWeight.Name = "confirmEdgeWeight";
            this.confirmEdgeWeight.UseVisualStyleBackColor = true;
            this.confirmEdgeWeight.Click += new System.EventHandler(this.confirmEdgeWeight_Click);
            // 
            // comboBoxCursors
            // 
            resources.ApplyResources(this.comboBoxCursors, "comboBoxCursors");
            this.comboBoxCursors.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxCursors.FormattingEnabled = true;
            this.comboBoxCursors.Items.AddRange(new object[] {
            resources.GetString("comboBoxCursors.Items"),
            resources.GetString("comboBoxCursors.Items1")});
            this.comboBoxCursors.Name = "comboBoxCursors";
            this.comboBoxCursors.SelectedIndexChanged += new System.EventHandler(this.comboBoxCursors_SelectedIndexChanged);
            // 
            // buttonClear
            // 
            resources.ApplyResources(this.buttonClear, "buttonClear");
            this.buttonClear.Name = "buttonClear";
            this.buttonClear.UseVisualStyleBackColor = true;
            this.buttonClear.Click += new System.EventHandler(this.buttonClear_Click);
            // 
            // buttonDelete
            // 
            resources.ApplyResources(this.buttonDelete, "buttonDelete");
            this.buttonDelete.Name = "buttonDelete";
            this.buttonDelete.UseVisualStyleBackColor = true;
            this.buttonDelete.Click += new System.EventHandler(this.buttonDelete_Click);
            // 
            // tableLayoutPanel1
            // 
            resources.ApplyResources(this.tableLayoutPanel1, "tableLayoutPanel1");
            this.tableLayoutPanel1.Controls.Add(this.panelColor, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.buttonColor, 0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            // 
            // panelColor
            // 
            resources.ApplyResources(this.panelColor, "panelColor");
            this.panelColor.Name = "panelColor";
            // 
            // buttonColor
            // 
            resources.ApplyResources(this.buttonColor, "buttonColor");
            this.buttonColor.Name = "buttonColor";
            this.buttonColor.UseVisualStyleBackColor = true;
            this.buttonColor.Click += new System.EventHandler(this.button1_Click);
            // 
            // groupBoxLang
            // 
            resources.ApplyResources(this.groupBoxLang, "groupBoxLang");
            this.groupBoxLang.Controls.Add(this.buttonEnglish);
            this.groupBoxLang.Controls.Add(this.buttonPolish);
            this.groupBoxLang.Name = "groupBoxLang";
            this.groupBoxLang.TabStop = false;
            // 
            // buttonEnglish
            // 
            resources.ApplyResources(this.buttonEnglish, "buttonEnglish");
            this.buttonEnglish.Name = "buttonEnglish";
            this.buttonEnglish.UseVisualStyleBackColor = true;
            this.buttonEnglish.Click += new System.EventHandler(this.buttonEnglish_Click);
            // 
            // buttonPolish
            // 
            resources.ApplyResources(this.buttonPolish, "buttonPolish");
            this.buttonPolish.Name = "buttonPolish";
            this.buttonPolish.UseVisualStyleBackColor = true;
            this.buttonPolish.Click += new System.EventHandler(this.buttonPolish_Click);
            // 
            // groupBoxIE
            // 
            resources.ApplyResources(this.groupBoxIE, "groupBoxIE");
            this.groupBoxIE.Controls.Add(this.buttonOpen);
            this.groupBoxIE.Controls.Add(this.buttonSave);
            this.groupBoxIE.Name = "groupBoxIE";
            this.groupBoxIE.TabStop = false;
            // 
            // buttonOpen
            // 
            resources.ApplyResources(this.buttonOpen, "buttonOpen");
            this.buttonOpen.Name = "buttonOpen";
            this.buttonOpen.UseVisualStyleBackColor = true;
            this.buttonOpen.Click += new System.EventHandler(this.buttonOpen_Click);
            // 
            // buttonSave
            // 
            resources.ApplyResources(this.buttonSave, "buttonSave");
            this.buttonSave.Name = "buttonSave";
            this.buttonSave.UseVisualStyleBackColor = true;
            this.buttonSave.Click += new System.EventHandler(this.buttonSave_Click);
            // 
            // Canvas
            // 
            resources.ApplyResources(this.Canvas, "Canvas");
            this.Canvas.Cursor = System.Windows.Forms.Cursors.Cross;
            this.Canvas.Name = "Canvas";
            this.Canvas.TabStop = false;
            this.Canvas.MouseClick += new System.Windows.Forms.MouseEventHandler(this.Canvas_MouseClick);
            this.Canvas.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.Canvas_MouseDoubleClick);
            this.Canvas.MouseMove += new System.Windows.Forms.MouseEventHandler(this.Canvas_MouseMove);
            this.Canvas.MouseUp += new System.Windows.Forms.MouseEventHandler(this.Canvas_MouseUp);
            // 
            // GraphForm
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tableLayoutMain);
            this.KeyPreview = true;
            this.Name = "GraphForm";
            this.Load += new System.EventHandler(this.GraphForm_Load);
            this.SizeChanged += new System.EventHandler(this.Form_SizeChanged);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Form_KeyDown);
            this.tableLayoutMain.ResumeLayout(false);
            this.tableLayoutRight.ResumeLayout(false);
            this.groupBoxEdit.ResumeLayout(false);
            this.tableLayoutPanel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.weightNumBox)).EndInit();
            this.tableLayoutPanel1.ResumeLayout(false);
            this.groupBoxLang.ResumeLayout(false);
            this.groupBoxIE.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.Canvas)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutMain;
        private System.Windows.Forms.TableLayoutPanel tableLayoutRight;
        private System.Windows.Forms.GroupBox groupBoxEdit;
        private System.Windows.Forms.Button buttonColor;
        private System.Windows.Forms.GroupBox groupBoxLang;
        private System.Windows.Forms.Button buttonEnglish;
        private System.Windows.Forms.Button buttonPolish;
        private System.Windows.Forms.GroupBox groupBoxIE;
        private System.Windows.Forms.Button buttonOpen;
        private System.Windows.Forms.Button buttonSave;
        private System.Windows.Forms.Panel panelColor;
        private System.Windows.Forms.PictureBox Canvas;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Button buttonClear;
        private System.Windows.Forms.Button buttonDelete;
        private System.Windows.Forms.ComboBox comboBoxCursors;
        private System.Windows.Forms.NumericUpDown weightNumBox;
        private System.Windows.Forms.Button confirmEdgeWeight;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
    }
}
