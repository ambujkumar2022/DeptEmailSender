using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DeptEmailSender.Data;
using DeptEmailSender.Models;
using System.Threading.Tasks;
using System.Linq;
using System;
using System.Data;

namespace DeptEmailSender.Controllers
{
    public class DepartmentsController : Controller
    {
        private readonly ApplicationDbContext _context;
        public DepartmentsController (ApplicationDbContext context)
        {
            _context = context;
        }

        //GET -Departments
        public async Task<IActionResult> Index( int? id) 
        {
            if (id == null)
            {
                var rootDepartments = await _context.Departments
                       .Where(d => d.ParentDepartmentId == null)
                       .ToListAsync();

                return View(rootDepartments);
            }
            else
            {
                var dept = await _context.Departments
                           .Include(d => d.SubDepartments)
                           .FirstOrDefaultAsync(d => d.DeptId == id);

                if(dept == null)
                {
                    return NotFound();
                }
                ViewBag.ParentDepartments = await GetParentDepartments(dept);
                return View("Details", dept);
            }
        }

        private async Task<List<Department>> GetParentDepartments(Department department)
        {
            var parents = new List<Department>();
            var current = department;
            while (current.ParentDepartmentId != null)
            {
                current = await _context.Departments
                          .FirstOrDefaultAsync(d => d.DeptId == current.ParentDepartmentId);
                parents.Add(current);
            }

            return parents;
        }

        //GET -Departments/Create
        public IActionResult Create()
        {
            return View();
        }
    }
}
