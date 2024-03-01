using Microsoft.AspNetCore.Http;
using ET;
using ET.Objetos.Item;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ET.Objetos;

namespace BASE_API.Controllers
{
    [Route("api/[controller]")]
    public class DepartmentController : ControllerBase
    {
        ContextDepartment contextDepartment=new ContextDepartment();
        [HttpGet("GetDepartments")]
        public async Task<IActionResult> Get()
        {
            try
            {
                var departments = contextDepartment.Select();
                return Ok(departments);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("Create")]
        public async Task<IActionResult> Create([FromBody] OTIB otib)
        {
            try
            {

                if (!ModelState.IsValid)
                    throw new Exception("Datos invalidos.");
                if (otib == null)
                    throw new Exception("Los datos del modelo son necesarios para continuar.");
                
                object id = contextDepartment.Insert(otib);

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
        public async Task<IActionResult> Update([FromBody] OTIB otib)
        {
            try
            {

                if (!ModelState.IsValid)
                    throw new Exception("Datos invalidos.");
                if (otib == null)
                    throw new Exception("Los datos del modelo son necesarios para continuar.");

                object id = contextDepartment.Update(otib,otib.Modify_User);

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
