﻿using FilterLists.Directory.Api.Contracts.Models;
using FilterLists.Directory.Application.Commands;
using FilterLists.Directory.Application.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;

namespace FilterLists.Directory.Api.Controllers;

public class ListsController : BaseController
{
    private readonly IMediator _mediator;

    public ListsController(IMemoryCache cache, IMediator mediator) : base(cache)
    {
        _mediator = mediator;
    }

    /// <summary>
    ///     Gets the FilterLists.
    /// </summary>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The FilterLists.</returns>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<ListVm>), StatusCodes.Status200OK)]
    public Task<IActionResult> Get(CancellationToken cancellationToken)
    {
        return CacheGetOrCreateAsync(() => _mediator.Send(new GetLists.Query(), cancellationToken));
    }

#if DEBUG

    /// <summary>
    ///     ALPHA: Creates the FilterList pending moderator approval.
    /// </summary>
    /// <param name="command">The command.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    [HttpPost]
    [ProducesResponseType(typeof(CreateList.Response), StatusCodes.Status202Accepted)]
    public async Task<IActionResult> Create(CreateList.Command command, CancellationToken cancellationToken)
    {
        var response = await _mediator.Send(command, cancellationToken);
        return AcceptedAtAction(
            nameof(ChangesController.GetDetails),
            nameof(ChangesController).Replace("Controller", string.Empty),
            new { id = response.ChangeId });
    }

#endif

    /// <summary>
    ///     Gets the details of the FilterList.
    /// </summary>
    /// <param name="id">The identifier of the FilterList.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The details of the FilterList.</returns>
    [HttpGet("{id:long}")]
    [ProducesResponseType(typeof(ListDetailsVm), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public Task<IActionResult> GetDetails(long id, CancellationToken cancellationToken)
    {
        return CacheGetOrCreateAsync(() => _mediator.Send(new GetListDetails.Query { Id = id }, cancellationToken), id);
    }
}
