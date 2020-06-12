using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Gallery.DAL.Model;

namespace Gallery.DAL.Repositories.Interfaces
{
    public interface IMediaRepository
    {
        // Media
        Task RegisterMediaToDataBaseAsync(Media media);
        Task RegisterTempMediaToDataBaseAsync(TempMedia tempMedia);
        Task ChangeDeleteStatusAsync(string name, bool status);
        Task<Media> GetMediaByNameAsync(string name); //MediaDTO in Future
        Task<bool> IsMediaExistAsync(string name);

        // MediaType
        Task RegisterMediaTypeToDataBaseAsync(MediaType type);
        Task<MediaType> GetMediaTypeByTypeAsync(string type);
        Task<bool> IsMediaTypeExistAsync(string type);

    }
}
