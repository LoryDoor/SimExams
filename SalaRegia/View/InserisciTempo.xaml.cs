using System;
using System.Windows;
using System.Windows.Controls;

namespace SalaRegia.View
{
    /// <summary>
    /// Logica di interazione per InserisciTempo.xaml
    /// </summary>
    public partial class InserisciTempo : Window
    {
        public int Tempo { get; private set; } = 0; // Tempo in millisecondi
        public InserisciTempo()
        {
            // Inizializza la finestra di inserimento tempo e i componenti dell'interfaccia utente.
            InitializeComponent();
        }

        // Metodo chiamato quando il pulsante "Conferma" viene cliccato.
        private void btnConferma_Click(object sender, RoutedEventArgs e)
        {
            // Prova a convertire il testo inserito in un numero intero.
            if (int.TryParse(txtTempo.Text, out int tempo))
            {
                Tempo = tempo * 1000; // Converti i secondi in millisecondi
                this.Close(); // Chiudi la finestra dopo la conferma
            }
            else
            {
                // Se la conversione fallisce, mostra un messaggio di errore.
                MessageBox.Show("Inserire un valore numerico valido.", "Formato non valido", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        // Metodo chiamato quando il pulsante "Annulla" viene cliccato.
        private void btnAnnulla_Click(object sender, RoutedEventArgs e)
        {
            Tempo = -1; // Indica che l'utente ha annullato l'inserimento
            this.Close(); // Chiudi la finestra senza salvare il valore
        }

        private void SceltaRapidaTempo_Checked(object sender, RoutedEventArgs e)
        {
            // Gestisce la selezione rapida del tempo tramite i RadioButton.
            RadioButton? radio = sender as RadioButton;
            if (radio != null)
            {
                string content = radio.Content.ToString().Replace("s", "");
                if (int.TryParse(content, out int valore))
                {
                    // Imposta il valore del TextBox in base alla selezione del RadioButton.
                    txtTempo.Text = valore.ToString();
                }
            }
        }
        // Metodo chiamato quando la finestra viene chiusa.
        private void Window_Closed(object sender, EventArgs e)
        {
            if (txtTempo.Text == string.Empty)
            {
                Tempo = -1; // Se la finestra viene chiusa senza inserire un valore, imposta il tempo a -1
            }
        }
    }
}

// I pugs sono passati di qui
// Davide Baldinu, Giada Croci, Lorenzo Porta