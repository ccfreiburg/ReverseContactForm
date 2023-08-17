namespace ContRev.Backend;

using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Web;
using System.Security.Cryptography;
using System.Text.Json;
using System.Text.RegularExpressions;

public interface IOneTimeLinkService
{
    Task<OneTimeLink> Get( string purpose, string email );
    Task<OneTimeLink> Check( string slug );
    void Invalidate( string slug );
    void InvalidateOld( );
    string GetFullUrl( string slug );

}

public class OneTimeLinkService : IOneTimeLinkService
{
    private string _secret;
    private string _baseUrl;
    private TimeSpan _valid;
    private ContRevDb _db;

    public OneTimeLinkService(AppSettings appSettings, ContRevDb db)
    {
        _secret = appSettings.Secret;
        _baseUrl = appSettings.BaseUrl;
        if (appSettings.Valid.EndsWith("d"))
            _valid = new TimeSpan(Convert.ToInt32(appSettings.Valid.Replace("d","")),0,0,0);
        else if (appSettings.Valid.EndsWith("h"))
            _valid = new TimeSpan(0,Convert.ToInt32(appSettings.Valid.Replace("h","")),0,0);
        else if (appSettings.Valid.EndsWith("s"))
            _valid = new TimeSpan(0,0,0,Convert.ToInt32(appSettings.Valid.Replace("s","")));
        else
            _valid = new TimeSpan(7,0,0,0);
        _db = db;
    }

    public async Task<OneTimeLink> Get( string purpose, string email ) {

        var entry = new OneTimeLink() {
            Slug = Protect(purpose+email),
            Email = email,
            Purpose = purpose,
            ValidTo = JsonSerializer.Serialize(DateTime.UtcNow+_valid) 
        };
        _db.OneTimeLinks.Add(entry);
        await _db.SaveChangesAsync();
        return entry;
    }

    public string GetFullUrl( string slug ) {
        return _baseUrl+"message/"+slug;
    }

    public async Task<OneTimeLink> Check( string slug ) {
        
        var otl = _db.OneTimeLinks.Where(x => x.Slug == slug).FirstOrDefault<OneTimeLink>();
        if (otl == null)
            return null;
        var validto = JsonSerializer.Deserialize<DateTime>(otl.ValidTo);
        if (DateTime.UtcNow>validto)
            return null;
        return otl;
    }

    public void Invalidate( string slug ) {
        var entry = _db.OneTimeLinks.Where(x => x.Slug == slug).FirstOrDefault<OneTimeLink>();
        Console.WriteLine("Delete");
        Console.WriteLine(entry.Slug);
        if (entry is OneTimeLink)
        {
            _db.OneTimeLinks.Remove(entry);
        } else return;
        _db.SaveChangesAsync();
    }

    public void InvalidateOld( ) {
        var toDelete = _db.OneTimeLinks.ToList().FindAll((entry) => {
            var validto = JsonSerializer.Deserialize<DateTime>(entry.ValidTo);
            return DateTime.UtcNow>validto;
        });
        _db.OneTimeLinks.RemoveRange(toDelete);
        _db.SaveChangesAsync();
    }

    public string Protect(string data)
    {
        string secureRandomString = Convert.ToBase64String(System.Security.Cryptography.RandomNumberGenerator.GetBytes(64));
        using (var sha = new System.Security.Cryptography.SHA256Managed()){
        // Convert the string to a byte array first, to be processed
            byte[] textBytes = System.Text.Encoding.UTF8.GetBytes(data + _secret + secureRandomString);
            byte[] hashBytes = sha.ComputeHash(textBytes);
        
            // Convert back to a string, removing the '-' that BitConverter adds
            string hash = BitConverter
                .ToString(hashBytes)
                .Replace("-", String.Empty);

            return hash;
        }
    }

}