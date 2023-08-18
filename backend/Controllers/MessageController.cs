namespace ContRev.Backend;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

[ApiController]
[Route("/api/[controller]")]
public class MessageController : ControllerBase
{
    private IOneTimeLinkService _oneTimeLinkService;
    private IEmailService _emailService;
    private ITemplateService _templateService;

    public MessageController(IOneTimeLinkService oneTimeLinkService, IEmailService emailService, ITemplateService templateService)
    {
        _oneTimeLinkService = oneTimeLinkService;
        _emailService = emailService;
        _templateService = templateService;
    }

    [HttpPost]
    public async Task<IActionResult> Send( MessageRequest request )
    {
        var success=false;
        var oneTimeLink = await _oneTimeLinkService.Check(request.Slug);
        if (oneTimeLink!=null && oneTimeLink is OneTimeLink) {
            var template = await _templateService.GetByPurpose(oneTimeLink.Purpose);
            Console.WriteLine(template);

            if (template != null)
                success = await _emailService.Send(
                    _templateService.To(template),
                    _templateService.Subject(template, request.Subject, oneTimeLink.Email),
                    request.Message
            );
        }
        _oneTimeLinkService.Invalidate(request.Slug);
        if (!success)
            return BadRequest(new { message = "Error sending Email" });
        else
            return Ok();
    }

}
