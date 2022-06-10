using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SnackBar.Services.ProductAPI.Model;
using SnackBar.Services.ProductAPI.Model.Dto;
using SnackBar.Services.ProductAPI.Repository;

namespace SnackBar.Services.ProductAPI.Controllers
{
    [Route("api/products")]
    public class ProductAPIController : ControllerBase
    {
        protected ResponseDto _response;

        private IProductRepository _repo;
        public ProductAPIController(IProductRepository repo)
        {
            _repo = repo;
            this._response = new ResponseDto();
        }

        //[Authorize]
        [HttpGet]
        [Route("/get-all-products")]
        public async Task<Object> GetAll()
        {
            try
            {
                IEnumerable<ProductDto> productsDtos =  await _repo.GetProducts();
                _response.Result = productsDtos;
                
            }catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Errors = new List<string>() { ex.ToString()};

            }
            return _response;
        }

        
       //[Authorize(Roles = "Admin")]
        [HttpGet]
        [Route("/get-product/{id}")]
        public async Task<Object> Get(int id)
        {
            try
            {
                ProductDto productsDto = await _repo.getProductById(id);
                _response.Result = productsDto;

            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Errors = new List<string>() { ex.ToString() };

            }
            return _response;
        }


        [Authorize]
        [HttpPost]
        [Route("/add-product")]
        [Authorize(Roles = "Admin")]
        public async Task<Object> BuildOn([FromBody] ProductDto productDto)
        {
            try
            {  
                ProductDto resultDto =  await _repo.AddOrUpdateProduct(productDto);
                _response.Result = resultDto;
            }catch (Exception ex)
            {
                _response.IsSuccess=false;
                _response.Errors =new List<string>() { ex.ToString() };
            }
            return _response;
        }


        [Authorize]
        [HttpPut]
        [Route("/modify-product")]
        public async Task<Object> ModifyOn([FromBody] ProductDto productDto)
        {
            try
            {
                ProductDto resultDto = await _repo.AddOrUpdateProduct(productDto);
                _response.Result = resultDto;
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Errors = new List<string>() { ex.ToString() };
            }
            return _response;
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete]
        [Route("/erase-product/{id}")]
        //https://localhost:7008/erase-product/5
        public async Task<Object> Erase(int id)
        {
            try
            {
                bool isErased = await _repo.DeleteProduct(id);
                _response.Result=isErased;
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Errors = new List<string>() { ex.ToString() };
            }
            return _response;
        }
    }
}
