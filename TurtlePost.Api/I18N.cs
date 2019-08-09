using System;
using System.Globalization;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text.Json;

namespace TurtlePost
{
    public class I18N
    {
        I18N()
        {
            var dict = new Dictionary<string, string>();
            InsertResources(CultureInfo.CurrentUICulture, dict);
            InsertResources(CultureInfo.GetCultureInfo("en"), dict);
            localizations = dict.ToImmutableDictionary();
        }

        public static I18N TR { get; } = new I18N();
        
        public string this[string key] => localizations.ContainsKey(key) ? localizations[key] : key;
        public string this[string key, params object[] values] =>
            localizations.ContainsKey(key) ? string.Format(localizations[key], values) : key;

        readonly ImmutableDictionary<string, string> localizations;

        static void InsertResources(CultureInfo culture, Dictionary<string, string> dict)
        {
            var resources = typeof(I18N).Assembly.GetManifestResourceNames();
            string? resourceName = default;

            while (!culture.Equals(CultureInfo.InvariantCulture))
            {
                var res = $"TurtlePost.Localization.{culture}";
                if (resources.Contains(res))
                {
                    resourceName = res;
                    break;
                }

                culture = culture.Parent;
            }

            if (resourceName == null)
                return;

            var translations = JsonDocument.Parse(
                typeof(I18N).Assembly.GetManifestResourceStream(resourceName),
                new JsonDocumentOptions {CommentHandling = JsonCommentHandling.Skip});


            foreach (var prop in translations.RootElement.EnumerateObject().Where(prop => !dict.ContainsKey(prop.Name)))
            {
                dict.Add(prop.Name, prop.Value.GetString());
            }
        }
    }
}