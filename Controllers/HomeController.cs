using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using MStarSupply.Data;
using MStarSupply.Models;
using X.PagedList;

namespace MStarSupply.Controllers
{
    public class HomeController : Controller
    {

        private readonly MStarSupplyContext _context;

        public HomeController(MStarSupplyContext context)
        {
            _context = context;
        }



        public async Task<IActionResult> Index()
        {
            ViewBag.Produtos = _context.Produto.ToList();
            var movimentacoes = _context.Movimentacoes.ToList();
            var produtos = _context.Produto.ToList();
            var dados = new List<Movimentacoes>();

            foreach (var movimentacao in movimentacoes)
            {
                dados.Add(movimentacao);
            }

            return _context.Movimentacoes != null ?
                          View(await _context.Movimentacoes.ToListAsync()) :
                          Problem("Entity set 'MStarSupplyContext.Movimentacoes'  is null.");

        }

    }
}
