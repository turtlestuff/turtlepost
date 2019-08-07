#pragma warning disable 169

using System;
using System.Globalization;
using System.Collections.Generic;
using System.Reflection;
using System.IO;
using System.Text.Json;

namespace TurtlePost
{
    public static class I18n
    {        
        private static Dictionary<string, string> strings;
        
        static I18n()
        {
            strings = new Dictionary<string, string>();
            
            InsertResources($"TurtlePost.Localization.{CultureInfo.CurrentUICulture.IetfLanguageTag}.json");
            InsertResources("TurtlePost.Localization.en-US.json");
        }
        
        private static void InsertResources(string manifestResourceName) {
            Stream? resourceStream = Assembly.GetExecutingAssembly().GetManifestResourceStream(manifestResourceName);
            
            if (resourceStream == null) {
                //Do nothing; this language is not found.
            } else {
                JsonElement englishStrings = JsonDocument.Parse(resourceStream).RootElement;
                foreach (JsonProperty prop in englishStrings.EnumerateObject()) {
                    if (!strings.ContainsKey(prop.Name)) strings.Add(prop.Name, prop.Value.GetString());
                }
            }
        }
        
        public static string _(string key)
        {
            if (strings.ContainsKey(key)) return strings[key];
            return key;
        }
    }
}