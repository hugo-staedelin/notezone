using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NoteZone
{
    class indice
    {
        // Structure de données de la class
        private int d_idx;
        // Constructeur de la classe
        public indice(int value)
        {
            d_idx = value;
        }
        // Méthodes de la classe
        public void setValue(int value)
        {
            d_idx = value;
        }
        public void increment()
        {
            d_idx++;
        }
        public void decrement()
        {
            d_idx--;
        }
        public int getIndice()
        {
            return d_idx;
        }
    }
    class customTab : TabPage
    {
        // Structure de données de la classe
        public RichTextBox d_textbox;
        private TabPage d_tab;
        private customTab d_customTab;
        // Constructeur de la classe
        public customTab()
        {
            d_textbox = new RichTextBox();
            Controls.Add(d_textbox);           // Rajout d'une textbox dans l'onglet
            d_textbox.Dock = DockStyle.Fill;        // La textbox s'étire sur l'entièreté de l'onglet
        }
        // Méthodes de la classe
        public void readText(string text)
        {
            d_textbox.Text = text;
        }
        public RichTextBox GetBox()
        {
            return d_textbox;
        }
        public void setCurrentTab(TabPage tab)
        {
            d_tab = tab;
        }
        public TabPage getCurrentTab()
        {
            return d_tab;
        }
        public bool isFirstTab() 
        {
            return d_tab.Text == "Nouveau 1";
        }
        public void createNewTab(TabControl tabControl, indice i)
        {
            d_customTab = new customTab();                         // Création d'un nouvel onglet
            tabControl.Controls.Add(d_customTab);                  // Rajout de l'onglet dans le control
            // Informations complémentaires sur l'onglet (coordonnées, noms, texte, etc)
            i.increment();
            d_customTab.Location = new System.Drawing.Point(4, 22);
            d_customTab.Name = "tabPage" + i.getIndice();
            d_customTab.Size = new System.Drawing.Size(792, 398);
            d_customTab.Padding = new System.Windows.Forms.Padding(3);
            d_customTab.Text = "Nouveau " + i.getIndice();
            tabControl.SelectedTab = d_customTab;                  // Sélection de l'onglet réalisé
        }
        public void removeTabPage(TabControl tabControl, indice i)
        {
            tabControl.Controls.Remove(d_tab);
            i.decrement();
        }
        public void resetTab(TabControl tabControl, indice i)
        {
            for (int j = i.getIndice() ; j > 1; --j)
            {
                tabControl.Controls.Remove(d_tab);
                i.decrement();
            }
        }
    }
    class TextManager
    {
        // Structure de données de la classe
        private string d_textContent;             // Chaine de caractère qui va sauvegarder ce qui est écrit
        // Constructeur de la classe
        public TextManager()
        {
            d_textContent = "";
        }
        // Méthodes de la classe
        public void saveCurrentText(RichTextBox richTextBox)
        {
            d_textContent = richTextBox.Text;     // Active la sauvegarde
        }
        public void loadSaveText(RichTextBox richTextBox)
        {
            richTextBox.Text = d_textContent;     // Charge la sauvegarde
        }
        public void resetText(RichTextBox richTextBox)
        {
            richTextBox.Text = "";              // Remet le controle à vide
        }
        public bool isTextBoxIsEmpty(RichTextBox richTextBox)
        {
            return richTextBox.Text == "";      // Teste si le controle est vide
        }
        public bool isTextBoxIsNotEmpty(RichTextBox richTextBox)
        {
            return richTextBox.Text != "";      // Teste si le controle dispose d'un ou plusieurs caractères
        }
    }
    class FileManager
    {
        // Structure de données de la classe
        private SaveFileDialog d_saveFileDialog;    // Dialog pour sauvegarde d'un fichier (équivalence du ofstream)
        private OpenFileDialog d_openFileDialog;    // Dialog pour lecture d'un fichier (équivalence du ifstream)
        private PrintDialog d_printDialog;          // Dialog pour impression
        private Stream d_stream;
        private StreamWriter d_streamWriter;            
        private string d_tabName;                   // Chaine de caractère pour le nom des onglets
        private TabPage d_tabPage;
        private customTab d_customTab;
        private FileExtensions d_fileExtension;
        // Constructeur de la classe
        public FileManager(){}
        // Méthodes de la classe
        public void saveFile(RichTextBox textBox)
        {
            d_fileExtension = new FileExtensions();
            d_saveFileDialog = new SaveFileDialog();                                        // Déclaration de l'objet
            d_saveFileDialog.Title = "Enregistrer sous";                                    // Titre de la fenêtre
            d_saveFileDialog.Filter = d_fileExtension.extensionList();                       // Sauvegarde/Filtre via les extensions suivantes
            d_saveFileDialog.DefaultExt = "txt";                                            // Extension par défaut
            if (d_saveFileDialog.ShowDialog() == DialogResult.OK){                          // Si la sauvegarde est executée
                using (d_stream = File.Open(d_saveFileDialog.FileName, FileMode.CreateNew)) // Ouvre le fichier en question, ou alors le créer
                using (d_streamWriter = new StreamWriter(d_stream)){
                    d_streamWriter.Write(textBox.Text);                                     // Le fichier aura le contenu de la RichTextBox passée en paramètre
                }
            }
        }
        public void loadFile(RichTextBox textBox,TabControl tabControl1, TabPage tabPage1, indice i)
        {
            d_fileExtension = new FileExtensions();
            d_openFileDialog = new OpenFileDialog();                            // Déclaration de l'objet
            d_openFileDialog.Title = "Ouvrir";                                  // Titre de la fenêtre
            d_openFileDialog.Filter = d_fileExtension.extensionList();           // Liste de toutes les extensions
            if (d_openFileDialog.ShowDialog() == DialogResult.OK)               // Si le fichier peut être lu
            {
                string fileName = File.ReadAllText(d_openFileDialog.FileName);  // Récupération du contenu du fichier
                if (tabPage1.Text != "Nouveau 1")                               // Si le premier onglet contient déjà le contenu d'un fichier, il réalise un nouvel onglet
                {
                    d_tabPage = new TabPage();
                    d_customTab = new customTab();
                    tabControl1.Controls.Add(d_tabPage);
                    d_customTab.readText(fileName);
                    d_tabPage.Controls.Add(d_customTab.GetBox());
                    d_tabPage.Location = new System.Drawing.Point(4, 22);
                    i.increment();
                    d_tabPage.Name = "tabPage" + i.getIndice();
                    d_tabPage.Padding = new System.Windows.Forms.Padding(3);
                    d_tabPage.Size = new System.Drawing.Size(792, 398);
                    d_tabPage.Text = d_openFileDialog.FileName;
                    d_tabName = d_tabPage.Text;                              // Récupération du nom de l'onglet
                    tabControl1.SelectedTab = d_tabPage;                     // Sélection du nouvel onglet
                }
                // Condition si page vide à faire
            }
            else { }
        }
        public string getFileNameForm()
        {
            return d_tabName + "- NoteZone";                                // [nomdufichier] - NoteZone
        }
        public void print()
        {
            d_printDialog = new PrintDialog();
            d_printDialog.ShowDialog();
        }
    }
    class FileExtensions
    {
        
        private string d_extensionStringList;
        public FileExtensions()
        {
            // Extensions de fichiers
            d_extensionStringList =
                "Normal text file|*.txt" +
                "|Unix script file|*.bash,*.sh,*.bsh,*.csh,*.bash_profile,*.bashrc,*.profile" +
                "|Batch file|*.bat,*.cmd" +
                "|C source file|*.c,*.lex" +
                "|C++ source file|*.h,*.hh,*.hpp,*.hxx,*.cpp,*.cxx,*.cc,*.ino" +
                "|C# source file|*.cs" +
                "|All types (*.*)| *.*";
        }
        public string extensionList()
        {
            return d_extensionStringList;                                   // Retourne la liste des extensions ci-dessus
        }
    }
    class Version
    {
        private string d_version;
        private DateTime d_dateTime;
        public Version()
        {
            d_version = "1.0.0" + d_dateTime.Date;
        }
        public string getVersion()
        {
            return d_version;
        }
    }
}
