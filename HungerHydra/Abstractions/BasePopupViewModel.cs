using HungerHydra.Enums;
using HungerHydra.Helpers;

namespace HungerHydra.Abstractions;

public abstract class BasePopupViewModel: BaseViewModel
{
    private readonly TaskCompletionSource<DialogReturnValue> _taskCompletionSource = new();

    public Task<DialogReturnValue> ReturnValueAsync()
    {
        return _taskCompletionSource.Task;
    }

    public void SetReturnValue(DialogReturnStatuses status)
    {
        _taskCompletionSource.TrySetResult(new DialogReturnValue(status));
    }
}