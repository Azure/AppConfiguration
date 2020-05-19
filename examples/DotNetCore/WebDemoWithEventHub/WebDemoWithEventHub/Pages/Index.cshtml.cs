namespace WebDemoWithEventHub.Pages
{
    using Microsoft.AspNetCore.Mvc.RazorPages;
    using Microsoft.Extensions.Options;

    public class IndexModel : PageModel
    {
        public IndexModel(IOptions<Settings> settings)
        {
            Settings = settings.Value;
        }

        public Settings Settings { get; }
    }
}
