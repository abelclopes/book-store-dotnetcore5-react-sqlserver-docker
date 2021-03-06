using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Api.Model
{
    public class PaginationList<T>
    {
        public PaginationList(int numeroPagina, int tamanhoPagina)
        {
            this.NumeroPagina = numeroPagina;
            this.TamanhoPagina = tamanhoPagina;
        }
        public int TotalItens { get; set; }
        public int NumeroPagina { get; }
        public int TamanhoPagina { get; }
        public List<T> Resultado { get; set; }
        public int TotalPaginas => (int)Math.Ceiling(this.TotalItens / (double)this.TamanhoPagina);
        public bool TemPaginaAnterior => this.NumeroPagina > 1;
        public bool TemPaginaPosterior => this.NumeroPagina < this.TotalPaginas;
        public int NumeroPaginaPosterior => this.TemPaginaPosterior ? this.NumeroPagina + 1 : this.TotalPaginas;
        public int NumeroPaginaAnterior => this.TemPaginaAnterior ? this.NumeroPagina - 1 : 1;

        public async Task<PaginationList<T>> Read(IQueryable<T> source)
        {
            this.TotalItens = await source.CountAsync();
            this.Resultado = await source.Skip(TamanhoPagina * (NumeroPagina - 1))
                                         .Take(TamanhoPagina)
                                         .ToListAsync();

            return this;
        }

        public PaginationList<T> Read(List<T> source)
        {
            this.TotalItens = source.Count();
            this.Resultado = source.Skip(TamanhoPagina * (NumeroPagina - 1))
                                   .Take(TamanhoPagina).ToList();                              

            return this;
        }
    }
}