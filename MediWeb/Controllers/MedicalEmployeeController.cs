using DataLayer;
using MediWeb.Models;
using MediWeb.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace MediWeb.Controllers
{
    public class MedicalEmployeeController : Controller
    {
        //Here, we will bind and get data from RegisterMedicalEmployeeViewModel and MedicalEmployeeDetails model
        //and we convert them to the DTOs that are then sent to the Service layers, and then in the service layers,
        //we can finally transform them to the Entity model and manipulate the data
        //Simialrly when returning data, we transform from EntityModel to the DTOs first, and lastly to View model
        private readonly MedicalEmployeeService _medicalEmployeeService;
        private readonly ClinicService _clinicService;
        private readonly UserManager<UserAccount> _userManager;

        public MedicalEmployeeController(MedicalEmployeeService service, ClinicService clinicService, UserManager<UserAccount> userManager)
        {
            _medicalEmployeeService = service;
            _clinicService = clinicService;
            _userManager = userManager;
        }

        // GET: MedicalEmployee
        public async Task<IActionResult> Index()
        {
            var medicalEmployees = await _medicalEmployeeService.GetAllAsync();
            var medicalEmployeesDetails = medicalEmployees.Select(me => new MedicalEmployeeDetailsViewModel
            {
                Id = me.Id,
                ClinicId = me.ClinicId,
                ClinicName = me.Clinic.Name,
                FirstName = me.UserAccount.FirstName,
                LastName = me.UserAccount.LastName,
                Email = me.UserAccount.Email
            });
            return View(medicalEmployeesDetails);
        }

        // GET: MedicalEmployee/Details/5
        public async Task<IActionResult> Details(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var medicalEmployee = await _medicalEmployeeService.GetByIdAsync(id.Value);
            if (medicalEmployee == null)
            {
                return NotFound();
            }

            return View(medicalEmployee);
        }

        // GET: MedicalEmployee/Create
        public async Task<IActionResult> Create()
        {
            var clinics = await _clinicService.GetAllAsync();
            ViewData["ClinicId"] = new SelectList(clinics, "Id", "Name");
            return View();
        }

        // POST: MedicalEmployee/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(RegisterMedicalEmployeeViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new UserAccount
                {
                    UserName = model.Email,
                    Email = model.Email,
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    CreatedDate = DateTime.UtcNow
                };
                var result = await _userManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    var medicalEmployee = new MedicalEmployee
                    {
                        ClinicId = model.ClinicId,
                        UserAccount = user,
                        UserAccountId = user.Id
                    };

                    await _medicalEmployeeService.AddAsync(medicalEmployee);
                    return RedirectToAction(nameof(Index));
                }                             
            }
            var clinics = await _clinicService.GetAllAsync();
            ViewData["ClinicId"] = new SelectList(clinics, "Id", "Name", model.ClinicId);
            return View(model);
        }

        // GET: MedicalEmployee/Edit/5
        public async Task<IActionResult> Edit(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var medicalEmployee = await _medicalEmployeeService.GetByIdAsync(id.Value);
            if (medicalEmployee == null)
            {
                return NotFound();
            }
            var clinics = await _medicalEmployeeService.GetAllAsync();
            ViewData["ClinicId"] = new SelectList(clinics, "Id", "Name", medicalEmployee.ClinicId);
            var medicalEmployeeDetails =new MedicalEmployeeDetailsViewModel
            {
                Id = medicalEmployee.Id,
                ClinicId = medicalEmployee.ClinicId,
                ClinicName = medicalEmployee.Clinic.Name,
                FirstName = medicalEmployee.UserAccount.FirstName,
                LastName = medicalEmployee.UserAccount.LastName,
                Email = medicalEmployee.UserAccount.Email
            };
            return View(medicalEmployeeDetails);
        }

        // POST: MedicalEmployee/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(MedicalEmployeeDetailsViewModel medicalEmployeeDetails)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var id = medicalEmployeeDetails.Id;
                    var medicalEmployee = await _medicalEmployeeService.GetByIdAsync(id);
                    medicalEmployee.UserAccount.FirstName = medicalEmployeeDetails.FirstName;
                    medicalEmployee.UserAccount.LastName = medicalEmployeeDetails.LastName;
                    medicalEmployee.UserAccount.Email = medicalEmployeeDetails.Email;

                    await _medicalEmployeeService.UpdateAsync(medicalEmployee);
                    await _userManager.UpdateAsync(medicalEmployee.UserAccount);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (await _medicalEmployeeService.GetByIdAsync(medicalEmployeeDetails.Id) == null)
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
            var clinics = await _medicalEmployeeService.GetAllAsync();
            ViewData["ClinicId"] = new SelectList(clinics, "Id", "Name", medicalEmployeeDetails.ClinicId);
            return RedirectToAction(nameof(Index));
        }

        // GET: MedicalEmployee/Delete/5
        public async Task<IActionResult> Delete(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var medicalEmployee = await _medicalEmployeeService.GetByIdAsync(id.Value);
            if (medicalEmployee == null)
            {
                return NotFound();
            }

            var medicalEmployeesDetails = new MedicalEmployeeDetailsViewModel
            {
                Id = medicalEmployee.Id,
                ClinicId = medicalEmployee.ClinicId,
                ClinicName = medicalEmployee.Clinic.Name,
                FirstName = medicalEmployee.UserAccount.FirstName,
                LastName = medicalEmployee.UserAccount.LastName,
                Email = medicalEmployee.UserAccount.Email
            };
            return View(medicalEmployeesDetails);
        }

        // POST: MedicalEmployee/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(long id)
        {
            await _medicalEmployeeService.DeleteAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
