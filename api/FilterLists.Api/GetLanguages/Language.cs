namespace FilterLists.Api.GetLanguages;

public record Language
{
    /// <summary>
    ///     The identifier.
    /// </summary>
    /// <example>37</example>
    public int Id { get; init; }

    /// <summary>
    ///     The unique ISO 639-1 code.
    /// </summary>
    /// <example>en</example>
    public string Iso6391 { get; init; } = string.Empty;

    /// <summary>
    ///     The unique ISO name.
    /// </summary>
    /// <example>English</example>
    public string Name { get; init; } = string.Empty;
}