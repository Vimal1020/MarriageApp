﻿using API.DTOs;
using API.Entities;
using API.Helpers;

namespace API.Interfaces
{
    public interface ILikesRepository
    {
        Task<UserLike> GetUserLike(int sourceUserId, int likedUserId);
        Task<AppUser> GetUserWithLikes(int userId);
        Task<PagedList<LikedDto>> GetUserLikes(LikesParams likesParams);
        Task<UserLike> GetUserToDislikeAsync(int source, int likeUser);
    }
}
