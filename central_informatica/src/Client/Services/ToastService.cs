using Microsoft.AspNetCore.Components;

namespace central_informatica.src.Client.Services;

public class ToastService
{
    private readonly List<ToastMessage> _toasts = new();

    public event Func<Task>? OnToastsChanged;

    public IReadOnlyList<ToastMessage> Toasts => _toasts.AsReadOnly();

    public async Task ShowAsync(
        string message,
        ToastType type = ToastType.Default,
        int durationMs = 5000
    )
    {
        var toast = new ToastMessage
        {
            Id = Guid.NewGuid(),
            Message = message,
            Type = type,
            CreatedAt = DateTime.Now,
            DurationMs = durationMs
        };

        _toasts.Add(toast);
        if (OnToastsChanged != null)
        {
            await OnToastsChanged.Invoke();
        }

        // Auto-remove after duration
        _ = Task.Delay(durationMs).ContinueWith(async _ => await RemoveAsync(toast.Id));
    }

    public void Show(string message, ToastType type = ToastType.Default, int durationMs = 5000)
    {
        _ = ShowAsync(message, type, durationMs);
    }

    public async Task RemoveAsync(Guid id)
    {
        var toast = _toasts.FirstOrDefault(t => t.Id == id);
        if (toast != null)
        {
            _toasts.Remove(toast);
            if (OnToastsChanged != null)
            {
                await OnToastsChanged.Invoke();
            }
        }
    }

    public void Remove(Guid id)
    {
        _ = RemoveAsync(id);
    }

    public async Task ClearAsync()
    {
        _toasts.Clear();
        if (OnToastsChanged != null)
        {
            await OnToastsChanged.Invoke();
        }
    }

    public void Clear()
    {
        _ = ClearAsync();
    }
}

public class ToastMessage
{
    public Guid Id { get; set; }
    public string Message { get; set; } = string.Empty;
    public ToastType Type { get; set; }
    public DateTime CreatedAt { get; set; }
    public int DurationMs { get; set; }
}

public enum ToastType
{
    Default,
    Success,
    Error,
    Warning,
    Info
}

// Clase estática para facilitar el uso
public static class Toast
{
    private static ToastService? _service;

    public static void Initialize(ToastService service)
    {
        _service = service;
    }

    public static void Show(
        string message,
        ToastType type = ToastType.Default,
        int durationMs = 5000
    )
    {
        _service?.Show(message, type, durationMs);
    }

    public static void Success(string message, int durationMs = 5000)
    {
        _service?.Show(message, ToastType.Success, durationMs);
    }

    public static void Error(string message, int durationMs = 5000)
    {
        _service?.Show(message, ToastType.Error, durationMs);
    }

    public static void Warning(string message, int durationMs = 5000)
    {
        _service?.Show(message, ToastType.Warning, durationMs);
    }

    public static void Info(string message, int durationMs = 5000)
    {
        _service?.Show(message, ToastType.Info, durationMs);
    }
}
