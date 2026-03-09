using AutoMapper;
using Catalogo_API.Context;
using Catalogo_API.DTOs;
using Catalogo_API.Models;
using Catalogo_API.Pagination;
using Catalogo_API.Repositories.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace Catalogo_API.Controllers;

[Route("[controller]")]
[ApiController]
public class CategoriasController : ControllerBase
{
    private readonly IUnitOfWork _uow;
    private readonly IMapper _mapper;

    public CategoriasController(IUnitOfWork uow, IMapper mapper)
    {
        _uow = uow;
        _mapper = mapper;
    }

    [HttpGet]
    public ActionResult<IEnumerable<CategoriaDTO>> Get()
    {
        var categorias = _uow.CategoriaRepository.GetAll();

        if (categorias is null)
            return NotFound();

        var categoriasDto = _mapper.Map<IEnumerable<CategoriaDTO>>(categorias);

        return Ok(categoriasDto);
    }

    [HttpGet("pagination")]
    public ActionResult<IEnumerable<CategoriaDTO>> Get([FromQuery]
                            CategoriasParameters categoriasParameters)
    {
        var categorias = _uow.CategoriaRepository.GetCategorias(categoriasParameters);

        var metadata = new
        {
            categorias.TotalCount,
            categorias.PageSize,
            categorias.CurrentPage,
            categorias.TotalPages,
            categorias.HasNext,
            categorias.HasPrevious
        };

        Response.Headers.Append("X-Pagination", JsonConvert.SerializeObject(metadata));

        var categoriasDto = _mapper.Map<IEnumerable<CategoriaDTO>>(categorias);

        return Ok(categoriasDto);
    }

    [HttpGet("{id:int}", Name = "ObterCategoria")]
    public ActionResult<CategoriaDTO> Get(int id)
    {
        var categoria = _uow.CategoriaRepository.Get(c => c.CategoriaId == id);

        if (categoria is null)
            return NotFound();

        var categoriaDto = _mapper.Map<CategoriaDTO>(categoria);

        return Ok(categoriaDto);
    }

    [HttpPost]
    public ActionResult<CategoriaDTO> Post (CategoriaDTO categoriaDto)
    {
        if (categoriaDto is null)
            return BadRequest();

        var categoria = _mapper.Map<Categoria>(categoriaDto);

        var categoriaCriada = _uow.CategoriaRepository.Create(categoria);
        _uow.Commit();

        var categoriaCriadaDto = _mapper.Map<CategoriaDTO>(categoriaCriada);

        return new CreatedAtRouteResult("ObterCategoria",
            new { id = categoriaCriadaDto.CategoriaId }, categoriaCriadaDto);
    }

    [HttpPut("{id:int}")]
    public ActionResult<CategoriaDTO> Put (int id, CategoriaDTO categoriaDto)
    {
        if (categoriaDto is null)
            return BadRequest();
        
        var categoria = _mapper.Map<Categoria>(categoriaDto);

        var categoriaAtualizada = _uow.CategoriaRepository.Update(categoria);
        _uow.Commit();

        var categoriaAtualizadaDto = _mapper.Map<CategoriaDTO>(categoriaAtualizada);

        return Ok(categoriaAtualizadaDto);
    }

    [HttpDelete("{id:int}")]
    public ActionResult<CategoriaDTO> Delete(int id)
    {
        var categoria = _uow.CategoriaRepository.Get(c => c.CategoriaId == id);

        if (categoria is null)
            return NotFound();

        var categoriaExcluida = _uow.CategoriaRepository.Delete(categoria);
        _uow.Commit();

        var categoriaExcluidaDto = _mapper.Map<CategoriaDTO>(categoriaExcluida);

        return Ok(categoriaExcluidaDto);
    }
}
