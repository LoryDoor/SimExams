using SalaRegia.View;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace SalaRegia.Model
{
    public class Riga
    {
        private string Tipo; // Tipo di file.
        private string Nome; // Nome del file senza estensione.
        private int NumeroRiga; // Numero della riga nella tabella dinamica zero-based.
        private Image ImmagineTipo; // Immagine rappresentante il tipo di file.
        private Border BordoImmagine; // Contenitore border per l'immagine del tipo di file.
        private Label LabelNome; // Label per il nome del file.
        private Border BordoBottoni; // Contenitore border per i bottoni.
        private StackPanel PannelloBottoni; // Pannello per i bottoni di invio e invio tra.

        public Riga(string tipo, string nome, int numeroRiga)
        {
            Tipo = tipo;
            Nome = nome;
            NumeroRiga = numeroRiga;

            // Creazione dell'immagine rappresentante il tipo di file.
            ImmagineTipo = new Image
            {
                // Modifica le proprietà dell'immagine.
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center,
                Width = 32,
                Height = 32,
            };
            ImmagineTipo.Source = SceltaImmagine(Tipo);

            // Creazione del contenitore border per l'immagine.
            BordoImmagine = new Border
            {
                // Modifica le proprietà del contenitore border.
                HorizontalAlignment = HorizontalAlignment.Stretch,
                VerticalAlignment = VerticalAlignment.Stretch,
                BorderBrush = Brushes.DarkGray,
                BorderThickness = new Thickness(LibreriaServer.Stile.BorderThickness)
            };
            BordoImmagine.Child = ImmagineTipo;

            // Creazione della label del nome del file.
            LabelNome = new Label
            {
                // Modifica le proprietà della label.
                Content = Nome,
                HorizontalAlignment = HorizontalAlignment.Stretch,
                VerticalAlignment = VerticalAlignment.Stretch,
                BorderBrush = Brushes.DarkGray,
                BorderThickness = new Thickness(LibreriaServer.Stile.BorderThickness),
                FontSize = LibreriaServer.Stile.FontSize,
            };

            // Creazione del contenitore border per i bottoni.
            BordoBottoni = new Border
            {
                // Modifica le proprietà del contenitore border.
                HorizontalAlignment = HorizontalAlignment.Stretch,
                VerticalAlignment = VerticalAlignment.Stretch,
                BorderBrush = Brushes.DarkGray,
                BorderThickness = new Thickness(LibreriaServer.Stile.BorderThickness)
            };
            // Creazione del pannello per i bottoni di invio e invio tra.
            PannelloBottoni = new StackPanel
            {
                // Modifica le proprietà del pannello dei bottoni.
                Orientation = Orientation.Horizontal,
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Stretch
            };
            // Creazione del bottone di invio.
            Button btn1 = new Button
            {
                // Modifica le proprietà del bottone di invio.
                Content = "Invia",
                Margin = new Thickness(LibreriaServer.Stile.ButtonMargin),
                Padding = new Thickness(LibreriaServer.Stile.ButtonPadding),
                FontSize = LibreriaServer.Stile.FontSize,
                FontWeight = FontWeights.Bold,
                Foreground = Brushes.White,
                Background = Brushes.Blue,
                Cursor = System.Windows.Input.Cursors.Hand
            };
            // Aggiunta dell'evento click al bottone di invio.
            btn1.Click += btnInvia_Click;
            PannelloBottoni.Children.Add(btn1);
            // Creazione del bottone di invio tra.
            Button btn2 = new Button
            {
                // Modifica le proprietà del bottone di invio tra.
                Content = "Invia tra",
                Margin = new Thickness(LibreriaServer.Stile.ButtonMargin),
                Padding = new Thickness(LibreriaServer.Stile.ButtonPadding),
                FontSize = LibreriaServer.Stile.FontSize,
                FontWeight = FontWeights.Bold,
                Foreground = Brushes.White,
                Background = Brushes.Blue,
                Cursor = System.Windows.Input.Cursors.Hand
            };
            // Aggiunta dell'evento click al bottone di invio tra.
            btn2.Click += btnInviaTra_Click;
            PannelloBottoni.Children.Add(btn2);
            BordoBottoni.Child = PannelloBottoni; // Aggiungi il pannello dei bottoni al bordo
        }

        // Metodo per scegliere l'immagine in base al tipo di file.
        private ImageSource SceltaImmagine(string tipo) 
        {
            string rinomina = LibreriaServer.IconeTipiFile[tipo.ToLower()];
            return new BitmapImage(new Uri(rinomina, UriKind.RelativeOrAbsolute));
        }

        // Metodo per aggiungere la riga alla tabella dinamica.
        public void AggiungiATabella(Grid tabella)
        {
            // Aggiunge una nuova riga alla tabella per questa riga.
            tabella.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
            // Aggiunge il bordo dell'immagine nella colonna 0, la label del nome nella colonna 1
            Grid.SetRow(BordoImmagine, NumeroRiga+1);
            Grid.SetColumn(BordoImmagine, 0);
            tabella.Children.Add(BordoImmagine);
            // Aggiunge la label del nome nella colonna 1 e il bordo dei bottoni nella colonna 2.
            Grid.SetRow(LabelNome, NumeroRiga+1);
            Grid.SetColumn(LabelNome, 1);
            tabella.Children.Add(LabelNome);
            // Aggiunge il bordo dei bottoni nella colonna 2.
            Grid.SetRow(BordoBottoni, NumeroRiga+1);
            Grid.SetColumn(BordoBottoni, 2);
            tabella.Children.Add(BordoBottoni);
        }

        // Metodo asincrono chiamato quando il bottone "Invia" viene cliccato.
        private async void btnInvia_Click(object sender, RoutedEventArgs e)
        {
            // Verifica che il sender sia effettivamente un Button.
            if (sender is Button btn)
            {
                // Disabilita il bottone per evitare click multipli durante l'operazione.
                btn.IsEnabled = false;
                try
                {
                    // Esegue l'invio dei dati in un task separato per non bloccare l'interfaccia utente.
                    await Task.Run(() =>
                    {
                        // Invia i dati utilizzando la classe InvioDati.
                        InvioDati.Invia(InvioDati.Percorsi[NumeroRiga], Nome + "." + Tipo);
                    });
                }
                finally
                {
                    // Riabilita il bottone dopo il completamento dell'operazione, anche in caso di errore.
                    btn.IsEnabled = true;
                }
            }
        }

        // Metodo chiamato quando il bottone di invio tra viene cliccato.
        private async void btnInviaTra_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button btn)
            {
                btn.IsEnabled = false;
                try
                {
                    // Apre una finestra per inserire il tempo di attesa prima dell'invio.
                    InserisciTempo inserisciTempo = new InserisciTempo();
                    inserisciTempo.ShowDialog();
                    if (inserisciTempo.Tempo != -1)
                    {
                        // Se il tempo inserito è valido, attende il tempo specificato e poi invia i dati.
                        await Task.Run(() =>
                        {
                            Thread.Sleep(inserisciTempo.Tempo);
                            InvioDati.Invia(InvioDati.Percorsi[NumeroRiga], Nome + "." + Tipo);
                        });
                    }
                }
                finally
                {
                    btn.IsEnabled = true;
                }
            }
        }
    }
}

// I pugs sono passati di qui
// Davide Baldinu, Giada Croci, Lorenzo Porta