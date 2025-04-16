using mf_dev_backend_2025.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace mf_dev_backend_2025.Controllers
{
    [Authorize]
    public class VeiculosController : Controller

    {
        private readonly AppDbContext _context; 
        public VeiculosController(AppDbContext context) 
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var dados = await _context.veiculos.ToListAsync();
            return View(dados);  
        }

        public IActionResult Create()
        {
            return View(); 
        }

        [HttpPost] 
        public async Task<IActionResult> Create(Veiculo veiculo)
        {
            if (ModelState.IsValid)
            {
                _context.veiculos.Add(veiculo);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(veiculo);
        }
         public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
                return NotFound();

            var dados = await _context.veiculos.FindAsync(id);
            if (dados == null)
                return NotFound();

            return View(dados);

        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, Veiculo veiculo)
        {
            if (id != veiculo.Id)
                return NotFound();

            if (ModelState.IsValid)
            {
                _context.veiculos.Update(veiculo);
          
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View();

        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
                return NotFound();
            var dados = await _context.veiculos.FindAsync(id);
            if (dados == null)
                return NotFound();

            return View(dados);  
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
                return NotFound();
            var dados = await _context.veiculos.FindAsync(id);
            if (dados == null)
                return NotFound();

            return View(dados);
        }

        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int? id)
        {
            if (id == null)
                return NotFound();
            var dados = await _context.veiculos.FindAsync(id);
            if (dados == null)
                return NotFound();

            _context.veiculos.Remove(dados);
            await _context.SaveChangesAsync();

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Relatorio(int? id)
        {
            if (id == null)
                return NotFound();

            var veiculo = await _context.veiculos.FindAsync(id);

            if (veiculo == null)
                return NotFound();

            var consumos = await _context.Consumos
                .Where(c => c.VeiculoId == id)
                .OrderByDescending(c => c.Data)
                .ToListAsync(); 

            decimal total = consumos.Sum(c => c.Valor);

            ViewBag.Veiculo = veiculo;
            ViewBag.Total = total;

            return View(consumos);
        }

    }
}
