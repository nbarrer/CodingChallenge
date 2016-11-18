namespace BairesDev.DAL.Utilities
{
    //Used to isolate file index from entity properties and change file format easy without major model
    //modifications
    enum IndexParameters
    {
        PublicUrl = 0,
        Name = 1,
        LastName = 2,
        Title = 3,
        GeographicArea = 4,
        NumberOfRecommendations = 5,
        NumberOfConnections = 6,
        CurrentRole = 7,
        Industry = 8,
        Country = 9,
    }
}
