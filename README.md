# ChatOverflow

Ein simpler Chat mit der Möglichkeit sich zu registrieren und Gruppen- sowie Einzelchats zu nutzen.

Dieses Projekt wird von mir wahrscheinlich nicht mehr weitergeführt. Wer interresse hat kann gerne dran weiterarbeiten.
Grundlegende Funktionen sind vorhanden, allerdings fehlt auch noch einiges.

<b>Funktioniert</b>
 - Anmelden / Registrieren
 - Gruppenchat erstellen
 - Nachrichten im Gruppenchat schreiben
 - Nachrichten im Gruppenchat empfangen
 
<b>Funktioniert nicht:</b>
 - Zugang verhindern wenn man nicht angemeldet ist
 - Einzelchats
 - Nutzereinstellungen (Profielbild usw.)
 
## Projekt aufsetzen

Backend:
 1. Alle Pakete für das Backend installieren (`dotnet restore`).
 2. MySql-Datenbank erstellen und in den appsettings.json eintragen.
Frontend:
 1. NodeJs installieren (am besten die neueste Version)
 2. Im Frontend-Order `Frontend/ChatOverflow/` alle npm-Packete installieren `npm install`
 3. Das Frontend ausführen `ng serve`
 
Ggf. beachten das der Port der API auch im Frontend richtig konfiguriert ist. `Frontend/ChatOverflow/src/app/services/api.service.ts`
