<div align="center">

![Icon](/assets/Icon128.png)

# CurvaLauncher

✨ *Einfacher, leichter und schneller Desktop-Launcher* ✨

[![License](https://img.shields.io/github/license/OrgEleCho/CurvaLauncher
)](LICENSE.txt) [![Version](https://img.shields.io/github/v/release/OrgEleCho/CurvaLauncher?include_prereleases
)](https://github.com/OrgEleCho/CurvaLauncher/releases) / [![EN-US](https://img.shields.io/badge/EN-US-blue)](README.md) [![ZH-Hans](https://img.shields.io/badge/中文-简体-red)](README.zh.md) [![DE](https://img.shields.io/badge/DE-de)](README.de.md)

</div>

<br />

## Einführung

CurvaLauncher ist ein einfacher Desktop-Launcher für Windows. 

- Programme und Anwendungen ausführen
- Berechnen mathematischer Ausdrücke
- Zusammenfassung von Daten erhalten
- Texte übersetzen
- ...

<br />

## Installation

1. [Neueste Version](https://github.com/OrgEleCho/CurvaLauncher/releases) herunterladen.
2. Entpacken Sie das Archiv und Sie finden die `CurvaLauncher.exe` im Verzeichnis.
3. `CurvaLauncher.exe` ausführen und genießen!

> Hinweis: Stellen Sie sicher, dass die [.NET Desktop Runtime 8.0.0 (x64)](https://dotnet.microsoft.com/en-us/download/dotnet/8.0) auf Ihrem Computer installiert ist.

<br />

## Verwendung

- `Alt + Leertaste` zum Öffnen des Launchers
- Etwas eingeben um ein Ergebnis zu erhalten
- `Pfeil hoch` und `Pfeil runter` zum Auswählen eines Eintrags
- `Eingabetaste` um den ausgewählten Eintrag aufzurufen

> Tipp: Einige Aufrufergebnisse werden in die Zwischenablage kopiert.

<br />

## Plugins

Die meisten Funktionen von CurvaLauncher sind in Form von Plugins verfügbar

### Integrierte Plugins

- RunApplication: Geben Sie den Namen der auszuführenden Anwendung ein.
  (Anwendungen im Startmenü und auf dem Desktop werden unterstützt)
- RunProgram: Geben Sie einen Befehl zum Ausführen ein.
  (In den Einstellungen können Verzeichnisse ein-/ausgeschlossen werden)
- Calculator: Geben Sie zur Berechnung einen mathematischen Ausdruck mit dem Präfix '=' ein.
  (Gängige mathematische Funktionen werden unterstützt, `PI` und `E` werden ebenfalls unterstützt)
- OpenUrl: Geben Sie eine URL zum Öffnen ein.
  (Verwenden Sie Ihren Standardbrowser)
- Translator: Geben Sie `>trans` und einen zu übersetzenden Text ein.
  (Zwischen `>trans` und dem Text ist ein Leerzeichen erforderlich. Sie können in den Einstellungen auch die Quell-/Zielsprache konfigurieren oder die Übersetzungs-Engine wechseln)
- Hashing: Geben Sie `#` und eine Hashing-Methode ein und anschließend einen Text oder Dateipfad, um eine Zusammenfassung zu erhalten
  (Zum Beispiel, '#md5 123' oder '#sha256 C:\Users\OrgEleCho\Desktop\test.txt'. Derzeit unterstützt `md5`, `sha1`, `sha256`, `sha384`, `sha512`)

<br />

### Eigenes Plguin erstellen

1. Klonen Sie den Code dieses Repository.
2. Erstellen Sie ein neues Projekt mit dem Zielframework `net8.0-windows`.
3. 'CurvaLauncher.Plugin' zur Projektreferenz hinzufügen
4. Erstellen Sie eine Plugin-Klasse, die die Schnittstelle `ISyncPlugin` oder `IAsyncPlugin` implementiert.
5. Implementieren Sie die Schnittstellenmitglieder und schreiben Sie die Hauptlogik.

> Tipp: Synchrone und asynchrone Plugins geben an, ob Ihr Plugin Abfragen synchron oder asynchron ausführt. Sie können eine davon basierend auf Ihrer Plugin-Logik auswählen. Abfrageergebnisse werden ebenfalls in synchrone und asynchrone unterteilt. Es wird lediglich das entsprechende Abfrageergebnis geerbt.
> 
> Beispielsweise kehrt ein Übersetzer-Plugin unmittelbar nach dem Auslösen eines Schlüsselworts zurück. Da kein asynchroner Vorgang erforderlich ist, ist das Plugin synchron. Wenn der Benutzer jedoch die Eingabetaste drückt, um einen Übersetzungsvorgang durchzuführen, erfordert dieser Vorgang eine Netzwerkanfrage. Mit anderen Worten, das Ergebnis dieses Plugins st asynchron. Daher sollten Sie `ISyncPlugin` und `AsyncQueryResult` verwenden, um dieses Plugin zu implementieren.

<br />

## Danke

- [Securify.ShellLink](https://github.com/securifybv/ShellLink/): Eine .NET-Klassenbibliothek zur Verarbeitung von ShellLink-Dateien (LNK).

<br />

---

<br />

<div align="center">

Vorschau

![](/assets/preview2.png)

![](/assets/preview4.png)

</div>
