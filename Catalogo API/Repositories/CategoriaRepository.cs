using Catalogo_API.Context;
using Catalogo_API.Models;
using Catalogo_API.Pagination;
using Catalogo_API.Repositories.Interfaces;

namespace Catalogo_API.Repositories;

public class CategoriaRepository : Repository<Categoria>, ICategoriaRepository
{
    public CategoriaRepository(CatalogoDbContext context) : base(context)
    {
    }

    public PagedList<Categoria> GetCategorias(CategoriasParameters categoriaParameters)
    {
        var categorias = GetAll().OrderBy(c => c.CategoriaId).AsQueryable();

        var categoriasOrdenadas = PagedList<Categoria>.ToPagedList(categorias, 
            categoriaParameters.PageNumber, categoriaParameters.PageSize);

        return categoriasOrdenadas;
    }
}
