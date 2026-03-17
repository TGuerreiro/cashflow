using CashFlow.Consolidado.Application.DTOs;
using CashFlow.Consolidado.Application.Queries.ObterConsolidadoPorData;
using CashFlow.Consolidado.Application.Queries.ObterConsolidadoPorPeriodo;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace CashFlow.Consolidado.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ConsolidadoController(ISender sender) : ControllerBase
{
    private readonly ISender _sender = sender;

    /// <summary>
    /// Obtem o saldo consolidado de uma data especifica.
    /// </summary>
    [HttpGet("{data}")]
    [ProducesResponseType(typeof(ApiResponse<ConsolidadoResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<ConsolidadoResponse>), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> ObterPorData(DateOnly data, CancellationToken cancellationToken)
    {
        var result = await _sender.Send(new ObterConsolidadoPorDataQuery(data), cancellationToken);

        if (result.IsFailure)
            return NotFound(ApiResponse<ConsolidadoResponse>.Fail(result.Error.Code, result.Error.Message));

        return Ok(ApiResponse<ConsolidadoResponse>.Ok(result.Value));
    }

    /// <summary>
    /// Obtem o relatorio consolidado de um periodo.
    /// </summary>
    [HttpGet]
    [ProducesResponseType(typeof(ApiResponse<IReadOnlyList<ConsolidadoResponse>>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<IReadOnlyList<ConsolidadoResponse>>), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> ObterPorPeriodo(
        [FromQuery] DateOnly dataInicio,
        [FromQuery] DateOnly dataFim,
        CancellationToken cancellationToken)
    {
        var result = await _sender.Send(
            new ObterConsolidadoPorPeriodoQuery(dataInicio, dataFim), cancellationToken);

        if (result.IsFailure)
            return BadRequest(ApiResponse<IReadOnlyList<ConsolidadoResponse>>.Fail(
                result.Error.Code, result.Error.Message));

        return Ok(ApiResponse<IReadOnlyList<ConsolidadoResponse>>.Ok(result.Value));
    }
}