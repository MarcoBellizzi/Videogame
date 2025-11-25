# Unity tutorial funzionalità base

Questo è un progetto da usare come baseline per sviluppare un videogioco con Unity. L'obiettivo e fare un MVP in cui sono presenti tutte le funzionalità principali di un videogioco (movimento di un personaggio, gestione della camera, nemici, boss, menù...). Per ognuno di essi sono trattati capitoli specifici in cui passo dopo passo viene spiegata l'implementazione.

### Configurazione dell'ambiente

La versione di Unity usata è Unity 6.0 (6000.0.62f1). Come Ide di sviluppo uso VS Code. Per funzionare tutto correttemente bisogna settare da Unity VS Code come ide

Edit -> Preferences -> External Tool -> VS Code

Selezionare tutte le check-box e cliccare su Rigenerate project files (per sicurezza). Questo crea un file .slnx e una serie di csproj nella root che VS Code deve riuscire a vedere. Per supportare questi file bisogna installare una versione di dotnet > 9.0.200. Puoi controllare la versione di dotnet con

dotnet --version

Su VS Code invece bisogna installare i seguenti plug-in

* C# Dev Kit
* Unity
* Unity Tools

Se tutto è corretto Dovresti avere attive l'Intellisense sugli script C# generati da Unity.

### Animazioni

Per le animazioni bisogno scaricare prima il modello, un file .fbx e metterlo dentro la cartella Models. A questo modello poi si attacca un animator che ne gestisce le animazioni. Ogni animazione che il modello intende fare va scaricata a parte e messa dentro la relativa cartella in Animations.

### Personaggio

Per la creazione di un personaggio base sono stati utilizzati modelli e animazione importati da Mixamo.

Si parte con un Empty Game Object. Ad esso è stato aggiunto il componente Character controller (utile per la gestione del movimento) e uno script in cui vengono gestiti tutti i suoi movimenti e azioni. Dentro questo oggetto ci vanno un altro game object "Player orientation" utile per capire verso dove sta guardando il giocatore. Viene inserito anche il modello da mixamo, che risulterà rigido e con l'animazione base con cui è stato scaricato.

Per gestire l'animazione viene creato un Animator (aggiunto come componente). Qui dentro va passato il riferimento di un'animazione. Al momento la struttura di un animator di un player è strutturata in due layer, uno per le animazione base e una per tutte le altre animazioni (in overrite della base). Quello base è composto solo da un Blend Tree, un oggetto che definisce l'animazione in base ad un parametro, in questo caso Speed, e definisce se il personaggio è in attesa, cammina o corre. Per passare da un'animazione all'altra bisogna passare dallo script. Nello script del Personaggio, da qualche parte dentro il metodo Update (che viene chiamato ad ogni frame) c'è un animator.setFloat("Speed"...) con cui si comunica il valore della velocità, in base a quel valore il Blend Tree anima il personaggio con una delle tre animazioni.

### Camera

Una funzionalità molto carina in Unity è la Cinemachine. Serve per creare una camera che segue il personaggio. Per usarla basta importare il package Cinemachine (in questo progetto 2.10.4), poi crearne una con GameObject -> Cinemachine -> FreeLookCamera. Da notare che alla camera pricipale viene aggiunto un componente di tipo CineMachineBrain con un rifermimento alla nuova FreeLookCamera. La nuova FreeLookCamera invece ha due parametri importanti da settare ovvero chi guardare e chi seguire (il trasform del player) e una serie di parametri con ui personalizzare i dischi (altezza e ampiezza) della telecamera. Così senza nulla lato script otteniamo una camera che ci segue sempre ad ogni spostamento.
