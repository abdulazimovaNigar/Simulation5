using Microsoft.AspNetCore.Mvc;
using SimulationTiyaGolf.Contexts;
using SimulationTiyaGolf.Models;

namespace SimulationTiyaGolf.Areas.Admin.Controllers;

[Area("Admin")]
public class LocationController(AppDbContext _context) : Controller
{
    public IActionResult Index()
    {
        var location = _context.Locations.ToList();
        return View(location);
    }
    [HttpGet]
    public IActionResult Create()
    {
        return View();
    }
    [HttpPost]
    public IActionResult Create(Location location)
    {
        if (!ModelState.IsValid) return View(location);
        _context.Locations.Add(location);
        _context.SaveChanges();
        return RedirectToAction(nameof(Index));
    }
    public IActionResult Delete(int id)
    {
        var location = _context.Locations.FirstOrDefault(l => l.Id == id);
        if (location == null) return NotFound();
        _context.Locations.Remove(location);
        _context.SaveChanges();
        return RedirectToAction(nameof(Index));
    }
    [HttpGet]
    public IActionResult Update(int id)
    {

        var location = _context.Locations.FirstOrDefault(l => l.Id == id);
        if (location is null) return NotFound();
        return View(location);
    }
    [HttpPost]
    public IActionResult Update(Location location) 
    {
        if (!ModelState.IsValid) return View(location);
        var newLocation = _context.Locations.FirstOrDefault(nl => nl.Id == location.Id);
        if (newLocation is null) return NotFound();
        newLocation.Country = location.Country;
        newLocation.City = location.City;

        _context.Locations.Update(newLocation);
        _context.SaveChanges();
        return RedirectToAction(nameof(Index));
    }
}
