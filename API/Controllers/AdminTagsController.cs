using API.Data;
using API.Models.Blog;
using API.Repositories.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminTagsController : ControllerBase
    {
        private readonly BloggieDbContext bloggieDbContext;
        private readonly ITagRepository tagRepository;

        public AdminTagsController(BloggieDbContext bloggieDbContext, ITagRepository tagRepository)
        {
            this.bloggieDbContext = bloggieDbContext;
            this.tagRepository = tagRepository;
        }

        //[HttpPost]
        //[ActionName("Add")]
        //public async Task<IActionResult> Add(AddTagRequest addTagRequest)
        //{
        //    ValidateAddTagRequest(addTagRequest);
        //    if (!ModelState.IsValid)
        //    {
        //        return View();
        //    }

        //    var tag = new Tag
        //    {
        //        Name = addTagRequest.Name,
        //        DisplayName = addTagRequest.DisplayName,
        //    };

        //    await tagRepository.AddAsync(tag);

        //    return RedirectToAction("List");
        //}

        [HttpGet("List")]
        
        public async Task<IActionResult> List(string? searchQuery, string? sortBy, string? sortDirection, int pageSize = 3, int pageNumber = 1)
        {
            var totalRecords = await tagRepository.CountAsync();
            var totalPages = Math.Ceiling((decimal)totalRecords / pageSize);

            if (pageNumber > totalPages)
            {
                pageNumber--;
            }

            if (pageNumber < 1)
            {
                pageNumber++;
            }

            //ViewBag.TotalPages = totalPages;
            //ViewBag.SearchQuery = searchQuery;
            //ViewBag.SortBy = sortBy;
            //ViewBag.SortDirection = sortDirection;
            //ViewBag.PageNumber = pageNumber;
            //ViewBag.PageSize = pageSize;
            //use db context to read tags
            var tags = await tagRepository.GetAllAsync(searchQuery, sortBy, sortDirection, pageNumber, pageSize);

            return Ok(tags);
        }
    }
}
