using System.Linq;
using System.Configuration;
using System.Collections.Generic;
using System.Collections.Specialized;

namespace SolutionFileProjectReader
{
    public static class ConfigUtil
    {
        /// <summary>
        /// Get application configuration value for the given key from the configuration.
        /// </summary>
        public static string GetAppConfigValue(string configKey)
        {
            return ConfigurationManager.AppSettings[configKey];
        }

        /// <summary>
        /// Get section values for the given section name from the configuration.
        /// </summary>
        public static List<string> GetSectionValues(string sectionName)
        {
            NameValueCollection categories = (NameValueCollection)ConfigurationManager.
               GetSection(sectionName);

            if (categories == null || categories.Count == 0)
                return null;

            List<string> values = new List<string>();

            // Loop through the keys and add the items along with selected status.
            foreach (string key in categories)
            {
                if (!string.IsNullOrWhiteSpace(categories[key]))
                {
                    values.Add(categories[key]);
                }
            }
            return values;
        }

        /// <summary>
        /// Get section key & values for the given section name from the configuration.
        /// </summary>
        public static List<KeyValuePair<string, string>> GetSectionKeyValues(string sectionName)
        {
            NameValueCollection categories = (NameValueCollection)ConfigurationManager.
               GetSection(sectionName);

            if (categories == null || categories.Count == 0)
                return null;

            List<KeyValuePair<string, string>> values = new List<KeyValuePair<string, string>>();

            // Loop through the keys and add the items along with selected status.
            foreach (string key in categories)
            {
                if (!string.IsNullOrWhiteSpace(categories[key]))
                {
                    values.Add(new KeyValuePair<string, string>(key, categories[key]));
                }
            }
            return values;
        }

        /// <summary>
        /// Get connection string for the given key from the configuration.
        /// </summary>
        public static string GetConnectionString(string key)
        {
            return ConfigurationManager.ConnectionStrings[key].ConnectionString;
        }
    }
}