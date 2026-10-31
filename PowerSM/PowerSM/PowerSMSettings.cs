using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;

namespace PowerSM
{
    /// <summary>
    /// User options for the Power Geometry tool, persisted per user in
    /// %APPDATA%\Blue Byte\PowerSM\settings.xml.
    /// (Previous versions read these from App.config, which is never loaded for an
    /// add-in DLL, so options were lost every time SOLIDWORKS restarted.)
    /// </summary>
    public static class PowerSMSettings
    {
        private static readonly object Sync = new object();
        private static Dictionary<string, string> values;

        private static string SettingsFile
        {
            get
            {
                return Path.Combine(
                    Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                    "Blue Byte", "PowerSM", "settings.xml");
            }
        }

        public static bool ForceMode
        {
            get { return GetBool("Force"); }
            set { Set("Force", value.ToString()); }
        }

        public static bool SetOverrideToFalse
        {
            get { return GetBool("SetOverrideToFalse"); }
            set { Set("SetOverrideToFalse", value.ToString()); }
        }

        public static bool ForceDimensionalRespect
        {
            get { return GetBool("ForceDimensionalRespect"); }
            set { Set("ForceDimensionalRespect", value.ToString()); }
        }

        public static bool ArchiveInZipFormat
        {
            get { return GetBool("ArchiveInZipFormat"); }
            set { Set("ArchiveInZipFormat", value.ToString()); }
        }

        public static string RecentDirectory
        {
            get { return Get("RecentDirectory") ?? string.Empty; }
            set { Set("RecentDirectory", value ?? string.Empty); }
        }

        public static void Save()
        {
            lock (Sync)
            {
                EnsureLoaded();
                try
                {
                    Directory.CreateDirectory(Path.GetDirectoryName(SettingsFile));
                    new XElement("PowerSM",
                        values.Select(kv => new XElement("Option",
                            new XAttribute("Name", kv.Key),
                            new XAttribute("Value", kv.Value))))
                        .Save(SettingsFile);
                }
                catch (Exception)
                {
                    // Settings are a convenience; never fail the tool because they can't be written.
                }
            }
        }

        private static bool GetBool(string key)
        {
            bool result;
            return bool.TryParse(Get(key), out result) && result;
        }

        private static string Get(string key)
        {
            lock (Sync)
            {
                EnsureLoaded();
                string value;
                return values.TryGetValue(key, out value) ? value : null;
            }
        }

        private static void Set(string key, string value)
        {
            lock (Sync)
            {
                EnsureLoaded();
                values[key] = value;
            }
        }

        private static void EnsureLoaded()
        {
            if (values != null)
                return;

            values = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
            try
            {
                if (File.Exists(SettingsFile))
                {
                    foreach (var option in XElement.Load(SettingsFile).Elements("Option"))
                    {
                        var name = (string)option.Attribute("Name");
                        if (!string.IsNullOrEmpty(name))
                            values[name] = (string)option.Attribute("Value") ?? string.Empty;
                    }
                }
            }
            catch (Exception)
            {
                // Corrupt or unreadable file: start from defaults.
            }
        }
    }
}
