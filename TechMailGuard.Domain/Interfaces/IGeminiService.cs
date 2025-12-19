namespace TechMailGuard.Domain.Interfaces;
public interface IGeminiService
{
    Task<string> GenerateVeilleAsync(string sujets, List<string> emailContents);
}
