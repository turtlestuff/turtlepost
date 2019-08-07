using System.Globalization;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;
using System.Text.Json;

namespace TurtlePost
{
    public static class I18n
    {
        static Dictionary<string, string> strings = new Dictionary<string, string>();
        
        static I18n()
        {
            InsertResources($"TurtlePost.Localization.{CultureInfo.CurrentUICulture.IetfLanguageTag}.json");
            InsertResources("TurtlePost.Localization.en-US.json");
        }

        static void InsertResources(string manifestResourceName) 
        {
            var resourceStream = Assembly.GetExecutingAssembly().GetManifestResourceStream(manifestResourceName);
            
            if (resourceStream == null) 
            {
                //Do nothing; this language is not found.
            } 
            else
            {
                var englishStrings = JsonDocument.Parse(resourceStream).RootElement;
                foreach (var prop in englishStrings.EnumerateObject().Where(prop => !strings.ContainsKey(prop.Name)))
                {
                    strings.Add(prop.Name, prop.Value.GetString());
                }
            }
        }
        
        public static string _(string key)
        {
            return strings.ContainsKey(key) ? strings[key] : key;
        }
    }
}