using System;
using System.Globalization;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text.Json;

namespace TurtlePost
{
    public class I18N
    {
        I18N()
        {
            var dict = new Dictionary<string, string>();
            var assembly = typeof(I18N).Assembly;
            var resources = assembly.GetManifestResourceNames();

            var culture = CultureInfo.CurrentUICulture;
            
            // Loop through cultures to populate translations list. It'll loop through the current UI culture's parents
            // until it hits invariant culture (the topmost culture). For each culture, it'll try to find a translation
            // with the name of the culture, fill in as many translation keys as it can, and continue to the parent
            // culture. That way, most translations can be culture-neutral ('de', 'nl') and if necessary certain
            // culture-specific translations can be added.
            while (true)
            {
                try
                {
                    var res = $"TurtlePost.Localization.{culture}";
                    if (resources.Contains(res))
                    {
                        // We've found a translation file for the current culture; parse the JSON
                        var translations = JsonDocument.Parse(
                            assembly.GetManifestResourceStream(res),
                            new JsonDocumentOptions {CommentHandling = JsonCommentHandling.Skip});

                        // Add only new translations to the list. That way, culture-specific translations take precedence
                        // over culture-neutral or invariant culture translations.
                        foreach (var prop in translations.RootElement.EnumerateObject())
                        {
                            if (!dict.ContainsKey(prop.Name))
                            {
                                dict.Add(prop.Name, prop.Value.GetString());
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine("Error reading translation file {0}: {1}", culture, ex);
                    Console.ResetColor();
                }
                
                if (culture.Equals(CultureInfo.InvariantCulture))
                    break;

                culture = culture.Parent;
            }
            
            localizations = dict.ToImmutableDictionary();
        }

        public static I18N TR { get; } = new I18N();
        
        public string this[string key] => localizations.ContainsKey(key) ? localizations[key] : key;
        public string this[string key, params object[] values] =>
            localizations.ContainsKey(key) ? string.Format(localizations[key], values) : key;

        readonly ImmutableDictionary<string, string> localizations;

        static void InsertResources(CultureInfo culture, Dictionary<string, string> dict)
        {
        }
    }
}