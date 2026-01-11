1. Abfrage: Ein Benutzer gibt eine Eingabeaufforderung an eine MCP-kompatible Anwendung. Dies könnte beispielsweise lauten: „Erstelle ein GitHub-Issue und benachrichtige mein Team auf Slack.“ Die Eingabeaufforderung gelangt über den MCP-Client in das System.

2. MCP-Client: Der MCP-Client empfängt die Eingabeaufforderung des Benutzers und bereitet sie für die Verarbeitung vor. Er verwaltet den Ablauf zwischen dem Modell, den verfügbaren Tools und der endgültigen Ausgabe. 

3. LLM (Sprachmodell): Der MCP-Client sendet die Abfrage an ein LLM wie GPT-4. Das LLM analysiert die Aufgabe, versteht die Absicht des Benutzers und plant die erforderlichen Schritte – einschließlich der zu verwendenden Tools. Dazu überprüft es die von MCP-Servern bereitgestellten Tool-Schemas.

4. Tool-Auswahl: Basierend auf seinem Verständnis der Anfrage und den verfügbaren Tools teilt das LLM dem MCP-Client mit, welcher MCP-Server aufgerufen und welche Funktion (Tool) ausgeführt werden soll.

5. Anfrage an MCP-Server senden: Der MCP-Client sendet eine Anfrage an den ausgewählten MCP-Server. Die Anfrage enthält den Funktionsnamen und alle vom Tool benötigten Parameter.

6. Tool-Ausführung über MCP-Server: Der MCP-Server empfängt die Anfrage und leitet sie an das entsprechende Tool oder den entsprechenden Dienst weiter, z. B. GitHub oder Google Drive. Das Tool validiert die Anfrage anhand eines JSON-Schemas, um sicherzustellen, dass alle erforderlichen Eingaben vorhanden und korrekt formatiert sind.

7. Antwort und Ergebnis: Sobald das Tool die Aufgabe abgeschlossen hat, sendet der MCP-Server die Antwort an den MCP-Client zurück. Der Client leitet das Ergebnis dann an das LLM weiter, das es möglicherweise zusammenfasst oder formatiert, bevor es an den Benutzer zurückgegeben wird.