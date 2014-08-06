using System;
using System.Globalization;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using System.Web;
using System.IO;
using Ames.Abstract;
using Ames.Entities;

namespace Ames.Domain {
    public class EFileRepository : I_EFileRepository {
        private EFAmesInfra db = new EFAmesInfra();

        public EFileInfo Get_EFile(string fileName) {

            var result = db.EFileInfo.Where(x => x.EFileName == fileName);
            return result.Count() > 0 ? result.First() : null;
        }

        public EFileInfo Get_EFileByGUID(string fileGuid) {
            var result = db.EFileInfo.Where(x => x.FileGUID.ToString() == fileGuid);
            return result.Count() > 0 ? result.First() : null;
        }

        public EFileInfo Upload_File(string AppRootPath, int year, int month, string location, string brand, string department,
            string type, string generateFrom, int expiryDuration, UploadFileInfo media) {

            if (media == null) {
                throw new InvalidOperationException("media", new Exception("media is empty or null"));
            }
            FileInfo eFile = new FileInfo(media.FileName);


            // set the path to save the media
            string dFolder = brand ?? department;
            dFolder = dFolder ?? "Others";
            string path = AppRootPath + "\\" + location + "\\" + dFolder + "\\" + type + "\\" + year + "\\";

            //Creates all directories and subdirectories in the specified path unless they already exist.
            Directory.CreateDirectory(path);

            // check media file size not too big
            if (media.ByteArray.LongLength > 16777216) {
                throw new InvalidOperationException("media", new Exception("The size of the file should not exceed 10 MB"));
            }

            // check media file type within the range
            var supportedTypes = new[] { "pdf", "doc", "xls", "ppt", "docx", "xlsx", "pptx" };
            var fileExt = System.IO.Path.GetExtension(media.FileName).Substring(1);
            if (!supportedTypes.Contains(fileExt)) {
                throw new InvalidOperationException("media", new Exception("Invalid type. Only the following types (pdf, doc, xls, ppt, docx, xlsx, pptx) are supported."));
            }

            // check if file already exist then delete to overwrite
            if (System.IO.File.Exists(path + media.FileName)) {
                System.IO.File.Delete(path + media.FileName);
            }
            //media.Stream.CopyToAsync( new FileStream(path + media.FileName, FileMode.Create));
            File.WriteAllBytes(path + media.FileName, media.ByteArray);

            // set expiry date for the media
            DateTime expiryDate = DateTime.Now;
            if (expiryDuration > 0)
                expiryDate = expiryDate.AddDays(expiryDuration);
            else
                expiryDate = DateTime.ParseExact("01/01/2100", "dd/MM/yyyy", new CultureInfo("en-US"));

            // save to repository; if same file exist, then don't change the fileGUID
            EFileInfo fileExist = Get_EFile(media.FileName);
            if (fileExist == null) {
                fileExist = new EFileInfo {
                    CreatedDateTime = DateTime.Now,
                    DirectoryPath = path,
                    EFileName = media.FileName,
                    Year = year,
                    Month = month,
                    Location = location,
                    Brand = brand,
                    Department = department,
                    Type = type,
                    GeneratedFrom = generateFrom,
                    ExpiryDate = expiryDate,
                    FileGUID = Guid.NewGuid()
                };
                db.EFileInfo.Add(fileExist);
            } else {
                fileExist.CreatedDateTime = DateTime.Now;
                fileExist.DirectoryPath = path;
                fileExist.Year = year;
                fileExist.Month = month;
                fileExist.Location = location;
                fileExist.Brand = brand;
                fileExist.Department = department;
                fileExist.Type = type;
                fileExist.GeneratedFrom = generateFrom;
                fileExist.ExpiryDate = expiryDate;
                db.Entry(fileExist).State = EntityState.Modified;
            }

            db.SaveChanges();

            return fileExist;
        }


    }
}
