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

        [HttpGet("get-all-tag-paginated")]

        public async Task<IActionResult> GetAllTagPaginated(string? searchQuery, string? sortBy, string? sortDirection, int pageSize = 3, int pageNumber = 1)
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
            var tags = await tagRepository.GetAllPaginatedAsync(searchQuery, sortBy, sortDirection, pageNumber, pageSize);

            return Ok(
                new JsonResult(new { tags = tags, TotalPages = totalPages, PageNumber = pageNumber, PageSize = pageSize })
                );
        }

        [HttpGet("get-all-blog-tags")]
        public async Task<ActionResult<List<Tag>>> GetAllBlogTags()
        {
            var tags = await tagRepository.GetAllBlogTags();

            return Ok(tags);
        }

        [HttpGet("Edit")]
        public async Task<IActionResult> Edit(Guid id)
        {
            var tag = await tagRepository.GetAsync(id);

            if (tag != null)
            {
                var editTagRequest = new EditTagRequest
                {
                    Id = tag.Id.ToString(),
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
            if (string.IsNullOrEmpty(editTagRequest.Id))
            {
                var tag = new Tag
                {
                    Name = editTagRequest.Name,
                    DisplayName = editTagRequest.DisplayName,
                };

                var result = await tagRepository.AddAsync(tag);

                if (tag == null) return BadRequest();

                return Ok(
                    new JsonResult(
                        new { title = "Success", message = "Tag has been added." }
                    ));
            }
            else
            {

                var tag = await tagRepository.GetAsync(Guid.Parse(editTagRequest.Id));

                if (tag == null) return NotFound();

                tag.Name = editTagRequest.Name;
                tag.DisplayName = editTagRequest.DisplayName;

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
            }


            return Ok(
                new JsonResult(
                    new { title = "Error", message = "Something went wrong!." }
                )
            );
            //return RedirectToAction("Edit", new { id = editTagRequest.Id });
        }

        [HttpDelete("delete-tag/{id}")]
        public async Task<IActionResult> DeleteTag(string id)
        {
            var tag = await tagRepository.DeleteAsync(Guid.Parse(id));

            if (tag != null)
            {
                return Ok(new JsonResult(new { title = "Success", message = "Tag has been deleted" }));
            }

            return BadRequest(SD.TagNotFound);
        }
    }
}
