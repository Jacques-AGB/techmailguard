using Microsoft.AspNetCore.Components;
using TechMailGuard.Application.Dtos;
using TechMailGuard.Web.Services;

namespace TechMailGuard.Web.Pages;

public class NewsletterDashboardBase : ComponentBase
{
    [Inject] public EmailHttpClient EmailService { get; set; } = default!;

    protected List<NewsletterDto>? Newsletters { get; set; }
    protected bool IsLoading { get; set; } = false;

    protected override async Task OnInitializedAsync()
    {
        await LoadData();
    }

    protected async Task LoadData()
    {
        IsLoading = true;
        Newsletters = await EmailService.GetNewslettersAsync();
        IsLoading = false;
    }
   

}