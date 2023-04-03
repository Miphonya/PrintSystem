using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing.Printing;
using System.IO;
using PdfiumViewer;
using System.Reflection.Emit;
using Microsoft.VisualBasic;


namespace PrintSystemGUI
{
    public partial class Form1 : Form
    {

        // Les valeurs par défaut
        PrinterSettings settings = new PrinterSettings();
        string defaultPrinterName;
        string printerName = null;
        bool colorIsMonochrome = true;
        Duplex isPrintDuplex = Duplex.Simplex;
        string pdfFile = null;
        private string selectedFile; // variable de classe pour stocker la valeur de selectedFile
        private string folderPath;
        List<string> selectedFiles = new List<string>();




        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            defaultPrinterName = settings.PrinterName;
            printerName = defaultPrinterName;
            materialLabel1.Text = printerName;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            // Afficher le guide d'utilisation
            MessageBox.Show("Explanation of the arguments : \n\n printer=<name and address of the printer> specifies the name of the printer (example: \\\\papercut.entreprise.exemple.ch\\myPrinter). (By default: Default printer)\n\n monochrome=<true/false> specifies if the monochrome mode is disable (by default: true)\n\n isPrintDuplex = <Duplex.Simplex/Duplex.Vertical> Specifies whether the duplex printing mode is set to off or to vertical duplex printing.\n\n file directory=<path to the PDF file> specifies the path to the PDF file to print (example: C:\\exemple\\my.pdf). \n\n V1.1.0 ");
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            // Mettre à jour la valeur de colorIsMonochrome en fonction de l'état de la case à cocher
            
                
        }
        private void button5_Click(object sender, EventArgs e)
        {
            string userInput = Microsoft.VisualBasic.Interaction.InputBox("Please enter the printer address.\n\n For example: \\\\printserver.example.com\\myprinter. \n\n For use default printer leave the text field blank and click OK or Cancel.", "Change printer 🖨", "");
            // MessageBox.Show("You have entered: " + userInput);

            printerName = userInput;

            if (string.IsNullOrEmpty(userInput))
            {
                printerName = settings.PrinterName;
            }


            // utiliser la propriété Text du contrôle Material Label pour afficher la valeur de la variable
            materialLabel1.Text = printerName;


        }




        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }


        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void button4_Click(object sender, EventArgs e)
        {
            selectedFiles.Clear();
            pdfFile = "";
            materialLabel3.Text = pdfFile;
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "PDF files (*.pdf)|*.pdf";
            openFileDialog.Multiselect = true;
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                // Ajouter les fichiers sélectionnés à la liste
                selectedFiles.AddRange(openFileDialog.FileNames);
            }

            // Utiliser la propriété Text du contrôle Material Label pour afficher la valeur de la variable
            materialLabel3.Text = string.Join(", ", selectedFiles);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            // Si aucun fichier PDF n'a été sélectionné, lever une exception
            if (selectedFiles.Count == 0)
            {
                MessageBox.Show("L'emplacement du fichier PDF doit être spécifié.", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (checkBox1.Checked)
            {
                colorIsMonochrome = false;
            }

            if (materialCheckBox2.Checked)
            {
                isPrintDuplex = Duplex.Vertical;
            }

            // Afficher les valeurs stockées
            // MessageBox.Show($"Nom de l'imprimante: {printerName}\n" + $"Mode monochrome: {colorIsMonochrome}\n" + $"Fichier PDF: {pdfFile}", "Mes valeurs");

                // Créer une instance de la classe PrintDocument
            PrintDocument pd = new PrintDocument();

            // Spécifier le nom de l'imprimante
            pd.PrinterSettings.PrinterName = printerName;

            // Spécifier le mode d'impression monochrome
            pd.PrinterSettings.DefaultPageSettings.Color = colorIsMonochrome;

            // Spécifier le mode d'impression recto-verso (duplex)
            pd.PrinterSettings.Duplex = isPrintDuplex;

            // Spécifier le format du papier
            pd.DefaultPageSettings.PaperSize = new PaperSize("A4", 827, 1169);

            foreach (string pdfFile in selectedFiles)
            {
                if (!File.Exists(pdfFile))
                {
                    MessageBox.Show("Le fichier PDF n'existe pas à l'emplacement spécifié.", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // Charger le document PDF
                PdfDocument pdfDocument = PdfDocument.Load(pdfFile);

                // Créer un objet PrintDocument à partir du document PDF
                PrintDocument printDocument = pdfDocument.CreatePrintDocument();

                // Assigner les paramètres d'impression sauvegardés auparavant
                printDocument.PrinterSettings = pd.PrinterSettings;
                printDocument.Print();
                if (materialCheckBox1.Checked)
                {
                    pdfDocument.Dispose();
                    File.Delete(pdfFile);
                    materialLabel3.Text = " ";
                }
                else
                {
                    pdfDocument.Dispose();
                }
            }

            // Réinitialiser la liste des fichiers sélectionnés
            selectedFiles.Clear();
            pdfFile = "";
            materialLabel3.Text= pdfFile;
        }



        private void materialSingleLineTextField1_Click(object sender, EventArgs e)
        {

        }

        private void materialLabel1_Click(object sender, EventArgs e)
        {

        }

        private void materialLabel2_Click(object sender, EventArgs e)
        {

        }

        private void materialCheckBox1_CheckedChanged(object sender, EventArgs e)
        {

        }


        private void materialFlatButton1_Click(object sender, EventArgs e)
        {

        }

        private void materialRaisedButton1_Click(object sender, EventArgs e)
        {

        }

        private void materialLabel3_Click(object sender, EventArgs e)
        {

        }

        private void label1_Click_1(object sender, EventArgs e)
        {

        }

        private void materialLabel1_Click_1(object sender, EventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {
            selectedFiles.Clear();
            pdfFile = "";
            materialLabel3.Text = pdfFile;
            FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog();
            folderBrowserDialog.ShowNewFolderButton = false;
            if (folderBrowserDialog.ShowDialog() == DialogResult.OK)
            {
                string selectedPath = folderBrowserDialog.SelectedPath;
                materialLabel3.Text = "folder: " + selectedPath;

                string[] pdfFiles = Directory.GetFiles(selectedPath, "*.pdf");
                if (pdfFiles.Length > 0)
                {
                    // stocker les noms de fichiers PDF dans une liste
                    List<string> pdfFileList = new List<string>(pdfFiles);

                    // parcourir la liste de fichiers PDF
                    foreach (string fileName in pdfFileList)
                    {
                        // afficher le nom du fichier PDF dans la console
                        // MessageBox.Show(fileName);
                    }

                    // Stocker la liste de fichiers PDF dans une variable de classe
                    selectedFiles = pdfFileList;
                }
                else
                {
                    // aucun fichier PDF trouvé dans le dossier sélectionné
                    selectedFiles = null;
                }
            }
        }

        private void materialCheckBox2_CheckedChanged(object sender, EventArgs e)
        {

        }
    }
}

