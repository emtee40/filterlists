using System.Runtime.CompilerServices;
using FilterLists.Api.Queries.Context;
using Microsoft.EntityFrameworkCore;

namespace FilterLists.Api.Queries.Handlers;

internal static class GetFilterLists
{
    public static async IAsyncEnumerable<FilterList> ExecuteAsync(IQueryContext queryContext,
        [EnumeratorCancellation] CancellationToken cancellationToken)
    {
        var query = queryContext.FilterLists
            .OrderBy(fl => fl.Id)
            .Select(fl => new FilterList
            {
                Id = fl.Id,
                Name = fl.Name,
                Description = fl.Description,
                LicenseId = fl.LicenseId,
                SyntaxIds = fl.FilterListSyntaxes
                    .OrderBy(fls => fls.SyntaxId)
                    .Select(fls => fls.SyntaxId),
                LanguageIds = fl.FilterListLanguages
                    .OrderBy(fll => fll.LanguageId)
                    .Select(fll => fll.LanguageId),
                TagIds = fl.FilterListTags
                    .OrderBy(flt => flt.TagId)
                    .Select(flt => flt.TagId),
                PrimaryViewUrl = fl.ViewUrls
                    .OrderBy(u => u.SegmentNumber)
                    .ThenBy(u => u.Primariness)
                    .Select(u => u.Url)
                    .FirstOrDefault(),
                MaintainerIds = fl.FilterListMaintainers
                    .OrderBy(flm => flm.MaintainerId)
                    .Select(flm => flm.MaintainerId)
            })
            .AsSplitQuery();
        await foreach (var filterList in query.AsAsyncEnumerable().WithCancellation(cancellationToken))
            yield return filterList;
    }

    public record FilterList
    {
        /// <summary>
        ///     The identifier.
        /// </summary>
        /// <example>301</example>
        public int Id { get; init; }

        /// <summary>
        ///     The unique name in title case.
        /// </summary>
        /// <example>EasyList</example>
        public string Name { get; init; } = default!;

        /// <summary>
        ///     The brief description in English (preferably quoted from the project).
        /// </summary>
        /// <example>
        ///     EasyList is the primary filter list that removes most adverts from international web pages, including unwanted
        ///     frames, images, and objects. It is the most popular list used by many ad blockers and forms the basis of over a
        ///     dozen combination and supplementary filter lists.
        /// </example>
        public string? Description { get; init; }

        /// <summary>
        ///     The identifier of the License under which this FilterList is released.
        /// </summary>
        /// <example>4</example>
        public int LicenseId { get; init; }

        /// <summary>
        ///     The identifiers of the Syntaxes implemented by this FilterList.
        /// </summary>
        /// <example>[ 3 ]</example>
        public IEnumerable<int> SyntaxIds { get; init; } = new HashSet<int>();

        /// <summary>
        ///     The identifiers of the Languages targeted by this FilterList.
        /// </summary>
        /// <example>[ 37 ]</example>
        public IEnumerable<int> LanguageIds { get; init; } = new HashSet<int>();

        /// <summary>
        ///     The identifiers of the Tags applied to this FilterList.
        /// </summary>
        /// <example>[ 2 ]</example>
        public IEnumerable<int> TagIds { get; init; } = new HashSet<int>();

        /// <summary>
        ///     The primary view URL.
        /// </summary>
        /// <example>https://easylist.to/easylist/easylist.txt</example>
        public Uri? PrimaryViewUrl { get; init; }

        /// <summary>
        ///     The identifiers of the Maintainers of this FilterList.
        /// </summary>
        /// <example>[ 7 ]</example>
        public IEnumerable<int> MaintainerIds { get; init; } = new HashSet<int>();
    }
}