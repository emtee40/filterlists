﻿namespace FilterLists.Api.Persistence.Entities;

public record FilterListViewUrl
{
    public int Id { get; init; }
    public int FilterListId { get; init; }
    public FilterList FilterList { get; init; } = null!;
    public int SegmentNumber { get; init; }
    public int Primariness { get; init; }
    public Uri Url { get; init; } = default!;
}