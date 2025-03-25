namespace MusicShop.Areas.Admin.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using MusicShop.Services.Cars;

    public class MusicController : AdminController
    {
        private readonly IInstrumentsService inst;

        public MusicController(IInstrumentsService inst) => this.inst = inst;

        public IActionResult All()
        {
            var inst = this.inst
                .All(publicOnly: false)
                .Cars;

            return View(inst);
        }

        public IActionResult ChangeVisibility(int id)
        {
            inst.ChangeVisility(id);

            return RedirectToAction(nameof(All));
        }
    }
}
