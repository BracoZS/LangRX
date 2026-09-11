using System.Collections;
using System.Globalization;
using System.Reflection;
using System.Resources;
using System.Windows;

namespace LangRX;

public class LangRXDictionary : ResourceDictionary
{
    private static LangRXDictionary? _instance;
    private static ResourceManager? _resourceManager;
    private Type? _resourceType;

    public LangRXDictionary()
    {
        _instance = this;

        // Wire the resource manager automatically (design-time or runtime).
        if (_resourceManager == null)
        {
            TryAutoDiscoverResourceManager();
        }

        if (_resourceManager != null)
        {
            SetCulture(CultureInfo.CurrentUICulture);
        }
    }

    /// <summary>Sets the ResourceManager used to populate the dictionary.</summary>
    public static void Initialize(ResourceManager resourceManager)
    {
        _resourceManager = resourceManager;
        if (_instance != null)
        {
            _instance.SetCulture(CultureInfo.CurrentUICulture);
        }
    }

    /// <summary>Switches the app culture and reloads the dictionary.</summary>
    public static void SetLanguage(string cultureCode)
    {
        SetLanguage(new CultureInfo(cultureCode));
    }

    public static void SetLanguage(CultureInfo culture)
    {
        CultureInfo.CurrentCulture = culture;
        CultureInfo.CurrentUICulture = culture;

        _instance?.SetCulture(culture);
    }

    private void SetCulture(CultureInfo culture)
    {
        if (_resourceManager == null) return;

        var resourceSet = _resourceManager.GetResourceSet(
            culture,
            createIfNotExists: true,
            tryParents: true);

        if (resourceSet == null) return;

        foreach (DictionaryEntry entry in resourceSet)
        {
            if (entry.Key is string key && entry.Value is string value)
                this[key] = value;
        }
    }

    #region Editor Designer compatibility

    /// <summary>Sets the resources type from XAML: <code>ResourceType="{x:Type props:Resources}"</code>.</summary>
    public Type? ResourceType
    {
        get => _resourceType;
        set
        {
            _resourceType = value;
            if (value != null)
            {
                var prop = value.GetProperty("ResourceManager", BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
                if (prop?.GetValue(null) is ResourceManager rm)
                {
                    Initialize(rm);
                }
            }
        }
    }

    private static void TryAutoDiscoverResourceManager()
    {
        try
        {
            var assemblies = AppDomain.CurrentDomain.GetAssemblies();
            foreach (var asm in assemblies)
            {
                var name = asm.FullName;
                if (name == null || name.StartsWith("System") || name.StartsWith("Microsoft") || name.StartsWith("mscorlib"))
                    continue;

                var resType = asm.GetTypes().FirstOrDefault(t =>
                    t.Name == "Resources" &&
                    (t.Namespace?.EndsWith("Properties") == true || t.Namespace?.Contains("Resources") == true));

                if (resType != null)
                {
                    var prop = resType.GetProperty("ResourceManager", BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
                    if (prop?.GetValue(null) is ResourceManager rm)
                    {
                        _resourceManager = rm;
                        break;
                    }
                }
            }
        }
        catch
        {
            // Best-effort: ignore reflection errors at design time.
        }
    }

    #endregion
}