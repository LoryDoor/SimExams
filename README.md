# Software per la simulazione medica SimExams #

![Logo SimExams](https://github.com/LoryDoor/SimExams/blob/master/SalaRegia/Immagini/logoSimExams.png)

## Cos'è SimExams? ##
SimExams è una soluzione software per PC per la simulazione medica, utilizzabile per inviare file tra due device, idealmente sala di regia e sala di simulazione.

## Come nasce? ##
Il progetto nasce all'interno di uno stage PCTO che ha coinvolto tre studenti del 4° anno presso l'Istituto Tecnico Tecnologico "Giacomo Fauser" di Novara e Simnova, il centro di simulazione medica dell'Università del Piemonte Orientale.
La richiesta del personale di Simnova era quella di un software per inviare i referti dei pazienti simulati da una sala di regia alla sala di simulazione in modo da semplificare il processo di preparazione degli scenari simulati, oggi operato con una presentazione PowerPoint da realizzare ad-hoc per ogni scenario.

## Come funziona? ##
La soluzione è composta da due software:
- **SalaRegia**: permette di caricare i file dei referti e inviarli alla sala di simulazione immediatamente o dopo un tempo impostato dall'utente;
- **SalaSimulazione**: permette di visualizzare i file provenienti dalla sala di regia;
per ulteriori dettagli sul funzionamento dei due software rimandiamo al [manuale di uso](https://github.com/LoryDoor/SimExams/blob/master/SimExams_Manuale_di_uso.pdf).

## Come è stato reallizzato? ##
Il progetto è codificato in Linguaggio C# con Framework .NET 8.0.
Le interfacce grafiche sono state realizzate tramite WPF (Windows Presentation Foundation).

Sono state inoltre incluse le librerie:
- [PdfiumViewer](https://github.com/pvginkel/PdfiumViewer): per la visualizzazione dei PDF;
- [LibVLCSharp](https://code.videolan.org/videolan/LibVLCSharp): per la visualizzazione dei video;

Come IDE è stato utilizzato Visual Studio 2022 edizione Comunity

## Specifiche tecniche ##
L'applicativo è distribuito in formato portable ed è compatibile solo con sistemi operativi Windows.
Per il corretto funzionamento è necessario installare sulle macchine dove veranno eseguiti i due applicativi il [framework .NET 8.0 (o versione successiva)](https://dotnet.microsoft.com/it-it/download/dotnet/8.0)

## Lingua ##
Il codice è stato commentato interamente in italiano.
Attualmente il software è disponibile soltanto in lingua italiana.
Il manuale di uso è redatto in lingua italiana.

## Distribuzione ##
Chi fosse interessato ad utilizzare SimExams, o volesse ricevere ulteriori informazioni sul progetto, contatti Simnova all'email [simnova@uniupo.it](mailto:simnova@uniupo.it)
