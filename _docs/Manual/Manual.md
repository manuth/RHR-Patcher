---
title: Bedienungsanleitung
---
{% include base.html %}
{% assign patches_doc = site.docs | where: "url", "/docs/DevDocs/Patches.html" | first %}
## Das Hauptmenü

<div style="float: right; ">
    <div class="thumbnail">
        <img src="{{ base }}/assets/images/Manual/Manual/MainMenu.png" />
        <div class="caption">
            <h3>Abbildung 1: Das Hauptmenü</h3>
            <p>
                Das Hauptmenü des RHR-Patchers
            </p>
        </div>
    </div>
</div>

Grundsätzlich bietet der Patcher zwei Funktionen:

  * Das Erstellen und Anwenden von Patches
  * Das Verwalten von Projekten

<div style="clear: both; "></div>

## Patches

### Was sind Patches?

Die [ProjectDatabase] ist für jegliche Art von Projekt (sei es Unity, GameMaker oder ein ROM-Hack) ausgelegt.  
Vorwiegend sind auf der [ProjectDatabase] sogenannte "ROM-Hacks" zu finden.  
Hierbei handelt es sich um Spiele (vorwiegend Spiele von Nintendo und vorwiegend Pokémon),
welche nach Belieben angepasst wurden. Ein gutes Beispiel hierfür ist [Pokémon - Sovereign of the Skies].

Würde, als Beispiel, der Autor, [Dragonfly], nun den ROM-Hack so, wie er nun ist, einfach hochladen,
würde er in einen rechtlichen Konflikt mit GameFreak kommen,
da das bearbeitete Spiel dennoch sehr viele Bestandteile des Originals
wie bspw. die gesamte Funktionsweise, beinhaltet, welche ein grosses Stück des Spiels darstellt.

An dieser Stelle kommen _Patches_ ins Spiel.  
Patches sind Dateien, welche Informationen darüber beinhalten, was an einer Datei geändert wurde.  
Würde man also einen Patch von folgenden beiden Dateien machen:

`Original.txt`:

```
Dies ist der Originale Inhalt, welcher urheberrechtlich geschützt ist.
Hallo, Anna!
```

`Neue Datei.txt`:

```
Dies ist der Originale Inhalt, welcher urheberrechtlich geschützt ist.
Hallo, Welt!
```

In dem Patch würde dann nur stehen, dass an der Position 78 in der neuen Datei "Welt" steht.  
Von dem urheberrechtlich geschützten Inhalt würde dann jedoch nichts mehr zu sehen sein.

Wenn nun also Dragonfly einen Patch von [Pokémon - Sovereign of the Skies] macht,
steht im Patch lediglich drin, was er an der Pokémon Smaragd-Edition verändert hat.  
Somit kann er den Patch ohne weiteres hochladen, da er keine urheberrechtlich geschützte Inhalte beinhaltet.

Andere Leute können sich also dann den Patch herunterladen,
Pokémon Smaragd aus irgendeiner Quelle beziehen und den Patch darauf anwenden.

Mehr Informationen zu Patches findet ihr [hier][Patches].

### Patch Anwenden

<div style="float: right; ">
    <div class="thumbnail">
        <img src="{{ base }}/assets/images/Manual/Manual/PatchApplication.png" />
        <div class="caption">
            <h3>Abbildung 2: Das Hauptmenü</h3>
            <p>
                So öffnt man das Patch-Anwendungs-Menü
            </p>
        </div>
    </div>
</div>

Klickt als erstes auf "Patch Anwenden", um das Patch-Anwendungs-Menü aufzurufen.

<div style="clear: both; "></div>

#### Das Patch-Anwendungs-Menü

<div style="float: left; ">
    <div class="thumbnail">
        <img src="{{ base }}/assets/images/Manual/Manual/PatchApplicationMenu.png" />
        <div class="caption">
            <h3>Abbildung 3: Das Patch-Anwendungs-Menü</h3>
            <p>
                So sieht das Patch-Anwendungs-Menü aus.
            </p>
        </div>
    </div>
</div>

In dem Patch-Anwendungs-Menü habt ihr die Möglichkeit, den Pfad zum Patch festzulegen,
welcher angwendet werden soll.  
Öffnet als nächstes die Datei, welche die Basis des Projekts ist.  
Als letztes könnt ihr optional die Datei auswählen, in der die Ausgabe gespeichert werden soll.  
Sollte kein Ausgabepfad angegeben sein, so wird die Original-Datei überschrieben.

<div style="float: right; ">
    <div class="thumbnail">
        <img src="{{ base }}/assets/images/Manual/Manual/PatchApplicationFinished.png" />
        <div class="caption">
            <h3>Abbildung 4: Ladebildschirm</h3>
        </div>
    </div>
</div>

<div style="clear: both; "></div>

### Patches Erstellen

<div style="float: right; ">
    <div class="thumbnail">
        <img src="{{ base }}/assets/images/Manual/Manual/PatchCreation.png" />
        <div class="caption">
            <h3>Abbildung 5: Das Hauptmenü</h3>
            <p>
                So öffnet man das Patch-Erstellungs-Menü
            </p>
        </div>
    </div>
</div>

Öffnet zunächst das Patch-Erstellungs-Menü durch einen Klick auf "Patch Erstellen".

#### Das Patch-Erstellungs-Menü

<div style="float: left; ">
    <div class="thumbnail">
        <img src="{{ base }}/assets/images/Manual/Manual/PatchCreationMenu.png" />
        <div class="caption">
            <h3>Abbildung 6: Das Patch-Erstellungs-Menü</h3>
        </div>
    </div>
</div>
<div style="clear: both; "></div>

In dem darauf folgenden Menü habt ihr die Möglichkeit, auszuwählen, nach welchem Patch-System der Patch erstellt werden soll.  
Unter "Speicherort des Patches" wählt ihr den Speicherort aus, an welchem der Patch abgelegt werden soll.  
Legt unter "Original-Datei" fest, welche Datei eure Basis für das Projekt ist.  
Als letztes wählt ihr nun unter "Modifikation" eure bearbeitete Datei aus und bestätigt die Auswahl mit "Start".

Innert kürzester Zeit ist der Patch erstellt.  

<div style="float: right; ">
    <div class="thumbnail">
        <img src="{{ base }}/assets/images/Manual/Manual/PatchCreationFinished.png" />
        <div class="caption">
            <h3>Abbildung 6: Ladebildschirm</h3>
        </div>
    </div>
</div>


<!--- References -->
[ProjectDatabase]:                  https://rhrpatcher.romresources.net/?Home
[RomHackersResources]:              https://board.romresources.net/
[Pokémon - Sovereign of the Skies]: https://rhrpatcher.romresources.net/?Projects&ID=1
[Dragonfly]:                        https://rhrpatcher.romresources.net/?Users&Name=Dragonfly
[Patches]:                          {{ patches_doc.url }}