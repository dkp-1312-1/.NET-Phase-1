using System.Globalization;
using System.Text.Json;
using Microsoft.Extensions.Localization;
 
namespace TraineeManagement.Api.Localization
{
    public class JsonStringLocalizer : IStringLocalizer
    {
        private readonly Dictionary<string, string> _localization = new();
 
        public JsonStringLocalizer()
        {
            // Get the current language of the request (e.g., "en" or "es")
            var culture = CultureInfo.CurrentUICulture.Name;
            var filePath = $"Resources/messages.{culture}.json";
 
            // If the file exists, read and deserialize it into our dictionary
            if (File.Exists(filePath))
            {
                var json = File.ReadAllText(filePath);
                _localization = JsonSerializer.Deserialize<Dictionary<string, string>>(json) ?? new();
            }
        }
 
        // This handles standard messages: _localizer["UnexpectedError"]
        public LocalizedString this[string name]
        {
            get
            {
                var value = _localization.TryGetValue(name, out var val) ? val : name;
                return new LocalizedString(name, value, resourceNotFound: value == name);
            }
        }
 
        // This handles messages with parameters: _localizer["TraineeNotFound", id]
        public LocalizedString this[string name, params object[] arguments]
        {
            get
            {
                var format = _localization.TryGetValue(name, out var val) ? val : name;
                var value = string.Format(format, arguments);
                return new LocalizedString(name, value, resourceNotFound: format == name);
            }
        }
 
        public IEnumerable<LocalizedString> GetAllStrings(bool includeParentCultures)
        {
            return _localization.Select(kvp => new LocalizedString(kvp.Key, kvp.Value));
        }
    }
}