using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Options;

namespace WebDemo.Pages
{
    public class IndexModel : PageModel
    {
        public Settings Settings { get; }

        public IndexModel(IOptionsSnapshot<Settings> options)
        {
            Settings = options.Value;
        }
    }
}
