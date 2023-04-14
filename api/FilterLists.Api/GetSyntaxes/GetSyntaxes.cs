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

namespace FilterLists.Api.GetSyntaxes;

internal class GetSyntaxes
{
    private readonly IQueryContext _queryContext;

    public GetSyntaxes(IQueryContext queryContext)
    {
        _queryContext = queryContext;
    }

    [OpenApiOperation(tags: "Syntaxes")]
    [OpenApiParameter(ODataExtensions.CountParamKey, Type = typeof(bool), In = ParameterLocation.Query,
        Description = ODataExtensions.CountParamDescription)]
    [OpenApiParameter(ODataExtensions.OrderByParamKey, Type = typeof(string), In = ParameterLocation.Query,
        Description = ODataExtensions.OrderByParamDescription)]
    [OpenApiParameter(ODataExtensions.SkipParamKey, Type = typeof(int), In = ParameterLocation.Query,
        Description = ODataExtensions.SkipParamDescription)]
    [OpenApiParameter(ODataExtensions.TopParamKey, Type = typeof(int), In = ParameterLocation.Query,
        Description = ODataExtensions.TopParamDescription)]
    [OpenApiResponseWithBody(HttpStatusCode.OK, "application/json", typeof(OData<List<Syntax>>))]
    [FunctionName("GetSyntaxes")]
    public async Task<OData<List<Syntax>>> RunAsync(
        [HttpTrigger(AuthorizationLevel.Function, "get", Route = "syntaxes")]
        HttpRequest req,
        CancellationToken cancellationToken)
    {
        return new OData<List<Syntax>>
        {
            Count = await _queryContext.Syntaxes.ApplyODataCount(req.Query, cancellationToken),
            Value = await _queryContext.Syntaxes
                .Select(s => new Syntax
                {
                    Id = s.Id,
                    Name = s.Name,
                    Description = s.Description,
                    Url = s.Url
                }).OrderBy(s => s.Id)
                .ApplyODataOrderBy(req.Query)
                .ApplyODataSkip(req.Query)
                .ApplyODataTop(req.Query)
                .ToListAsync(cancellationToken)
        };
    }
}