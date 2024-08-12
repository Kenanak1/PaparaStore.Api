﻿using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Http;
using PaparaStore.Base.Response;
using PaparaStore.Business.Cqrs;
using PaparaStore.Data.UnitOfWork;
using PaparaStore.Schema;
using System.Security.Claims;

namespace PaparaStore.Business.Query;
public class GetUserContactByUserIdQueryHandler : IRequestHandler<GetUserContactByUserIdQuery, ApiResponse<UserContactResponse>>
{
    private readonly IUnitOfWork unitOfWork;
    private readonly IMapper mapper;
    private readonly IHttpContextAccessor httpContextAccessor;

    public GetUserContactByUserIdQueryHandler(IUnitOfWork unitOfWork, IMapper mapper, IHttpContextAccessor httpContextAccessor)
    {
        this.unitOfWork = unitOfWork;
        this.mapper = mapper;
        this.httpContextAccessor = httpContextAccessor;
    }
    public async Task<ApiResponse<UserContactResponse>> Handle(GetUserContactByUserIdQuery request, CancellationToken cancellationToken)
    {
        var userIdClaim = httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier);
        var userId = long.Parse(userIdClaim.Value);
        var entity = await unitOfWork.UserContactRepository.FirstOrDefaultAsync(w => w.UserId == userId, "User");
        var mapped = mapper.Map<UserContactResponse>(entity);
        return new ApiResponse<UserContactResponse>(mapped);
    }
}