using System;

namespace SalaRegia.Model
{
    public class LibreriaServer
    {
        public const string ComandoElimina = "MASTER_SAYS_CLEAR_ALL";       // Comando per lanciare il reset
        public const string IndirizzoLoopback = "127.0.0.1";                // Indirizzo IP di loopback
        public const int Porta = 5000;                                      // Porta su cui il server ascolta le connessioni
        public const int DimensioneBuffer = 1024;                           // Dimensione di ciascun pacchetto da inviare
        public const string FiltroFile = "Supportati(*.txt,*.png,*.jpg,.jpeg,*.heic,*.mp4,*.pdf)|*.txt;*.png;*.jpg;*.jpeg;*.heic;*.mp4;*.pdf" +
            "|Testo semplice (*.txt)|*.txt" +
            "|Immagini (*.png,*.jpg,.jpeg,*.heic)|*.png;*.jpg;*.jpeg;*.heic" +
            "|Video (*.mp4)|*.mp4" +
            "|Documenti PDF (*.pdf)|*.pdf" +
            "|Tutti i file (*.*)|*.*\\";                                    // Filtro per la selezione dei file
        public const string PercorsoFileIndirizzoIp = @".\indirizzo.txt";    // Percorso del file che contiene l'indirizzo IP

        public struct Stile
        {
            public const int FontSize = 14;         // Dimensione del font per le label
            public const int BorderThickness = 1;   // Spessore del bordo delle celle della tabella
            public const int ButtonMargin = 2;      // Margine dei bottoni
            public const int ButtonPadding = 5;     // Padding dei bottoni
        }
        public struct PercorsiIcone // Percorsi delle icone per i tipi di file
        {
            public const string Pdf = "Immagini/iconaPdf.png";
            public const string Txt = "Immagini/iconaTxt.png";
            public const string Video = "Immagini/iconaVideo.png";
            public const string Foto = "Immagini/iconaFoto.png";
        }

        public static readonly Dictionary<string, string> IconeTipiFile = new() // Associazione tra estensioni dei file e i percorsi delle icone
        {
            { "pdf", PercorsiIcone.Pdf },
            { "txt", PercorsiIcone.Txt },
            { "mp4", PercorsiIcone.Video },
            { "png", PercorsiIcone.Foto },
            { "jpg", PercorsiIcone.Foto },
            { "jpeg", PercorsiIcone.Foto },
            { "heic", PercorsiIcone.Foto }
        };
    }
}

// I pugs sono passati di qui
// Davide Baldinu, Giada Croci, Lorenzo Porta