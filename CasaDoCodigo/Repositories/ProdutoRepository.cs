using CasaDoCodigo.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CasaDoCodigo.Repositories
{
    public class ProdutoRepository : BaseRepository<Produto>, IProdutoRepository
    {
        private readonly ICategoriaRepository categoriaRepository;
        public ProdutoRepository(ApplicationContext contexto,
            ICategoriaRepository categoriaRepository) : base(contexto)
        {
            this.categoriaRepository = categoriaRepository;
        }

        public IList<Produto> GetProdutos()
        {
            return dbSet.Include(p => p.Categoria)
                .ToList();
        }
        public IList<Produto> GetProdutos(string pesquisa)
        {
            IQueryable<Produto> consulta = dbSet.Include(p => p.Categoria);

            if (!string.IsNullOrWhiteSpace(pesquisa))
            {
                consulta = consulta.Where(p => p.Nome.Contains(pesquisa) || p.Categoria.Nome.Contains(pesquisa));
            }

            return consulta.ToList();
        }

        public async Task SaveProdutos(List<Livro> livros)
        {
            foreach (var livro in livros)
            {
                if (!dbSet.Where(p => p.Codigo == livro.Codigo).Any())
                {

                    Categoria categoria = await categoriaRepository.NovaCategoria(livro.Categoria); //Vai criar a categoria, se necessario
                    dbSet.Add(new Produto(livro.Codigo, livro.Nome, livro.Preco, categoria)); // Salva o produto
                }
            }
            await contexto.SaveChangesAsync();
        }
    }

    public class Livro
    {
        public string Codigo { get; set; }
        public string Nome { get; set; }
        public string Categoria { get; set; }
        public string Subcategoria { get; set; }
        public decimal Preco { get; set; }
    }
}
