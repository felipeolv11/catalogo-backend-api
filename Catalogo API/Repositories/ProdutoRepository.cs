using Catalogo_API.Context;
using Catalogo_API.Models;
using Catalogo_API.Pagination;
using Catalogo_API.Repositories.Interfaces;

namespace Catalogo_API.Repositories;

public class ProdutoRepository : Repository<Produto>, IProdutoRepository
{
    public ProdutoRepository(CatalogoDbContext context) : base(context)
    {
    }

    public PagedList<Produto> GetProdutos(ProdutosParameters produtoParams)
    {
        var produtos = GetAll().OrderBy(p => p.ProdutoId).AsQueryable();

        var produtosOrdenados = PagedList<Produto>.ToPagedList(produtos,
            produtoParams.PageNumber, produtoParams.PageSize);

        return produtosOrdenados;
    }

    public IEnumerable<Produto> GetProdutosPorCategoria(int id)
    {
        return GetAll().Where(c => c.CategoriaId == id);
    }
}
