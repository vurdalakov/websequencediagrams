namespace Vurdalakov.WebSequenceDiagrams
{
    using System;
    using System.Collections.Generic;

    public static class PluginManager
    {
        public static IWebSequenceDiagramsPlugin[] FindPlugins()
        {
            var typeNames = new List<IWebSequenceDiagramsPlugin>();

            var interfaceType = typeof(IWebSequenceDiagramsPlugin);

            foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
            {
                foreach (var type in assembly.GetTypes())
                {
                    if (interfaceType.IsAssignableFrom(type) && type.IsClass)
                    {
                        var plugin = Activator.CreateInstance(type) as IWebSequenceDiagramsPlugin;
                        typeNames.Add(plugin);
                    }
                }
            }

            typeNames.Sort((p1, p2) => p1.GetMenuName().CompareTo(p2.GetMenuName()));

            return typeNames.ToArray();
        }
    }
}
