using System.Windows;
using LangRX;

namespace LangRXDemo;

public partial class MainWindow : Window
{
    private bool _english;

    public MainWindow()
    {
        InitializeComponent();
    }

    private void ChangeLanguage_Click(object sender, RoutedEventArgs e)
    {
        // Switches culture and reloads the dictionary; DynamicResources update automatically.
        _english = !_english;
        LangRXDictionary.SetLanguage(_english ? "en-US" : "es-ES");
    }
}