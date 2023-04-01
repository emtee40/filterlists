﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FilterLists.Api.Entities;

public record Dependent
{
    public int DependencyFilterListId { get; init; }
    public FilterList DependencyFilterList { get; init; } = default!;
    public int DependentFilterListId { get; init; }
    public FilterList DependentFilterList { get; init; } = default!;
}

internal class DependentTypeConfiguration : IEntityTypeConfiguration<Dependent>
{
    public virtual void Configure(EntityTypeBuilder<Dependent> builder)
    {
        builder.HasKey(d => new { d.DependencyFilterListId, d.DependentFilterListId });
        builder.HasOne(d => d.DependencyFilterList)
            .WithMany(fl => fl.DependentFilterLists)
            .HasForeignKey(d => d.DependencyFilterListId);
        builder.HasOne(d => d.DependentFilterList)
            .WithMany(fl => fl.DependencyFilterLists)
            .HasForeignKey(d => d.DependentFilterListId);
        builder.HasDataJsonFile<Dependent>();
    }
}