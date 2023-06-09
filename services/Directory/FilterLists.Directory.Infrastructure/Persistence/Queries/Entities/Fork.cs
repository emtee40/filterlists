﻿using System.Globalization;
using EFCore.NamingConventions.Internal;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FilterLists.Directory.Infrastructure.Persistence.Queries.Entities;

public record Fork
{
    public long UpstreamFilterListId { get; init; }
    public FilterList UpstreamFilterList { get; init; } = default!;
    public long ForkFilterListId { get; init; }
    public FilterList ForkFilterList { get; init; } = default!;
}

internal class ForkTypeConfiguration : IEntityTypeConfiguration<Fork>
{
    public virtual void Configure(EntityTypeBuilder<Fork> builder)
    {
        // TODO: register and resolve INameRewriter
        var nr = new SnakeCaseNameRewriter(CultureInfo.InvariantCulture);

        builder.ToTable($"{nr.RewriteName(nameof(Fork))}s");
        builder.HasKey(f => new { f.UpstreamFilterListId, f.ForkFilterListId });
        builder.HasOne(f => f.UpstreamFilterList)
            .WithMany(fl => fl.ForkFilterLists)
            .HasForeignKey(f => f.UpstreamFilterListId)
            .HasConstraintName("fk_forks_filter_lists_upstream_filter_list_id");
        builder.HasOne(f => f.ForkFilterList)
            .WithMany(fl => fl.UpstreamFilterLists)
            .HasForeignKey(f => f.ForkFilterListId)
            .HasConstraintName("fk_forks_filter_lists_fork_filter_list_id");
        builder.HasQueryFilter(f => f.UpstreamFilterList.IsApproved && f.ForkFilterList.IsApproved);
        builder.HasDataJsonFile<Fork>();
    }
}
