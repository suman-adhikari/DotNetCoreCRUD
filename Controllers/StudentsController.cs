using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using CORE2.Models;

using CORE2.Data;
using Microsoft.EntityFrameworkCore;

namespace CORE2.Controllers
{
    public class StudentsController : Controller
    {
        private readonly SchoolContext _context;

        public StudentsController(SchoolContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index(){
             return View(await _context.Students.ToListAsync());
        }

        public async Task<IActionResult> Details(int? id){
            if(id==null) 
              return NotFound();

            var student = await _context.Students
                         .Include(s=>s.Enrollments)
                          .ThenInclude(e=>e.Course)
                          .AsNoTracking()
                          .SingleOrDefaultAsync(m=>m.ID==id);
            
            if(student==null)
               return NotFound();

            return View(student);                
        }

        public async Task<IActionResult> Create(){
            //Student student = new Student();
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Student student){
            try{
                if(ModelState.IsValid){
                    _context.Add(student);
                    await _context.SaveChangesAsync();
                    return Json(student);
                }
            }catch(DbUpdateException){
                ModelState.AddModelError("", "Unable to save changes. " +
                    "Try again, and if the problem persists " +
                    "see your system administrator.");
                    }

            return View(student);
        }
    

       public async Task<IActionResult> Delete(int? id){

            if(id==null) 
              return NotFound();

            var student = await _context.Students.SingleOrDefaultAsync(m=>m.ID==id);
             _context.Students.Remove(student);   
             await _context.SaveChangesAsync();
             return Json(student);          
       }

      
       public async Task<IActionResult> Edit(int?id){
            if(id==null) 
              return NotFound();

             var student = await _context.Students                        
                          .SingleOrDefaultAsync(m=>m.ID==id);

             return View(student);              
       }
    
       [HttpPost]
      public async Task<IActionResult> Edit(Student student){
          if(student==null) 
              return NotFound();

            _context.Students.Update(student);   
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");                      
      }
    
    }
}