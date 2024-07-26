using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System.Globalization;
using System.Net;

namespace GestAgape.Infrastructure.Utilities
{
    public class IpInfo
    {
        [JsonProperty("ip")]
        public string Ip { get; set; }

        [JsonProperty("hostname")]
        public string Hostname { get; set; }

        [JsonProperty("city")]
        public string City { get; set; }

        [JsonProperty("region")]
        public string Region { get; set; }

        [JsonProperty("country")]
        public string Country { get; set; }

        [JsonProperty("loc")]
        public string Loc { get; set; }

        [JsonProperty("org")]
        public string Org { get; set; }

        [JsonProperty("postal")]
        public string Postal { get; set; }
    }
    public enum FileType
    {
        Image,
        Document,
    }
    public class GestAgapeUtilitiesFunctions
    {
        public static string ProfileImageFolder = "ProfileImageFolder/";
        public static string LogoImageFolder = "LogoImageFolder/";
        public static string CvFolder = "CvFolder/";
        public static string dossier = "DossierFolder/";
        public static string GetLocation(string? ipAddress)
        {
            if (ipAddress.IsNullOrEmpty())
                return "Not Found, because IP Adress are Empty";

            IpInfo ipInfo = new IpInfo();
            try
            {
                string info = new WebClient().DownloadString("http://ipinfo.io/" + ipAddress);
                ipInfo = JsonConvert.DeserializeObject<IpInfo>(info);
                RegionInfo myRI1 = new RegionInfo(ipInfo.Country);
                ipInfo.Country = myRI1.EnglishName;
            }
            catch (Exception)
            {
                ipInfo.Country = null;
            }

            return $"{ipInfo.Country}, {ipInfo.Region}, {ipInfo.City}, {ipInfo.Hostname}";

        }
        public static string UploadFile(IWebHostEnvironment _hostingEnv, IFormFile? file, FileType type, string name, string outputFolder)
        {
            try
            {
                if (file != null)
                {
                    string ext1;
                    string ext2;
                    string ext3;
                    if (type == FileType.Image)
                    {
                        ext1 = ".jpg";
                        ext2 = ".jpeg";
                        ext3 = ".png";
                    }
                    else
                    {
                        ext1 = ".pdf";
                        ext2 = ".doc";
                        ext3 = ".docx";
                    }
                    string ext = Path.GetExtension(file.FileName);
                    if (ext.ToLower() == ext1 || ext.ToLower() == ext2 || ext.ToLower() == ext3)
                    {
                        var newName = "File_" + new DateTimeOffset(DateTime.UtcNow).ToUnixTimeMilliseconds().ToString() + "_" + name.ToUpper() + ext;
                        var filePath = Path.Combine(_hostingEnv.WebRootPath, "Data", outputFolder, newName.ToString());

                        using (var fileSteam = new FileStream(filePath, FileMode.Create))
                        {
                            file.CopyTo(fileSteam);
                        }
                        return newName.ToString();
                    }
                    else
                    {
                        return "";
                    }
                }
                else
                {
                    return "";
                }
            }
            catch
            {
                return "";
            }
        }
    }
    public class SessionInfo
    {
        public static string UserId = "UserId";
        public static string AffectationId = "AffectationId";
        public static string AnneeAcademiqueId = "AnneeAcademiqueId";
        public static string AnneeAcademiqueName = "AnneeAcademiqueName";
        public static string IPAddress = "IPAddress";
        public static string CurrentCampusId = "CurrentCampusId";
        public static string CurrentCampusName = "CurrentCampusName";

    }
}
