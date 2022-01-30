#nullable disable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebApp.Data;
using WebApp.Models;
using WebApp.Services;

namespace WebApp.Controllers
{
    public class TopicController : Controller
    {
        private readonly ApplicationDbContext _context;

        public TopicController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Topic
        public async Task<IActionResult> Index()
        {
            return View(await _context.Topics.ToListAsync());
        }

        // GET: Topic/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var topic = await _context.Topics
                .FirstOrDefaultAsync(m => m.TopicId == id);
            var itemList = (from m in _context.TopicItems where m.TopicId == topic.TopicId select m).ToList();

            if (topic == null)
            {
                return NotFound();
            }

            return View(new TopicViewModel()
            {
                UserId = topic.UserId,
                Info = topic.Info,
                TopicId = topic.TopicId,
                Name = topic.Name,
                Items = itemList
            });        }

        // GET: Topic/Create
        [Authorize]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Topic/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("TopicId,Name,Info,UserId")] Topic topic)
        {
            if (ModelState.IsValid)
            {
                if (topic.UserId == 0)
                {
                    foreach (var i in _context.Users)
                    {
                        if (i.Email.Equals(User.Identity.Name))
                        {
                            topic.UserId = i.Id;
                            break;
                        }
                    }
                }

                _context.Topics.Add(topic);
                Console.WriteLine(topic.UserId + " " + topic.TopicId);
                _context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }

            return View(topic);
        }

        // GET: Topic/Edit/5
        [Authorize]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var topic = await _context.Topics.FindAsync(id);
            var itemList = (from m in _context.TopicItems where m.TopicId == topic.TopicId select m).ToList();

            if (topic == null)
            {
                return NotFound();
            }

            if (!CheckUserId(topic))
            {
                return RedirectToAction("Index");
            }

            return View(new TopicViewModel()
            {
                UserId = topic.UserId,
                Info = topic.Info,
                TopicId = topic.TopicId,
                Name = topic.Name,
                Items = itemList
            });
        }

        // POST: Topic/Edit/5
            // To protect from overposting attacks, enable the specific properties you want to bind to.
            // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
            [HttpPost]
            [ValidateAntiForgeryToken]
            public async Task<IActionResult> Edit(int id, [Bind("TopicId,Name,Info,UserId")] Topic topic)
            {
                if (id != topic.TopicId)
                {
                    return NotFound();
                }

                if (topic.UserId == 0)
                {
                    topic.UserId = _context.Topics.FirstOrDefault(m => m.TopicId == topic.TopicId).UserId;
                }

                if (ModelState.IsValid)
                {
                    try
                    {
                        _context.Topics.FirstOrDefault(m => m.TopicId == topic.TopicId).Name = topic.Name;
                        _context.Topics.FirstOrDefault(m => m.TopicId == topic.TopicId).Info = topic.Info;
                        _context.Topics.FirstOrDefault(m => m.TopicId == topic.TopicId).UserId = topic.UserId;
                        await _context.SaveChangesAsync();
                    }
                    catch (DbUpdateConcurrencyException)
                    {
                        if (!TopicExists(topic.TopicId))
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

                return View(new TopicViewModel()
                {
                    UserId = topic.UserId,
                    Info = topic.Info,
                    TopicId = topic.TopicId,
                    Name = topic.Name,
                    Items = new List<TopicItem>()
                });
            }

            
            // GET: Topic/Delete/5
            [Authorize]
            public async Task<IActionResult> Delete(int? id)
            {
                if (id == null)
                {
                    return NotFound();
                }

                var topic = await _context.Topics
                    .FirstOrDefaultAsync(m => m.TopicId == id);

                if (topic == null)
                {
                    return NotFound();
                }

                if (!CheckUserId(topic))
                {
                    return RedirectToAction("Index");
                }

                return View(topic);
            }

            // POST: Topic/Delete/5
            [HttpPost, ActionName("Delete")]
            [ValidateAntiForgeryToken]
            public async Task<IActionResult> DeleteConfirmed(int id)
            {
                var topic = await _context.Topics.FindAsync(id);
                _context.Topics.Remove(topic);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            public IActionResult TestPage()
            {
                return View(2);
            }

            private bool TopicExists(int id)
            {
                return _context.Topics.Any(e => e.TopicId == id);
            }

            public bool CheckUserId(Topic topic)
            {
                if (User.IsInRole("admin"))
                {
                    return true;
                }

                foreach (var i in _context.Users)
                {
                    if (i.Email.Equals(User.Identity.Name))
                    {
                        if (i.Id != topic.UserId)
                        {
                            return false;
                        }
                        else break;
                    }
                }

                return true;
            }
        }
    }