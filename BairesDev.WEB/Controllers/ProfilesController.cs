using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using BairesDev.DAL.Models;
using BairesDev.DAL.Repositories;
using System.Configuration;

namespace BairesDev.WEB.Controllers
{
    public class ProfilesController : Controller
    {
        private IProfileRepository profileRepository = new ProfileRepository();

        private static string filePath = System.Web.HttpContext.Current.Server.MapPath(ConfigurationManager.AppSettings.Get("ProfilesFile"));

        private const char value_separator = ',';

        private static ICollection<Profile> flteredProfiles;

        private static bool loadedProfiles = false;

        private static string DefaultTitleParameter = ConfigurationManager.AppSettings.Get("DefaultTitleParameter");

        private static string DefaultIndustryParameter = ConfigurationManager.AppSettings.Get("DefaultIndustryParameter");


        // GET: Profiles
        public ActionResult Index()
        {
            //Load profiles from file
            if(!loadedProfiles)
            {
                profileRepository.LoadProfiles(filePath);
                loadedProfiles = true;
            }

            if(flteredProfiles != null && flteredProfiles.Count > 0)
            {
                ViewBag.Count = flteredProfiles.Count;
                ViewBag.DefaultTitle = DefaultTitleParameter;
                ViewBag.DefaultIndustry = DefaultIndustryParameter;
                return View("Index", flteredProfiles);
            }
            else
            {
                //Load default values to text boxes
                ViewBag.DefaultTitle = DefaultTitleParameter;
                ViewBag.DefaultIndustry = DefaultIndustryParameter;

                //Create default profile with default values
                Profile profile = new Profile();
                profile.Title = DefaultTitleParameter;
                profile.Industry = DefaultIndustryParameter;

                SearchProfiles(profile);
                return View();
            }
        }
        [HttpPost]
        public ActionResult SearchProfiles([Bind(Include = "Title, CurrentRole, Industry, Id")] Profile profile)
        {
            Dictionary<string, List<string>> parameters = new Dictionary<string, List<string>>();
                        
            if (!string.IsNullOrEmpty(profile.Title))
            {
                parameters.Add("Title", GetValues(profile.Title));               
            }

            if (!string.IsNullOrEmpty(profile.CurrentRole))
            {
                parameters.Add("CurrentRole", GetValues(profile.CurrentRole));
            }

            if (!string.IsNullOrEmpty(profile.Industry))
            {
                parameters.Add("Industry", GetValues(profile.Industry));
            }

            List<Profile> filterProfiles;
            if (parameters.Count > 0)
            {
                filterProfiles = profileRepository.FilterProfiles(parameters).ToList();
            }
            else
            {
                filterProfiles = profileRepository.GetProfiles().ToList();
            }
            
            ViewBag.Count = filterProfiles.Count;
            flteredProfiles = filterProfiles;
            //Session["SearchParameters"] = profile;

            return View("Index", filterProfiles);
        }

        // GET: Profiles/Details/5
        public ActionResult Details(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Profile profile = profileRepository.GetProfileById((Guid)id);
            if (profile == null)
            {
                return HttpNotFound();
            }
            return View(profile);
        }

        public ActionResult ClearSearch()
        {
            DefaultTitleParameter = ViewBag.DefaultTitle = string.Empty;
            DefaultIndustryParameter = ViewBag.DefaultIndustry = string.Empty;

            return View("Index", new List<Profile>());
        }

        private List<string> GetValues(string parameter)
        {
            List<string> ret = new List<string>();

            if (parameter.Split(value_separator).Length > 0)
            {               
                string[] values = parameter.Split(value_separator);
                foreach (string value in values)
                {
                    ret.Add(value);
                }
            }

            return ret;
        }

    }
}
