using ApplicationCore.Entities;
using ApplicationCore.Interfaces;
using ApplicationCore.Specifications;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Web.Interfaces;
using Web.ViewModels;

namespace Web.Services
{
    public class HomeViewModelService : IHomeViewModelService
    {
        public IAsyncRepository<Product> _productRepository { get; }
        public IAsyncRepository<Category> _categoryRepository { get; }
        public IAsyncRepository<Author> _authorRepository { get; }

        public HomeViewModelService(IAsyncRepository<Product> productRepository, IAsyncRepository<Category> categoryRepository, IAsyncRepository<Author> authorRepository)
        {
            _productRepository = productRepository;
            _categoryRepository = categoryRepository;
            _authorRepository = authorRepository;
        }

        public async Task<HomeIndexViewModel> GetHomeIndexViewModel(int? categoryId, int? authorId, int page, int pageSize)
        {
            var spec = new ProductsWithAuthorSpecification(categoryId, authorId);
            var specPaginated = new ProductsWithAuthorSpecification(categoryId, authorId, (page - 1) * pageSize, pageSize);
            var totalItems = await _productRepository.CountAsync(spec);
            var totalPages = (int)Math.Ceiling((double)totalItems / pageSize);
            var products = await _productRepository.ListAsync(specPaginated);

            var vm = new HomeIndexViewModel()
            {

                Products = products.Select(x => new ProductViewModel()
                {
                    Id = x.Id,
                    Name = x.Name,
                    PictureUri = x.PictureUri,
                    Price = x.Price,
                    AuthorName = x.Author?.FullName
                }).ToList(),
                Authors = await GetAuthors(),
                Categories = await GetCategories(),
                PaginationInfo = new PaginationInfoViewModel()
                {
                    Page = page,
                    ItemsPerPage = products.Count,
                    TotalItems = totalItems,
                    TotalPages = totalPages,
                    HasPrev = page > 1,
                    HasNext = page < totalPages
                }
            };
            return vm;
        }
        public async Task<List<SelectListItem>> GetAuthors()
        {
            var authors = await _authorRepository.ListAllAsync();
            var items = authors.Select(x => new SelectListItem()
            {
                Text = x.FullName,
                Value = x.Id.ToString()
            }).OrderBy(x => x.Text).ToList();

            items.Insert(0, new SelectListItem() { Text = "All", Value = null });
            return items;

        }

        public async Task<List<SelectListItem>> GetCategories()
        {
            var categories = await _categoryRepository.ListAllAsync();
            var items = categories.Select(x => new SelectListItem()
            {
                Text = x.CategoryName,
                Value = x.Id.ToString()
            }).OrderBy(x => x.Text).ToList();

            items.Insert(0, new SelectListItem() { Text = "All", Value = null });
            return items;
        }

    }
}
