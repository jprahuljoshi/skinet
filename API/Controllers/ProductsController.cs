using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.DataTransferObjects;
using AutoMapper;
using Core.Entities;
using Core.Interfaces;
using Core.Specifications;
using Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController : ControllerBase
    {
        /* When we want to use a single respository */
        // private readonly IProductRepository _repo;
        // public ProductsController(IProductRepository repo)
        // {
        //     _repo = repo;
        // }

        /* When we want to use Generic respository */
        private readonly IGenericRepository<Product> _productsRepo;
        private readonly IGenericRepository<ProductBrand> _brandRepo;
        private readonly IGenericRepository<ProductType> _typeRepo;
        private readonly IMapper _mapper;

        public ProductsController(
            IGenericRepository<Product> productsRepo,
            IGenericRepository<ProductBrand> brandRepo,
            IGenericRepository<ProductType> typeRepo,
            IMapper mapper
        )
        {
            _productsRepo = productsRepo;
            _brandRepo = brandRepo;
            _typeRepo = typeRepo;
            _mapper = mapper;
        }

        [HttpGet]
        // public async Task<ActionResult<List<Product>>> getProducts()
        public async Task<ActionResult<IReadOnlyList<ProductToReturnDto>>> getProducts()
        {
            /* When we want to use a single respository */
            // var products = await _repo.GetProductsAsync();

            /* When we want to use Generic respository */
            // var products = await _productsRepo.ListAllAsync();

            /* When we want to use Specifications */
            var spec = new ProductWithTypesAndBrandsSpecification();
            var products = await _productsRepo.ListAsync(spec);

            return Ok(_mapper.Map<IReadOnlyList<Product>, IReadOnlyList<ProductToReturnDto>>(products));
        }

        [HttpGet("{id}")]
        // public async Task<ActionResult<Product>> getProduct(int id)
        public async Task<ActionResult<ProductToReturnDto>> getProduct(int id)
        {
            /* When we want to use a single respository */
            // return await _repo.GetProductByIdAsync(id);

            /* When we want to use Generic respository */
            // return await _productsRepo.GetByIdAsync(id);

            /* When we want to use Specifications */
            var spec = new ProductWithTypesAndBrandsSpecification(id);
            var product = await _productsRepo.GetEntityWithSpec(spec);
            return _mapper.Map<Product, ProductToReturnDto>(product);
        }

        [HttpGet("brands")]
        public async Task<ActionResult<IReadOnlyList<ProductBrand>>> getProductBrands()
        {
            // return Ok(await _repo.GetProductBrandsAsync());
            return Ok(await _brandRepo.ListAllAsync());
        }

        [HttpGet("types")]
        public async Task<ActionResult<IReadOnlyList<ProductType>>> getProductTypes()
        {
            // return Ok(await _repo.GetProductTypesAsync());
            return Ok(await _typeRepo.ListAllAsync());
        }

    }
}