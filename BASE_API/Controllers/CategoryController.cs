using ET.Objetos.Item;
using ET.Objetos;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ET;
using Microsoft.AspNetCore.Components.Forms;

namespace BASE_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        ContextCategory contextCategory=new ContextCategory();
        [HttpGet("GetCategories")]
        public async Task<IActionResult> Get()
        {
            try
            {
                var categories = contextCategory.Select();
                return Ok(categories);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("GetCategoriesIndex")]
        public async Task<IActionResult> GetCategoryIndex()
        {
            try
            {
                var categories = contextCategory.getCategoryIndex();
                return Ok(categories);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("GetCategoryByDepto")]
        public async Task<IActionResult> GetCategoryByDepto(int idDepto)
        {
            try
            {
                var categories = contextCategory.Select(new Dictionary<string, string>() { { "Id_OTIB",idDepto.ToString() } });
                return Ok(categories);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("Create")]
        public async Task<IActionResult> Create([FromBody] OITC oitc)
        {
            try
            {

                if (!ModelState.IsValid)
                    throw new Exception("Datos invalidos.");
                if (oitc == null)
                    throw new Exception("Los datos del modelo son necesarios para continuar.");

                object id = contextCategory.Insert(oitc);

                var responseData = new ResponseData
                {
                    status = "success",
                    msj = "Proceso realizado con exito.",
                    CodeError = 0
                };
                return Ok(responseData);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ResponseData
                {
                    status = "error",
                    msj = $"Error: {ex.Message}",
                    CodeError = 1
                });
            }
        }

        [HttpPost("Update")]
        public async Task<IActionResult> Update([FromBody] OITC oitc)
        {
            try
            {

                if (!ModelState.IsValid)
                    throw new Exception("Datos invalidos.");
                if (oitc == null)
                    throw new Exception("Los datos del modelo son necesarios para continuar.");

                object id = contextCategory.Update(oitc, oitc.Modify_User);

                var responseData = new ResponseData
                {
                    status = "success",
                    msj = "Proceso realizado con exito.",
                    CodeError = 0
                };
                return Ok(responseData);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ResponseData
                {
                    status = "error",
                    msj = $"Error: {ex.Message}",
                    CodeError = 1
                });
            }
        }
    }
}
