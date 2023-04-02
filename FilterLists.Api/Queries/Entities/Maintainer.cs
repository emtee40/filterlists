﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FilterLists.Api.Queries.Entities;

internal record Maintainer
{
    public int Id { get; init; }
    public string Name { get; init; } = default!;
    public Uri? Url { get; init; }
    public string? EmailAddress { get; init; }
    public string? TwitterHandle { get; init; }
    public IEnumerable<FilterListMaintainer> FilterListMaintainers { get; init; } = new HashSet<FilterListMaintainer>();
}

internal class MaintainerTypeConfiguration : IEntityTypeConfiguration<Maintainer>
{
    public virtual void Configure(EntityTypeBuilder<Maintainer> builder)
    {
        builder.HasIndex(m => m.Name)
            .IsUnique();
        builder.HasDataJsonFile<Maintainer>();
    }
}