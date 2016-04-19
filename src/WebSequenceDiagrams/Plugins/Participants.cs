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
                    if (line.Trim().StartsWith("participant ", StringComparison.CurrentCultureIgnoreCase))
                    {
                        line = line.Trim().Substring("participant ".Length);

                        var parts = line.Split(new String[] { " as " }, StringSplitOptions.None);

                        var participant = parts[0].Trim();
                        if (participant.StartsWith("\"") && participant.EndsWith("\""))
                        {
                            participant = participant.Substring(1, participant.Length - 2).Trim();
                        }

                        if (2 == parts.Length)
                        {
                            this.AliasesCount++;
                        }

                        this.Add(participant, 2 == parts.Length ? parts[1].Trim() : null);
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
    }
}
