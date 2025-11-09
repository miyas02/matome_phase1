using matome_phase1.scraper.Configs;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace matome_phase1.constants {
    public enum ScraperExceptionType {
        ContentNodeNotFound,
        ConfigJsonLogicNotFound,
        InvalidLogicValue,
        ContentNodeIsNull,
        NavigateToPagesIsNull
    }
    public class ConfigException : Exception {
        private static readonly List<string> list = new List<string>();
        private static readonly Dictionary<ScraperExceptionType, string> Messages = new()
        {
            { ScraperExceptionType.ContentNodeNotFound, "Content node not found in the document." },
            { ScraperExceptionType.ConfigJsonLogicNotFound, "Logic property not found in the config JSON." },
            { ScraperExceptionType.InvalidLogicValue, "Invalid logic value in JSON configuration." },
            { ScraperExceptionType.ContentNodeIsNull, "Content node is null. Please check the HTML structure." },
            { ScraperExceptionType.NavigateToPagesIsNull, "NavigateToPages property not found in the config JSON." }
        };

        /// <summary>
        /// 
        /// </summary>
        /// <param name="type"></param>
        /// <param name="config" type="AbstractScraperConfig"></param>
        /// <param name="additionalMessage" type="string"></param>
        public ConfigException(ScraperExceptionType type, AbstractScraperConfig? config = null, string ? additionalMessage = null)
            : base(FormatMessage(type, config, additionalMessage)) {
        }

        private static string FormatMessage(ScraperExceptionType type, AbstractScraperConfig? config, string? additionalMessage) {
            string baseMessage = Messages.ContainsKey(type) ? Messages[type] : "Unknown scraper error.";
            if (!string.IsNullOrEmpty(additionalMessage))
                baseMessage += " : [additional Message] " + additionalMessage;
            if (config != null)
                 Console.WriteLine("AbstractScraperConfig : " + config);

            // デバッグログに出力
            Console.WriteLine(baseMessage);
            return baseMessage;
        }
    }
}
