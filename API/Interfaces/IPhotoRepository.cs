using API.DTOs;
using API.Entities;

namespace API.Interfaces
{
    public interface IPhotoRepository
    {
        Task<IEnumerable<PhotoForApprovalDto>> GetUnApprovedPhoto();
        Task<Photo> GetPhotoById(int id);
        void RemovePhoto(Photo photo);
        void Update(Photo photo);
        Task<AppUser> GetUserByPhotoId(int photoId);
    }
}
