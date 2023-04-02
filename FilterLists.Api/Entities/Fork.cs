﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FilterLists.Api.Entities;

public record Fork
{
    public int UpstreamFilterListId { get; private init; }
    public FilterList UpstreamFilterList { get; private init; } = default!;
    public int ForkFilterListId { get; private init; }
    public FilterList ForkFilterList { get; private init; } = default!;
}

internal class ForkTypeConfiguration : IEntityTypeConfiguration<Fork>
{
    public virtual void Configure(EntityTypeBuilder<Fork> builder)
    {
        builder.HasKey(f => new { f.UpstreamFilterListId, f.ForkFilterListId });
        builder.HasOne(f => f.UpstreamFilterList)
            .WithMany(fl => fl.ForkFilterLists)
            .HasForeignKey(f => f.UpstreamFilterListId);
        builder.HasOne(f => f.ForkFilterList)
            .WithMany(fl => fl.UpstreamFilterLists)
            .HasForeignKey(f => f.ForkFilterListId)
            .OnDelete(DeleteBehavior
                .ClientSetNull); // https://github.com/dotnet/efcore/issues/12581#issuecomment-403080615;
        builder.HasDataJsonFile<Fork>();
    }
}