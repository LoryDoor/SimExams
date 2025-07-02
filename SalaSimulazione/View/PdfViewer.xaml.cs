using PdfiumViewer;
using System;
using System.IO;
using System.Windows;

namespace SalaSimulazione.View
{
    /// <summary>
    /// Logica di interazione per Window1.xaml
    /// </summary>
    public partial class PdfViewerWindow : Window
    {
        PdfDocument Document;
        public PdfViewerWindow(string pdfPath)
        {
            InitializeComponent();

            PdfViewer viewer = new PdfViewer
            {
                // Imposta il dock del visualizzatore PDF per riempire l'intera finestra
                Dock = DockStyle.Fill
            };

            // Aggiunge il visualizzatore PDF all'host del controllo
            // successivamente carica il documento PDF dal percorso specificato
            // ed infine imposta il documento PDF nel visualizzatore
            pdfHost.Child = viewer;
            Document = PdfDocument.Load(pdfPath);
            viewer.Document = Document;
            //Console.WriteLine($"PdfViewer: Apertura del file PDF {Path.GetFullPath(pdfPath)} riuscita.");
        }

        // Metodo chiamato quando la finestra viene chiusa.
        private void Window_Closed(object sender, EventArgs e)
        {
            // Rilascia le risorse del documento PDF quando la finestra viene chiusa
            Document.Dispose();
        }
    }
}

// I pugs sono passati di qui
// Davide Baldinu, Giada Croci, Lorenzo Porta