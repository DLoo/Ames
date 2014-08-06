using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Ames.Entities;

namespace Ames.Abstract {
    public interface I_EFileRepository {

        EFileInfo Get_EFile(string FileID);

        EFileInfo Get_EFileByGUID(string fileGuid);

        EFileInfo Upload_File(string AppRootPath, int year, int month, string location, string brand, string department, string type, string generateFrom, int expiryDuration, UploadFileInfo media);
    }
}
