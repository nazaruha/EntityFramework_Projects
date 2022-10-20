using WinFormsShop.Data.Entities;
using WinFormsShop.Data;
using Bogus;
using System.Data;
using Microsoft.Data.SqlClient;
using System.Net;
using WinFormsShop.Helpers;

namespace WinFormsShop
{
    internal static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            // To customize application configuration such as set high DPI settings or default font,
            // see https://aka.ms/applicationconfiguration.
            SeedData();
            ApplicationConfiguration.Initialize();
            Application.Run(new CategoryForm());
        }
        static void SeedData()
        {
            using (MyDataContext dataContext = new MyDataContext())
            {
                if (!dataContext.Categories.Any()) // if table is empty
                {
                    Faker<CategoryEntity> faker = new Faker<CategoryEntity>()
                        .RuleFor(c => c.IsDelete, (f, u) => f.Random.Bool())
                        .RuleFor(c => c.DateCreated, (f, u) => f.Date.Between(new DateTime(1999, 1, 1), DateTime.Now))
                        .RuleFor(c => c.Title, (f, u) => f.Name.JobTitle())
                        .RuleFor(c => c.Priority, (f, u) => f.Random.Number(0, 10));

                    for (int i = 0; i < 10; i++)
                    {
                        var category = faker.Generate();
                        category.ParentId = null;
                        category.Image = GetImage();
                        MultipleImage(category.Image);
                        dataContext.Categories.Add(category);
                    }
                    dataContext.SaveChanges();
                }
                else
                {
                    //MessageBox.Show("There are categories");
                }
            }
        }
        static string GetImage()
        {
            Faker faker_photo = new Faker();
            string imageUrl = faker_photo.Image.PlaceImgUrl();
            string fileSaveName = System.IO.Path.GetRandomFileName() + ".jpg";
            string saveLocation = System.IO.Path.Combine(Directory.GetCurrentDirectory(), $"Images\\{fileSaveName}");
            byte[] imageBytes;
            HttpWebRequest imageRequest = (HttpWebRequest)WebRequest.Create(imageUrl);
            WebResponse imageResponse = imageRequest.GetResponse();

            Stream responseStream = imageResponse.GetResponseStream();

            using (BinaryReader br = new BinaryReader(responseStream))
            {
                imageBytes = br.ReadBytes(500000);
                br.Close();
            }
            responseStream.Close();
            imageResponse.Close();

            FileStream fs = new FileStream(saveLocation, FileMode.Create);
            BinaryWriter bw = new BinaryWriter(fs);
            try
            {
                bw.Write(imageBytes);
            }
            finally
            {
                fs.Close();
                bw.Close();
            }
            return fileSaveName;
        }

        static void MultipleImage(string imageName)
        {
            String dir = System.IO.Path.Combine(Directory.GetCurrentDirectory(), "Images");
            Bitmap bmp = new Bitmap(dir + $"\\{imageName}"); // safeFileName

            for (int i = 1; i < 6; i++)
            {
                var bmpSave = ImageWorker.CompressImage(bmp, i * 32, i * 32);
                string imageSave = System.IO.Path.Combine(dir, $"{i * 32}_" + imageName);
                bmpSave.Save(imageSave, System.Drawing.Imaging.ImageFormat.Jpeg);
            }
        }
    }
}