namespace ContRev.Backend;

using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Web;
using System.Security.Cryptography;
using System.ComponentModel.DataAnnotations;

public interface ITemplateService
{
    string To(EmailTemplate template);
    string Subject(EmailTemplate template, string subject, string email);
    string Message(EmailTemplate template, string message, string email);
    Task<EmailTemplate> Add( TemplateRequest template );
    Task<bool> Update( EmailTemplate template );
    Task<bool> Delete( int id );
    Task<EmailTemplate> GetByPurpose( string purpose );
    EmailTemplate[] GetAll();
    Task<string> GetMessageByPurpose( string purpose, string link, string email );
}

public class TemplateService : ITemplateService
{
    private ContRevDb _db;

    public TemplateService(ContRevDb db)
    {
        _db = db;
    }
    public string To(EmailTemplate template) {
        return template.To;
    }
    public string Subject(EmailTemplate template, string subject, string email) {
        return template.Subject.Replace("##Subject##", subject).Replace("##From##",email);
    }
    public string Message(EmailTemplate template, string link, string email) {
        return template.Html.Replace("##Link##", link).Replace("##Email##",email);
    }

    public async Task<EmailTemplate> Add( TemplateRequest template ) {
        var entry = new EmailTemplate(){
            Active = template.Active,
            Purpose = template.Purpose,
            To = template.To,
            From = template.From,
            Subject = template.Subject,
            Html = template.Html
        };
        _db.EmailTemplates.Add(entry);
        await _db.SaveChangesAsync();
        return entry;
    }

    public async Task<bool> Update( EmailTemplate template ) {
        var entry = await _db.EmailTemplates.FindAsync(template.Id);
        if (entry is null) return false;
        entry.Active = template.Active;
        entry.Purpose = template.Purpose;
        entry.To = template.To;
        entry.From = template.From;
        entry.Subject = template.Subject;
        entry.Html = template.Html;
        await _db.SaveChangesAsync();
        return true;
    }

    public async Task<bool> Delete( int id ) {
        var entry = await _db.EmailTemplates.FindAsync(id);
        if (entry is EmailTemplate)
        {
            _db.EmailTemplates.Remove(entry);
        } else return false;
        await _db.SaveChangesAsync();
        return true;
    }
    
    public EmailTemplate[] GetAll() {
        return  _db.EmailTemplates.ToArray<EmailTemplate>();
    }
    public async Task<EmailTemplate> GetByPurpose( string purpose ) {
        var template = _db.EmailTemplates.Where(x => x.Purpose == purpose && x.Active).FirstOrDefault<EmailTemplate>();
        return template;
    }

    public async Task<string> GetMessageByPurpose( string purpose, string link, string email ) {
        var template = await GetByPurpose(purpose);
        return Message(template,link,email);
    }

}