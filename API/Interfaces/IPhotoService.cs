using System.Threading.Tasks;
using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Http;

namespace API.Interfaces
{
    public interface IPhotoService
    {
         Task<ImageUploadResult> AddPhotoAsync(IFormFile file);//inside we r going to have two methods Both going be asynchronousthat r going to return tasks 
         Task<DeletionResult> DeletePhotoAsync(string publicId);//add another task & this is going to be for the deletion results
       ///(string publicId)//now each file that we upload to cloadnary is going to be given a public ID and we  need to use this in order to deete the image   
    }// we cant delte it just by the URl The cloadnary gives us we need to very specifically use this public ID & if we go back to ourphoto 
}