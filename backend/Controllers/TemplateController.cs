namespace ContRev.Backend;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

[ApiController]
[Route("/api/[controller]")]
public class TemplateController : ControllerBase
{
    private ITemplateService _templateService;

    public TemplateController( ITemplateService templateService)
    {
        _templateService = templateService;
    }

    [HttpPost]
    [Authorize]
    public async Task<IActionResult> Add( TemplateRequest request )
    {
        Console.WriteLine(request);

        var entry = await _templateService.Add(request);
        if (entry == null)
            return BadRequest(new { message = "Error" });
        else
            return Ok(entry);
    }

    [HttpPut]
    [Authorize]
    public async Task<IActionResult> Update( EmailTemplate request )
    {
        var entry = await _templateService.Update(request);
        if (entry == null)
            return BadRequest(new { message = "Error" });
        else
            return Ok(entry);
    }

    [HttpDelete]
    [Authorize]
    public async Task<IActionResult> Delete( int id )
    {
        var success = await _templateService.Delete(id);
        if (!success)
            return BadRequest(new { message = "Error" });
        else
            return Ok();
    }

    [HttpGet]
    [Authorize]
    public IActionResult Get( )
    {
        var templates = _templateService.GetAll();
        return Ok(templates);
    }

    [HttpGet("purposes")]
    public IActionResult Purposes( )
    {
        var templates = _templateService.GetAll();
        return Ok(templates.Where((x)=>x.Active).Select((x)=> new PurposeRequest{
                Slug = x.Slug,
                Purpose = x.Purpose
            }).ToArray<PurposeRequest>());    }
}
