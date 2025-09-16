using AutoMapper;
using JuanApp.Application.Models.CategoryDtos;
using JuanApp.Application.Models.ProductDtos;
using JuanApp.Application.Repositories;
using JuanApp.Application.Services.Interfaces;
using JuanApp.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace JuanApp.Application.Services.Concretes
{
    public class ProductService(
        IGenericRepository<Product> productRepository,
        IGenericRepository<ProductImage> productImageRepository,
        IGenericRepository<ProductColor> productColorRepository,
        IGenericRepository<ProductSize> productSizeRepository,
        IGenericRepository<ProductTag> productTagRepository,
        IMapper mapper) : IProductService
    {
        public async Task<bool> CreateAsync(CreateProductDto product)
        {
            var productEntity = mapper.Map<Product>(product);
            await productRepository.AddAsync(productEntity);
            var result = await productRepository.SaveAsync();
            
            if (result > 0)
            {
                // Add additional images
                foreach (var imageUrl in product.AdditionalImageUrls)
                {
                    var productImage = new ProductImage
                    {
                        ProductId = productEntity.Id,
                        ImageUrl = imageUrl
                    };
                    await productImageRepository.AddAsync(productImage);
                }
                
                // Add colors
                foreach (var colorId in product.SelectedColorIds)
                {
                    var productColor = new ProductColor
                    {
                        ProductId = productEntity.Id,
                        ColorId = colorId
                    };
                    await productColorRepository.AddAsync(productColor);
                }
                
                // Add sizes
                foreach (var sizeId in product.SelectedSizeIds)
                {
                    var productSize = new ProductSize
                    {
                        ProductId = productEntity.Id,
                        SizeId = sizeId
                    };
                    await productSizeRepository.AddAsync(productSize);
                }
                
                // Add tags
                foreach (var tagId in product.SelectedTagIds)
                {
                    var productTag = new ProductTag
                    {
                        ProductId = productEntity.Id,
                        TagId = tagId
                    };
                    await productTagRepository.AddAsync(productTag);
                }
                
                return await productRepository.SaveAsync() > 0;
            }
            
            return false;
        }

        public async Task DeleteAsync(int id)
        {
            var product = await productRepository.GetByIdAsync(id);
            if (product != null)
            {
                productRepository.Remove(product);
                await productRepository.SaveAsync();
            }
        }

        public async Task<IEnumerable<ProductDto>> GetAllAsync()
        {
            var products = productRepository.GetAll()
                .Include(p => p.Category)
                .Include(p => p.ProductImages.Where(pi => !pi.IsDeleted))
                .Include(p => p.ProductColors).ThenInclude(pc => pc.Color)
                .Include(p => p.ProductSizes).ThenInclude(ps => ps.Size)
                .Include(p => p.ProductTags).ThenInclude(pt => pt.Tag)
                .Where(p => !p.IsDeleted)
                .OrderByDescending(p => p.Created);
                
            var productList = await products.ToListAsync();
            return productList.Select(MapProductToDto);
        }

        public async Task<ProductDto?> GetByIdAsync(int id)
        {
            var product = await productRepository.GetAll()
                .Include(p => p.Category)
                .Include(p => p.ProductImages.Where(pi => !pi.IsDeleted))
                .Include(p => p.ProductColors).ThenInclude(pc => pc.Color)
                .Include(p => p.ProductSizes).ThenInclude(ps => ps.Size)
                .Include(p => p.ProductTags).ThenInclude(pt => pt.Tag)
                .FirstOrDefaultAsync(p => p.Id == id && !p.IsDeleted);
                
            return product != null ? MapProductToDto(product) : null;
        }

        public async Task<IEnumerable<ProductDto>> GetByCategoryAsync(int categoryId)
        {
            var products = productRepository.GetAll()
                .Include(p => p.Category)
                .Include(p => p.ProductImages.Where(pi => !pi.IsDeleted))
                .Where(p => !p.IsDeleted && p.CategoryId == categoryId)
                .OrderByDescending(p => p.Created);
                
            var productList = await products.ToListAsync();
            return productList.Select(MapProductToDto);
        }

        public async Task<IEnumerable<ProductDto>> GetByGenderAsync(string gender)
        {
            var products = productRepository.GetAll()
                .Include(p => p.Category)
                .Include(p => p.ProductImages.Where(pi => !pi.IsDeleted))
                .Where(p => !p.IsDeleted && p.Gender == gender)
                .OrderByDescending(p => p.Created);
                
            var productList = await products.ToListAsync();
            return productList.Select(MapProductToDto);
        }

        public async Task<IEnumerable<ProductDto>> SearchAsync(string searchTerm)
        {
            var products = productRepository.GetAll()
                .Include(p => p.Category)
                .Include(p => p.ProductImages.Where(pi => !pi.IsDeleted))
                .Where(p => !p.IsDeleted && 
                           (p.Name.Contains(searchTerm) || 
                            p.Description.Contains(searchTerm)))
                .OrderByDescending(p => p.Created);
                
            var productList = await products.ToListAsync();
            return productList.Select(MapProductToDto);
        }

        public async Task UpdateAsync(ProductDto product)
        {
            var existingProduct = await productRepository.GetAll()
                .Include(p => p.ProductImages.Where(pi => !pi.IsDeleted))
                .Include(p => p.ProductColors)
                .Include(p => p.ProductSizes)
                .Include(p => p.ProductTags)
                .FirstOrDefaultAsync(p => p.Id == product.Id);
                
            if (existingProduct != null)
            {
                // Update basic properties
                existingProduct.Name = product.Name;
                existingProduct.Price = product.Price;
                existingProduct.Discount = product.Discount;
                existingProduct.Description = product.Description;
                existingProduct.Stock = product.Stock;
                existingProduct.Gender = product.Gender;
                existingProduct.MainImageUrl = product.MainImageUrl;
                existingProduct.CategoryId = product.CategoryId;
                
                // Update colors
                var existingColors = existingProduct.ProductColors.ToList();
                foreach (var color in existingColors)
                {
                    productColorRepository.Remove(color);
                }
                
                foreach (var colorId in product.SelectedColorIds)
                {
                    var productColor = new ProductColor
                    {
                        ProductId = product.Id,
                        ColorId = colorId
                    };
                    await productColorRepository.AddAsync(productColor);
                }
                
                // Update sizes
                var existingSizes = existingProduct.ProductSizes.ToList();
                foreach (var size in existingSizes)
                {
                    productSizeRepository.Remove(size);
                }
                
                foreach (var sizeId in product.SelectedSizeIds)
                {
                    var productSize = new ProductSize
                    {
                        ProductId = product.Id,
                        SizeId = sizeId
                    };
                    await productSizeRepository.AddAsync(productSize);
                }
                
                // Update tags
                var existingTags = existingProduct.ProductTags.ToList();
                foreach (var tag in existingTags)
                {
                    productTagRepository.Remove(tag);
                }
                
                foreach (var tagId in product.SelectedTagIds)
                {
                    var productTag = new ProductTag
                    {
                        ProductId = product.Id,
                        TagId = tagId
                    };
                    await productTagRepository.AddAsync(productTag);
                }
                
                productRepository.Update(existingProduct);
                await productRepository.SaveAsync();
            }
        }

        public async Task<bool> AddProductImageAsync(int productId, string imageUrl)
        {
            var productImage = new ProductImage
            {
                ProductId = productId,
                ImageUrl = imageUrl
            };
            
            await productImageRepository.AddAsync(productImage);
            return await productImageRepository.SaveAsync() > 0;
        }

        public async Task<bool> RemoveProductImageAsync(int productImageId)
        {
            var productImage = await productImageRepository.GetByIdAsync(productImageId);
            if (productImage != null)
            {
                productImageRepository.Remove(productImage);
                return await productImageRepository.SaveAsync() > 0;
            }
            return false;
        }

        private ProductDto MapProductToDto(Product product)
        {
            return new ProductDto
            {
                Id = product.Id,
                Name = product.Name,
                Price = product.Price,
                Discount = product.Discount,
                Description = product.Description,
                Stock = product.Stock,
                Gender = product.Gender,
                MainImageUrl = product.MainImageUrl,
                CategoryId = product.CategoryId,
                CategoryName = product.Category?.Name ?? "",
                Created = product.Created,
                LastModified = product.LastModified,
                ProductImages = product.ProductImages?.Select(pi => new ProductImageDto
                {
                    Id = pi.Id,
                    ImageUrl = pi.ImageUrl,
                    ProductId = pi.ProductId
                }).ToList() ?? new List<ProductImageDto>(),
                SelectedColorIds = product.ProductColors?.Select(pc => pc.ColorId).ToList() ?? new List<int>(),
                SelectedSizeIds = product.ProductSizes?.Select(ps => ps.SizeId).ToList() ?? new List<int>(),
                SelectedTagIds = product.ProductTags?.Select(pt => pt.TagId).ToList() ?? new List<int>()
            };
        }
    }
}