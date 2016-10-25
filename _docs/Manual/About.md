---
title: Über den RHR-Patcher
---

## Der RHR-Patcher

Der RHR-Patcher ist ein Programm, welches Patches in drei verschiednenen Formaten erstellen oder anwenden kann.  
Zudem Verfügt er über die Funktionalität, auf die [ProjectDatabase] zuzugreifen.
Dies erlaubt ihm, aktive Projekte auch direkt herunterzuladen und auszuführen/zu patchen.

Ein weiteres nützliches Feature ist die zeitgesteuerte Ausführung von Patch-Vorgängen.
So kann beispielsweise ein ROM-Hacker seinen RHR-Patcher so konfigurieren,
dass jede Woche am Abend ein Patch des Projektes erstellt wird.

## Weitere Komponente

Das Projekt "RHR-Patcher" bringt noch zusätzliche Komponente mit sich,
welche vorwiegend für Entwickler ausgelegt sind.

Folgende weitere Komponente bietet der RHR-Patcher:

### ProjectDBClient

Der ProjectDBClient erlaubt es einem, eine Verbindung zur [ProjectDatabase] herzustellen,
um sich darauf befindliche Projekte herunterzuladen, zu bearbeiten oder zu erfassen.  
Durch Nutzung des ProjectDBClients kann man sich auch selbst ein .NET-Programm oder
eine ASPX-Seite erstellen, welche auf die [ProjectDatabase] zugreift.

### Patches
Das Projekt _Patches_ (Name noch nicht festgelegt) beihnaltet übersichtliche Klassen und interfaces,
welche dazu dienen, dass man einen eigenen Patcher schreiben oder ein eigenes Patch-Format erstellen kann.

<!--- References -->
[ProjectDatabase]: https://rhrpatcher.romresources.net/?Home