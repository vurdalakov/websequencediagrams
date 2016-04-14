Set args = WScript.Arguments

If (args.Count <> 3) And (args.Count <> 4) Then
    WScript.Echo "Usage: ReplaceText.vbs <file name> <old text> <new text> [/i]"
	WScript.Quit 1
End If

caseSensitive = 0
For i = 3 to args.Count-1
    If LCase(args(i)) = "/i" Then caseSensitive = 1
Next

Set fso = CreateObject("Scripting.FileSystemObject")

fileName = args(0)
oldText = args(1)
newText = args(2)

WScript.Echo "Replacing '" + oldText + "' to '" + newText + "' in '" + fileName + "'"

If Not fso.FileExists(fileName) Then
    WScript.Echo "File does not exist: " + fileName
	WScript.Quit 2
End If

Set file = fso.OpenTextFile(fileName, 1)
text = file.ReadAll
file.Close

text = Replace(text, oldText, newText, 1, -1, caseSensitive)

Set file = fso.OpenTextFile(fileName, 2)
file.Write text
file.Close
