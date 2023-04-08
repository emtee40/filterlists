using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FilterLists.Api.Infrastructure.Context;
using Microsoft.AspNetCore.Http;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.EntityFrameworkCore;

namespace FilterLists.Api.GetSyntaxes;

internal class GetSyntaxes
{
    private readonly IQueryContext _queryContext;

    public GetSyntaxes(IQueryContext queryContext)
    {
        _queryContext = queryContext;
    }

    [FunctionName("GetSyntaxes")]
    public Task<List<Syntax>> RunAsync(
        [HttpTrigger(AuthorizationLevel.Function, "get", Route = "syntaxes")]
        HttpRequest req,
        CancellationToken cancellationToken)
    {
        return _queryContext.Syntaxes
            .OrderBy(s => s.Id)
            .Select(s => new Syntax
                {
                    Id = s.Id,
                    Name = s.Name,
                    Description = s.Description,
                    Url = s.Url,
                    FilterListIds = s.FilterListSyntaxes
                        .OrderBy(fls => fls.FilterListId)
                        .Select(sls => sls.FilterListId),
                    SoftwareIds = s.SoftwareSyntaxes
                        .OrderBy(ss => ss.SoftwareId)
                        .Select(ss => ss.SoftwareId)
                }
            ).ToListAsync(cancellationToken);
    }
}