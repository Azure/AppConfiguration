namespace WebDemoWithEventHub.Pages
{
    using Microsoft.AspNetCore.Mvc.RazorPages;

    public class IndexModel : PageModel
    {
        public IndexModel(ISettingsProvider settingsProvider)
        {
            Settings = settingsProvider.GetSettings();
        }

        public Settings Settings { get; }
    }
}
