namespace Vurdalakov.WebSequenceDiagrams
{
    using System;

    public interface IWebSequenceDiagramsPlugin
    {
        String GetMenuName();
        String GetDescription();
        String ModifyScript(String script);
    }
}
