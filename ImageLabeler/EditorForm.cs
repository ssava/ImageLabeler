using ImageLabeler.Objects;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace ImageLabeler
{
    public partial class EditorForm : Form
    {
        PictureBox pBox;
        EditorDataSet t;
        EditorBox sbox;

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
                EditorBox curr = t.GetBox(e.X, e.Y);

                /* Se la zona su cui si è cliccato non huna box; ritorna */
                if (curr == EditorBox.None)
                    return;

                /* Se la EditorBox corrente non è uguale all'ultima usata, */
                if (!curr.Equals(sbox))
                {
                    /* In precedenza ne era già state selezionata un'altra, */
                    if (sbox != null)
                        /* E l'ultima è attualmente selezionata. Deselezionala */
                        if (sbox.IsSelected)
                        {
                            pBox.Image = sbox.ChangeState(t);
                            boxEditor.Enabled = false;
                        }

                }

                /* Imposta l'ultima selezionata con la corrente */
                sbox = curr;

                /* Cambia il suo stato */
                Image rendered = sbox.ChangeState(t);

                /* Se è stata selezionata una box imposta l'editor dati */
                topXnum.Maximum = rendered.Width;
                topYnum.Maximum = rendered.Height;
                widthNum.Maximum = rendered.Width;
                heightNum.Maximum = rendered.Height;

                topXnum.Value = sbox.Left;
                topYnum.Value = sbox.Top;
                widthNum.Value = sbox.Width;
                heightNum.Value = sbox.Height;
                labelTxt.Text = sbox.Label;

                pBox.Image.Dispose();
                pBox.Image = rendered;
                boxEditor.Enabled = true;

                return;
            }

            if(e.Button == MouseButtons.Right)
            {
                //if (sbox != null)
                //    return;

                EditorImage dImg = (EditorImage)treeView1.SelectedNode.Tag;

                dImg.AddBox(new EditorBox(new DataBox
                {
                    Top = e.Y,
                    Left = e.X,
                    Height = 200,
                    Width = 100,
                    Label = "new box"
                }));

                pBox.Image.Dispose();
                pBox.Image = dImg.Render();

                return;
            }
        }

        private void treeView1_AfterSelect(object sender, TreeViewEventArgs e)
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

            t = EditorDataSet.Load(fDlg.FileName);

            TreeNode root = treeView1.Nodes.Add(t.Name);

            foreach (var dsImg in t.Images)
                root.Nodes.Add(new TreeNode
                {
                    Tag = dsImg,
                    Text = dsImg.ToString()
                });

            editorStatusLabel.Text = string.Format(@"Dataset {0} loaded. {1} images", t.Name, t.Images.Count);
        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void SetCurrentBoxSize(object sender, EventArgs e)
        {
            if (sender == null)
                return;

            if (sbox == null)
                return;

            if (!sbox.IsSelected)
                return;

            NumericUpDown sendObj = (NumericUpDown)sender;

            string name = sendObj.Name;
            int top = sbox.Top;
            int left = sbox.Left;
            int width = sbox.Width;
            int height = sbox.Height;

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

            sbox.SetRectangle(newBox);
            RenderCurrentImage();
        }

        private void RenderCurrentImage()
        {
            EditorImage dImg = (EditorImage)treeView1.SelectedNode.Tag;

            pBox.Image = dImg.Render();
        }

        private void btnApply_Click(object sender, EventArgs e)
        {
            sbox.SetLabel(labelTxt.Text);
            t.Save();
        }

        private void addImageToDatasetToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (sender == null)
                return;

            OpenFileDialog fDlg = new OpenFileDialog();

            if (fDlg.ShowDialog() != DialogResult.OK)
                return;

            try
            {
                EditorImage addImg = t.AddImage(fDlg.FileName);

                treeView1.Nodes.Clear();

                TreeNode root = treeView1.Nodes.Add(t.Name);

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

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (sender == null)
                return;

            if (t == null)
                return;

            t.Save();
        }

        private void btnCreate_Click(object sender, EventArgs e)
        {
            if (sender == null)
                return;

            if (sbox != null)
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
    }
}
