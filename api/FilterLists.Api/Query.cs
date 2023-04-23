﻿using FilterLists.Api.Persistence;
using FilterLists.Api.Persistence.Entities;
using HotChocolate.Types;

namespace FilterLists.Api;

[QueryType]
public static class Query
{
    [UsePaging(IncludeTotalCount = true, DefaultPageSize = 10)]
    [UseProjection]
    public static IQueryable<FilterList> GetFilterLists(FilterListsDbContext dbContext)
    {
        return dbContext.FilterLists;
    }
}