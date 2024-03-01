using ET.Objetos.Item;
using ET.Objetos;
using Microsoft.AspNetCore.Mvc;
namespace BASE_API.Controllers
{
    [Route("api/[controller]")]
    public class ItemController : ControllerBase
    {
        ContextItem contextItem=new ContextItem();
        [HttpGet("GetItems")]
        public async Task<IActionResult> Get()
        {
            try
            {
                var categories = contextItem.Select();
                return Ok(categories);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("Create")]
        public async Task<IActionResult> Create([FromBody] OITM oitm)
        {
            try
            {

                if (!ModelState.IsValid)
                    throw new Exception("Datos invalidos.");
                if (oitm == null)
                    throw new Exception("Los datos del modelo son necesarios para continuar.");

                object id = contextItem.Insert(oitm);

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
        public async Task<IActionResult> Update([FromBody] OITM oitm)
        {
            try
            {

                if (!ModelState.IsValid)
                    throw new Exception("Datos invalidos.");
                if (oitm == null)
                    throw new Exception("Los datos del modelo son necesarios para continuar.");

                object id = contextItem.Update(oitm, oitm.Modify_User);

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
