using SalaSimulazione.Model;
using System.IO;
using System.Media;
using System.Net.Sockets;
using System.Windows;
using System.Windows.Controls;

namespace SalaSimulazione
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Task? TskRicezione; // Task per la ricezione dei dati in background
        public int NumRighe = 0; // Numero di riga della tabella dinamica, inizialmente impostato a 0.

        public MainWindow()
        {
            // Inizializza la finestra principale e i componenti dell'interfaccia utente.
            InitializeComponent();


            if (!Path.Exists(LibreriaClient.PercorsoSalvataggio))
            {
                // Crea la cartella di salvataggio se non esiste
                Directory.CreateDirectory(LibreriaClient.PercorsoSalvataggio);
            }
        }

        // Metodo per aggiungere una nuova riga alla tabella dinamica.
        private void NuovaRiga(string nome)
        {
            // Utilizza Dispatcher per aggiornare l'interfaccia utente in modo sicuro dal thread di background
            Dispatcher.Invoke(() =>
            {
                if (nome != LibreriaClient.ComandoElimina)
                {
                    try
                    {
                        // Se il nome è divrso dal comandoElimina verifica se il nome contiene un punto
                        // per separare il tipo di file e il nome.
                        // Dopo averli separati crea una nuova Riga con i dati ottenuti.
                        // Aggiunge poi la riga alla tabella dinamica e incrementa il numero di righe.
                        Riga riga;
                        string[] nomi = nome.Split('.');
                        riga = new Riga(nomi[1], nomi[0], NumRighe);
                        riga.AggiungiATabella(TabellaDinamica);
                        NumRighe++;
                    }
                    catch (Exception ex)
                    {
                        //Console.WriteLine($"Errore durante la creazione della riga: {ex.Message}");
                        // Gestione delle eccezioni durante la creazione della riga.
                        System.Windows.MessageBox.Show($"Errore durante la creazione della riga: {ex.Message}", "ERRORE", MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }
                    // Riproduce un suono di avviso per indicare che una nuova riga è stata aggiunta
                    SystemSounds.Beep.Play();
                    //Console.WriteLine($"MainWindow: Nuova riga aggiunta.");
                }
            });
        }
        // Metodo chiamato quando la finestra è caricata
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                // Inizializza il listener per la ricezione dei dati
                RicezioneDati.Listener.Start();
            }
            catch (Exception ex)
            {
                //Console.WriteLine($"MainWindow: Errore durante l'inizializzazione della trasmissione: {ex.Message}");
                // Gestione delle eccezioni durante l'inizializzazione della trasmissione e chiusura dell'applicazione in caso di errore.
                System.Windows.MessageBox.Show($"Errore durante l'inizializzazione della trasmissione: {ex.Message}", "ERRORE", MessageBoxButton.OK, MessageBoxImage.Error);
                System.Windows.Application.Current.Shutdown();
            }

            //Console.WriteLine("MainWindow: TcpListener avviato corettamente.");

            // Avvia il task per la ricezione dei dati in background
            TskRicezione = Task.Factory.StartNew(() =>
            {
                while (true)
                {
                    try
                    {
                        // Riceve i dati dal server e li salva nel percorso specificato
                        if (RicezioneDati.Ricevi(LibreriaClient.PercorsoSalvataggio) == 0)
                        {
                            NuovaRiga(Path.GetFileName(RicezioneDati.Percorsi[NumRighe]));
                        }
                    }
                    catch (SocketException socketEx)
                    {
                        //Console.WriteLine($"MainWindow: La trasmissione è stata interrotta: {socketEx.Message}");
                        // Gestione delle eccezioni durante la ricezione dei dati.
                        System.Windows.MessageBox.Show($"La trasmissione è stata interrotta: {socketEx.Message}", "", MessageBoxButton.OK, MessageBoxImage.Information);
                        break;
                    }
                }
            });
        }

        //Metodo chiamato quando la finestra principale viene chiusa.
        private void Window_Closed(object sender, EventArgs e)
        {
            // Ferma il listener per la ricezione dei dati e chiude l'applicazione.
            RicezioneDati.Listener.Stop();
            System.Windows.Application.Current.Shutdown();
        }

        //Metodo chiamato quando il pulsante "Reset" viene cliccato.
        public void Reset()
        {
            try
            {
                // Rimuovi solo le righe dati, lasciando l'intestazione (assumendo che sia la prima riga)
                if (TabellaDinamica.RowDefinitions.Count > 1)
                {
                    // Rimuovi tutti i figli tranne quelli della riga di intestazione (Row 0)
                    for (int i = TabellaDinamica.Children.Count - 1; i >= 0; i--)
                    {
                        var child = TabellaDinamica.Children[i];
                        if (Grid.GetRow(child) != 0)
                        {
                            TabellaDinamica.Children.RemoveAt(i);
                        }
                    }
                    // Rimuovi tutte le RowDefinitions tranne la prima (intestazione)
                    while (TabellaDinamica.RowDefinitions.Count > 1)
                    {
                        TabellaDinamica.RowDefinitions.RemoveAt(1);
                    }
                }
                NumRighe = 0;
                foreach (string percorso in RicezioneDati.Percorsi)
                {
                    if (File.Exists(percorso))
                    {
                        File.Delete(percorso);
                    }
                }
                RicezioneDati.Percorsi.Clear();
            }
            catch (Exception ex)
            {
                //Console.WriteLine($"MainWindow: Errore durante il reset della tabella: {ex.Message}");
                // Gestione delle eccezioni durante il reset della tabella.
                System.Windows.MessageBox.Show($"Errore durante il reset della tabella: {ex.Message}", "ERRORE", MessageBoxButton.OK, MessageBoxImage.Error);
                System.Windows.Application.Current.Shutdown();
            }
            // Riproduce un suono di avviso per indicare che il reset è stato completato
            SystemSounds.Beep.Play();
        }
    }
}

// I pugs sono passati di qui
// Davide Baldinu, Giada Croci, Lorenzo Porta