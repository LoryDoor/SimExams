using System;
using System.Reflection;

namespace SalaSimulazione.Model
{
    public static class Libreria
    {
        public const string ComandoElimina = "MASTER_SAYS_CLEAR_ALL";                                                                           // Comando per lanciare il reset
        public const int Porta = 5000;                                                                                                          // Porta su cui il server ascolta le connessioni
        public static readonly string PercorsoSalvataggio = $@"{Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)}\Ricevuti\";   // Percorso di salvataggio dei file ricevuti
        public static readonly string[] TipiSupportati = { "pdf", "txt", "mp4", "png", "jpg", "jpeg", "heic" };                                 // Tipi di file supportati

        public struct Stile
        {
            public const int FontSize = 14;         // Dimensione del font per le label
            public const int BorderThickness = 1;   // Spessore del bordo delle celle della tabella
            public struct ImageZoom
            {
                public const double Step = 0.1;     // Passo di zoom per i bottoni di zoom
                public const double MinZoom = 0.1;  // Zoom minimo   
                public const double MaxZoom = 5.0;  // Zoom massimo
            }
            public struct TxtZoom
            {
                public const int MinFontSize = 8;   // Dimensione minima del font
                public const int MaxFontSize = 48;  // Dimensione massima del font
                public const int Step = 2;          // Passo di zoom per i bottoni di zoom
            }
        }
        public struct Dimensioni
        {
            public const int Buffer = 1024;     // Dimensione di ciascun pacchetto da inviare
            public const int NomeFile = 1024;   // Dimensione massima del nome del file
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
        public static readonly Dictionary<string, MethodInfo> Visualizzatori = new() // Associazione tra estensioni dei file e i metodi di visualizzazione
        {
            { "pdf", typeof(Riga).GetMethod("VisualizzaPdf") ?? throw new InvalidOperationException("VisualizzaPdf method not found.") },
            { "txt", typeof(Riga).GetMethod("VisualizzaTxt") ?? throw new InvalidOperationException("VisualizzaTxt method not found.") },
            { "mp4", typeof(Riga).GetMethod("VisualizzaVideo") ?? throw new InvalidOperationException("VisualizzaVideo method not found.") },
            { "png", typeof(Riga).GetMethod("VisualizzaImmagine") ?? throw new InvalidOperationException("VisualizzaImmagine method not found.") },
            { "jpg", typeof(Riga).GetMethod("VisualizzaImmagine") ?? throw new InvalidOperationException("VisualizzaImmagine method not found.") },
            { "jpeg", typeof(Riga).GetMethod("VisualizzaImmagine") ?? throw new InvalidOperationException("VisualizzaImmagine method not found.") },
            { "heic", typeof(Riga).GetMethod("VisualizzaImmagine") ?? throw new InvalidOperationException("VisualizzaImmagine method not found.") }
        };
    }
}

// I pugs sono passati di qui
// Davide Baldinu, Giada Croci, Lorenzo Porta