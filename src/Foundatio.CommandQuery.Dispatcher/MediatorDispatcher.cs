using Foundatio.Mediator;

namespace Foundatio.CommandQuery.Dispatcher;

/// <summary>
/// A dispatcher that uses <see cref="IMediator"/> to send requests.  Use for Blazor Interactive Server rendering mode.
/// </summary>
public class MediatorDispatcher : IDispatcher
{
    private readonly IMediator _mediator;

    /// <summary>
    /// Initializes a new instance of the <see cref="MediatorDispatcher"/> class.
    /// </summary>
    /// <param name="mediator">The <see cref="IMediator"/> to send request to.</param>
    /// <exception cref="ArgumentNullException">When <paramref name="mediator"/> is null</exception>
    public MediatorDispatcher(IMediator mediator)
    {
        _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
    }

    /// <inheritdoc />
    public async ValueTask<TResponse?> Send<TResponse>(
        object request,
        CancellationToken cancellationToken = default)
    {
        return await _mediator.InvokeAsync<TResponse?>(request, cancellationToken).ConfigureAwait(false);
    }
}
