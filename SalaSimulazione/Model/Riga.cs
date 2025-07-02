using SalaSimulazione.View;
using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace SalaSimulazione.Model
{
    public class Riga
    {
        private string Tipo; // Tipo di file.
        private string Nome; // Nome del file senza estensione.
        private int NumeroRiga; // Numero della riga nella tabella dinamica zero-based.
        private System.Windows.Controls.Image ImmagineTipo; // Immagine rappresentante il tipo di file.
        private Border BordoImmagine; // Contenitore border per l'immagine del tipo di file.
        private System.Windows.Controls.Label LabelNome; // Label per il nome del file.
        private System.Windows.Controls.Button BtnVisualizza; // Bottone per visualizzare il file.

        public Riga(string tipo, string nome, int numeroRiga)
        {
            Tipo = tipo;
            Nome = nome;
            NumeroRiga = numeroRiga;

            // Creazione dell'immmagine rappresentante il tipo di file.
            ImmagineTipo = new System.Windows.Controls.Image
            {
                // Modifica le propriet� dell'immagine.
                HorizontalAlignment = System.Windows.HorizontalAlignment.Center, 
                VerticalAlignment = VerticalAlignment.Center,
                Width = 32,
                Height = 32,
            };
            ImmagineTipo.Source = SceltaImmagine(Tipo);

            // Creazione del contenitore border per l'immagine.
            BordoImmagine = new Border
            {
                // Modifica le propriet� del contenitore border.
                HorizontalAlignment = System.Windows.HorizontalAlignment.Stretch,
                VerticalAlignment = VerticalAlignment.Stretch,
                BorderBrush = System.Windows.Media.Brushes.DarkGray,
                BorderThickness = new Thickness(LibreriaClient.Stile.BorderThickness)
            };
            BordoImmagine.Child = ImmagineTipo;

            // Creazione della label del nome del file.
            LabelNome = new System.Windows.Controls.Label
            {
                // Modifica le propriet� della label.
                Content = Nome,
                HorizontalAlignment = System.Windows.HorizontalAlignment.Stretch,
                VerticalAlignment = VerticalAlignment.Stretch,
                BorderBrush = System.Windows.Media.Brushes.DarkGray,
                BorderThickness = new Thickness(LibreriaClient.Stile.BorderThickness),
                FontSize = LibreriaClient.Stile.FontSize,
                Cursor = System.Windows.Input.Cursors.Hand
            };

            // Creazione del bottone per visualizzare il file.
            BtnVisualizza = new System.Windows.Controls.Button
            {
                // Modifica le propriet� del bottone.
                Content = "Visualizza",
                HorizontalAlignment = System.Windows.HorizontalAlignment.Stretch,
                VerticalAlignment = VerticalAlignment.Stretch,
                FontSize = LibreriaClient.Stile.FontSize,
                FontWeight = FontWeights.Bold,
                Foreground = System.Windows.Media.Brushes.White,
                Background = System.Windows.Media.Brushes.Blue,
                Cursor = System.Windows.Input.Cursors.Hand
            };
            // Aggiunta degli eventi al bottone e alla label.
            BtnVisualizza.Click += btnVisualizza_Click;
            LabelNome.MouseLeftButtonUp += labelNome_Click;
        }

        //Metodo per scegliere l'immagine in base al tipo di file.
        private ImageSource SceltaImmagine(string tipo)
        {
            string percorso = LibreriaClient.IconeTipiFile[tipo.ToLower()];
            return new BitmapImage(new Uri(percorso, UriKind.RelativeOrAbsolute));
        }

        // Metodo per aggiungere la riga alla tabella dinamica.
        public void AggiungiATabella(Grid tabella)
        {
            tabella.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
            // Aggiunge il bordo dell'immagine nella colonna 0.
            Grid.SetRow(BordoImmagine, NumeroRiga+1);
            Grid.SetColumn(BordoImmagine, 0);
            tabella.Children.Add(BordoImmagine);
            // Aggiunge la label del nome nella colonna 1.
            Grid.SetRow(LabelNome, NumeroRiga+1);
            Grid.SetColumn(LabelNome, 1);
            tabella.Children.Add(LabelNome);
            // Aggiunge il bottone nella colonna 2.
            Grid.SetRow(BtnVisualizza, NumeroRiga+1);
            Grid.SetColumn(BtnVisualizza, 2);
            tabella.Children.Add(BtnVisualizza);
        }

        // Metodo chiamato quando il bottone per visualizzare il file viene cliccato.
        void btnVisualizza_Click(object sender, RoutedEventArgs e)
        {
            // Chiama il metodo per aprire il file in base al tipo.
            ApriFile();
        }
        // Metodo chiamato quando la label del nome viene cliccata.
        void labelNome_Click(object sender, RoutedEventArgs e)
        {
            // Chiama il metodo per aprire il file in base al tipo.
            ApriFile();
        }

        //Metodo per visualizzare i file pdf.
        public void VisualizzaPdf(string nomeFile, string estensione)
        {
            // Controlla se il file esiste e lo apre con il visualizzatore PDF.
            string percorso = $@"{LibreriaClient.PercorsoSalvataggio}\{nomeFile}.{estensione}";
            if (File.Exists(percorso))
            {
                PdfViewerWindow viewer = new PdfViewerWindow(percorso);
                viewer.Show();
            }
            else
            {
                // Se il file non esiste, mostra un messaggio di errore.
                System.Windows.MessageBox.Show($"File PDF {Path.GetFullPath(percorso)} non trovato.", "Errore", MessageBoxButton.OK, MessageBoxImage.Warning);
                //Console.WriteLine($"Riga {NumeroRiga}: File PDF {Path.GetFullPath(percorso)} non trovato.");
            }
        }
        // Metodi per visualizzare i file txt.
        public void VisualizzaTxt(string nomeFile, string estensione)
        {
            // Controlla se il file esiste e lo apre con il visualizzatore di testo.
            string percorso = $@"{LibreriaClient.PercorsoSalvataggio}\{nomeFile}.{estensione}";
            if (File.Exists(percorso))
            {
                TxtViewer viewer = new TxtViewer(percorso);
                viewer.Show();
            }
            else
            {
                // Se il file non esiste, mostra un messaggio di errore.
                System.Windows.MessageBox.Show($"File txt {Path.GetFullPath(percorso)} non trovato.", "Errore", MessageBoxButton.OK, MessageBoxImage.Warning);
                //Console.WriteLine($"Riga {NumeroRiga}: File txt {Path.GetFullPath(percorso)} non trovato.");
            }
        }
        // Metodi per visualizzare le immagini.
        public void VisualizzaImmagine(string nomeFile, string estensione)
        {
            // Controlla se il file esiste e lo apre con il visualizzatore di immagini.
            string percorso = $@"{LibreriaClient.PercorsoSalvataggio}\{nomeFile}.{estensione}";
            if (File.Exists(percorso))
            {
                ImageViewer imageViewer = new ImageViewer(percorso);
                imageViewer.Show();
            }
            else
            {
                // Se il file non esiste, mostra un messaggio di errore.
                System.Windows.MessageBox.Show($"Immagine {Path.GetFullPath(percorso)} non trovata.", "Errore", MessageBoxButton.OK, MessageBoxImage.Warning);
                //Console.WriteLine($"Riga {NumeroRiga}: Immagine {Path.GetFullPath(percorso)} non trovata.");
            }
        }
        // Metodo per visualizzare i video.
        public void VisualizzaVideo(string nomeFile, string estensione)
        {
            // Controlla se il file esiste e lo apre con il visualizzatore di video.
            string percorso = $@"{LibreriaClient.PercorsoSalvataggio}\{nomeFile}.{estensione}";
            if (File.Exists(percorso))
            {
                VideoViewer viewer = new VideoViewer(percorso);
                viewer.Show();
            }
            else
            {
                // Se il file non esiste, mostra un messaggio di errore.
                System.Windows.MessageBox.Show($"Video {Path.GetFullPath(percorso)} non trovato.", "Errore", MessageBoxButton.OK, MessageBoxImage.Warning);
                //Console.WriteLine($"Riga {NumeroRiga}: Video {Path.GetFullPath(percorso)} non trovato.");
            }
        }

        // Metodo per aprire il file in base al tipo.
        private void ApriFile()
        {
            // Controlla il tipo di file e chiama il metodo appropriato per visualizzarlo.
            if(Array.IndexOf(LibreriaClient.TipiSupportati, Tipo) != -1)
            {
                LibreriaClient.Visualizzatori[Tipo].Invoke(this, [Nome, Tipo]);
            }
            else
            {
                System.Windows.MessageBox.Show("Tipo file non supportato.", "Errore", MessageBoxButton.OK, MessageBoxImage.Warning);
                //Console.WriteLine($"Riga {NumeroRiga}: Tipo file {Tipo} non supportato.");
            }
        }
    }
}

// I pugs sono passati di qui
// Davide Baldinu, Giada Croci, Lorenzo Porta