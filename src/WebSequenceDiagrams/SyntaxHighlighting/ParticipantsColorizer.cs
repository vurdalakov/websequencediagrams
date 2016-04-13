namespace Vurdalakov.WebSequenceDiagrams
{
    using System;
    using System.Text.RegularExpressions;
    using System.Windows.Media;

    using ICSharpCode.AvalonEdit;
    using ICSharpCode.AvalonEdit.Document;
    using ICSharpCode.AvalonEdit.Rendering;

    public class ParticipantsColorizer : DocumentColorizingTransformer
    {
        private readonly TextEditor textEditor;

        public ParticipantsColorizer(TextEditor textEditor)
        {
            this.textEditor = textEditor;
        }

        protected override void ColorizeLine(DocumentLine line)
        {
            if (line.IsDeleted || (0 == line.Length))
            {
                return;
            }

            var text = this.textEditor.Text.Substring(line.Offset, line.Length);

            var colon = text.IndexOf(':');
            if (colon <= 0)
            {
                var dummy = FindParticipant(line) || FindActivate(line);
                return;
            }

            if (FindNote(line, "note left of ", colon) || FindNote(line, "note right of ", colon) || FindNote(line, "note over ", colon))
            {
                return;
            }

            text = text.Substring(0, colon);

            var match = Regex.Match(text, "(<?<?--?>>?)");
            if (match.Groups.Count != 2)
            {
                return;
            }

            var i = text.IndexOf(match.Groups[1].Value);

            ChangeLinePart(line.Offset, line.Offset + i, HighlightParticipant);
            ChangeLinePart(line.Offset + i + match.Groups[1].Value.Length, line.Offset + colon, HighlightParticipant);
        }

        private void HighlightParticipant(VisualLineElement visualLineElement)
        {
            visualLineElement.TextRunProperties.SetForegroundBrush(Brushes.Green);
        }

        // participant Client
        // participant Server as SRV
        private Boolean FindParticipant(DocumentLine line)
        {
            var text = this.textEditor.Text.Substring(line.Offset, line.Length);

            const String participant = "participant ";
            const String @as = " as ";

            if (!text.Trim().StartsWith(participant, StringComparison.CurrentCultureIgnoreCase))
            {
                return false;
            }

            var i1 = text.IndexOf(participant) + participant.Length;
            var i2 = text.IndexOf(@as, i1);

            if (i2 < 0)
            {
                ChangeLinePart(line.Offset + i1, line.EndOffset, HighlightParticipant);
            }
            else
            {
                ChangeLinePart(line.Offset + i1, line.Offset + i2, HighlightParticipant);
                ChangeLinePart(line.Offset + i2 + @as.Length, line.EndOffset, HighlightParticipant);
            }

            return true;
        }

        // note right of Client: This is displayed right of Client.
        // note left of Server: This is displayed left of Client.
        // note over Client, Server: This is displayed over Client and Server.
        private Boolean FindNote(DocumentLine line, String keyword, Int32 colon)
        {
            var text = this.textEditor.Text.Substring(line.Offset, line.Length);

            if (!text.Trim().StartsWith(keyword, StringComparison.CurrentCultureIgnoreCase))
            {
                return false;
            }

            var pos = text.IndexOf(keyword) + keyword.Length;

            while (true)
            {
                var comma = text.IndexOf(',', pos);
                if (comma < 0)
                {
                    break;
                }

                ChangeLinePart(line.Offset + pos, line.Offset + comma, HighlightParticipant);
                pos = comma + 1;
            }

            ChangeLinePart(line.Offset + pos, line.Offset + colon, HighlightParticipant);

            return true;
        }

        // activate Client
        // deactivate Server
        private Boolean FindActivate(DocumentLine line)
        {
            var colon = line.EndOffset - line.Offset;
            return FindNote(line, "activate ", colon) || FindNote(line, "deactivate ", colon);
        }
    }

}
