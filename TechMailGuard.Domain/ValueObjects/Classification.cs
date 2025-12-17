using TechMailGuard.Domain.Enum;

namespace TechMailGuard.Domain.ValueObjects;
public readonly record struct Classification
{

    public ClassificationType Type { get; init; }
    public double ConfidenceScore { get; init; }

    public Classification(ClassificationType type, double confidenceScore) 
    {
        if (confidenceScore < 0.0 || confidenceScore > 1.0) 
        {
            throw new ArgumentOutOfRangeException(nameof(confidenceScore), "Confidence score must be between 0.0 and 1.0.");
        }

        Type = type;
        ConfidenceScore = confidenceScore;
    }
}
