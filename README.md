# WamWooWam.Wpf
My personal WPF toolkit and theme, designed to make my life 100x easier.

## Themes
Using my personal WPF theme is pretty easy to do, at least in my opinion.

Firstly, correctly manifest your application. In Visual Studio, right-click your project, go to Add, and add a blank app manifest. Then uncomment the given `supportedOS` declarations. This allows me to detect which version of Windows the app is currently running on, without this, Windows 8.1 and 10 report their version as `Microsoft Windows 6.2.9200.0`, because of course they fucking do.

Now, add a line like the following to your app startup, before showing any UI:
```cs
using WamWooWam.Wpf.Themes;
...
Themes.SetTheme();
```

#### Light/Dark mode

If you want to enforce a specific mode, you can do something a la:
```cs
// Enforce light theme
Themes.SetTheme(light: true);
```
If not, all's g, we'll try and grab the current user's theme on Windows 10, or default to the light theme on Windows 7 and 8.

#### Accent Colours

If you want to enforce a specific accent colour, like a specific brand colour, you can do something like:
```cs
// Set theme colour to Discord Blurple
Themes.SetTheme(accentColour: Color.FromRgb(0x72, 0x89, 0xDA)); 
```
If you don't do this, no matter, we'll try and grab the current Windows accent colour instead, which works 99% of the time.

