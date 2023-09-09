using GeekShopping.Web.Models;
using Microsoft.AspNetCore.DataProtection.KeyManagement.Internal;

namespace GeekShopping.Web.Services.IServices
{
    public interface IProductSefvice
    {
        Task<IEnumerable<ProductModel>> FindAllProducts();
        Task<ProductModel> FindById(long id);
        Task<ProductModel> CreateProduct(ProductModel model);
        Task<ProductModel> UpdateProduct(ProductModel model);
        Task<bool> DeleteProductById(long id);
    }
}
