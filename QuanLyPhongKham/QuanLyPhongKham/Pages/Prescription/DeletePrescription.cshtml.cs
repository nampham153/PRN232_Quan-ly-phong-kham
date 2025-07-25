using BusinessAccessLayer.IService;
using DataAccessLayer.ViewModels;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc;

public class DeletePrescriptionModel : PageModel
{
    private readonly IPrescriptionService _service;

    public DeletePrescriptionModel(IPrescriptionService service)
    {
        _service = service;
    }

    [BindProperty]
    public PrescriptionViewModel Prescription { get; set; }

    public IActionResult OnGet(int id)
    {
        var prescription = _service.GetPrescriptionById(id);
        if (prescription == null) return NotFound();
        Prescription = prescription;
        return Page();
    }

    public IActionResult OnPost(int id)
    {
        _service.DeletePrescription(id);
        return RedirectToPage("ListPrescription");
    }
}