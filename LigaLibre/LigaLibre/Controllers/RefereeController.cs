using FluentValidation;
using LigaLibre.Application.DTOs;
using LigaLibre.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LigaLibre.API.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class RefereeController (IRefereeServices refereeServices, IValidator<CreateRefereeDto> createValidator, IValidator<UpdateRefereeDto> updateValidator): ControllerBase
{
    [HttpGet]
    [Route("GetAllReferees")]
    public async Task<IActionResult> GetAllReferees()
    {
        var referees = await refereeServices.GetAllRefereeAsync();

        return Ok(referees);
    }
    [HttpGet]
    [Route("GetActivesReferees")]

    public async Task<IActionResult> GetActivesReferees()
    {
        var referees = await refereeServices.GetActiveRefereeAsync();

        return Ok(referees);
    }
    [HttpGet]
    [Route("GetRefereesById")]
    public async Task<IActionResult>GetRefereeById(int id)
    {
        var referee = await refereeServices.GetRefereeByIdAsync(id);

        return Ok(referee);
    }

    [HttpPost]
    [Route("CreateReferee")]
    public async Task<IActionResult> CreateReferee([FromBody]CreateRefereeDto createRefereDto)
    {
        var validationResult = await createValidator.ValidateAsync(createRefereDto);
        
        if (!validationResult.IsValid)
            return BadRequest(validationResult.Errors);
        
        var referee = await refereeServices.CreateRefereeAsync(createRefereDto);
        return CreatedAtAction(nameof(GetRefereeById), new { id = referee.Id }, referee);
    }
    [HttpPut]
    [Route("UpdateReferee")]
    public async Task<IActionResult> UpdateReferee(int id, [FromBody]UpdateRefereeDto refereeDto)
    {
        var validationResult = await updateValidator.ValidateAsync(refereeDto);
        
        if (!validationResult.IsValid)
            return BadRequest(validationResult.Errors);
        
        var referee = await refereeServices.UpdateRefereeAsync(id, refereeDto);
        return Ok(referee);
    }
    [HttpDelete]
    [Route("DeleteReferee")]
    public async Task<IActionResult> DeleteReferee(int id)
    {
        var deleted = await refereeServices.DeleteRefereeAsync(id);
        if (!deleted) return NotFound($"El arbitro con id:{id} no encontrado");
        return NoContent();
    }
}
