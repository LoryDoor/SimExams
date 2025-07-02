using SalaSimulazione.Model;
using System;
using System.IO;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace SalaSimulazione.View
{
    /// <summary>
    /// Logica di interazione per ImageViewer.xaml
    /// </summary>
    public partial class ImageViewer : Window
    {
        private double ZoomFactor = 1.0;

        public ImageViewer(string imagePath)
        {
            InitializeComponent();
            // Inizializziamo la trasformazione sull'immagine
            ImmagineVisualizzata.LayoutTransform = new ScaleTransform(ZoomFactor, ZoomFactor);
            ImmagineVisualizzata.RenderTransformOrigin = new System.Windows.Point(0.5, 0.5);
            // Impostiamo l'immagine da visualizzare
            BitmapImage image = new BitmapImage();
            image.BeginInit();
            image.CacheOption = BitmapCacheOption.OnLoad;
            image.UriSource = new Uri($"file:///{Path.GetFullPath(imagePath)}");
            image.EndInit();
            image.Freeze();
            ImmagineVisualizzata.Source = image;
            //Console.WriteLine($"ImageViewer: Apertura dell'immagine {Path.GetFullPath(imagePath)} riuscita.");
        }

        // Metodo chiamato quando clicchiamo sul pulsante per aumentare lo zoom dell'immagine.
        private void btnZoomIn_Click(object sender, RoutedEventArgs e)
        {
            // Aumentiamo il fattore di zoom, assicurandoci che non superi il massimo consentito
            ZoomFactor = Math.Min(ZoomFactor + Libreria.Stile.ImageZoom.Step, Libreria.Stile.ImageZoom.MaxZoom);
            AggiornaZoom();
        }

        // Metodo chiamato quando clicchiamo sul pulsante per diminuire lo zoom dell'immagine.
        private void btnZoomOut_Click(object sender, RoutedEventArgs e)
        {
            // Diminuiamo il fattore di zoom, assicurandoci che non scenda al di sotto del minimo consentito.
            ZoomFactor = Math.Max(ZoomFactor - Libreria.Stile.ImageZoom.Step, Libreria.Stile.ImageZoom.MinZoom);
            AggiornaZoom();
        }

        //Metodo chiamato per aggiornare lo zoom dell'immagine.
        private void AggiornaZoom()
        {
            if (ImmagineVisualizzata.LayoutTransform is ScaleTransform scale)
            {
                // Aggiorniamo il fattore di scala dell'immagine in base al nuovo zoom.
                scale.ScaleX = ZoomFactor;
                scale.ScaleY = ZoomFactor;
            }
        }
    }
}

// I pugs sono passati di qui
// Davide Baldinu, Giada Croci, Lorenzo Porta