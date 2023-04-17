using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.IO;
using System.Linq;
using MStarSupply.Data;
using MStarSupply.Models;
using X.PagedList;
using iText.Kernel.Font;
using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Canvas;
using iText.Layout;
using iText.Layout.Element;
using iText.Layout.Properties;
using iText.Kernel.Colors;
using iText.IO.Font.Constants;
using iText.Layout.Borders;



namespace MStarSupply.Controllers
{
    public class MovimentacoesController : Controller
    {
        private readonly MStarSupplyContext _context;

        public MovimentacoesController(MStarSupplyContext context)
        {
            _context = context;
        }
        public List<Movimentacoes> GetMovimentacoes()
        {
            var dbContext = _context.Movimentacoes;
            return dbContext.Include(m => m.Produto).ToList();
        }

        // GET: Movimentacoes
        public async Task<IActionResult> Index(int? page, string filtro_atual, string busca_caixa_digitacao, int quantidade_de_dados_por_pagina)
        {
            //referencia o contexto de movimentacoes para obter os dados que devem ser tratados.
            var todas_movimentacoes = GetMovimentacoes();
            //cria uma nova lista de dados baseado no modelo de dados Movimentacoes para gerar o resultado dos dados para retorno.
            var dados_filtrados = new List<Movimentacoes>();
            //Define a pagina atual do grid
            int grid_pagina_atual = (page ?? 1);

            //verifica se a caixa de busca está vazia para determinar se mantem o dado da caixa ou exibe os dados por default(estado inicial)
            //caso o valor da caixa seja digitado, pela primeira vez ele irá definir a pagina do grid para a página 1
            if (!String.IsNullOrEmpty(busca_caixa_digitacao))
            {
                grid_pagina_atual = 1;
                if (filtro_atual == null)
                {
                    filtro_atual = busca_caixa_digitacao;
                }
                else if (filtro_atual != busca_caixa_digitacao)
                {
                    filtro_atual = busca_caixa_digitacao;
                }else if (filtro_atual != null && busca_caixa_digitacao == null)
                {
                    filtro_atual = null;
                }
            }

            //alimenta o objeto dados com os resultados de busca
            foreach (var movimentacao in todas_movimentacoes)
            {
                //Inicia a verificação dos dados da caixa de busca, se os dados da caixa de busca corresponderem
                //A algum dado dos campos disponíveis em visualização, ainda não foi implementado a busca por datas.
                if (filtro_atual != null && filtro_atual.IndexOf("saida", StringComparison.OrdinalIgnoreCase) >= 0)
                {
                    if (!movimentacao.TipoMovimentacao)
                    {
                        dados_filtrados.Add(movimentacao);
                    }

                }
                else if (filtro_atual != null && filtro_atual.IndexOf("entrada", StringComparison.OrdinalIgnoreCase) >= 0)
                {
                    if (movimentacao.TipoMovimentacao)
                    {
                        dados_filtrados.Add(movimentacao);
                    }
                }
                else if (filtro_atual != null && filtro_atual.Equals("ativo", StringComparison.OrdinalIgnoreCase))
                {
                    if (movimentacao.Produto.Ativo)
                    {
                        dados_filtrados.Add(movimentacao);
                    }
                }
                else if (filtro_atual != null && filtro_atual.Equals("inativo", StringComparison.OrdinalIgnoreCase))
                {
                    if (!movimentacao.Produto.Ativo)
                    {
                        dados_filtrados.Add(movimentacao);
                    }
                }
                else if (filtro_atual != null && (
                    movimentacao.Local.Contains(filtro_atual, StringComparison.OrdinalIgnoreCase) || 
                    movimentacao.Produto.Nome.Contains(filtro_atual, StringComparison.OrdinalIgnoreCase) ||
                    movimentacao.Produto.Fabricante.Contains(filtro_atual, StringComparison.OrdinalIgnoreCase) ||
                    movimentacao.Produto.Tipo.Contains(filtro_atual, StringComparison.OrdinalIgnoreCase)
                    ))
                {
                        dados_filtrados.Add(movimentacao);
                }
            }

            //define os valores para a View, caso a quantidade dados seja menor do que o minimo necessário
            //será definido para o grid os valores minimos de exibição e paginação

            //define a quantidade minina de dados por paginação do grid
            if (quantidade_de_dados_por_pagina < 6)
            {
                quantidade_de_dados_por_pagina = 6;
            }
            
            
            //ViewBag para retornar a lista de produtos disponíveis
            ViewBag.Produtos = _context.Produto.ToList();
            //ViewBags par manter as personalizações dos filtros, paginação, quantidade de itens por página e etc
            ViewBag.quantidade_de_dados_por_pagina = quantidade_de_dados_por_pagina;
            ViewBag.Page = grid_pagina_atual;
            ViewBag.filtro_atual = filtro_atual;
            
            //define os valores para a View, caso a quantidade de páginas seja menor do que o minimo
            //será definido que o grid deve o valor minimo permitido de dados permitido para o filtro
            
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
                if ((todas_movimentacoes.Count / quantidade_de_dados_por_pagina) < 1)
                {
                    grid_pagina_atual = 1;
                }
                //Retorna o valor com todas as movimentações
                return View(todas_movimentacoes.ToPagedList(grid_pagina_atual, quantidade_de_dados_por_pagina));
            }
            
        }


