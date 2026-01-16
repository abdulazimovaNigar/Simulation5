using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SimulationTiyaGolf.Contexts;
using SimulationTiyaGolf.Helpers;
using SimulationTiyaGolf.ViewModels;

namespace SimulationTiyaGolf.Areas.Admin.Controllers;

[Area("Admin")]
public class EventController : Controller
{
    private readonly AppDbContext _context;
    private readonly IWebHostEnvironment _environment;
    private readonly string folderPath;

    public EventController(AppDbContext context, IWebHostEnvironment environment)
    {
        _context = context;
        _environment = environment;
        folderPath = Path.Combine(_environment.WebRootPath, "images");
    }


    public async Task<IActionResult> IndexAsync()
    {
        var events = await _context.Events.Select(eventik => new EventGetVM() 
        { 
            Id = eventik.Id,
            Title = eventik.Title,
            Description = eventik.Description,
            EventDate = eventik.EventDate,
            Image = eventik.Image,
            LocationCountry = eventik.Location.Country,
            LocationCity = eventik.Location.City
        }).ToListAsync();
        return View(events);
    }
    
    public async Task<IActionResult> Create()
    {
        await SendLocationsWithViewBag();
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Create(EventCreateVM vm)
    {
        await SendLocationsWithViewBag();
        if (!ModelState.IsValid) return View(vm);

        var isExistEvent = await _context.Locations.AnyAsync(x => x.Id == vm.LocationId);
        if (!isExistEvent) 
        {
            ModelState.AddModelError("LocationId", "This location is not found");
            return View(vm);
        }
        if (vm.Image.Length > 2*1024*1024) 
        {
            ModelState.AddModelError("Image", "Image max size must be 2mb");
            return View(vm);
        }
        if (!vm.Image.ContentType.Contains("image")) 
        {
            ModelState.AddModelError("Image", "You can upload file with image type only");
            return View(vm);
        }
        string uniqueFileName = await vm.Image.FileUploadAsync(folderPath);

        Event eventik = new () 
        { 
            EventDate = vm.EventDate,
            Image = uniqueFileName,
            Title = vm.Title,
            Description = vm.Description,
            LocationId = vm.LocationId,
            Price = vm.Price
        };
        
        await _context.Events.AddAsync(eventik);
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }
    public async Task<IActionResult> Delete(int id)
    {
        var eventik = await _context.Events.FirstOrDefaultAsync(l => l.Id == id);
        if (eventik == null) return NotFound();
        _context.Events.Remove(eventik);
        await _context.SaveChangesAsync();

        string deletedFilePath = Path.Combine(folderPath, eventik.Image);
        FileHelper.FileDelete(deletedFilePath);

        return RedirectToAction(nameof(Index));
    }
    [HttpGet]
    public async Task<IActionResult> Update(int id)
    {

        var eventik = await _context.Events.FirstOrDefaultAsync(l => l.Id == id);
        if (eventik is null) return NotFound();

        EventUpdateVM vm = new()
        {
            EventDate = eventik.EventDate,
            Title = eventik.Title,
            Description = eventik.Description,
            LocationId = eventik.LocationId,
            Price = eventik.Price
        };

        await SendLocationsWithViewBag();
        return View(vm);
    }
    [HttpPost]
    public async Task<IActionResult> Update(EventUpdateVM vm)
    {
        await SendLocationsWithViewBag();
        if (!ModelState.IsValid) return View(vm);
        var existEvent = await _context.Events.FindAsync(vm.Id);
        if (existEvent is null) return NotFound();

        var isExistLocation = await _context.Locations.AnyAsync(x => x.Id == vm.LocationId);
        if (!isExistLocation)
        {
            ModelState.AddModelError("LocationId", "This location is not found");
            return View(vm);
        }

        if (!vm.Image?.CheckSize(2) ?? false) 
        {
            ModelState.AddModelError("Image", "Image max size must be not more than 2 MB");
            return View(vm);
        }
        if (!vm.Image?.CheckType("Image") ?? false) 
        {
            ModelState.AddModelError("Image", "You can upload image type only");
            return View(vm);
        }

        existEvent.Title = vm.Title;
        existEvent.Description = vm.Description;
        existEvent.LocationId = vm.LocationId;
        existEvent.EventDate = vm.EventDate;
        existEvent.Price = vm.Price;
        if(vm.Image is not null) 
        {
            string newImage = await vm.Image.FileUploadAsync(folderPath);

            string deletedImage = Path.Combine(folderPath, existEvent.Image);
            FileHelper.FileDelete(deletedImage);

            existEvent.Image = newImage;
        }

        _context.Events.Update(existEvent);
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    private async Task SendLocationsWithViewBag()
    {
        var locations = await _context.Locations.Select(x => new SelectListItem()
        {
            Text = $"{x.Country},{x.City}",
            Value = x.Id.ToString()

        }).ToListAsync();
        ViewBag.Locations = locations;
    }
}
