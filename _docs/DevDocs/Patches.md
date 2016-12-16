---
title: Patches
---
{% assign manual_doc = site.docs | where: "url", "/docs/Manual/Manual.html" | first %}
Patches sind Dateien, welche Informationen über Unterschiede zwischen zweier Dateien beinhalten.  
Eine Präzisere Information zu diesem Fakt finden Sie in der [Bedienungsanleitung].

Die Patches unterscheiden sich grundsätzlich durch das _Patching-System_ in dem sie geschrieben wurden.  
Das Patching-System definiert quasi das Dateiformat des Patches.
Vom RHR-Patcher werden 3 verschiedene Patching-Systeme unterstützt, welche im Folgenden genauer beschrieben werden:

  * [**International Patching-System** kurz: **IPS**](#{{ 'International Patching-System' | slugify }})
  * [**Universal Patching-System** kurz: **UPS**](#{{ 'Universal Patching-System' | slugify }})
  * [**RomHackersResources Patching-System** kurz: **RPS**](#{{ 'RomHackersResources Patching-System' | slugify }})

<div class="panel panel-primary">
    <div class="panel-heading">
        <div class="panel-title"><span class="glyphicon glyphicon-info-sign"></span> Information: Magic-Bytes</div>
    </div>
    <div class="panel-body">
        Magic-Bytes sind die Bytes, die zu Beginn eines Blocks oder einer Datei stehen.<br />
        Da sie, wie auch die Datei-Endungen, dafür gedacht sind, Dateien identifizierbar zu machen,
        werden sie zum Teil auch "Datei-Signaturen" genannt.
    </div>
</div>

## International Patching-System

Das International Patching-System ist ein sehr simples System,
welches man mit nur wenig Erfahrung ohne Probleme selbst programmieren kann.  
Genau darauf ist es aufgelegt: Simpel aufgebaut, schnell, übersichtlich und leicht verständlich.

  * :thumbsup: Die Vorteile des IPS-Systems
      * Simpel aufgebaut
      * Gut strukturiert
      * Übersichtlich
      * Schnell
  * :thumbsdown: Die Nachteile des IPS-Systems
      * Die Dateigrösse ist auf 16 MB begrenzt

### Datei-Struktur

| Name                  | Länge (in Bytes)  | Beschreibung                                                                                                                                          |
|-----------------------|------------------:|-------------------------------------------------------------------------------------------------------------------------------------------------------|
| Header                |                 5 | Die Magic-Bytes `PATCH`                                                                                                                               |
| Records               | Beliebig          | Blöcke, in denen Informationen zu sich geänderten Daten beinhaltet. Hierbei kann es sich wahlweise um einen normalen oder einen RLE-Block handeln.    |
| EndOfFile-Indicator   |                 3 | Die Bytes, welche das Ende der Datei markieren: `EOF`                                                                                                 |
| Output-Size           |          0 oder 3 | Falls die Modifikation eine andere Grösse als das Original hat, wird diese nach den EOF-Indikator geschrieben                                         |

#### Normaler Block

| Name              | Länge (in Bytes)  | Beschreibung                                                                                                              |
|-------------------|------------------:|---------------------------------------------------------------------------------------------------------------------------|
| Offset            |                3  | Die Position, an der sich der Block befindet                                                                              |
| <var>Size</var>   |                2  | Die Grösse des Blocks. Dieser Wert ist stets grösser als 0. Falls der Wert 0 ist, handelt es sich um einen _RLE-Block_    |
| Data              | <var>Size</var>   | Die Daten, welche der Block beinhaltet                                                                                    |

#### RLE-Block

RLE heisst ausgeschrieben "run-length encoding".
Diese Codierung dient dazu, längere Wiederholungen von Symbolen verlustfrei zu speichern.  
Als kleines Beispiel kann man die Folge
`00000 111`  
auch einfach als  
`50 31`  
Was soviel heisst, dass die Folge 5 Nullen und drei Einsen beinhaltet.  
Diesem Prinzip folgt auch der RLE Encoded Block im IPS:

| Name              | Länge (in Bytes)  | Beschreibung                                          |
|-------------------|-------------------|-------------------------------------------------------|
| Offset            |                 3 | Die Position, an der sich der Block befindet          |
| RLE-Indicator     |                 2 | Hat immer den Wert 0                                  |
| RLE-Size          |                 2 | Die Grösse des RLE-Blocks                             |
| Value             |                 1 | Der Wert, mit dem der Block aufgefüllt werden soll.   |

Mehr Informationen zum Dateiformat finden Sie auf [ZeroSoft].

## Universal Patching-System

Mit dem UPS-System hat man es sich zur Aufgabe gemacht, ein Patching-System zu entwickeln,
welches genauso leicht implementier- und programmierbar ist, aber einige Vorteile wie unbegrenzt grosse Dateien zulässt.

Mit diesem Hintergedanken entwickelte man das Universal Patching-System

  * :thumbsup: Vorteile
      * Unendlich grosse Dateien
      * Original-Datei lässt sich wiederherstellen, wenn man den Patch auf die neue Datei anwendet
      * Leicht implementierbar

### Variable Length-Encoding

Den grossen Unterschied zu IPS macht das UPS durch das sogenannte _"Variable Length-Encoding"_, kurz: VLE.  
VLE hat den Vorteil, dass Angaben nicht mehr eine gewisse Anzahl Bytes haben müssen
(Bspw. kann ein Offset im IPS maximal 3 Bytes lang sein, was eine maximal-Zahl von 16777215 => 16 MB zulässt und womöglich zu wenig ist),
sondern Zahlen beliebiger Länge beinhalten kann und automatisch erkennt, wann eine Zahl zu Ende ist.

Folgender Pseudo-Code zeigt die decodierung eines VLE-Werts:

```csharp
public static long Decode(byte[] encoded)
{
    long result = 0;

    for (int i = 0; i < encoded.Length; i++)
    {
        int value = encoded[i] & 0x7F;
        if (i > 0)
            value++;
        result += value << (i * 7);
    }
    return result;
}
```

Und dieser Pseudo-Code zeigt, wie ein Wert VLE-Codiert wird:

```csharp
public static byte[] Encode(long value)
{
    List<byte> result = new List<byte>();
    long part = 0;

    do
    {
        part = value & 0x7F;
        value >>= 7;
        if (value == 0)
            part |= 0x80;
        result.Add((byte)part);
        value--;
    }
    while (part < 0x80);

    return result.ToArray();
}
```

Diese Codierungs-Methode wird in diesem Patching-System vielfach verwendet und wird fortan nur noch als "VLE" bezeichnet.

### Datei-Struktur

| Name              | Länge (in Bytes)  | Beschreibung                                                      |
|-------------------|------------------:|-------------------------------------------------------------------|
| Header            |                 4 | Die Magic-Bytes `UPS1`                                            |
| Original-size     |          variabel | Die Grösse der originalen Datei; VLE-Codiert                      |
| Modified-size     |          variabel | Die Grösse der bearbeiteten Datei; VLE-Codiert                    |
| Records           |          variabel | Die Blöcke, welche Informationen zu geänderten Daten beinhalten   |
| Original-Checksum |                 4 | Der CRC32-Hash der originalen Datei                               |
| Modified-Checksum |                 4 | Der CRC32-Hash der bearbeiteten Datei                             |
| Patch-Checksum    |                 4 | Der CRC32-Hash der Patch-Datei abgesehen von den letzten 4 Bytes  |

#### Die Blöcke

| Name                  | Länge (in Bytes)  | Beschreibung                                                  |
|-----------------------|------------------:|---------------------------------------------------------------|
| Relative Difference   |          variabel | Der Abstand zum letzten Block ***oder*** zum Anfang der Datei |
| Data                  |          variabel | originale Datei XOR bearbeitete Datei                         |
| EndOfBlock Indicator  |                 1 | Abschliessendes `0x00`                                        |

## RomHackersResources Patching-System

Das RHR-Patching-System wurde mit dem Hintergedanken entwickelt,
dass es extrem Platzsparend sein könnte, in das UPS auch die RLE-Blöcke mit einfliessen zu lassen.

Daraus resultiert das neue RomHackersResources Patching-System.

  * :thumbsup: Vorteile
      * Platzsparend
      * Leicht implementierbar
      * Unbegrenzt grosse Dateien
  * :thumbsdown: Nachteile
      * Patchvorgang lässt sich nicht rückgängig machen

### Datei-Struktur

| Name              | Länge (in Bytes)  | Beschreibung                                                                                                                          |
|-------------------|------------------:|---------------------------------------------------------------------------------------------------------------------------------------|
| Header            |                 3 | Die Magic-Bytes `RHR`                                                                                                                 |
| Modified-Size     |          variabel | Die Grösse der bearbeiteten Datei; VLE-Codiert                                                                                        |
| Records           |          variabel | Die Blöcke, welche Informationen zu geänderten Daten beinhalten. Dabei kann es sich wahlweise um RLE oder normale Blöckel handeln.    |
| Original-Checksum |                 4 | Der CRC32-Hash der originalen Datei                                                                                                   |
| Modified-Checksum |                 4 | Der CRC32-Hash der bearbeiteten Datei                                                                                                 |
| Patch-Checksum    |                 4 | Der CRC32-Hash der Patch-Datei abgesehen von den letzten 4 Bytes                                                                      |

#### Normaler Block

| Name                  | Länge (in Bytes)  | Beschreibung                                                                                                                          |
|-----------------------|------------------:|---------------------------------------------------------------------------------------------------------------------------------------|
| Relative Difference   |          variabel | Der Abstand zum letzten Block ***oder*** zum Anfang der Datei                                                                         |
| <var>Size</var>       |          variabel | Die Grösse des Blocks; VLE-Codiert. Dieser Wert ist stets grösser als 0. Falls der Wert 0 ist, handelt es sich um einen _RLE-Block_   |
| Data                  |   <var>Size</var> | Die Daten, welche der Block beinhaltet                                                                                                |

#### RLE-Block

| Name                  | Länge (in Bytes)  | Beschreibung                                                  |
|-----------------------|------------------:|---------------------------------------------------------------|
| Relative Difference   |          variabel | Der Abstand zum letzten Block ***oder*** zum Anfang der Datei |
| RLE-Indicator         |                 1 | Hat immer den Wert 0; VLE-Codiert.                            |
| RLE-Size              |          variabel | Die Grösse des RLE-Blocks; VLE-Codiert.                       |
| Value                 |                 1 | Der Wert, mit dem der Block aufgefüllt werden soll.           |

<!--- References -->
[Bedienungsanleitung]:  {{ manual_doc.url }}
[ZeroSoft]:             http://zerosoft.zophar.net/ips.php