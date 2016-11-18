using System;
using System.Collections.Generic;
using System.Linq;
using BairesDev.DAL.Models;
using System.IO;
using BairesDev.DAL.Utilities;

namespace BairesDev.DAL.Repositories
{
    public class ProfileRepository : IProfileRepository, IDisposable
    {
        private static ICollection<Profile> profiles;

        private static char separator = '|';

        public ICollection<Profile> FilterProfiles(Dictionary<string, List<string>> parameters)
        {
            if (profiles == null)
                return null;

            List<Profile> result = new List<Profile>();

            try
            {
                //Filter loaded model based on front-end parameters
                foreach (KeyValuePair<string, List<string>> entry in parameters)
                {
                    //Iterate by all the values for the actual key
                    foreach (string parameterValue in entry.Value)
                    {
                        string value = parameterValue.ToUpper();
                        switch (entry.Key)
                        {
                            case "Title":
                                result.AddRange(profiles.Where(x => x.Title.ToUpper().Contains(value)));
                                break;

                            case "CurrentRole":
                                result.AddRange(profiles.Where(x => x.CurrentRole.ToUpper().Contains(value)));
                                break;

                            case "Industry":
                                result.AddRange(profiles.Where(x => x.Industry.ToUpper().Contains(value)));
                                break;

                            case "NumberOfConnections":
                                int numConnections = 0;
                                int.TryParse(value, out numConnections);
                                result.AddRange(profiles.Where(x => x.NumberOfConnections == numConnections));
                                break;

                            case "NumberOfRecommendations":
                                int numRecomendations = 0;
                                int.TryParse(value, out numRecomendations);
                                result.AddRange(profiles.Where(x => x.NumberOfRecommendations == numRecomendations));
                                break;
                        }
                    }
                }
            }
            catch(Exception ex)
            {
                throw new Exception("Something goes wrong. Please try again later");
            }
            return result;

        }

        public void LoadProfiles(string filepath)
        {
            profiles = new List<Profile>();
            //Read from file for load all information on model
            try
            {
                string[] lines = File.ReadAllLines(filepath);

                foreach (string line in lines)
                {
                    int numRecommendations = 0;
                    int numConnections = 0;

                    var profile = line.Split(separator);

                    int.TryParse(profile[(int)IndexParameters.NumberOfRecommendations], out numRecommendations);
                    int.TryParse(profile[(int)IndexParameters.NumberOfConnections], out numConnections);

                    //Create Profile entity
                    Profile newProfile = new Profile();
                    newProfile.PublicUrl = profile[(int)IndexParameters.PublicUrl].Trim();
                    newProfile.Name = profile[(int)IndexParameters.Name].Trim();
                    newProfile.LastName = profile[(int)IndexParameters.LastName].Trim();
                    newProfile.Title = profile[(int)IndexParameters.Title].Trim();
                    newProfile.GeographicArea = profile[(int)IndexParameters.GeographicArea].Trim();
                    newProfile.NumberOfRecommendations = numRecommendations;
                    newProfile.NumberOfConnections = numConnections;
                    newProfile.CurrentRole = profile[(int)IndexParameters.CurrentRole].Trim();
                    newProfile.Industry = profile[(int)IndexParameters.Industry].Trim();
                    newProfile.Country = profile[(int)IndexParameters.Country].Trim();
                    newProfile.Id = Guid.NewGuid();

                    profiles.Add(newProfile);
                }
            }
            catch(Exception ex)
            {
                throw new Exception("There was a problem reading the file. Please check the file",ex.InnerException);
            }
        }

        public ICollection<Profile> GetProfiles()
        {
            return profiles;
        }

        public Profile GetProfileById(Guid Id)
        {
            return profiles.FirstOrDefault(x => x.Id == Id);
        }

        private bool disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    //context.Dispose();
                }
            }
            this.disposed = true;
        }
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

    }
}
