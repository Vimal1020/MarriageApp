using API.DTOs;
using API.Entities;
using API.Interfaces;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Sqlite.Storage.Internal;

namespace API.Data
{
    public class PhotoRepository : IPhotoRepository
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;

        public PhotoRepository(DataContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<IEnumerable<PhotoForApprovalDto>> GetUnApprovedPhoto()
        {

            var query = _context.Photos
                        .IgnoreQueryFilters().
                        Where(x => !x.isApproved).
                        Select(u => new PhotoForApprovalDto
                        {
                            id = u.Id,
                            Username = u.AppUser.UserName,
                            Url = u.Url,
                            isApproved = u.isApproved
                        });

            return await query.ToListAsync();

        }
        public async Task<Photo> GetPhotoById(int id)
        {
            var query = await _context.Photos.IgnoreQueryFilters().Where(x => x.Id == id).
            AsQueryable().FirstOrDefaultAsync();

            return query;
        }

        public async Task<AppUser> GetUserByPhotoId(int photoId)
        {
            var query = await _context.Users.IgnoreQueryFilters()
                               .Include(u => u.Photos)
                               .SingleOrDefaultAsync(u => u.Photos.Any(p => p.Id == photoId));

            return query;
        }

        public void RemovePhoto(Photo photo)
        {
            _context.Photos.Remove(photo);
        }
        public void Update(Photo photo)
        {
            _context.Entry(photo).State = EntityState.Modified;
        }
    }
}
