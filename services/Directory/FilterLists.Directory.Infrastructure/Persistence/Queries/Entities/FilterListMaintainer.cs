﻿using System.Globalization;
using EFCore.NamingConventions.Internal;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FilterLists.Directory.Infrastructure.Persistence.Queries.Entities;

public record FilterListMaintainer
{
    public long FilterListId { get; init; }
    public FilterList FilterList { get; init; } = default!;
    public long MaintainerId { get; init; }
    public Maintainer Maintainer { get; init; } = default!;
}

internal class FilterListMaintainerTypeConfiguration : IEntityTypeConfiguration<FilterListMaintainer>
{
    public virtual void Configure(EntityTypeBuilder<FilterListMaintainer> builder)
    {
        // TODO: register and resolve INameRewriter
        var nr = new SnakeCaseNameRewriter(CultureInfo.InvariantCulture);

        builder.ToTable($"{nr.RewriteName(nameof(FilterListMaintainer))}s");
        builder.HasKey(flm => new { flm.FilterListId, flm.MaintainerId });
        builder.HasQueryFilter(flm => flm.FilterList.IsApproved && flm.Maintainer.IsApproved);
        builder.HasDataJsonFile<FilterListMaintainer>();
    }
}
