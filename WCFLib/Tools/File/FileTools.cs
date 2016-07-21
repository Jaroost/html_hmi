using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace WCFLib
{
    public static class FileTools
    {
        /// <summary>
        /// Recherche le fichier fileWithExtension dans le dossier currentFolder récursivement. Retourne une chaine null si pas trouvé
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="currentFolder"></param>
        /// <returns></returns>
        public static string FindFile(string fileWithExtension, string currentFolder)
        {
            string file;
            foreach (string filename in Directory.GetFiles(currentFolder))
            {
                file = Path.GetFileName(filename);
                if (file.LowerEquals(fileWithExtension))
                {
                    return filename;
                }
            }

            foreach (string directory in Directory.GetDirectories(currentFolder))
            {
                string ret = FindFile(fileWithExtension, directory);
                if (!string.IsNullOrEmpty(ret))
                {
                    return ret;
                }
            }
            return string.Empty;
        }

        public static List<FileInfo> GetAllFiles(string folder, bool recursive, Predicate<FileInfo> predicate=null)
        {
            List<FileInfo> list = new List<FileInfo>();
            FillFileInfoList(folder, list, recursive);
            if(predicate!=null)
            {
                list=list.FindAll(predicate);
            }
            return list;
        }

        private static void FillFileInfoList(string folder, List<FileInfo> list, bool recursive)
        {
            var dir = new DirectoryInfo(folder);
            list.AddRange(dir.GetFiles());
            if(recursive)
            {
                foreach(string subFolder in Directory.GetDirectories(folder))
                {
                    FillFileInfoList(subFolder, list, recursive);
                }
            }
        }

        public static string GetUniqueFilename(string folder, string extension=".txt")
        {
            return Path.Combine(folder, GuidTools.NewGuidStr() + extension);
        }

        public static DateTime GetLastEditionTime(string filename)
        {
            return File.GetLastWriteTime(filename);
        }

        public static bool IsFileLastEditionOlderThan(string filename, TimeSpan interval)
        {
            DateTime lastEdition = GetLastEditionTime(filename);
            DateTime now = DateTime.Now;
            TimeSpan timeBetween=now-lastEdition;
            return timeBetween > interval;
        }

        public static void ClearOldFilesFolder(string folder, TimeSpan interval)
        {
            foreach(string file in Directory.GetFiles(folder))
            {
                if(IsFileLastEditionOlderThan(file, interval))
                {
                    File.Delete(file);
                }
            }
            
            foreach(string direcotry in Directory.GetDirectories(folder))
            {
                ClearOldFilesFolder(direcotry, interval);
            }
        }

        public static string GetFileContentType(string fullFilename)
        {
            string fileExtension = Path.GetExtension(fullFilename).ToLower();
            string contentType;
            switch (fileExtension)
            {
                case ".html":
                    contentType = "text/html";
                    break;
                case ".css":
                    contentType = "text/css";
                    break;
                case ".js":
                    contentType = "application/javascript";
                    break;
                case ".png":
                    contentType = "image/png";
                    break;
                case ".gif":
                    contentType = "image/gif";
                    break;
                case ".jpg":
                    contentType = "image/jpeg";
                    break;
                case ".json":
                    contentType = "application/json";
                    break;
                case ".pdf":
                    contentType = "application/pdf";
                    break;
                case ".xml":
                    contentType = "application/xml";
                    break;
                case ".zip":
                    contentType = "application/zip";
                    break;
                default:
                    contentType = "text/html";
                    break;
            }
            return contentType;
        }

        public static Stream StreamFromFile(string filename)
        {
            return File.OpenRead(filename);
        }

        public static Stream StreamFromString(string s)
        {
            MemoryStream stream = new MemoryStream();
            StreamWriter writer = new StreamWriter(stream);
            writer.Write(s);
            writer.Flush();
            stream.Position = 0;
            return stream;
        }

        public static string GetFileVersion(string filename)
        {
            if(File.Exists(filename))
            {
                return AssemblyName.GetAssemblyName(filename).Version.ToString();
            }
            return string.Empty;
        }

        public static string GetFirstExeVersion(string folder)
        {
            string exeFile = FindByExtension(folder, ".exe");
            return FileTools.GetFileVersion(exeFile);
        }

        public static string CreateUniqueFolderName(string baseFolder)
        {
            string newFolder=Path.Combine(baseFolder, GuidTools.NewGuidStr());
            Directory.CreateDirectory(newFolder);
            return newFolder;
        }

        public static string GetUniqueFolderName(string folder, string foldername)
        {
            int inc=1;
            string newFoldername=foldername;
            while(Directory.Exists(Path.Combine(folder, newFoldername)))
            {
                newFoldername=foldername+"-"+inc;
                inc++;
            }
            return Path.Combine(folder, newFoldername);
        }

       

        public static string FindByExtension(string folder, string extension)
        {
            foreach(string file in Directory.GetFiles(folder))
            {
                if(Path.GetExtension(file).LowerEquals(extension))
                {
                    return file;
                }
            }
            return string.Empty;
        }

        /// <summary>
        /// Détermine si le dossier folder est en cours d'utilisation par un autre processus si oui retourne true sinon false
        /// </summary>
        /// <param name="folder"></param>
        /// <returns></returns>
        public static bool IsFolderInUse(string folder)
        {
            foreach (string file in Directory.GetFiles(folder))
            {
                if (FileTools.IsFileInUse(file))
                {
                    return true;
                }
            }
            foreach (string fold in Directory.GetDirectories(folder))
            {
                if (IsFolderInUse(fold))
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Détermine si le fichier filename est en cours d'utilisation par un autre processure si oui retourne true sinon false
        /// </summary>
        /// <param name="filename"></param>
        /// <returns></returns>
        public static bool IsFileInUse(string filename)
        {
            FileStream stream = null;
            FileInfo file = new FileInfo(filename);

            try
            {
                stream = file.Open(FileMode.Open, FileAccess.ReadWrite, FileShare.None);
            }
            catch (IOException)
            {
                return true;
            }
            finally
            {
                if (stream != null)
                    stream.Close();
            }

            return false;
        }

        public static void CreateFolderIfNotExists(string folder)
        {
            if (!Directory.Exists(folder))
            {
                Directory.CreateDirectory(folder);
            }
        }
    }
}
