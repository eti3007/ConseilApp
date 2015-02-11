using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;

namespace ConseilApp
{
    public class FileUpload
    {
        private HttpFileCollectionBase Files;
        private string UrlFolder;
        private string PersonneId;
        private string StyleId;

        // Constructors
        public FileUpload(HttpFileCollectionBase files, int personneId, int styleId, bool pourVetement)
        {
            this.Files = files;
            this.PersonneId = personneId.ToString();
            this.StyleId = styleId.ToString();
            this.UrlFolder = pourVetement ? VetementUrl() : HabillageUrl();
        }
        public FileUpload(int personneId, int styleId, bool pourVetement)
        {
            this.PersonneId = personneId.ToString();
            this.StyleId = styleId.ToString();
            this.UrlFolder = pourVetement ? VetementUrl() : HabillageUrl();
        }
        public FileUpload(int personneId)
        {
            this.PersonneId = personneId.ToString();
        }


        // Upload entire file
        public List<string> UploadWholeFile()
        {
            List<string> PhotoSauvegardees = new List<string>();

            HttpPostedFileBase file = null;
            string fileName = string.Empty;
            string fullPath = string.Empty;

            for (int i = 0; i < this.Files.Count; i++)
            {
                file = this.Files[i];

                if (!string.IsNullOrEmpty(file.FileName))
                {
                    fileName = Path.GetFileName(file.FileName);
                    fullPath = this.UrlFolder + fileName;
                    
                    // vérifie que le dossier de la personne existe (le cas contraire on le créé)
                    if (!CeDossierExiste(this.UrlFolder))
                    {
                        Directory.CreateDirectory(this.UrlFolder);
                    }

                    // vérifie si ce fichier n'existe pas déjà dans le dossier :
                    if (!CeFichierExiste(fullPath))
                    {
                        // copie du fichier physiquement sur le disque :
                        file.SaveAs(fullPath);
                    }

                    PhotoSauvegardees.Add(fileName);
                }
            }

            file = null;
            fileName = string.Empty;
            fullPath = string.Empty;

            return PhotoSauvegardees;
        }

        /// <summary>
        /// Complète l'url des images
        /// </summary>
        public List<string> CompleteUrlPicture(List<string> urlList, bool pourVetement)
        {
            List<string> result = new List<string>();

            string url = string.Empty;
            string cheminPhysique = string.Empty;

            if (pourVetement) {
                url = VetementUrl(true);
                cheminPhysique = VetementUrl();
            }
            else {
                url = HabillageUrl(true);
                cheminPhysique = HabillageUrl();
            }

            foreach (string item in urlList)
            {
                if (CeFichierExiste(cheminPhysique+item))
                    if (!result.Contains(url + item))
                    {
                        result.Add(url + item);
                    }
            }
            return result;
        }

        /// <summary>
        /// Return le nom de l'image
        /// </summary>
        public string GetPictureNameFromUrl(string s)
        {
            // ~/Images/Photo/Vetement/5/RocheBrute.JPG
            string[] tab = s.Split('/').ToArray();
            return tab[tab.Count() - 1];
        }

        /// <summary>
        /// Supprime le fichier du disque dur
        /// </summary>
        public bool DeleteFile(string fileName)
        {
            bool result = false;

            string fullPath = string.Empty;
            fullPath = this.UrlFolder + GetPictureNameFromUrl(fileName);

            if (CeDossierExiste(this.UrlFolder))
            {
                if (CeFichierExiste(fullPath))
                {
                    FileSystemInfo file;
                    file = new FileInfo(fullPath);

                    if (file != null)
                    {
                        file.Delete();
                        result = true;
                    }
                }
            }

            return result;
        }


        private bool CeFichierExiste(string fullpath)
        {
            bool result = false;

            FileSystemInfo file;
            file = new FileInfo(fullpath);

            if (file != null)
            {
                if (!string.IsNullOrEmpty(file.Extension) && file.Exists)
                    result = true;
                file = null;
            }
            return result;
        }

        private bool CeDossierExiste(string path)
        {
            bool result = false;

            FileSystemInfo folder;
            folder = new DirectoryInfo(path);
            
            if (folder != null)
            {
                if (string.IsNullOrEmpty(folder.Extension) && folder.Exists)
                    result = true;
                folder = null;
            }
            return result;
        }

        /// <summary>
        /// Construit le chemin physique du dossier vêtement pour la personne
        /// </summary>
        private string VetementUrl(bool physique = false)
        {
            if (physique)
                return System.Configuration.ConfigurationManager.AppSettings["DossierVetement"].ToString() + this.PersonneId + "/" + this.StyleId + "/";
            else
                return Path.Combine(
                    System.Web.HttpContext.Current.Server.MapPath(
                        System.Configuration.ConfigurationManager.AppSettings["DossierVetement"].ToString() + this.PersonneId + "/" + this.StyleId + "/"
                    )
                );
        }

        /// <summary>
        /// Construit le chemin physique du dossier habillage pour la personne
        /// </summary>
        private string HabillageUrl(bool physique = false)
        {
            if (physique)
                return System.Configuration.ConfigurationManager.AppSettings["DossierHabillage"].ToString() + this.PersonneId + "/" + this.StyleId + "/";
            else
                return Path.Combine(
                    System.Web.HttpContext.Current.Server.MapPath(
                        System.Configuration.ConfigurationManager.AppSettings["DossierHabillage"].ToString() + this.PersonneId + "/" + this.StyleId + "/"
                    )
                );
        }
    }
}