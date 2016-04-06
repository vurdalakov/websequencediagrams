namespace Vurdalakov.WebSequenceDiagrams
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Text.RegularExpressions;

    public static class WebSequenceDiagramsActions
    {
        public static String ExtractParticipants(String script)
        {
            var lines = new List<String>();
            var participants = new Dictionary<String, String>();

            using (var stringReader = new StringReader(script))
            {
                var line = "";
                while ((line = stringReader.ReadLine()) != null)
                {
                    if (line.Trim().StartsWith("participant ", StringComparison.CurrentCultureIgnoreCase))
                    {
                        line = line.Trim().Substring("participant ".Length);

                        var parts = line.Split(new String[] { " as " }, StringSplitOptions.None);

                        var participant = parts[0].Trim();
                        if (participant.StartsWith("\"") && participant.EndsWith("\""))
                        {
                            participant = participant.Substring(1, participant.Length - 2).Trim();
                        }

                        participants.Add(participant, 2 == parts.Length ? parts[1].Trim() : null);
                    }
                    else
                    {
                        lines.Add(line);
                    }
                }
            }

            var lineIndexToInsert = 0;
            foreach (var line in lines)
            {
                if (!String.IsNullOrEmpty(line) && !line.StartsWith("#") && !line.StartsWith("title "))
                {
                    break;
                }
                lineIndexToInsert++;
            }

            for (var i = lineIndexToInsert; i < lines.Count; i++)
            {
                var matches = Regex.Match(lines[i], "(.+?)<?-+>+(.+?):");
                if (3 == matches.Groups.Count)
                {
                    var participant1 = matches.Groups[1].Value.Trim();
                    var participant2 = matches.Groups[2].Value.Trim();
                    if (!participants.ContainsKey(participant1) && !participants.ContainsValue(participant1))
                    {
                        participants.Add(participant1, null);
                    }
                    if (!participants.ContainsKey(participant2) && !participants.ContainsValue(participant2))
                    {
                        participants.Add(participant2, null);
                    }
                }
            }

            var participantLines = new List<String>();

            //if (!String.IsNullOrEmpty(lines[lineIndexToInsert]))
            //{
                participantLines.Add("");
            //}
            foreach (var participant in participants)
            {
                participantLines.Add("participant " + (null == participant.Value ? participant.Key : String.Format("{2}{0}{2} as {1}", participant.Key, participant.Value, participant.Key.Contains(" ") ? "\"" : "")));
            }
            participantLines.Add("");
            lines.InsertRange(lineIndexToInsert, participantLines);

            return String.Join("\r\n", lines);
        }
    }
}
