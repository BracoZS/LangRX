using System.Windows;
using LangRX;

namespace LangRXDemo;

public partial class App : Application
{
    public App()
    {
        // The dictionary reads strings from Properties/Resources.resx.
        LangRXDictionary.Initialize(LangRXDemo.Properties.Resources.ResourceManager);
    }
}