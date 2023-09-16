using API.DTOs;
using API.Entities;
using API.Extensions;
using API.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    public class AdminController : BaseApiController
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IPhotoService _photoService;

        public AdminController(UserManager<AppUser> userManager,IUnitOfWork unitOfWork, IPhotoService photoService,IMapper mapper)
        {
            _userManager = userManager;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _photoService = photoService;
        }

        [Authorize(Policy = "RequireAdminRole")]
        [HttpGet("users-with-roles")]
        public async Task<ActionResult> GetUsersWithRoles()
        {
            var users = await _userManager.Users
                .Include(r => r.UserRoles)
                .ThenInclude(r => r.Role)
                .OrderBy(u => u.UserName)
                .Select(u => new
                {
                    u.Id,
                    Username = u.UserName,
                    Roles = u.UserRoles.Select(r => r.Role.Name).ToList()

                })
                .ToListAsync();

            return Ok(users);
        }

        [HttpPost("edit-roles/{username}")]
        public async Task<ActionResult> EditRoles(string username, [FromQuery] string roles)
        {
            var selectedRoles = roles.Split(",").ToArray();
            var user = await _userManager.FindByNameAsync(username);
            if (user == null) return NotFound("Could not found the user");

            var userRoles = await _userManager.GetRolesAsync(user);

            var result = await _userManager.AddToRolesAsync(user, selectedRoles.Except(userRoles));

            if (!result.Succeeded) return BadRequest("Failed to add to roles");

            result = await _userManager.RemoveFromRolesAsync(user, userRoles.Except(selectedRoles));

            if (!result.Succeeded) return BadRequest("Failed to remove from roles");

            return Ok(await _userManager.GetRolesAsync(user));
        }

        [Authorize(Policy = "ModeratorPhotoRole")]
        [HttpGet("photos-to-moderate")]
        public ActionResult GetPhotosForModeration()
        {
            return Ok("Only for admin and moderators");
        }

        [Authorize(Policy = "ModeratorPhotoRole")]
        [HttpGet("GetPhotosForApproval")]
        public async Task<ActionResult> GetPhotosForApproval()
        {
            return Ok(await _unitOfWork.PhotoRepository.GetUnApprovedPhoto());
        }

        [Authorize(Policy = "RequireAdminRole")]
        [HttpPut("GetPhotoApproved/{id}")]
        public async Task<ActionResult> GetPhotoApproved(int id) 
        {
           var photo = await _unitOfWork.PhotoRepository.GetPhotoById(id);

           if (photo == null) return NotFound();

            if (photo.isApproved == true) BadRequest("Photo is already approved by admin");
            
            photo.isApproved = true;
            var user = await _unitOfWork.PhotoRepository.GetUserByPhotoId(photo.Id);
            if(user == null) return NotFound();

            if (!user.Photos.Any(p => p.IsMain))
            {
                photo.IsMain = true;
            }
            _unitOfWork.PhotoRepository.Update(photo);
            if (await _unitOfWork.Complete()) return NoContent();

            return BadRequest("Not able to approve the  photo");

        }

        [Authorize(Policy = "RequireAdminRole")]
        [HttpDelete("GetPhotoRejected/{id}")]
        public async Task<ActionResult> GetPhotoRejected(int id)
        {
            var photo = await _unitOfWork.PhotoRepository.GetPhotoById(id);

            if (photo == null) return NotFound();
           
            if (!string.IsNullOrEmpty(photo.PublicId))
            {
                var result = await _photoService.DeletePhotoAsync(photo.PublicId);
                if(result.Result == "ok")
                {
                    _unitOfWork.PhotoRepository.RemovePhoto(photo);

                    if (await _unitOfWork.Complete()) { return Ok(); }
                } 
            }

            return BadRequest("Not able to reject this photo");
        }

    }
}
