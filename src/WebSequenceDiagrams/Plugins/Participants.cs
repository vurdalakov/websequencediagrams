namespace Vurdalakov.WebSequenceDiagrams
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Text.RegularExpressions;

    public class Participants : Dictionary<String, String>
    {
        public Int32 AliasesCount { get; private set; }

        public Participants(String script)
        {
            this.AliasesCount = 0;

            using (var stringReader = new StringReader(script))
            {
                var line = "";
                while ((line = stringReader.ReadLine()) != null)
                {
                    var participant = GetParticipant(line);
                    if (participant != null)
                    {
                        if (participant[1] != null)
                        {
                            this.AliasesCount++;
                        }

                        this.Add(participant[0], participant[1]);
                    }
                    else
                    {
                        var matches = Regex.Match(line, "(.+?)<?-+>+(.+?):");
                        if (3 == matches.Groups.Count)
                        {
                            AddUser(matches.Groups[1].Value.Trim());
                            AddUser(matches.Groups[2].Value.Trim());
                        }
                    }
                }
            }
        }

        private void AddUser(String userName)
        {
            if (!this.ContainsKey(userName) && !this.ContainsValue(userName))
            {
                this.Add(userName, null);
            }
        }

        public static String[] GetParticipant(String line)
        {
            const String participant = "participant ";

            if (!line.Trim().StartsWith(participant, StringComparison.CurrentCultureIgnoreCase))
            {
                return null;
            }

            line = line.Trim().Substring(participant.Length);

            var parts = line.Split(new String[] { " as " }, StringSplitOptions.None);

            var array = new String[2];
            array[0] = RemoveQuotes(parts[0]);
            array[1] = RemoveQuotes(2 == parts.Length ? parts[1] : null);

            return array;
        }

        private static String RemoveQuotes(String text)
        {
            if (String.IsNullOrWhiteSpace(text))
            {
                return null;
            }

            text = text.Trim();

            if (text.StartsWith("\"") && text.EndsWith("\""))
            {
                text = text.Substring(1, text.Length - 2).Trim();
                return String.IsNullOrEmpty(text) ? null : text;
            }
            else
            {
                return text;
            }
        }
    }
}
