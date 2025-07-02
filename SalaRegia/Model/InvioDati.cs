using System;
using System.IO;
using System.Net.Sockets;
using System.Text;
using System.Windows;

namespace SalaRegia.Model
{
    public static class InvioDati
    {
        public static string? IndirizzoIp;              // Indirizzo IP del computer a cui inviare i file
        public static List<string> Percorsi = new();    // Lista dei percorsi dei file da inviare

        // Metodo chiamato per inviare una stringa al server.
        public static void Invia(string stringa)
        {
            // Crea un client TCP e una rete di flusso per inviare la stringa al server.
            TcpClient? client = null;
            NetworkStream? stream = null;
            try
            {
                // Imposta l'indirizzo IP e la porta del server e crea un nuovo TcpClient. 
                // Il flusso di rete viene ottenuto dal client per inviare i dati al server
                // tramite una stringa convertita in un array di byte.
                // In seguito all'invio della stringa, il metodo esce senza ulteriori operazioni.
                client = new TcpClient(IndirizzoIp, Libreria.Porta);
                stream = client.GetStream();
                byte[] dati = Encoding.UTF8.GetBytes(stringa);
                stream.Write(dati);
                return;
            }
            catch (Exception ex)
            {
                // In caso di errore durante l'invio dei dati, viene mostrato un messaggio di errore all'utente.
                MessageBox.Show("Errore durante l'invio dei dati: " + ex.Message, "ERRORE", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                // Chiude il flusso di rete e il client TCP se sono stati creati con successo.
                if (stream != null) stream.Close(); 
                if (client != null) client.Close();
            }
        }

        // Metodo chiamato per inviare un file al server.
        public static void Invia(string percorsoFile, string nomeFile)
        {
            // Crea un client TCP e una rete di flusso per inviare la stringa al server.
            TcpClient? client = null;
            NetworkStream? stream = null;
            try
            {
                client = new TcpClient(IndirizzoIp, Libreria.Porta);
                stream = client.GetStream();
                // Invio del nome
                byte[] nome = Encoding.UTF8.GetBytes(nomeFile);
                stream.Write(nome);
                // Invio del file
                // Costruzione del pacchetto
                byte[] dati = File.ReadAllBytes(percorsoFile);
                byte[] lunghezzaDati = BitConverter.GetBytes(dati.Length);
                byte[] pacchetto = new byte[4 + dati.Length];
                lunghezzaDati.CopyTo(pacchetto, 0);
                dati.CopyTo(pacchetto, 4);
                // Invio al client
                int byteInviati = 0;
                int byteRimanenti = pacchetto.Length;
                while (byteRimanenti > 0)
                {
                    // Calcola la dimensione del prossimo pacchetto da inviare, che non può superare la dimensione del buffer.
                    // Scrive il pacchetto nel flusso di rete.
                    // Aggiorna il conteggio dei byte inviati e quelli rimanenti.
                    int dimensioneProssimoPacchetto = (byteRimanenti > Libreria.DimensioneBuffer) ? Libreria.DimensioneBuffer : byteRimanenti;
                    stream.Write(pacchetto, byteInviati, dimensioneProssimoPacchetto);
                    byteInviati += dimensioneProssimoPacchetto;
                    byteRimanenti -= dimensioneProssimoPacchetto;
                }
            }
            catch (Exception ex)
            {
                // In caso di errore durante l'invio dei dati, viene mostrato un messaggio di errore all'utente.
                MessageBox.Show($"Errore durante l'invio dei dati: {ex.Message}", "ERRORE", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                // Chiude il flusso di rete e il client TCP se sono stati creati con successo.
                if (stream != null) stream.Close();
                if (client != null) client.Close();
            }
        }
    }
}

// I pugs sono passati di qui
// Davide Baldinu, Giada Croci, Lorenzo Porta