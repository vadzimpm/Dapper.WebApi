using System.Collections.Generic;
using System.Threading.Tasks;
using Dapper.Application.Interfaces;
using Dapper.Core.Entities;
using Dapper.WebApi.Helpers;
using Microsoft.AspNetCore.Mvc;

namespace Dapper.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public ProductController(IUnitOfWork unitOfWork)
        {
            this._unitOfWork = unitOfWork;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var data = await TryCatchAspectHelper.ExecuteActionAndLogError<IEnumerable<Product>>(async args => await _unitOfWork.Products.GetAllAsync());
            return data;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var data = await TryCatchAspectHelper.ExecuteActionAndLogError<Product>(async args => await _unitOfWork.Products.GetByIdAsync(id));

            if (data.Value == null) return NotFound("Product was not found");

            return data;
        }

        [HttpPost]
        public async Task<IActionResult> Add(Product product)
        {
            var data = await TryCatchAspectHelper.ExecuteActionAndLogError(async args => await _unitOfWork.Products.AddAsync(product));
            return data;
        }

        [HttpPut]
        public async Task<IActionResult> Update(Product product)
        {
            var data = await TryCatchAspectHelper.ExecuteActionAndLogError(async args => await _unitOfWork.Products.UpdateAsync(product));
            return data;
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(int id)
        {
            var data = await TryCatchAspectHelper.ExecuteActionAndLogError(async args => await _unitOfWork.Products.DeleteAsync(id));
            return data;
        }
    }
}