        // GET: Movimentacoes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Movimentacoes == null)
            {
                return NotFound();
            }

            var movimentacoes = await _context.Movimentacoes
                .FirstOrDefaultAsync(m => m.Id == id);
            if (movimentacoes == null)
            {
                return NotFound();
            }

            return View(movimentacoes);
        }

        // GET: Movimentacoes/Create
        public IActionResult Create()
        {
            ViewBag.Produtos = _context.Produto.ToList();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Produto_id,DataHora,Quantidade, TipoMovimentacao, Local")] Movimentacoes movimentacoes)
        {
            var produto = await _context.Produto.FindAsync(movimentacoes.Produto_id);
            movimentacoes.Produto = produto;
            if (produto == null)
            {
                ModelState.AddModelError(string.Empty, "Produto inválido");
                return View(movimentacoes);
            }
            if (ModelState.IsValid)
            {
                _context.Add(movimentacoes);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            ViewBag.Produtos = _context.Produto.ToList();
            return View(movimentacoes);
        }

        // GET: Movimentacoes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            ViewBag.Produtos = _context.Produto.ToList();
            if (id == null || _context.Movimentacoes == null)
            {
                return NotFound();
            }

            var movimentacoes = await _context.Movimentacoes.FindAsync(id);
            if (movimentacoes == null)
            {
                return NotFound();
            }
            return View(movimentacoes);
        }

        // POST: Movimentacoes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Produto_id,DataHora,Quantidade,TipoMovimentacao,Local")] Movimentacoes movimentacoes)
        {
            ViewBag.Produtos = _context.Produto.ToList();
            if (id != movimentacoes.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(movimentacoes);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MovimentacoesExists(movimentacoes.Id))
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
            return View(movimentacoes);
        }

        // GET: Movimentacoes/Delete/5
        public async Task<IActionResult> DeleteDoModal(int Id)
        {
            var item = await _context.Movimentacoes.FindAsync(Id);

            if (item == null)
            {
                return NotFound();
            }

            _context.Movimentacoes.Remove(item);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }


        public IActionResult GerarRelatorio()
        {
            var movimentacoes = GetMovimentacoes();// chama o método para obter os dados de movimentação e usa o iText7 para gerar um PDF

            // Agrupa as movimentações por produto e tipo de movimentação
            var gruposEntrada = movimentacoes.Where(mov => mov.TipoMovimentacao == true)
                                              .GroupBy(mov => new { mov.Produto_id, mov.TipoMovimentacao });

            var gruposSaida = movimentacoes.Where(mov => mov.TipoMovimentacao == false)
                                            .GroupBy(mov => new { mov.Produto_id, mov.TipoMovimentacao });

            using var memoryStream = new MemoryStream();
            var pdfWriter = new PdfWriter(memoryStream);
            var pdfDocument = new PdfDocument(pdfWriter);
            var document = new Document(pdfDocument);

            document.Add(new Paragraph("Movimentações de entrada")
                .SetBackgroundColor(ColorConstants.GREEN)
                .SetFontColor(ColorConstants.BLACK)
                .SetTextAlignment(TextAlignment.CENTER));

            foreach (var grupo in gruposEntrada)
            {
                var agrupamento = grupo.GroupBy(mov => new { Ano = mov.DataHora.Year, Mes = mov.DataHora.Month });
                var produto = grupo.First().Produto?.Nome ?? "Não especificado";

                var table = new Table(new float[] { 1, 1, 1 });
                var total = grupo.Sum(mov => mov.Quantidade); // Soma das quantidades das movimentações do grupo
                var headerCell = new Cell(1, 4)
                    .Add(new Paragraph($"Produto: {produto} - Total: {total}"))
                    .SetFontColor(ColorConstants.WHITE)
                    .SetBackgroundColor(ColorConstants.DARK_GRAY)
                    .SetTextAlignment(TextAlignment.CENTER);
                table.AddHeaderCell(headerCell);

                var headerFont = PdfFontFactory.CreateFont(StandardFonts.HELVETICA_BOLD);
                table.AddHeaderCell("Mês/Ano").SetFont(headerFont);
                table.AddHeaderCell("Quantidade").SetFont(headerFont);
                table.AddHeaderCell("Local").SetFont(headerFont);

                foreach (var mov in grupo)
                {
                    var mesAno = mov.DataHora.ToString("MM/yyyy"); // Obter o mês e ano formatados como "MM/yyyy"
                    table.AddCell(mesAno).SetTextAlignment(TextAlignment.CENTER).SetBorder(Border.NO_BORDER);
                    table.AddCell(mov.Quantidade.ToString()).SetTextAlignment(TextAlignment.CENTER).SetBorder(Border.NO_BORDER);
                    table.AddCell(mov.Local).SetTextAlignment(TextAlignment.CENTER).SetBorder(Border.NO_BORDER);
                }

                document.Add(table);
                document.Add(new Paragraph("\n"));
            }

            document.Add(new AreaBreak(AreaBreakType.NEXT_PAGE));

            document.Add(new Paragraph("Movimentações de saída")
                .SetBackgroundColor(ColorConstants.RED)
                .SetFontColor(ColorConstants.BLACK)
                .SetTextAlignment(TextAlignment.CENTER));

            foreach (var grupo in gruposSaida)
            {
                var produto = grupo.First().Produto?.Nome ?? "Não especificado";
                var total = grupo.Sum(mov => mov.Quantidade); // Soma das quantidades das movimentações do grupo
                var table = new Table(new float[] { 1, 1, 1 });
                var agrupamento = grupo.GroupBy(mov => new { Ano = mov.DataHora.Year, Mes = mov.DataHora.Month }); // Agrupa os dados por mês e ano
                var headerCell = new Cell(1, 4)
                    .Add(new Paragraph($"Produto: {produto} - Total: {total}"))
                    .SetFontColor(ColorConstants.WHITE)
                    .SetBackgroundColor(ColorConstants.DARK_GRAY)
                    .SetTextAlignment(TextAlignment.CENTER);
                table.AddHeaderCell(headerCell);

                var headerFont = PdfFontFactory.CreateFont(StandardFonts.HELVETICA_BOLD);
                table.AddHeaderCell("Mês/Ano").SetFont(headerFont);
                table.AddHeaderCell("Quantidade").SetFont(headerFont);
                table.AddHeaderCell("Local").SetFont(headerFont);

                foreach (var mov in grupo)
                {
                    var mesAno = mov.DataHora.ToString("MM/yyyy"); // Obter o mês e ano formatados como "MM/yyyy"
                    table.AddCell(mesAno).SetTextAlignment(TextAlignment.CENTER).SetBorder(Border.NO_BORDER);
                    table.AddCell(mov.Quantidade.ToString()).SetTextAlignment(TextAlignment.CENTER).SetBorder(Border.NO_BORDER);
                    table.AddCell(mov.Local).SetTextAlignment(TextAlignment.CENTER).SetBorder(Border.NO_BORDER);
                }

                document.Add(table);
                document.Add(new Paragraph("\n"));

            }

            document.Close();

            var bytes = memoryStream.ToArray();

            return File(bytes, "application/pdf", "relatorio.pdf");
        }






        private bool MovimentacoesExists(int id)
        {
          return (_context.Movimentacoes?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
