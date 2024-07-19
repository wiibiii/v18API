using API.Data;
using API.Models.Blog;
using API.Models.ViewModel.Blog;
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

            return Ok(
                new JsonResult(new { tags = tags, TotalPages = totalPages, PageNumber = pageNumber, PageSize = pageSize  } )
                );
        }

        [HttpGet("Edit")]
        public async Task<IActionResult> Edit(Guid id)
        {
            var tag = await tagRepository.GetAsync(id);

            if (tag != null)
            {
                var editTagRequest = new EditTagRequest
                {
                    Id = tag.Id,
                    Name = tag.Name,
                    DisplayName = tag.DisplayName,
                };

                return Ok(editTagRequest);
            }

            return Ok(null);
        }

        [HttpPost("Edit")]
        public async Task<IActionResult> Edit(EditTagRequest editTagRequest)
        {
            var tag = new Tag
            {
                Id = editTagRequest.Id,
                Name = editTagRequest.Name,
                DisplayName = editTagRequest.DisplayName,
            };

            var updatedTag = await tagRepository.UpdateAsync(tag);

            if (updatedTag != null)
            {
                //show success notification
                return Ok(
                new JsonResult(
                    new { title = "Success", message = "Tag has been updated." }
                )
            );
            }
            

            return Ok(
                new JsonResult(
                    new { title = "Error", message = "Something went wrong!." }
                )
            );
            //return RedirectToAction("Edit", new { id = editTagRequest.Id });
        }
    }
}
