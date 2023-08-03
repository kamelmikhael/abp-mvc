using Acme.BookStore.Books;
using Acme.BookStore.Books.Dtos;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System;
using System.Threading.Tasks;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Form;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.Linq;

namespace Acme.BookStore.Web.Pages.Books;

public class CreateModalModel : BookStorePageModel
{
    [BindProperty]
    public CreateBookViewModel Book { get; set; }

    public List<SelectListItem> Authors { get; set; }

    private readonly IBookAppService _bookAppService;

    public CreateModalModel(IBookAppService bookAppService)
    {
        _bookAppService = bookAppService;
    }

    public async Task OnGet()
    {
        Book = new CreateBookViewModel();

        var authorLookup = await _bookAppService.GetAuthorLookupAsync();
        Authors = authorLookup.Items
            .Select(x => new SelectListItem(x.Name, x.Id.ToString()))
            .ToList();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        await _bookAppService.CreateAsync(
            ObjectMapper.Map<CreateBookViewModel, CreateUpdateBookDto>(Book));
        return NoContent();
    }

    public class CreateBookViewModel
    {
        [SelectItems(nameof(Authors))]
        [DisplayName("Author")]
        public Guid AuthorId { get; set; }

        [Required]
        [StringLength(128)]
        public string Name { get; set; }

        [Required]
        public BookType Type { get; set; } = BookType.Undefined;

        [Required]
        [DataType(DataType.Date)]
        public DateTime PublishDate { get; set; } = DateTime.Now;

        [Required]
        public float Price { get; set; }
    }
}