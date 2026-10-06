using System;
using System.Collections.Generic;

namespace SmartCraftStorage.Shared
{
    // Matches an item against a comma-separated list typed into the config. Players know
    // items by different names — what the game shows ("Oats"), the prefab name used by
    // console commands ("Oat"), or the localization token ("$item_oat") — so any of the
    // three counts, ignoring case.
    internal static class ItemNameFilter
    {
        private static string _parsedFrom;
        private static HashSet<string> _names = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

        // Localizing allocates, and this runs every time a station looks for input, so
        // each token is localized once per language.
        private static readonly Dictionary<string, string> DisplayNames = new Dictionary<string, string>();
        private static string _displayLanguage;

        public static bool Matches(string list, ItemDrop item)
        {
            if (item == null || string.IsNullOrWhiteSpace(list))
            {
                return false;
            }

            var names = Parse(list);
            if (names.Count == 0)
            {
                return false;
            }

            string token = item.m_itemData.m_shared.m_name;
            return names.Contains(item.gameObject.name)
                || names.Contains(token.TrimStart('$'))
                || names.Contains(DisplayName(token));
        }

        private static HashSet<string> Parse(string list)
        {
            if (list == _parsedFrom)
            {
                return _names;
            }

            var names = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            foreach (var part in list.Split(','))
            {
                // "$item_oat" and "item_oat" are the same token to a player.
                var name = part.Trim().TrimStart('$');
                if (name.Length > 0)
                {
                    names.Add(name);
                }
            }

            _names = names;
            _parsedFrom = list;
            return names;
        }

        private static string DisplayName(string token)
        {
            var localization = Localization.instance;
            if (localization == null)
            {
                return token;
            }

            string language = localization.GetSelectedLanguage();
            if (language != _displayLanguage)
            {
                DisplayNames.Clear();
                _displayLanguage = language;
            }

            if (!DisplayNames.TryGetValue(token, out var shown))
            {
                shown = localization.Localize(token);
                DisplayNames[token] = shown;
            }

            return shown;
        }
    }
}
