namespace XMS.Mobile.Core.Models;

public sealed class MobileTask
{
    public Guid Id { get; init; }

    public string Title { get; init; } = string.Empty;

    public string Instruction { get; init; } = string.Empty;

    public string? ExpectedBarcode { get; init; }

    public MobileTaskType Type { get; init; }
}
