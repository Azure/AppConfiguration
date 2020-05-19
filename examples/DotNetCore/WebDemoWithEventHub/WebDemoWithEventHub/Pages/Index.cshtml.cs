namespace WebDemoWithEventHub.Pages
{
    using Microsoft.AspNetCore.Mvc.RazorPages;
    using Microsoft.Extensions.Options;

    public class IndexModel : PageModel
    {
        public IndexModel(IOptionsSnapshot<Settings> options)
        {
            Settings = options.Value;
        }

        public Settings Settings { get; }
    }
}
