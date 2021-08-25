// this is going to implement the Iphoto service interface & we ll 
using System.Threading.Tasks;
using API.Helpers;
using API.Interfaces;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;

namespace API.Services
{
    public class PhotoService : IPhotoService//need inside our phot service is we r going to need to have accces to cloudinary 
    {
        private readonly Cloudinary _cloudinary;// we need to give this details of our API keys 
        
        ///need inside our phot service is we r going to need to have accces to cloudinary need constructor for this
        public PhotoService(IOptions<CloudinarySettings> config)//get our configurationand the way that we get our configuration when we have set up a clas to store our configuration is we use the options interface and we can bring in this form 
        {
            var acc = new Account // e careful of the ordering here bcoz when we add parameters inside parentheses in this way & it doesn't take the configuaraion as an object then we have to get the ordering correct 
            (// Warnign// be careful with the ordering here bcoz inside parentheses cloudberries expecting thse to come in the exact order we have here 
                config.Value.CloudName,
                config.Value.ApiKey,
                config.Value.ApiSecret

            ); // this a cloadinary account this take configuration options inside parentheses

            _cloudinary = new Cloudinary(acc);//acc variable we created up here         
        }

        public async Task<ImageUploadResult> AddPhotoAsync(IFormFile file)
        {
            var uploadResult = new ImageUploadResult(); // got smthing to store our result in that we get back from cloudinary 7 we going 
            if (file.Length > 0)//gona check to see if we have smthing in our file perimeter Length is a property that we have available on a file 
            {  //using var stream = file.OpenReadStream();// getting our file as a stream of data 
                using var stream = file.OpenReadStream();//add logic to upload our file to cloud by using a stream
                //bcoz our stram is smthing that we r going to want to dispose of as soon as we r finished with this method 
                //OpenReadStream();//is not asynchronous method no need of await 
                var uploadParams = new ImageUploadParams
                {
                    File = new FileDescription(file.FileName, stream),
                    Transformation = new Transformation().Height(500).Width(500).Crop("fill").Gravity("face") //what we want to transform our image into now where a square img sites say what we r going to do
                // if they upload an image no matter what aspect ratio it is then we r going to crop it to a square & we r going to forcus on the face 
                };
                uploadResult = await _cloudinary.UploadAsync(uploadParams);// this is the part where we actually upload the file to cloud finally got in cloudnary thia has only recently been avaibale & asynchronous methos to do this 
                
            }
            return uploadResult;
        }

        public async Task<DeletionResult> DeletePhotoAsync(string publicId)
        {
            var deleteParams = new DeletionParams(publicId);//pass publicID that we get from our perimeter here 
            var result = await _cloudinary.DestroyAsync(deleteParams);

           return result;
        }
    }
}