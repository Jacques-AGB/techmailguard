namespace TechMailGuard.Web.Pages;

using Microsoft.AspNetCore.Components;
using System.Net.Http.Json;

public class VeilleDashboardBase : ComponentBase
{
    [Inject] public HttpClient Http { get; set; } = default!; 

    protected VeilleConfiguration Config { get; set; } = new();
    protected string RapportIA { get; set; } = string.Empty;
    protected bool IsLoading { get; set; } = false;

    protected async Task GenererRapport()
    {
        IsLoading = true;
        StateHasChanged();

        try
        {

            var response = await Http.PostAsJsonAsync("api/emails/generate-veille",
                new { Sujets = Config.SujetsInteret });

            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadFromJsonAsync<VeilleResult>();
                RapportIA = result?.Rapport ?? "Aucun rapport généré.";
            }
        }
        catch (Exception ex)
        {
            RapportIA = $"Erreur : {ex.Message}";
        }
        finally
        {
            IsLoading = false;
            StateHasChanged();
        }
    }
}


public class VeilleResult { public string Rapport { get; set; } = string.Empty; }