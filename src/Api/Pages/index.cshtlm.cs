using Microsoft.AspNetCore.Mvc.RazorPages;
using Readings;

namespace Api.Pages;

public class IndexModel : PageModel
{
    private readonly IReadingService _readingService;

    public IndexModel(IReadingService readingService)
    {
        _readingService = readingService;
    }

    public async Task OnGetAsync(CancellationToken cancellationToken) { }
}
