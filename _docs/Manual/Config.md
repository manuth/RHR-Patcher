---
title: Konfigurationsanleitung
---
{% include base.html %}
Der RHR-Patcher ist umfangreich konfigurierbar. So lässt er beispielsweise Backups, Aufgabenplanung und FTP-Upload zu.  
Im Folgenden werden die Optionen beschrieben.

## Allgemeine Einstellungen

<div style="float: left; ">
    <div class="thumbnail">
        <img src="{{ base }}/assets/images/Manual/Config/General.png" />
        <div class="caption">
            <h3>Abbildung 1: Die Optionen</h3>
            Diese Abbildung zeigt den Options-Dialog
        </div>
    </div>
</div>
<div style="clear: both; "></div>

In den Optionen hat man die Möglichkeit, für vielerlei Ereignisse bestimmte Aktionen auszuführen.  
Wählt aus, ob nach Beendigung eines Patch-Vorgangs das Fenster mit den Schaltflächen "Hauptmenü" und "Beenden" geöffnet bleiben soll,
oder ob das Programm sich beenden oder in das Hauptmenü wechseln soll.

Des Weiteren lässt sich festlegen, ob man Mitteilungen mittels Popup-Fenster erhalten will, wenn Patch-Vorgänge beendet wurden.

Unter zurücksetzen lassen sich sämtliche Einstellungen zurücksetzen und "Sprache" ermöglicht es einem, die derzeitige Anzeigesprache anzupassen.

## Backups

<div style="float: left; ">
    <div class="thumbnail">
        <img src="{{ base }}/assets/images/Manual/Config/Backup.png" />
        <div class="caption">
            <h3>Abbildung 2: Die Optionen</h3>
            So aktiviert man die Backups
        </div>
    </div>
</div>
<div style="clear: both; "></div>

Hakt, um die Backup-Funktion zu aktivieren, die Option "Erstellen von Backups" an und wählt in der darunterliegenden Textbox den Pfad aus,
an dem die Backups abgelegt werden sollen.

Sobald diese Einstellungen übernommen wurden, werden beim Erstellen von Patches Backups im vorgegebenen Ordner angelegt.

## FTP-Upload

<div style="float: left; ">
    <div class="thumbnail">
        <img src="{{ base }}/assets/images/Manual/Config/FTPUpload.png" />
        <div class="caption">
            <h3>Abbildung 2: Die FTP-Einstellungen</h3>
            So aktiviert man den FTP-Upload
        </div>
    </div>
</div>
<div style="clear: both; "></div>

Gebt, um den FTP-Upload zu aktivieren, die Server-Adresse, den Port, FTP-Nutzername und FTP-Passwort an.  
Optional können zusätzlich auch noch auf dem FTP-Server Backups gespeichert werden.

> ***Note***:  
> An demselben Tag erstellte Patches werden überschrieben.

## Automatisierte Patch-Vorgänge (Aufgabenplanung)

Im Options-Fenster findet ihr im Tab "Patch-Vorgang automatisieren" die Aufgabenplanung für den RHR-Patcher.
Hier ist es euch möglich, automatische Erstellungen von Patches festzulegen.

Dies lässt sich ideal kombinieren mit den automatischen Backups und dem FTP-Upload.