using SalaRegia.Model;
using System;
using System.IO;
using System.Net;
using System.Windows;

namespace SalaRegia.View
{
    /// <summary>
    /// Logica di interazione per Impostazioni.xaml
    /// </summary>
    public partial class Impostazioni : Window
    {
        public Impostazioni()
        {
            // Inizializza la finestra delle impostazioni e i componenti dell'interfaccia utente.
            InitializeComponent();

            txtIndirizzo.Text = InvioDati.IndirizzoIp;
            if (InvioDati.IndirizzoIp == Libreria.IndirizzoLoopback)
            {
                checkLoopback.IsChecked = true;
                txtIndirizzo.IsReadOnly = true;
            }
            else
            {
                checkLoopback.IsChecked = false;
                txtIndirizzo.IsReadOnly = false;
            }
        }

        // Metodo chiamato quando il pulsante "Salva" viene cliccato.
        private void btnSalva_Click(object sender, RoutedEventArgs e)
        {
            // Ottiene l'indirizzo IP inserito dall'utente.
            string indirizzo = txtIndirizzo.Text;
            // Verifica se l'indirizzo IP è valido.
            if (IPAddress.TryParse(indirizzo, out _))
            {
                // Se l'indirizzo è valido, lo salva nella classe InvioDati e chiude la finestra.
                InvioDati.IndirizzoIp = indirizzo;
                ScriviIndirizzo(indirizzo);
                this.Close();
            }
            else
            {
                // Se l'indirizzo non è valido, mostra un messaggio di errore.
                MessageBox.Show("L'indirizzo IP inserito non è valido.", "Formato non valido", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        // Metodo chiamato quando la checkBox "Loopback" viene selezionata.
        private void checkLoopback_Checked(object sender, RoutedEventArgs e)
        {
            // Imposta l'indirizzo IP di loopback in modalità di sola lettura.
            txtIndirizzo.Text = Libreria.IndirizzoLoopback;
            txtIndirizzo.IsReadOnly = true;
        }

        // Metodo chiamato quando la checkBox "Loopback" viene deselezionata.
        private void checkLoopback_Unchecked(object sender, RoutedEventArgs e)
        {
            // Rimuove l'indirizzo IP di loopback e consente la modifica dell'indirizzo IP.
            txtIndirizzo.Text = string.Empty;
            txtIndirizzo.IsReadOnly = false;
        }

        // Metodo per scrivere l'indirizzo IP in un file di testo.
        private void ScriviIndirizzo(string indirizzo)
        {
            try
            {
                StreamWriter stream = new StreamWriter(Libreria.PercorsoFileIndirizzoIp, false);
                stream.WriteLine(indirizzo);
                stream.Close(); // Chiude lo stream dopo la scrittura.
            }
            catch (Exception ex)
            {
                // Gestione delle eccezioni durante la scrittura del file.
                MessageBox.Show($"Errore durante il salvataggio dell'indirizzo: {ex.Message}", "ERRORE", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}

// I pugs sono passati di qui
// Davide Baldinu, Giada Croci, Lorenzo Porta