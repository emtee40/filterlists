using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using FilterLists.Api.Infrastructure.Context;
using Microsoft.AspNetCore.Http;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

namespace FilterLists.Api.GetSoftware;

internal class GetSoftware
{
    private readonly IQueryContext _queryContext;

    public GetSoftware(IQueryContext queryContext)
    {
        _queryContext = queryContext;
    }

    [OpenApiOperation(tags: "Software")]
    [OpenApiParameter(ODataExtensions.CountParamKey, Type = typeof(bool), In = ParameterLocation.Query,
        Description = ODataExtensions.CountParamDescription)]
    [OpenApiParameter(ODataExtensions.OrderByParamKey, Type = typeof(string), In = ParameterLocation.Query,
        Description = ODataExtensions.OrderByParamDescription)]
    [OpenApiParameter(ODataExtensions.SkipParamKey, Type = typeof(int), In = ParameterLocation.Query,
        Description = ODataExtensions.SkipParamDescription)]
    [OpenApiParameter(ODataExtensions.TopParamKey, Type = typeof(int), In = ParameterLocation.Query,
        Description = ODataExtensions.TopParamDescription)]
    [OpenApiResponseWithBody(HttpStatusCode.OK, "application/json", typeof(OData<List<Software>>))]
    [FunctionName("GetSoftware")]
    public async Task<OData<List<Software>>> RunAsync(
        [HttpTrigger(AuthorizationLevel.Function, "get", Route = "software")]
        HttpRequest req,
        CancellationToken cancellationToken)
    {
        return new OData<List<Software>>
        {
            Count = await _queryContext.Software.ApplyODataCount(req.Query, cancellationToken),
            Value = await _queryContext.Software
                .Select(s => new Software
                {
                    Id = s.Id,
                    Name = s.Name,
                    Description = s.Description,
                    HomeUrl = s.HomeUrl,
                    DownloadUrl = s.DownloadUrl,
                    SupportsAbpUrlScheme = s.SupportsAbpUrlScheme
                }).OrderBy(s => s.Id)
                .ApplyODataOrderBy(req.Query)
                .ApplyODataSkip(req.Query)
                .ApplyODataTop(req.Query)
                .ToListAsync(cancellationToken)
        };
    }
}