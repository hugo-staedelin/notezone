using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NoteZone
{
    public partial class Form1 : Form
    {
        indice d_i = new indice(1);
        RichTextBox d_richTextBox;
        FileManager d_fileManager = new FileManager();
        TextManager d_textManager = new TextManager();
        customTab d_curTab = new customTab();
        public Form1()
        {
            InitializeComponent();
            this.Text = "Nouveau 1 - NoteZone";                   // [nomdufichier] - NoteZone
        }
        public void richTextBox1_TextChanged(object sender, EventArgs e)
        {
            d_richTextBox = (RichTextBox)sender;
            if (d_richTextBox.Text != "")
            { 
                this.Text = "*" + tabPage1.Text + " - NoteZone";  // L'application récupère le nom du fichier + le nom de l'app
                undoBtn.Enabled = true;                           // Activation bouton annuler
                annulerToolStripMenuItem.Enabled = true;          //
                redoBtn.Enabled = false;                          // Désactivation bouton rétablir
                rétablirToolStripMenuItem.Enabled = false;        // 
                cutBtn.Enabled = true;                            // Activation du bouton couper
            }
        }
        private void ShowMessage(object sender, EventArgs e)
        {
            MessageBox.Show("Message");
        }
        private void newTabBtn_Click(object sender, EventArgs e)
        {
            d_curTab.createNewTab(tabControl1, d_i);         // Création d'un nouvel onglet

        }
        private void saveFileBtn_Click(object sender, EventArgs e)
        {
            d_fileManager.saveFile(d_richTextBox);            // Sauvegarde du fichier
            this.Text = d_fileManager.getFileNameForm();     // [nomdufichier] - NoteZone
        }
        private void loadFileBtn_Click(object sender, EventArgs e)
        {
            d_fileManager.loadFile(d_richTextBox, tabControl1, d_curTab, d_i); // Ouvrir un fichier
            //this.Text = d_fileManager.getFileNameForm();                     // [nomdufichier] - NoteZone
        }
        private void closeTabBtn_Click(object sender, EventArgs e)
        {
            if (d_i.getIndice() > 1)
            {
                d_curTab.setCurrentTab(tabControl1.SelectedTab);
                d_curTab.removeTabPage(tabControl1, d_i);           // Suppression de l'onglet selectionné (via menu)
            }
        }
        private void closeAllTabBtn_Click(object sender, EventArgs e)
        {
            d_curTab.resetTab(tabControl1, d_i);        
        }
        private void quitterToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
        private void undoBtn_Click(object sender, EventArgs e)
        {
            undoBtn.Enabled = false;
            annulerToolStripMenuItem.Enabled = false;
            rétablirToolStripMenuItem.Enabled = true;
            redoBtn.Enabled = true;
            d_textManager.saveCurrentText(d_richTextBox);
            d_textManager.resetText(d_richTextBox);
        }
        private void redoBtn_Click(object sender, EventArgs e)
        {
            d_textManager.loadSaveText(d_richTextBox);
            redoBtn.Enabled = false;
            undoBtn.Enabled = true;
            annulerToolStripMenuItem.Enabled = true;
            rétablirToolStripMenuItem.Enabled = false;
        }
        private void tabControl1_MouseClick(object sender, MouseEventArgs e)
        {
            //
            // Suppression d'un onglet via le bouton molette de la souris
            //
            var tabControl = sender as TabControl;
            var tabs = tabControl.TabPages;
            if (e.Button == MouseButtons.Middle)
            {
                if (d_i.getIndice() > 1 && tabControl1.SelectedTab != d_curTab.getCurrentTab())
                {
                    tabs.Remove(tabs.Cast<TabPage>().Where((t, i) => tabControl.GetTabRect(i).Contains(e.Location)).First());
                    d_i.decrement();
                }
            }
        }
        private void tabControl1_Selecting(object sender, TabControlCancelEventArgs e)
        {
            try
            {
                TabPage currentTab = tabControl1.SelectedTab;     // Sauvegarde de l'onglet selectionné
                this.Text = currentTab.Text + " - NoteZone";      // [nomdufichier] - NoteZone, selon l'onglet sélectionné
            }
            catch(Exception ex)
            {
                ex.ToString();
            }
        }
        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            /*
            DialogResult result = MessageBox.Show("Sauvegarder le fichier 'FICHIER' ?", "Sauvegarder", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {
                Application.Exit();
            }
            else if (result == DialogResult.No || result == DialogResult.Cancel)
            {

            }
            */
        }
        private void àProposDeNoteZoneToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AboutBox1 aboutBox1 = new AboutBox1();
            aboutBox1.Show();
        }

        private void printFileBtn_Click(object sender, EventArgs e)
        {
            d_fileManager.print();
        }
    }
    
}
