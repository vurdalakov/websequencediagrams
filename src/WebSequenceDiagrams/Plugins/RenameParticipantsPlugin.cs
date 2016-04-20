namespace Vurdalakov.WebSequenceDiagrams
{
    using System;
    using System.IO;
    using System.Text;
    using System.Text.RegularExpressions;
    using System.Windows;

    public class RenameParticipantsPlugin : IWebSequenceDiagramsPlugin
    {
        public String GetMenuName()
        {
            return "Renames Participants...";
        }

        public String GetDescription()
        {
            return ""; // TODO
        }

        public String ModifyScript(String script)
        {
            var participants = new Participants(script);
            var viewModel = new RenameParticipantsViewModel(participants);

            var dlg = new RenameParticipantsPluginWindow(Application.Current.MainWindow, viewModel);
            dlg.ShowDialog();

            if (!dlg.DialogResult.HasValue || !dlg.DialogResult.Value)
            {
                return script;
            }

            var isUser = viewModel.IsUser;
            var oldName = viewModel.OldName;
            var newName = viewModel.NewName;

            var newScript = new StringBuilder();
            using (var stringReader = new StringReader(script))
            {
                var line = "";
                while ((line = stringReader.ReadLine()) != null)
                {
                    var participant = Participants.GetParticipant(line);
                    if (participant != null)
                    {
                        if (null == participant[1])
                        {
                            if (isUser)
                            {
                                newScript.AppendFormat("participant {0}\r\n", Rename(participant[0], oldName, newName));
                            }
                            else
                            {
                                newScript.AppendLine(line);
                            }
                        }
                        else
                        {
                            if (isUser)
                            {
                                newScript.AppendFormat("participant {0} as {1}\r\n", Rename(participant[0], oldName, newName), participant[1]);
                            }
                            else
                            {
                                newScript.AppendFormat("participant {0} as {1}\r\n", participant[0], Rename(participant[1], oldName, newName));
                            }
                        }
                    }
                    else
                    {
                        var matches = Regex.Match(line, @"(.+?)(<?-+>+)(.+?):(.*)");
                        if (5 == matches.Groups.Count)
                        {
                            newScript.AppendFormat("{0}{1}{2}{3}{4}\r\n",
                                Rename(matches.Groups[1].Value.Trim(), oldName, newName),
                                matches.Groups[2].Value.Trim(),
                                Rename(matches.Groups[3].Value.Trim(), oldName, newName),
                                ":",
                                matches.Groups[4].Value.Trim());
                        }
                        else
                        {
                            newScript.AppendLine(line);
                        }
                    }
                }

                return newScript.ToString();
            }
        }

        private String Rename(String name, String oldName, String newName)
        {
            return name.Equals(oldName) ? newName : name;
        }
    }
}

