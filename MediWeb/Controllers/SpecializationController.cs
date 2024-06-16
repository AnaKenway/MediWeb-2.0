using DataLayer;
using MediWeb.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MediWeb.Controllers;

public class SpecializationController : Controller
{
    private readonly SpecializationService _specializationService;

    public SpecializationController(SpecializationService specializationService)
    {
        _specializationService = specializationService;
    }
    public async Task<IActionResult> Index()
    {
        var specializations = await _specializationService.GetAllAsync();
        return View(specializations.OrderBy(s => s.Id));
    }

    // GET: Specialization/Details/5
    public async Task<IActionResult> Details(long? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var specialization = await _specializationService.GetByIdAsync(id.Value);
        if (specialization == null)
        {
            return NotFound();
        }

        return View(specialization);
    }

    // GET: Specialization/Create
    public async Task<IActionResult> Create()
    {
        return View();
    }

    // POST: Specialization/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(Specialization model)
    {
        if (ModelState.IsValid)
        {
            var specialization = await _specializationService.AddAsync(model);
            return RedirectToAction(nameof(Index));
        }
        return View(model);
    }

    // GET: Specialization/Edit/5
    public async Task<IActionResult> Edit(long? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var specialization = await _specializationService.GetByIdAsync(id.Value);

        if (specialization == null)
        {
            return NotFound();
        }

        return View(specialization);
    }

    // POST: Specialization/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(Specialization model)
    {
        if (ModelState.IsValid)
        {
            try
            {
                var result = await _specializationService.UpdateAsync(model);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (await _specializationService.GetByIdAsync(model.Id) == null)
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
        return RedirectToAction(nameof(Index));
    }

    // GET: Specialization/Delete/5
    public async Task<IActionResult> Delete(long? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var specialization = await _specializationService.GetByIdAsync(id.Value);
        if (specialization == null)
        {
            return NotFound();
        }

        return View(specialization);
    }

    // POST: Specialization/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(long id)
    {
        await _specializationService.DeleteAsync(id);
        return RedirectToAction(nameof(Index));
    }
}
