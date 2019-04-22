using Plugin.Media;
using Plugin.Media.Abstractions;
using System;
using System.Threading.Tasks;

namespace FourPlaces.Services
{
    // permet de manipuler les medias
    public class MediaService
    {
        public MediaService()
        {

        }
        

        // dimensions de l'image
        public async Task<int> ImageEdge()
        {
            int end = 50;

            int borneSup = end;
            int borneInf = 1;
            int mid;

            while (end < end * 2 * 10)
            {
                while (borneInf != borneSup)
                {
                    mid = borneInf + (borneSup - borneInf) / 2;
                    if (await App.SERVICE.TestImage(mid))
                    {
                        if (!await App.SERVICE.TestImage(mid + 1))
                        {
                            return mid;
                        }
                        else
                        {
                            borneInf = mid;
                        }
                    }
                    else
                    {
                        borneSup = mid;
                    }
                }
                end = end * 2;
            }
            return 1;

        }

        // choisir une image
        public async Task<MediaFile> ChooseImage()
        {
            await CrossMedia.Current.Initialize();
            if (CrossMedia.Current.IsPickPhotoSupported)
            {
                MediaFile photo = await CrossMedia.Current.PickPhotoAsync();
                return photo;
            }
            return null;
        }


        // prendre une photo
        public async Task<MediaFile> TakePicture()
        {
            try
            {
                if (CrossMedia.Current.IsCameraAvailable && CrossMedia.Current.IsTakePhotoSupported)
                {
                    var media = new StoreCameraMediaOptions
                    {
                        Directory = "Receipts",
                        Name = $"{DateTime.UtcNow}.jpg"
                    };

                    var file = await CrossMedia.Current.TakePhotoAsync(media);
                    return file;
                }
            }
            catch (Exception e) {
                //Console.WriteLine("====================================" + e);
            }
            return null;
        }
    }
}
