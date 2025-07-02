using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Windows;

namespace SalaSimulazione.Model
{
    public static class RicezioneDati
    {
        public static TcpListener Listener = new TcpListener(IPAddress.Any, LibreriaClient.Porta); // Porta su cui il server ascolta le connessioni in arrivo
        public static List<string> Percorsi = new();    // Lista dei percorsi dei file ricevuti

        public static int Ricevi(string percorsoFile)
        {
            int returnCode = 0; // Codice di ritorno per indicare lo stato della ricezione
            TcpClient? client = null;
            NetworkStream? stream = null;
            // Controllo sulle eccezioni
            try
            {
                // Accetta una connessione in arrivo e ottiene il flusso di rete dal client
                client = Listener.AcceptTcpClient();
                stream = client.GetStream();
                // Lettura del nome del file
                byte[] byteNomeFile = new byte[LibreriaClient.Dimensioni.NomeFile];
                int nomeFileBytesRead = stream.Read(byteNomeFile, 0, byteNomeFile.Length);
                string nomeFile = Encoding.UTF8.GetString(byteNomeFile, 0, nomeFileBytesRead).TrimEnd('\0');
                // Verifica se il nome del file corrisponde al comando di eliminazione
                if (nomeFile == LibreriaClient.ComandoElimina)
                {
                    System.Windows.Application.Current.Dispatcher.Invoke(() =>
                    {
                        if (System.Windows.Application.Current.MainWindow is MainWindow mainWindow)
                        {
                            //Se il nome del file corrisponde al comando di eliminazione, mostra un messaggio di conferma
                            mainWindow.Reset(); 
                        }
                    });
                    return 2; // Richiesta di eliminazione
                }
                // Lettura del file
                int byteLetti = 0;
                int byteTotaliLetti = 0;
                // Lettura della lunghezza dei dati in arrivo
                byte[] lunghezza = new byte[4];
                byteLetti = stream.Read(lunghezza, 0, 4);
                int lunghezzaDati = BitConverter.ToInt32(lunghezza, 0);
                // Lettura dei dati
                int byteRimanenti = lunghezzaDati;
                byte[] dati = new byte[lunghezzaDati];
                while (byteRimanenti > 0)
                {
                    // Legge i dati in pacchetti di dimensione dimBuffer
                    int dimensioneProssimoPacchetto = (byteRimanenti > LibreriaClient.Dimensioni.Buffer) ? LibreriaClient.Dimensioni.Buffer : byteRimanenti;
                    // Legge i dati dal flusso e li memorizza nell'array dati.
                    byteLetti = stream.Read(dati, byteTotaliLetti, dimensioneProssimoPacchetto);
                    // Aggiorna il conteggio dei byte totali letti e dei byte rimanenti da leggere.
                    byteTotaliLetti += byteLetti; 
                    byteRimanenti -= byteLetti;
                }
                // Salvataggio del file
                percorsoFile += nomeFile;
                File.WriteAllBytes(percorsoFile, dati);
                Percorsi.Add(percorsoFile);
                //Console.WriteLine($"Ricezione dati: Sono stati ricevuti {byteTotaliLetti} byte. Nome file ricevuto: {nomeFile}");
                returnCode = 0; // Ricezione completata con successo
            }
            catch (SocketException)
            {
                // Propaga l'eccezione per gestirla nel chiamante
                throw; 
            }
            catch (Exception ex)
            {
                //Console.WriteLine($"Ricezione dati: Errore durante la ricezione dei dati: {ex.Message}");
                // Gestione delle eccezioni durante la ricezione dei dati
                System.Windows.MessageBox.Show($"Errore durante la ricezione dei dati: {ex.Message}", "ERRORE", MessageBoxButton.OK, MessageBoxImage.Error);
                returnCode = 1;
            }
            finally
            {
                // Controlli per evitare NullReferenceException.
                if (stream != null) stream.Close();
                if (client != null) client.Close();
            }
            // Restituisce il codice di ritorno che indica lo stato della ricezione.
            return returnCode; 
        }
    }
}

// I pugs sono passati di qui
// Davide Baldinu, Giada Croci, Lorenzo Porta