namespace ContRev.Backend;

using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

public interface IUserService
{
    AuthenticateResponse Authenticate(AuthenticateRequest model);
    IEnumerable<User> GetAll();
    User GetById(int id);
}

public class UserService : IUserService
{
    // users hardcoded for simplicity, store in a db with hashed passwords in production applications

    private string _secret;
    private string _admin;
    private string _pass;

    public UserService(AppSettings appSettings)
    {
        _secret = appSettings.Secret;
        _admin = appSettings.Admin;
        _pass = appSettings.Pass;
    }

    public AuthenticateResponse Authenticate(AuthenticateRequest model)
    {
        if (!(model.Username==_admin && model.Password==_pass)) return null;

        var user= new User() {
            FirstName = "Admin",
            LastName = "Admin",
            Username = _admin
        };
        
        // authentication successful so generate jwt token
        var token = generateJwtToken(user);

        return new AuthenticateResponse(user, token);
    }

    public IEnumerable<User> GetAll()
    {
        return new List<User>();
    }

    public User GetById(int id)
    {
        return new User(); //FirstOrDefault(x => x.Id == id);
    }

    // helper methods

    private string generateJwtToken(User user)
    {
        // generate token that is valid for 7 days
        var tokenHandler = new JwtSecurityTokenHandler();

        var key = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_secret));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256Signature);
        var subject = new[] { new Claim("Username", user.Username) };

         var token = new JwtSecurityToken("Jwt:Issuer",
              "Jwt:Audience",
              null, //subject,
              expires: DateTime.Now.AddMinutes(120),
              signingCredentials: credentials);

        return new JwtSecurityTokenHandler().WriteToken(token);
        // var tokenDescriptor = new SecurityTokenDescriptor
        // {
        //     ,
        //     expires = DateTime.UtcNow.AddDays(7),
        //     signingCredentials = credentials
        // };
        // var token = tokenHandler.CreateToken(tokenDescriptor);
        //return tokenHandler.WriteToken(token);
    }
    
}