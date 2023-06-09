﻿using FilterLists.Directory.Domain.Aggregates.FilterLists;
using FilterLists.SharedKernel.Domain.SeedWork;

namespace FilterLists.Directory.Domain.Aggregates.Licenses;

public class License : Entity
{
    protected License() { }

    public string Name { get; private init; } = default!;
    public Uri? Url { get; private init; }
    public bool PermitsModification { get; private init; }
    public bool PermitsDistribution { get; private init; }
    public bool PermitsCommercialUse { get; private init; }
    public virtual IEnumerable<FilterList> FilterLists { get; private init; } = new HashSet<FilterList>();
}
