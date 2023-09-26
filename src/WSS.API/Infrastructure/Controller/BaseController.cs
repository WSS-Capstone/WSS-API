using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace WSS.API.Infrastructure.Controller;

/// <summary>
///     Use BaseController class as a base class to define all of neccessary things that came across other controllers.
/// </summary>
[ApiController]
public abstract class BaseController : ControllerBase
{
    /// <summary>
    ///     Initializes a new instance of the <see cref="BaseController" /> class.
    /// </summary>
    /// <param name="mediator">The mediator instance.</param>
    protected BaseController(IMediator mediator)
    {
        Mediator = mediator;
    }

    /// <summary>
    ///     Gets mediator instance.
    /// </summary>
    protected IMediator Mediator { get; }
}