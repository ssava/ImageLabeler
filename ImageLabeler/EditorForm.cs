using ImageLabeler.Filters;
using ImageLabeler.Objects;
using ImageLabeler.Services;
using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace ImageLabeler
{
    public partial class EditorForm : Form
    {
        readonly PictureBox pBox;
        EditorDataSet t;
        EditorBox renderedSelectedBox;

        // TODO: Refactor (configure DI)
        IDatasetService datasetService = new DatasetService();

        public EditorForm()
        {
            InitializeComponent();

            panel1.AutoScroll = true;

            pBox = new PictureBox();
            panel1.Controls.Add(pBox);

            pBox.SizeMode = PictureBoxSizeMode.AutoSize;

            editorStatusLabel.Text = string.Empty;

            pBox.MouseClick += OnImageClick;

        }

        private void OnImageClick(object sender, MouseEventArgs e)
        {
            if (sender == null)
                return;

            if (pBox.Image == null)
                return;

            EditorImage t = (EditorImage)treeView1.SelectedNode.Tag;

            if (t == null)
                return;

            if (e.Button == MouseButtons.Left)
            {

                /* Box su cui si è cliccato null altrimenti */
                EditorBox clickedBox = t.GetBox(e.X, e.Y);

                /* Se la zona su cui si è cliccato non ha una box */
                if (clickedBox == EditorBox.None)
                {
                    t.Boxes.FindAll(b => b.IsSelected).ForEach(b => b.ChangeState(t));
                    return;
                }

                if (renderedSelectedBox != clickedBox)
                {
                    if (renderedSelectedBox != EditorBox.None)
                        renderedSelectedBox?.ChangeState(t);
                }

                renderedSelectedBox = clickedBox;

                Image rendered = renderedSelectedBox?.ChangeState(t); //365ms

                if (rendered == null)
                    return;

                /* Se è stata selezionata una box imposta l'editor dati */
                SetEditorData(rendered);                              // 1270ms

                pBox.Image.Dispose();
                pBox.Image = rendered;
                boxEditor.Enabled = true;

                return;
            }

            if (e.Button == MouseButtons.Right)
            {
                FilterOptions fOpts = new FilterOptions();
                Bitmap bmpMask = new Bitmap(t.File);

                EditorImage dImg = (EditorImage)treeView1.SelectedNode.Tag;

                bmpMask = new Bitmap(t.File);

                FillRegionDetector det = new FillRegionDetector(e.Location);

                bmpMask = new ContrastFilter(0.8f).Apply(bmpMask);
                det.Apply(bmpMask);

                Rectangle area = det.Detected;

                if (area.Width <= 0 || area.Height <= 0)
                {
                    MessageBox.Show(@"No valid box detected!", @"No box detected", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    bmpMask.Dispose();
                    return;
                }

                dImg.AddBox(new EditorBox(new DataBox
                {
                    Top = area.Top,
                    Left = area.Left,
                    Height = area.Height,
                    Width = area.Width,
                    Label = "new box"
                }));


                bmpMask.Dispose();
                pBox.Image.Dispose();
                pBox.Image = dImg.Render();

                return;
            }
        }

        private void SetEditorData(Image rendered)
        {
            topXnum.Maximum = rendered.Width;
            topYnum.Maximum = rendered.Height;
            widthNum.Maximum = rendered.Width;
            heightNum.Maximum = rendered.Height;

            topXnum.Value = renderedSelectedBox.Left;
            topYnum.Value = renderedSelectedBox.Top;
            widthNum.Value = renderedSelectedBox.Width;
            heightNum.Value = renderedSelectedBox.Height;
            labelTxt.Text = renderedSelectedBox.Label;
        }

        private void TreeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {
            if (sender == null)
                return;

            if (e.Node.Tag == null)
                return;

            EditorImage dImg = (EditorImage)e.Node.Tag;

            pBox.Image?.Dispose();
            pBox.Image = dImg.Render();

            editorStatusLabel.Text = string.Format("Image {0} has {1} boxes", dImg.ToString(), dImg.Boxes.Count);
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog fDlg = new OpenFileDialog
            {
                Filter = "XML Files (*.xml)|*.xml"
            };

            if (fDlg.ShowDialog() != DialogResult.OK)
                return;

            treeView1.Nodes.Clear();

            t = EditorDataSet.Load(datasetService, fDlg.FileName);

            TreeNode root = treeView1.Nodes.Add("{0} - [{1} image(s)]", t.Name, t.Images.Count);

            foreach (var dsImg in t.Images)
                root.Nodes.Add(new TreeNode
                {
                    Tag = dsImg,
                    Text = dsImg.ToString()
                });

            labelTxt.Items.AddRange(t.Labels);

            editorStatusLabel.Text = string.Format(@"Dataset {0} loaded. {1} images", t.Name, t.Images.Count);
        }

        private void GroupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void SetCurrentBoxSize(object sender, EventArgs e)
        {
            if (sender == null)
                return;

            if (renderedSelectedBox == null)
                return;

            if (!renderedSelectedBox.IsSelected)
                return;

            NumericUpDown sendObj = (NumericUpDown)sender;

            string name = sendObj.Name;
            int top = renderedSelectedBox.Top;
            int left = renderedSelectedBox.Left;
            int width = renderedSelectedBox.Width;
            int height = renderedSelectedBox.Height;

            switch (name)
            {
                case "topXnum":
                    left = Convert.ToInt32(sendObj.Value);
                    break;

                case "topYnum":
                    top = Convert.ToInt32(sendObj.Value);
                    break;

                case "widthNum":
                    width = Convert.ToInt32(sendObj.Value);
                    break;

                case "heightNum":
                    height = Convert.ToInt32(sendObj.Value);
                    break;
            }

            Rectangle newBox = new Rectangle(left, top, width, height);

            renderedSelectedBox.SetRectangle(newBox);
            RenderCurrentImage();
        }

        private void RenderCurrentImage()
        {
            EditorImage dImg = (EditorImage)treeView1.SelectedNode.Tag;

            pBox.Image = dImg.Render();
        }

        private void BtnApply_Click(object sender, EventArgs e)
        {
            renderedSelectedBox.SetLabel(labelTxt.Text);
            datasetService.Save(t.DlibObject);
            EditorImage dImg = (EditorImage)treeView1.SelectedNode.Tag;
            pBox.Image = renderedSelectedBox.ChangeState(dImg);
        }

        private void AddImageToDatasetToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (sender == null)
                return;

            OpenFileDialog fDlg = new OpenFileDialog();

            if (fDlg.ShowDialog() != DialogResult.OK)
                return;

            AddImageToDataset(fDlg.FileName);
        }

        private void AddImageToDataset(string fPath)
        {
            try
            {
                if (t == null)
                {
                    t = new EditorDataSet(new DataSet
                    {
                        Name = @"Temp",
                        Comment = @"Draft"
                    });
                }

                EditorImage addImg = t.AddImage(fPath);

                treeView1.Nodes.Clear();

                TreeNode root = treeView1.Nodes.Add("{0} - [{1} image(s)]", t.Name, t.Images.Count);

                foreach (var dsImg in t.Images)
                    root.Nodes.Add(new TreeNode
                    {
                        Tag = dsImg,
                        Text = dsImg.ToString()
                    });
            }
            catch (Exception)
            {

            }
        }

        private void SaveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (sender == null)
                return;

            if (t == null)
                return;

            datasetService.Save(t.DlibObject);
        }

        private void BtnCreate_Click(object sender, EventArgs e)
        {
            if (sender == null)
                return;

            if (renderedSelectedBox != null)
                return;

            EditorImage dImg = (EditorImage)treeView1.SelectedNode.Tag;

            dImg.AddBox(new EditorBox(new DataBox
            {
                Top = 100,
                Left = 100,
                Height = 200,
                Width = 100,
                Label = "new box"
            }));

            pBox.Image = dImg.Render();
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            return;
        }

        private void Button1_Click_1(object sender, EventArgs e)
        {
            if (sender == null)
                return;

            if (renderedSelectedBox == EditorBox.None)
                return;

            EditorImage dImg = (EditorImage)treeView1.SelectedNode.Tag;

            Image region = dImg.GetImage(renderedSelectedBox);

            SaveFileDialog sfdlg = new SaveFileDialog();

            if (sfdlg.ShowDialog() != DialogResult.OK)
                return;

            if (string.IsNullOrEmpty(sfdlg.FileName))
                return;

            region.Save(sfdlg.FileName);
            region.Dispose();
        }

        private void AddFolderToDatasetToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (sender == null)
                return;

            FolderBrowserDialog fDlg = new FolderBrowserDialog();

            if (fDlg.ShowDialog() != DialogResult.OK)
                return;

            if (string.IsNullOrEmpty(fDlg.SelectedPath) && string.IsNullOrWhiteSpace(fDlg.SelectedPath))
                return;

            string[] files = Directory.GetFiles(fDlg.SelectedPath);

            foreach(string fPath in files)
            {
                AddImageToDataset(fPath);
            }
        }
    }
}
