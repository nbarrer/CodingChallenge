using BairesDev.DAL.Models;
using System;
using System.Collections.Generic;

namespace BairesDev.DAL.Repositories
{
    public interface IProfileRepository : IDisposable
    {
        ICollection<Profile> FilterProfiles(Dictionary<string, List<string>> parameters);

        void LoadProfiles(string filepath);

        ICollection<Profile> GetProfiles();

        Profile GetProfileById(Guid Id);

    }
}
