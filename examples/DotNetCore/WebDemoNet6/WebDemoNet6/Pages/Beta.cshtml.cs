using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.FeatureManagement;

namespace WebDemoNet6.Pages
{
    public class BetaModel : PageModel
    {
        private IFeatureManagerSnapshot _featureManager;

        public BetaModel(IFeatureManagerSnapshot featureManager)
        {
            _featureManager = featureManager;
        }
        
        public async Task<IActionResult> OnGetAsync()
        {
            // Respond 404 NotFound if the 'Beta' feature is not enabled
            return await _featureManager.IsEnabledAsync("Beta")
                ? Page()
                : base.NotFound();
        }
    }
}
