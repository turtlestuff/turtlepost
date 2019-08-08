using System.Globalization;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Reflection;
using System.Linq;
using System.Text.Json;

namespace TurtlePost
{
    public class I18N
    {
        I18N()
        {
            var dict = new Dictionary<string, string>();
            InsertResources($"TurtlePost.Localization.{CultureInfo.CurrentUICulture.Name}.json", dict);
            InsertResources("TurtlePost.Localization.en-US.json", dict);
            Localizations = dict.ToImmutableDictionary();
        }
        
        public static I18N TR { get; } = new I18N();
        public string this[string key] => Localizations.ContainsKey(key) ? Localizations[key] : key;
        
        readonly ImmutableDictionary<string, string> Localizations; 
        
        static void InsertResources(string manifestResourceName, Dictionary<string, string> dict) 
        {
            var resourceStream = Assembly.GetExecutingAssembly().GetManifestResourceStream(manifestResourceName);
            
            if (resourceStream == null) 
            {
                //Do nothing; this language is not found.
            } 
            else
            {
                var englishStrings = JsonDocument.Parse(resourceStream).RootElement;
                foreach (var prop in englishStrings.EnumerateObject().Where(prop => !dict.ContainsKey(prop.Name)))
                {
                    dict.Add(prop.Name, prop.Value.GetString());
                }
            }
        }
    }
}