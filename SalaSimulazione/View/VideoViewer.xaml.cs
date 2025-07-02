using LibVLCSharp.Shared;
using LibVLCSharp.WPF;
using System;
using System.Windows;
using System.Windows.Threading;

namespace SalaSimulazione.View
{
    public partial class VideoViewer : Window
    {
        private string VideoPath;   // Percorso del video da visualizzare
        private LibVLC LibVLC;  // Istanza di LibVLC per la riproduzione video
        private Media Media;    // Media associato al video da riprodurre
        private MediaPlayer MediaPlayer;    // MediaPlayer per controllare la riproduzione del video
        private DispatcherTimer PositionTimer;  // Timer per aggiornare la posizione del video nella barra di avanzamento
        private bool IsSeeking = false; // Indica se l'utente sta cercando una posizione specifica nel video

        public VideoViewer(string path)
        {
            // Inizializza i componenti dell'interfaccia e configura la riproduzione video utilizzando LibVLC: 
            // imposta il percorso del video, inizializza la libreria, crea il MediaPlayer e lo associa al VideoView. 
            // Inoltre, configura un DispatcherTimer che aggiorna periodicamente la barra di avanzamento del video ogni 200 ms.
            InitializeComponent();
            VideoPath = path;
            Core.Initialize();
            LibVLC = new LibVLC();
            MediaPlayer = new MediaPlayer(LibVLC);
            VideoView.MediaPlayer = MediaPlayer;
            PositionTimer = new DispatcherTimer();
            PositionTimer.Interval = TimeSpan.FromMilliseconds(200);
            PositionTimer.Tick += UpdateSeekBar;
            //Console.WriteLine($"VideoViewer: Apertura del video {Path.GetFullPath(path)} riuscita.");
        }

        // Metodo chiamato quando la finestra viene caricata.
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            // Carica il video specificato dal percorso e avvia la riproduzione.
            Media = new Media(LibVLC, VideoPath);
            MediaPlayer.Play(Media);
            PositionTimer.Start();
        }

        // Metodo chiamato per aggiornare la barra di avanzamento del video.
        private void UpdateSeekBar(object sender, EventArgs e)
        {
            if (!IsSeeking && MediaPlayer.Length > 0)
            {
                // Aggiorna il valore della SeekBar in base alla posizione corrente del video.
                SeekBar.Value = MediaPlayer.Time * 1000.0 / MediaPlayer.Length;
            }
            if (MediaPlayer.State == VLCState.Ended)
            {
                if (MediaPlayer.Media != null)
                {
                    // Se il video è terminato, riavvia la riproduzione in loop del video.
                    MediaPlayer.Stop();
                    MediaPlayer.Play(MediaPlayer.Media);
                }
            }
        }
        //Metodo chiamato quando l'utente clicca sul pulsante della riproduzione del video.
        private void btnPlay_Click(object sender, RoutedEventArgs e)
        {
            // Avvia la riproduzione del video se il MediaPlayer è stato inizializzato.
            MediaPlayer?.Play();
        }
        // Metodo chiamato quando l'utente clicca sul pulsante di pausa del video.
        private void btnPause_Click(object sender, RoutedEventArgs e)
        {
            // Mette in pausa la riproduzione del video se il MediaPlayer è stato inizializzato.
            MediaPlayer?.Pause();
        }
        // Metodo chiamato quando l'utente clicca sul pulsante di stop del video.
        private void btnStop_Click(object sender, RoutedEventArgs e)
        {
            // Ferma la riproduzione del video se il MediaPlayer è stato inizializzato.
            MediaPlayer?.Stop();
        }
        // Metodo chiamato quando l'utente clicca sul pulsante di riavvolgimento rapido del video.
        private void btnRewind_Click(object sender, RoutedEventArgs e)
        {
            if (MediaPlayer.Media != null)
            {
                // Riavvolge il video se il MediaPlayer è stato inizializzato.
                MediaPlayer.Stop();
                MediaPlayer.Play(MediaPlayer.Media);
            }
        }

        // Metodo chiamato quando il valore della SeekBar cambia.
        private void SeekBar_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (IsSeeking)
            {
                // Se l'utente sta già effettuando uno spostamento manuale nella timeline,
                // esce dal metodo per evitare conflitti o aggiornamenti indesiderati.
                return;
            }

            if (MediaPlayer == null || MediaPlayer.Length <= 0)
            {
                // Se il MediaPlayer non è stato inizializzato oppure non è presente alcun contenuto da riprodurre,
                // esce dal metodo per evitare errori o operazioni inutili.
                return;
            }

            if (SeekBar.IsMouseCaptureWithin)
            {
                // Se l'utente sta interagendo con la barra di avanzamento,
                // imposta il flag IsSeeking a true per evitare conflitti,
                // calcola il nuovo tempo di riproduzione in base al valore della SeekBar e aggiorna la posizione del video. 
                // Infine, reimposta IsSeeking a false per indicare che lo spostamento è terminato.
                IsSeeking = true;
                long newTime = (long)(SeekBar.Value * MediaPlayer.Length / 1000.0);
                MediaPlayer.Time = newTime;
                IsSeeking = false;
            }
        }
        //Metodo chiamato alla chiusura della finestra del visualizzatore video.
        private void Window_Closed(object sender, EventArgs e)
        {
            // Ferma il MediaPlayer e rilascia le risorse associate al Media.
            MediaPlayer?.Stop();
            Media?.Dispose();
        }
    }
}

// I pugs sono passati di qui
// Davide Baldinu, Giada Croci, Lorenzo Porta