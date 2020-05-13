namespace WebDemoWithEventHub.Pages
{
    using Microsoft.AspNetCore.Mvc.RazorPages;
    using Microsoft.Extensions.Options;

    public class IndexModel : PageModel
    {
        public IndexModel(ISettingsProvider settingsProvider)
        {
            Settings = settingsProvider.GetSettings();
        }

        public Settings Settings { get; }
    }
}
