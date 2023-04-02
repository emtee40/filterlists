﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FilterLists.Api.Entities;

public record Syntax
{
    public int Id { get; private init; }
    public string Name { get; private init; } = default!;
    public string? Description { get; private init; }
    public Uri? Url { get; private init; }
    public IEnumerable<FilterListSyntax> FilterListSyntaxes { get; private init; } = new HashSet<FilterListSyntax>();
    public IEnumerable<SoftwareSyntax> SoftwareSyntaxes { get; private init; } = new HashSet<SoftwareSyntax>();
}

internal class SyntaxTypeConfiguration : IEntityTypeConfiguration<Syntax>
{
    public virtual void Configure(EntityTypeBuilder<Syntax> builder)
    {
        builder.HasIndex(s => s.Name).IsUnique();
        builder.HasDataJsonFile<Syntax>();
    }
}