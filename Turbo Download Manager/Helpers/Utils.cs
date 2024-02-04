using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Turbo_Download_Manager.Helpers
{
    public static class Utils
    {
        public static string GetFileName(Uri url)
        {
            string urlPath = url.AbsolutePath;
            string[] pathChuncks = urlPath.Split("/");
            string fileName = Path.GetFileNameWithoutExtension(pathChuncks[pathChuncks.Length - 1]);
            if(fileName.Length >= 255)
            {
                fileName = fileName.Substring(0, 254);
            }
            return HttpUtility.UrlDecode(fileName);
        }

        public static string GetAccurateExtension(string curExt, string fileName)
        {
            switch(curExt)
            {
                case ".bin":
                    if (fileName.EndsWith(".exe"))
                        return ".exe";
                    if (fileName.EndsWith(".msi"))
                        return ".msi";
                    break;
            }

            return curExt;
        }

        public static string GetExtensionFromMimeType(string mimeType)
        {
            switch(mimeType)
            {
                case "application/pdf":
                    return ".pdf";
                case "text/plain":
                    return ".txt";
                case "application/octet-stream":
                    return ".bin";
                case "audio/aac":
                    return ".aac";
                case "image/apng":
                    return ".apng";
                case "image/bmp":
                    return ".bmp";
                case "application/x-bzip":
                    return ".bz";
                case "application/x-bzip2":
                    return ".bz2";
                case "text/css":
                    return ".css";
                case "text/csv":
                    return ".csv";
                case "application/msword":
                    return ".doc";
                case "application/vnd.openxmlformats-officedocument.wordprocessingml.document":
                    return ".docx";
                case "application/epub+zip":
                    return ".epub";
                case "application/gzip":
                    return ".gz";
                case "image/gif":
                    return ".gif";
                case "text/html":
                    return ".html";
                case "image/vnd.microsoft.icon":
                    return ".ico";
                case "application/java-archive":
                    return ".jar";
                case "image/jpeg":
                    return ".jpg";
                case "text/javascript":
                    return ".js";
                case "application/json":
                    return ".json";
                case "audio/mpeg":
                    return ".mp3";
                case "video/mp4":
                    return ".mp4";
                case "video/mpeg":
                    return ".mpeg";
                case "application/vnd.oasis.opendocument.text":
                    return ".odt";
                case "audio/ogg":
                    return ".oga";
                case "video/ogg":
                    return ".ogv";
                case "image/png":
                    return ".png";
                case "application/x-httpd-php":
                    return ".php";
                case "application/vnd.ms-powerpoint":
                    return ".ppt";
                case "application/vnd.openxmlformats-officedocument.presentationml.presentation":
                    return ".pptx";
                case "application/vnd.rar":
                case "application/x-rar-compressed":
                    return ".rar";
                case "application/rtf":
                    return ".rtf";
                case "application/x-sh":
                    return ".sh";
                case "image/svg+xml":
                    return ".svg";
                case "application/x-tar":
                    return ".tar";
                case "audio/wav":
                    return ".wav";
                case "video/webm":
                    return ".webm";
                case "image/webp":
                    return ".webp";
                case "font/woff":
                    return ".woff";
                case "application/vnd.ms-excel":
                    return ".xls";
                case "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet":
                    return ".xlsx";
                case "application/xml":
                    return ".xml";
                case "application/zip":
                    return ".zip";
                case "application/x-7z-compressed":
                    return ".7z";
                case "application/x-iso9660-image":
                    return ".iso";
            }

            return "";
        }
    }
}
