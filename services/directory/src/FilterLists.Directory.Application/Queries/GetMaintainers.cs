﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using FilterLists.Directory.Infrastructure.Persistence.Queries.Context;
using FilterLists.Directory.Infrastructure.Persistence.Queries.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace FilterLists.Directory.Application.Queries
{
    public static class GetMaintainers
    {
        public class Query : IRequest<IEnumerable<MaintainerViewModel>>
        {
        }

        public class Handler : IRequestHandler<Query, IEnumerable<MaintainerViewModel>>
        {
            private readonly IQueryContext _context;
            private readonly IMapper _mapper;

            public Handler(IQueryContext context, IMapper mapper)
            {
                _context = context;
                _mapper = mapper;
            }

            public async Task<IEnumerable<MaintainerViewModel>> Handle(
                Query request,
                CancellationToken cancellationToken)
            {
                return await _context.Maintainers
                    .ProjectTo<MaintainerViewModel>(_mapper.ConfigurationProvider)
                    .ToListAsync(cancellationToken);
            }
        }

        public class MaintainerViewModelProfile : Profile
        {
            public MaintainerViewModelProfile()
            {
                CreateMap<Maintainer, MaintainerViewModel>()
                    .ForMember(m => m.FilterListIds,
                        o => o.MapFrom(m => m.FilterListMaintainers.Select(fl => fl.FilterList.Id)));
            }
        }

        public class MaintainerViewModel
        {
            public int Id { get; set; }
            public string Name { get; set; } = null!;
            public Uri? Url { get; set; }
            public string? EmailAddress { get; set; }
            public string? TwitterHandle { get; set; }
            public IEnumerable<int>? FilterListIds { get; set; }
        }
    }
}