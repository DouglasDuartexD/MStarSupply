using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MStarSupply.Data;
using MStarSupply.Migrations;
using MStarSupply.Models;
using X.PagedList;

namespace MStarSupply.Controllers
{
    public class ProdutosController : Controller
    {
        private readonly MStarSupplyContext _context;

        public ProdutosController(MStarSupplyContext context)
        {
            _context = context;
        }
        public List<Produto> GetProdutos()
        {
            var dbContext = _context.Produto;
            return dbContext.ToList();
        }
        // GET: Produtos
        public async Task<IActionResult> Index(int? page, string filtro_atual, string busca_caixa_digitacao, int quantidade_de_dados_por_pagina)
        {

            //referencia o contexto de produtos e obter os dados que devem ser tratados.
            var todos_produtos = GetProdutos();
            //cria uma nova lista de dados baseado no modelo de dados Produto para gerar o resultado dos dados para retorno.
            var dados_filtrados = new List<Produto>();
            //Define a pagina atual do grid
            int grid_pagina_atual = (page ?? 1);
            //verifica se a caixa de busca está vazia para determinar se mantem o dado da caixa ou exibe os dados por default(estado inicial)
            if (!String.IsNullOrEmpty(busca_caixa_digitacao))
            {
                page = 1;
                if (filtro_atual == null)
                {
                    filtro_atual = busca_caixa_digitacao;
                }
                else if (filtro_atual != busca_caixa_digitacao)
                {
                    filtro_atual = busca_caixa_digitacao;
                }
                else if (filtro_atual != null && busca_caixa_digitacao == null)
                {
                    filtro_atual = null;
                }
            }

            //alimenta o objeto dados com os resultados de busca
            foreach (var produto in todos_produtos)
            {
                //Inicia a verificação dos dados da caixa de busca, se os dados da caixa de busca corresponderem
                //A algum dado dos campos disponíveis em visualização, ainda não foi implementado a busca por datas.
                if (filtro_atual != null && filtro_atual.Equals("ativo", StringComparison.OrdinalIgnoreCase))
                {
                    if (produto.Ativo)
                    {
                        dados_filtrados.Add(produto);
                    }

                }
                else if (filtro_atual != null && filtro_atual.Equals("inativo", StringComparison.OrdinalIgnoreCase))
                {
                    if (!produto.Ativo)
                    {
                        dados_filtrados.Add(produto);
                    }
                }
                else if (filtro_atual != null && (produto.Nome.Contains(filtro_atual, StringComparison.OrdinalIgnoreCase) ||
                     produto.Fabricante.Contains(filtro_atual, StringComparison.OrdinalIgnoreCase) ||
                     produto.Tipo.Contains(filtro_atual, StringComparison.OrdinalIgnoreCase))
                   )
                {
                    dados_filtrados.Add(produto);
                }
            }
            //ViewBags manter as personalizações dos filtros, paginação, quantidade de itens por página e etc
            ViewBag.quantidade_de_dados_por_pagina = quantidade_de_dados_por_pagina;
            ViewBag.Page = page;
            ViewBag.filtro_atual = filtro_atual;

            //define os valores para a View, caso a quantidade dados seja menor do que o minimo necessário
            //será definido para o grid os valores minimos de exibição e paginação

            //define a quantidade minina de dados por paginação do grid
            if (quantidade_de_dados_por_pagina < 6)
            {
                quantidade_de_dados_por_pagina = 6;
            }
            
            if (dados_filtrados.Count > 0)
            {
                //define a pagina atual do grid baseado na quantidade de dados e a quantidade de dados por página
                if ((dados_filtrados.Count / quantidade_de_dados_por_pagina) < 1)
                {
                    grid_pagina_atual = 1;
                }
                //Retorna os dados filtrados caso exista dados definido pelos filtros
                return View(dados_filtrados.ToPagedList(grid_pagina_atual, quantidade_de_dados_por_pagina));
            }
            else
            {
                //define a pagina atual do grid baseado na quantidade de dados e a quantidade de dados por página
                if ((dados_filtrados.Count / quantidade_de_dados_por_pagina) < 1)
                {
                    grid_pagina_atual = 1;
                }
                //Retorna o valor com todas as movimentações
                return View(todos_produtos.ToPagedList(grid_pagina_atual, quantidade_de_dados_por_pagina));
            }
        }

        // GET: Produtos/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Produto == null)
            {
                return NotFound();
            }

            var produto = await _context.Produto
                .FirstOrDefaultAsync(m => m.Id == id);
            if (produto == null)
            {
                return NotFound();
            }

            return View(produto);
        }

        // GET: Produtos/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Produtos/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Nome,Fabricante,Tipo,Ativo")] Produto produto)
        {
            if (ModelState.IsValid)
            {
                _context.Add(produto);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(produto);
        }

        // GET: Produtos/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Produto == null)
            {
                return NotFound();
            }

            var produto = await _context.Produto.FindAsync(id);
            if (produto == null)
            {
                return NotFound();
            }
            return View(produto);
        }

        // POST: Produtos/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Nome,Fabricante,Tipo,Ativo")] Produto produto)
        {
            if (id != produto.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(produto);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProdutoExists(produto.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(produto);
        }

        // GET: Produtos/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Produto == null)
            {
                return NotFound();
            }

            var produto = await _context.Produto
                .FirstOrDefaultAsync(m => m.Id == id);
            if (produto == null)
            {
                return NotFound();
            }

            return View(produto);
        }

        // POST: Produtos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Produto == null)
            {
                return Problem("Entity set 'MStarSupplyContext.Produto'  is null.");
            }
            var produto = await _context.Produto.FindAsync(id);
            if (produto != null)
            {
                _context.Produto.Remove(produto);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProdutoExists(int id)
        {
          return (_context.Produto?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
