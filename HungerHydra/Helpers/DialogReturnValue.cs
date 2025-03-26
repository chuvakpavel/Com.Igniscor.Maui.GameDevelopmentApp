using HungerHydra.Enums;

namespace HungerHydra.Helpers;

public class DialogReturnValue
{
    public readonly DialogReturnStatuses Status;

    public DialogReturnValue(DialogReturnStatuses status)
    {
        this.Status = status;
    }
}