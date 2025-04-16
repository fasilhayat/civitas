namespace Civitas.Api.Core.Entities;

public class AccessRole
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string? ThirdPartyId { get; set; }

    // Optional: If you want to include a reference back to AccessLevelControl
    public int AccessLevelControlId { get; set; }

    public AccessLevelControl? AccessLevelControl { get; set; }
}
