using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FilterLists.Api.GetFilterListSummaries;
using FilterLists.Api.GetLanguages;
using FilterLists.Api.GetLicenses;
using FilterLists.Api.GetMaintainers;
using FilterLists.Api.GetSoftware;
using FilterLists.Api.GetSyntaxes;
using FilterLists.Api.GetTags;
using FilterLists.Api.Infrastructure.Context;
using FilterLists.Api.OData;
using Microsoft.AspNetCore.Http;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.EntityFrameworkCore;

namespace FilterLists.Api.GetFilterListTable;

public class GetFilterListTable
{
    private readonly IQueryContext _queryContext;

    public GetFilterListTable(IQueryContext queryContext)
    {
        _queryContext = queryContext;
    }

    [FunctionName(nameof(GetFilterListTable))]
    public async Task<FilterListTable> RunAsync(
        [HttpTrigger(AuthorizationLevel.Function, "get", Route = "list-table")]
        HttpRequest req,
        CancellationToken cancellationToken)
    {
        // TODO: ⚡️ run all queries in parallel (requires separate DbContexts)?
        return new FilterListTable
        {
            FilterLists = new OData<List<FilterListSummary>>
            {
                Value = await _queryContext.FilterLists
                    .OrderBy(fl => fl.Id)
                    .ApplyODataOrderBy(req.Query)
                    .ApplyODataSkip(req.Query)
                    .ApplyODataTop(req.Query)
                    .ProjectToFilterListSummaries()
                    .ToListAsync(cancellationToken),
                Count = await _queryContext.FilterLists
                    .ApplyODataSkip(req.Query)
                    .ApplyODataTop(req.Query)
                    .ApplyODataCount(req.Query, cancellationToken)
            },
            Languages = await _queryContext.Languages.ProjectToLanguages().ToListAsync(cancellationToken),
            Licenses = await _queryContext.Licenses.ProjectToLicenses().ToListAsync(cancellationToken),
            Maintainers = await _queryContext.Maintainers.ProjectToMaintainers().ToListAsync(cancellationToken),
            Software = await _queryContext.Software.ProjectToSoftware().ToListAsync(cancellationToken),
            Syntaxes = await _queryContext.Syntaxes.ProjectToSyntaxes().ToListAsync(cancellationToken),
            Tags = await _queryContext.Tags.ProjectToTags().ToListAsync(cancellationToken)
        };
    }
}