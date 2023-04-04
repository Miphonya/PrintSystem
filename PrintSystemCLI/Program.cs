using System;
using System.Collections.Generic;
using System.Drawing.Printing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrintSystemCLI
{
    class Program
    {
        static void Main(string[] args)
        {
            // Les valeurs par défaut
            PrinterSettings settings = new PrinterSettings();
            string defaultPrinterName = settings.PrinterName;
            string printerName = defaultPrinterName;
            bool colorIsMonochrome = false;
            bool needhelp = false;
            Duplex Printduplex = Duplex.Simplex;
            string folderPath = "";
            Console.WriteLine($"ici{folderPath}");

            foreach (string arg in args)
            {
                string[] parts = arg.Split('=');
                if (parts.Length == 2)
                {
                    string key = parts[0].Trim();
                    string value = parts[1].Trim();
                    switch (key)
                    {
                        case "--printer":
                            printerName = value;
                            break;
                        case "--monochrome":
                            colorIsMonochrome = bool.Parse(value);
                            break;
                        case "--isPrintduplex":
                            Printduplex = value.ToLower() == "vertical" ? Duplex.Vertical : Duplex.Simplex;
                            break;
                        case "--folderPath":
                            folderPath = value;
                            break;
                        case "--help":
                            needhelp = bool.Parse(value);
                            break;
                        default:
                            Console.WriteLine($"Argument inconnu : {key}={value}");
                            break;
                    }
                }
            }

            if(needhelp == true)
            {
                Console.WriteLine("Explanation of the arguments : \n\n\t printer=<name and address of the printer> specifies the name of the printer (example: \\\\papercut.entreprise.exemple.ch\\myPrinter). (By default: Default printer)\n\n\t monochrome=<true/false> specifies if the monochrome mode is disable (by default: true)\n\n\t isPrintDuplex = <Duplex.Simplex/Duplex.Vertical> Specifies whether the duplex printing mode is set to off or to vertical duplex printing.\n\n\t file directory=<path to the PDF file> specifies the path to the PDF file to print (example: C:\\exemple\\my.pdf). \n\n V1.0.0 ");
                Console.WriteLine("Press any key to exit...");
                Console.ReadKey(); // Attend que l'utilisateur appuie sur une touche
                Environment.Exit(1);
            }


            // Afficher les valeurs des arguments
            Console.WriteLine($"Imprimante:\t{printerName}");
            Console.WriteLine($"Monochrome:\t{colorIsMonochrome}");
            Console.WriteLine($"Duplex:\t\t{Printduplex}");
            Console.WriteLine($"Chemin du dossier: {folderPath}");

            // Console.WriteLine("Press any key to exit...");
            // Console.ReadKey(); // Attend que l'utilisateur appuie sur une touche

            // Si l'imprimante spécifiée n'est pas disponible, utiliser l'imprimante par défaut
            if (!PrinterSettings.InstalledPrinters.Cast<string>().Contains(printerName))
            {
                Console.WriteLine($"Imprimante non disponible: {printerName}. Utilisation de l'imprimante par défaut: {defaultPrinterName}");
                printerName = defaultPrinterName;
            }

            if (folderPath == null)
            {
                Console.WriteLine("Veuillez spécifier le chemin du dossier contenant les fichiers PDF à imprimer.");
                folderPath = Console.ReadLine();
            }

            if (!Directory.Exists(folderPath))
            {
                Console.WriteLine($"Dossier introuvable: {folderPath}");
                Environment.Exit(1);
            }

            string[] files = Directory.GetFiles(folderPath, "*.pdf");
            if (files.Length == 0)
            {
                Console.WriteLine($"Aucun fichier PDF trouvé dans le dossier: {folderPath}");
                Environment.Exit(1);
            }
            Console.WriteLine($"Nombre de fichiers PDF trouvés dans le dossier : {files.Length}");
            foreach (string file in files)
            {
                Console.WriteLine($"Impression du fichier : {file}");

                // Créer une instance de la classe PrintDocument
                PrintDocument pd = new PrintDocument();

                // Spécifier le nom de l'imprimante
                pd.PrinterSettings.PrinterName = printerName;

                // Spécifier le mode d'impression monochrome
                pd.PrinterSettings.DefaultPageSettings.Color = !colorIsMonochrome;

                // Spécifier le mode d'impression recto-verso (duplex)
                pd.PrinterSettings.Duplex = Printduplex;

                // Spécifier le format du papier
                pd.DefaultPageSettings.PaperSize = new PaperSize("A4", 827, 1169);

                // Charger le document PDF
                PdfiumViewer.PdfDocument pdfDocument = PdfiumViewer.PdfDocument.Load(file);

                // Créer un objet PrintDocument à partir du document PDF
                PrintDocument printDocument = pdfDocument.CreatePrintDocument();

                // Imprimer le document
                printDocument.Print();

                // Libérer les ressources
                pdfDocument.Dispose();
                printDocument.Dispose();
            }
        }
    }
}
