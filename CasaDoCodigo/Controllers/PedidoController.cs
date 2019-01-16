using CasaDoCodigo.Models;
using CasaDoCodigo.Models.ViewModels;
using CasaDoCodigo.Repositories;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CasaDoCodigo.Controllers
{
    public class PedidoController : Controller
    {
        private readonly IProdutoRepository produtoRepository;
        private readonly IPedidoRepository pedidoRepository;
        private readonly IItemPedidoRepository itemPedidoRepository;

        public PedidoController(IProdutoRepository produtoRepository,
            IPedidoRepository pedidoRepository,
            IItemPedidoRepository itemPedidoRepository)
        {
            this.produtoRepository = produtoRepository;
            this.pedidoRepository = pedidoRepository;
            this.itemPedidoRepository = itemPedidoRepository;
        }

        public IActionResult Carrossel()
        {
            return View(produtoRepository.GetProdutos());
        }

        public async Task<IActionResult> Carrinho(string codigo)
        {
            if (!string.IsNullOrEmpty(codigo))
            {
                await pedidoRepository.AddItem(codigo);
            }

            Pedido taskPedido = await pedidoRepository.GetPedido();
            List<ItemPedido> itens = taskPedido.Itens;
            CarrinhoViewModel carrinhoViewModel = new CarrinhoViewModel(itens);
            return base.View(carrinhoViewModel);
        }

        public async Task<IActionResult> Cadastro()
        {
            var pedido = await pedidoRepository.GetPedido();

            if (pedido == null)
            {
                return RedirectToAction("Carrossel");
            }

            return View(pedido.Cadastro);
        }

        public IActionResult BuscaDeProdutos(string pesquisa)
        {
            // True = Houve resultado
            // False = não houve resultado
            if(string.IsNullOrEmpty(pesquisa)) //Se a pesquisa for vazia, retorna todos os produtos
            {
                var viewModel1 = new BuscaViewModel(produtoRepository.GetProdutos(), true);
                return View(viewModel1);
            }



            if(produtoRepository.GetProdutos(pesquisa) == null) //Se o resultado da pesquisa for nulo
            {
                var viewModelComNenhumProdutoEncontrado = new BuscaViewModel(false); //Nao houve resultado, entao fica false
                return View(viewModelComNenhumProdutoEncontrado); // Retorna a view sem resultado
            }

            var viewModel = new BuscaViewModel(produtoRepository.GetProdutos(pesquisa), true); //Retorna os produtos da pesquisa
            return View(viewModel);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Resumo(Cadastro cadastro)
        {
            if (ModelState.IsValid)
            {
                return View(await pedidoRepository.UpdateCadastro(cadastro));
            }
            return RedirectToAction("Cadastro");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<UpdateQuantidadeResponse> UpdateQuantidade([FromBody]ItemPedido itemPedido)
        {
            return await pedidoRepository.UpdateQuantidade(itemPedido);
        }
    }
}
