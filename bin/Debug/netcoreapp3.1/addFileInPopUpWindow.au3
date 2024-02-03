; Assuming the file open dialog box is already open
WinWaitActive("Open") ; Waits for the dialog box with the title "Open" to become active
ConsoleWrite("File open dialog box is active." & @CRLF)

; Get the directory of the currently executing script
$scriptDir = @ScriptDir
ConsoleWrite("Script directory: " & $scriptDir & @CRLF)

; Construct the full file path
$filePath = $scriptDir & "\testfile.txt"

; Send the file path to the file upload dialog
Send($filePath)
ConsoleWrite("File path sent to file upload dialog." & @CRLF)

Send("{ENTER}")
ConsoleWrite("Enter key sent." & @CRLF)