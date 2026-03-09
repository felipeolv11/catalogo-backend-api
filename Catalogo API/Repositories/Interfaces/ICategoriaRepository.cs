using Catalogo_API.Models;
using Catalogo_API.Pagination;

namespace Catalogo_API.Repositories.Interfaces;

public interface ICategoriaRepository : IRepository<Categoria>
{
    PagedList<Categoria> GetCategorias(CategoriasParameters categoriasParams);
}
