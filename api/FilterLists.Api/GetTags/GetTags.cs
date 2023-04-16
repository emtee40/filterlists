using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using FilterLists.Api.Infrastructure.Context;
using FilterLists.Api.OData;
using Microsoft.AspNetCore.Http;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

namespace FilterLists.Api.GetTags;

public class GetTags
{
    private readonly IQueryContext _queryContext;

    public GetTags(IQueryContext queryContext)
    {
        _queryContext = queryContext;
    }

    [OpenApiOperation(tags: nameof(Infrastructure.Entities.Tag))]
    [OpenApiParameter(ODataExtensions.OrderByParamKey, Type = typeof(string), In = ParameterLocation.Query,
        Description = ODataExtensions.OrderByParamDescription)]
    [OpenApiParameter(ODataExtensions.SkipParamKey, Type = typeof(int), In = ParameterLocation.Query,
        Description = ODataExtensions.SkipParamDescription)]
    [OpenApiParameter(ODataExtensions.TopParamKey, Type = typeof(int), In = ParameterLocation.Query,
        Description = ODataExtensions.TopParamDescription)]
    [OpenApiParameter(ODataExtensions.CountParamKey, Type = typeof(bool), In = ParameterLocation.Query,
        Description = ODataExtensions.CountParamDescription)]
    [OpenApiResponseWithBody(HttpStatusCode.OK, "application/json", typeof(OData<List<Tag>>))]
    [FunctionName(nameof(GetTags))]
    public async Task<OData<List<Tag>>> RunAsync(
        [HttpTrigger(AuthorizationLevel.Function, "get", Route = "tags")]
        HttpRequest req,
        CancellationToken cancellationToken)
    {
        return new OData<List<Tag>>
        {
            Value = await _queryContext.Tags
                .OrderBy(t => t.Id)
                .ApplyODataOrderBy(req.Query)
                .ApplyODataSkip(req.Query)
                .ApplyODataTop(req.Query)
                .ProjectToTags()
                .ToListAsync(cancellationToken),
            Count = await _queryContext.Tags
                .ApplyODataSkip(req.Query)
                .ApplyODataTop(req.Query)
                .ApplyODataCount(req.Query, cancellationToken)
        };
    }
}