using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CRUDCORE.Models;
using CRUDCORE.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace CRUDCORE.Controllers
{
    public class HomeController : Controller
    {
        private readonly DbcrudcoreContext _DBContex;

        public HomeController(DbcrudcoreContext context)
        {
            _DBContex = context;
        }

        public IActionResult Index()
        {
            List<Empleado> lista = _DBContex.Empleados.Include(c => c.oCargo).ToList();
            return View(lista);
        }

        [HttpGet]
        public IActionResult Empleado_Detalle(int idEmpleado)
        {
            EmpleadoVM oEmpleadoVM = new EmpleadoVM()
            {
                oEmpleado = new Empleado(),
                oListaCargo = _DBContex.Cargos.Select(cargo => new SelectListItem()
                {
                    Text = cargo.Descripcion,
                    Value = cargo.IdCargo.ToString()
                }).ToList()
            };

            if(idEmpleado != 0)
            {
                oEmpleadoVM.oEmpleado = _DBContex.Empleados.Find(idEmpleado);
            }

            return View(oEmpleadoVM);
        }

        [HttpPost]
        public IActionResult Empleado_Detalle(EmpleadoVM oEmpleadoVM)
        {
            if (oEmpleadoVM.oEmpleado.IdEmpleado == 0)
            {
                _DBContex.Empleados.Add(oEmpleadoVM.oEmpleado);
            }
            else
            {
                _DBContex.Empleados.Update(oEmpleadoVM.oEmpleado);
            }

            _DBContex.SaveChanges();

            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public IActionResult Eliminar(int idEmpleado)
        {
            Empleado oEmpleado = _DBContex.Empleados.Include(c => c.oCargo).Where(e=>e.IdEmpleado == idEmpleado).FirstOrDefault();
          
            return View(oEmpleado);
        }

        [HttpPost]
        public IActionResult Eliminar(Empleado oEmpleado)
        {
            _DBContex.Empleados.Remove(oEmpleado);
            _DBContex.SaveChanges();

            return RedirectToAction("Index", "Home");
        }

    }
}