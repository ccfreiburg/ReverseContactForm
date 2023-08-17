namespace ContRev.Backend;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

[ApiController]
[Route("/api/[controller]")]
public class ValidationController : ControllerBase
{

    private IEmailService _emailer;
    private IOneTimeLinkService _oneTimeLinkService;
    private ITemplateService _templateService;

    public ValidationController( IEmailService emailer, IOneTimeLinkService oneTimeLinkService, ITemplateService templateService )
    {
        _emailer = emailer;
        _oneTimeLinkService = oneTimeLinkService;
        _templateService =templateService;
    }

    [HttpPost()]
    public async Task<IActionResult> Validate( ValidationRequest userInput )
    {
        var link = await _oneTimeLinkService.Get(userInput.Purpose, userInput.Email);
        var url = _oneTimeLinkService.GetFullUrl(link.Slug);
        var message = await _templateService.GetMessageByPurpose(userInput.Purpose,url,userInput.Email);
        _emailer.Send(userInput.Email,"Link to Calvary Chapel Freiburg Contact Form",message);
        return Ok();
    }

    [HttpGet]
    public async Task<IActionResult> Validate( string slug )
    {
        _oneTimeLinkService.InvalidateOld();
        var otp = await _oneTimeLinkService.Check(slug);
        if (otp==null)
            return BadRequest(new { message = "Fabricated Link" });
        return Ok(otp);
    }
}
