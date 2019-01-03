using CasaDoCodigo.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace CasaDoCodigo.Repositories
{
    public interface ICategoriaRepository
    {
        Task<Categoria> NovaCategoria(string nome);
    }

    public class CategoriaRepository : BaseRepository<Categoria>, ICategoriaRepository
    {
        public CategoriaRepository(ApplicationContext contexto) : base(contexto)
        {
        }

        public async Task<Categoria> NovaCategoria(string nome) //método assíncrono para salvar uma nova categoria
        {
            var categoria = dbSet.Where(c => c.Nome == nome)
                .SingleOrDefault();

            if (categoria == null) // Se a categoria for nula, ou seja, se nao existir, ai ele cria a categoria
            {
                var novaCategoria = new Categoria()
                {
                    Nome = nome
                };

                categoria = dbSet.Add(novaCategoria).Entity;
            }
            await contexto.SaveChangesAsync();
            return categoria;
        }
    }
}