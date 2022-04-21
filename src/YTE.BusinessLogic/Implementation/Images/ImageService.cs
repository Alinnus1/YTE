using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using YTE.BusinessLogic.Base;
using YTE.Common;
using YTE.DataAccess;
using YTE.Entities;
using Image = YTE.Entities.Image;

namespace YTE.BusinessLogic.Implementation.Images
{
    public class ImageService : BaseService
    {
        public ImageService(ServiceDependencies serviceDependencies) : base(serviceDependencies)
        {
        }

        public void SetImage(IEditUser model, UnitOfWork uow, User user)
        {
            if (model.Image != null)
            {
                return;
            }
            var imagetemp = System.Drawing.Image.FromStream(model.Image.OpenReadStream());
            var resized = new Bitmap(imagetemp, new Size(250, 250));
            using var imageStream = new MemoryStream();
            resized.Save(imageStream, ImageFormat.Jpeg);
            var imageBytes = imageStream.ToArray();

            var image = new Image()
            {
                Id = Guid.NewGuid(),
                Content = imageBytes,
                ImageName = user.UserName + "_profile_picture",
                IsActive = true
            };

            uow.Images.Insert(image);
            if (user.Image.ImageName != "stock_profile_picture_img")
            {
                user.Image.IsActive = false;
            }
            user.ImageId = image.Id;
        }

        public void SetStockImage(User user, UnitOfWork uow)
        {
            var stock = uow.Images.Get()
                .FirstOrDefault(i => i.ImageName == "stock_profile_picture_img");
            user.ImageId = stock.Id;
        }

        public void SetBackground(IEditArtModel model, UnitOfWork uow, Entities.ArtObject artObject)
        {
            var backgroundIdtOrg = uow.ArtObjects.Get()
                 .Where(a => a.Id == artObject.Id)
                 .Select(a => a.BackgroundId)
                 .FirstOrDefault();
            if (model.Background != null)
            {
                var imagetemp = System.Drawing.Image.FromStream(model.Background.OpenReadStream());
                var resized = new Bitmap(imagetemp, new Size(1200, 800));
                using var imageStream = new MemoryStream();
                resized.Save(imageStream, ImageFormat.Jpeg);
                var imageBytes = imageStream.ToArray();

                var image = new Image()
                {
                    Id = Guid.NewGuid(),
                    Content = imageBytes,
                    ImageName = artObject.Name + "_background_img",
                    IsActive = true
                };

                uow.Images.Insert(image);
                if (artObject.Background != null)
                {
                    if (artObject.Background.ImageName != "stock_poster_background_img")
                    {
                        artObject.Background.IsActive = false;
                    }
                }
                artObject.BackgroundId = image.Id;
            }
            else
            {
                artObject.BackgroundId = backgroundIdtOrg;
            }
        }
        public void SetStockPosterBackground(UnitOfWork uow, Entities.ArtObject artObject)
        {
            var stock = uow.Images.Get()
                .FirstOrDefault(i => i.ImageName == "stock_poster_background_img");

            artObject.PosterId = stock.Id;
            artObject.BackgroundId = stock.Id;

        }
        public void SetPoster(IEditArtModel model, UnitOfWork uow, Entities.ArtObject artObject)
        {
            var posterIdtOrg = uow.ArtObjects.Get()
                .Where(a => a.Id == artObject.Id)
                .Select(a => a.PosterId)
                .FirstOrDefault();

            if (model.Poster != null)
            {
                var imagetemp = System.Drawing.Image.FromStream(model.Poster.OpenReadStream());
                var resized = new Bitmap(imagetemp, new Size(230, 345));
                using var imageStream = new MemoryStream();
                resized.Save(imageStream, ImageFormat.Jpeg);
                var imageBytes = imageStream.ToArray();

                var image = new Image()
                {
                    Id = Guid.NewGuid(),
                    Content = imageBytes,
                    ImageName = artObject.Name + "_poster_img",
                    IsActive = true

                };

                uow.Images.Insert(image);
                if (artObject.Poster != null)
                {
                    if (artObject.Poster.ImageName != "stock_poster_background_img")
                    {
                        artObject.Poster.IsActive = false;
                    }

                }
                artObject.PosterId = image.Id;
            }
            else
            {
                artObject.PosterId = posterIdtOrg;
            }
        }

        public void DeleteInactiveImages()
        {
            ExecuteInTransaction(uow =>
            {
                var inactiveImages = uow.Images.Get()
                        .Where(i => i.IsActive == false)
                        .ToList();

                foreach (var image in inactiveImages)
                {
                    uow.Images.Delete(image);
                }

                uow.SaveChanges();
            });
        }
    }
}
