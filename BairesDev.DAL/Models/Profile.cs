using System;

namespace BairesDev.DAL.Models
{
    public class Profile
    {
        public Guid Id { get; set; }

        public string PublicUrl { get; set; }

        public string Name { get; set; }

        public string LastName { get; set; }

        public string Title { get; set; }

        public string GeographicArea { get; set; }

        public int NumberOfRecommendations { get; set; }

        public int NumberOfConnections { get; set; }

        public string CurrentRole { get; set; }

        public string Industry { get; set; }

        public string Country { get; set; }
    }
}
