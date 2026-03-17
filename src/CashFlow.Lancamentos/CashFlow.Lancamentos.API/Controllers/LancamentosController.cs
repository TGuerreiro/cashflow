using CashFlow.Lancamentos.Application.Commands.AtualizarLancamento;
using CashFlow.Lancamentos.Application.Commands.CriarLancamento;
using CashFlow.Lancamentos.Application.Commands.RemoverLancamento;
using CashFlow.Lancamentos.Application.DTOs;
using CashFlow.Lancamentos.Application.Queries.ObterLancamentoPorId;
using CashFlow.Lancamentos.Application.Queries.ObterLancamentosPaginado;
using CashFlow.Lancamentos.Application.Queries.ObterLancamentosPorData;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace CashFlow.Lancamentos.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class LancamentosController(ISender sender) : ControllerBase
{
    private readonly ISender _sender = sender;

    /// <summary>
    /// Cria um novo lançamento (débito ou crédito).
    /// </summary>
    [HttpPost]
    [ProducesResponseType(typeof(ApiResponse<LancamentoResponse>), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ApiResponse<LancamentoResponse>), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Criar(
        [FromBody] CriarLancamentoCommand command,
        CancellationToken cancellationToken)
    {
        var result = await _sender.Send(command, cancellationToken);

        if (result.IsFailure)
            return BadRequest(ApiResponse<LancamentoResponse>.Fail(result.Error.Code, result.Error.Message));

        return CreatedAtAction(
            nameof(ObterPorId),
            new { id = result.Value.Id },
            ApiResponse<LancamentoResponse>.Ok(result.Value));
    }

    /// <summary>
    /// Atualiza um lançamento existente.
    /// </summary>
    [HttpPut("{id:guid}")]
    [ProducesResponseType(typeof(ApiResponse<LancamentoResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<LancamentoResponse>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse<LancamentoResponse>), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Atualizar(
        Guid id,
        [FromBody] AtualizarLancamentoRequest request,
        CancellationToken cancellationToken)
    {
        var command = new AtualizarLancamentoCommand(
            id, request.Data, request.Valor, request.Tipo, request.Descricao);

        var result = await _sender.Send(command, cancellationToken);

        if (result.IsFailure)
        {
            if (result.Error.Code.Contains("NotFound"))
                return NotFound(ApiResponse<LancamentoResponse>.Fail(result.Error.Code, result.Error.Message));

            return BadRequest(ApiResponse<LancamentoResponse>.Fail(result.Error.Code, result.Error.Message));
        }

        return Ok(ApiResponse<LancamentoResponse>.Ok(result.Value));
    }

    /// <summary>
    /// Remove um lançamento pelo Id.
    /// </summary>
    [HttpDelete("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Remover(Guid id, CancellationToken cancellationToken)
    {
        var result = await _sender.Send(new RemoverLancamentoCommand(id), cancellationToken);

        if (result.IsFailure)
            return NotFound(ApiResponse<object>.Fail(result.Error.Code, result.Error.Message));

        return NoContent();
    }

    /// <summary>
    /// Obtem um lançamento pelo Id.
    /// </summary>
    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(ApiResponse<LancamentoResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<LancamentoResponse>), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> ObterPorId(Guid id, CancellationToken cancellationToken)
    {
        var result = await _sender.Send(new ObterLancamentoPorIdQuery(id), cancellationToken);

        if (result.IsFailure)
            return NotFound(ApiResponse<LancamentoResponse>.Fail(result.Error.Code, result.Error.Message));

        return Ok(ApiResponse<LancamentoResponse>.Ok(result.Value));
    }

    /// <summary>
    /// Lista lançamentos de uma data especifica.
    /// </summary>
    [HttpGet("por-data/{data}")]
    [ProducesResponseType(typeof(ApiResponse<IReadOnlyList<LancamentoResponse>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> ObterPorData(DateOnly data, CancellationToken cancellationToken)
    {
        var result = await _sender.Send(new ObterLancamentosPorDataQuery(data), cancellationToken);
        return Ok(ApiResponse<IReadOnlyList<LancamentoResponse>>.Ok(result.Value));
    }

    /// <summary>
    /// Lista lançamento com paginação.
    /// </summary>
    [HttpGet]
    [ProducesResponseType(typeof(ApiResponse<PagedResponse<LancamentoResponse>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> ObterPaginado(
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 20,
        CancellationToken cancellationToken = default)
    {
        var result = await _sender.Send(
            new ObterLancamentosPaginadoQuery(page, pageSize), cancellationToken);

        return Ok(ApiResponse<PagedResponse<LancamentoResponse>>.Ok(result.Value));
    }
}

public record AtualizarLancamentoRequest(
    DateOnly Data,
    decimal Valor,
    CashFlow.Shared.Domain.Enums.TipoLancamento Tipo,
    string Descricao);
