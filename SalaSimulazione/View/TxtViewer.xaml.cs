using SalaSimulazione.Model;
using System;
using System.IO;
using System.Windows;

namespace SalaSimulazione.View
{
    /// <summary>
    /// Logica di interazione per TxtViewer.xaml
    /// </summary>
    public partial class TxtViewer : Window
    {
        public TxtViewer(string filePath)
        {
            //Inizializza i componenti dell'interfaccia e legge il contenuto del file di testo e lo visualizza nella TextBox
            InitializeComponent();
            TextContent.Text = File.ReadAllText(filePath);
            //Console.WriteLine($"TxtViewer: Apertura del file {Path.GetFullPath(filePath)} riuscita.");
        }

        private void btnZoomIn_Click(object sender, RoutedEventArgs e)
        {
            // Controlla se la dimensione del font è inferiore alla dimensione massima
            if (TextContent.FontSize < Libreria.Stile.TxtZoom.MaxFontSize) // Controlla se la dimensione del font è inferiore alla dimensione massima
            {
                TextContent.FontSize += Libreria.Stile.TxtZoom.Step; // Aumenta la dimensione del font
            }
        }

        private void btnZoomOut_Click(object sender, RoutedEventArgs e)
        {
            // Controlla se la dimensione del font è superiore alla dimensione minima
            if (TextContent.FontSize > Libreria.Stile.TxtZoom.MinFontSize) // Controlla se la dimensione del font è superiore alla dimensione minima
            {
                TextContent.FontSize -= Libreria.Stile.TxtZoom.Step; // Diminuisce la dimensione del font
            }
        }
    }
}

// I pugs sono passati di qui
// Davide Baldinu, Giada Croci, Lorenzo Porta