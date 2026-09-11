# LangRX

Dynamic localization library for WPF applications based on `.resx` files with runtime language switching support. `No restart`, `no window recreation`. 

## Highlights

- **No code tied to resources:** your XAML just uses `{DynamicResource KeyName}`, like a normal `ResourceDictionary`.
- **Plays well with the designer:** the dictionary auto-discovers the app's `ResourceManager`, so it also works in the XAML editor without running the app.
- **No extra dependencies:** plain WPF.

## Install

```bash
dotnet add package LangRX
```

## Prerequisites

- .NET Framework 4.8 or .NET 8.0-windows+, WPF.
- Resource Files: .resx files configured with Visual Studio's default generator (ResXFileCodeGenerator), ensuring that the Properties.Resources class is available in the project.

## Quick Start

**1. Register the dictionary in `App.xaml`:**

```xml
<Application xmlns:lang="clr-namespace:LangRX;assembly=LangRX"
             StartupUri="MainWindow.xaml">
    <Application.Resources>
        <ResourceDictionary>
            <ResourceDictionary.MergedDictionaries>
                <lang:LangRXDictionary />
            </ResourceDictionary.MergedDictionaries>
        </ResourceDictionary>
    </Application.Resources>
</Application>
```

> **Note:** If the XAML designer doesn't show the texts, pass the resources type explicitly:
>
> ```xml
> xmlns:props="clr-namespace:MyApp.Properties">
>
> <lang:LangRXDictionary ResourceType="{x:Type props:Resources}" />
> ```

**2. Point it at your resources in `App.xaml.cs`:**

```csharp
public App()
{
    LangRXDictionary.Initialize(MyApp.Properties.Resources.ResourceManager);
}
```

**3. Bind strings in XAML:**

```xml
<TextBlock Text="{DynamicResource Greeting}" />
<Button Content="{DynamicResource ButtonChange}" />
```

**4. Switch language:**

Every control bound with `DynamicResource` repaints immediately.

```csharp
LangRXDictionary.SetLanguage("en-US");   // or "es-ES"
```

## API

| Member | Description |
|--------|-------------|
| `Initialize(ResourceManager)` | Sets the `ResourceManager` to populate the dictionary from. |
| `SetLanguage(string)` | Switch culture by code (e.g. `"en-US"`) and reload. |
| `SetLanguage(CultureInfo)` | Same, from a `CultureInfo`. |


`SetLanguage` also sets `CurrentCulture`/`CurrentUICulture`. Culture fallback follows the resx rules (e.g. `en-US` → `en` → neutral).

## Demo

A runnable example lives in [LangRXDemo](LangRXDemo): a button that toggles between `en-US` and `es-ES` using the neutral `Resources.resx` and satellite `Resources.es.resx`.

## License

[MIT](LICENSE) — Copyright (c) 2026 Bracozs