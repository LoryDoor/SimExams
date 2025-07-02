using SalaRegia.Model;
using SalaRegia.View;
using Microsoft.Win32;
using System;
using System.IO;
using System.Net;
using System.Windows;
using System.Windows.Controls;

namespace Interfaccia_Server
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private int NumRighe = 0;   // Numero di riga della tabella dinamica, inizialmente impostato a 0.

        public MainWindow()
        {
            // Inizializza la finestra principale e i componenti dell'interfaccia utente.
            InitializeComponent();

            // Verifica se il percorso di salvataggio esiste, altrimenti lo crea.
            if (!File.Exists(LibreriaServer.PercorsoFileIndirizzoIp))
            {
                // Crea il file "indirizzo.txt" se non esiste.
                using (StreamWriter stream = new StreamWriter(LibreriaServer.PercorsoFileIndirizzoIp, false))
                {
                    stream.WriteLine(LibreriaServer.IndirizzoLoopback); // Scrive l'indirizzo IP di loopback nel file.
                }
            }
            else
            {
                LeggiIndirizzo(); // Legge l'indirizzo IP dal file "indirizzo.txt".
            }
        }

        // Metodo per aggiungere una nuova riga alla tabella dinamica.
        private void NuovaRiga(string nome)
        {
            try
            {
                // Verifica se il nome contiene un punto per separare il tipo di file e il nome.
                // Dopo averli separati crea una nuova Riga con i dati ottenuti.
                // Aggiunge poi la riga alla tabella dinamica e incrementa il numero di righe.
                string[] nomi = nome.Split('.');
                Riga riga = new Riga(nomi[1], nomi[0], NumRighe);
                riga.AggiungiATabella(TabellaDinamica);
                NumRighe++;
            }
            catch (Exception ex)
            {
                // Gestione delle eccezioni durante la creazione della riga.
                MessageBox.Show($"Errore durante la creazione della riga: {ex.Message}", "ERRORE", MessageBoxButton.OK, MessageBoxImage.Error);
                Application.Current.Shutdown();
            }
        }

        // Metodo chiamato quando il pulsante "Aggiungi" viene cliccato.
        private void btnAggiungi_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Apre un finestra per la selezione del file da aggiungere intitolata "Scegli il referto"
                // e filra i tipi di file supportati.
                OpenFileDialog fileDialog = new OpenFileDialog();
                fileDialog.Title = "Scegli il referto: ";
                fileDialog.Filter = LibreriaServer.FiltroFile;
                if (fileDialog.ShowDialog() == true)
                {
                    // Se un file è stato selezionato, aggiunge il percorso del file alla lista dei percorsi
                    // e chiama il metodo NuovaRiga per creare una nuova riga nella tabella dinamica con il nome del file.
                    string percorsoFile = fileDialog.FileName;
                    InvioDati.Percorsi.Add(percorsoFile);
                    string nomeFile = Path.GetFileName(percorsoFile);
                    NuovaRiga(nomeFile);
                }
            }
            catch (Exception ex)
            {
                // Gestione delle eccezioni durante l'aggiunta del file.
                MessageBox.Show("Errore durante l'aggiunta del file: " + ex.Message, "Errore", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        // Metodo chiamato quando il pulsante "Impostazioni" viene cliccato.
        private void btnImpostazioni_Click(object sender, RoutedEventArgs e)
        {
            // Apre la finestra delle impostazioni quando il pulsante "Impostazioni" viene cliccato.
            Impostazioni impostazioni = new Impostazioni();
            impostazioni.ShowDialog();
        }

        // Metodo chiamato quando il pulsante "Reset" viene cliccato.
        private void btnReset_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (TabellaDinamica.RowDefinitions.Count <= 1)
                {
                    // Se la tabella non contiene righe di dati, mostra un messaggio di avviso.
                    MessageBox.Show("La tabella è già vuota.", "Errore", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }
                // Mostra un messaggio di conferma per il reset della tabella.
                switch (MessageBox.Show("Sei sicuro di voler resettare la tabella? ATTENZIONE. L'azione è irreversibile!", "Conferma Reset", MessageBoxButton.YesNo, MessageBoxImage.Question))
                {
                    case MessageBoxResult.Yes:
                        // Rimozione delle righe di dati dalla tabella locale.
                        if (TabellaDinamica.RowDefinitions.Count > 1)
                        {
                            // Rimuovi tutti i figli tranne quelli della riga di intestazione (Row 0).
                            for (int i = TabellaDinamica.Children.Count - 1; i >= 0; i--)
                            {
                                var child = TabellaDinamica.Children[i];
                                if (Grid.GetRow(child) != 0)
                                {
                                    TabellaDinamica.Children.RemoveAt(i);
                                }
                            }
                            // Rimuovi tutte le RowDefinitions tranne la prima (intestazione).
                            while (TabellaDinamica.RowDefinitions.Count > 1)
                            {
                                TabellaDinamica.RowDefinitions.RemoveAt(1);
                            }
                        }
                        NumRighe = 0;
                        InvioDati.Percorsi.Clear();
                        // Invio del comando di reset al client.
                        InvioDati.Invia(LibreriaServer.ComandoElimina);
                        break;

                    case MessageBoxResult.No:
                        break;
                }
            }
            catch (Exception ex)
            {
                // Gestione delle eccezioni durante il reset della tabella.
                MessageBox.Show($"Errore durante il reset:{ex.Message}", "ERRORE", MessageBoxButton.OK, MessageBoxImage.Error);
                Application.Current.Shutdown();
            }
        }
        // Metodo chiamato quando la finestra principale viene chiusa.
        private void Window_Closed(object sender, EventArgs e)
        {
            // Invia il comando di chiusura al client e termina l'applicazione.
            Application.Current.Shutdown();
        }

        private void LeggiIndirizzo()
        {
            try
            {
                using (StreamReader stream = new StreamReader(LibreriaServer.PercorsoFileIndirizzoIp))
                {
                    string? indirizzo = stream.ReadLine();
                    if (indirizzo != null)
                    {
                        InvioDati.IndirizzoIp = indirizzo;
                    }
                }
            }
            catch (Exception ex)
            {
                // Gestione delle eccezioni durante la lettura del file.
                MessageBox.Show($"Errore durante la lettura dell'indirizzo: {ex.Message}", "ERRORE", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}

// I pugs sono passati di qui
// Davide Baldinu, Giada Croci, Lorenzo Porta