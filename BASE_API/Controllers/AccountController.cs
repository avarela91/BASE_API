using ET.Objetos;
using ET;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ET.Objetos.Authentication;
using ET.Objetos.Item;

namespace BASE_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        ContextPermiso contextPermiso = new ContextPermiso();
        ContextUser contextLoginModel = new ContextUser();
        ContextModule contextModule = new ContextModule();

        //[Route("LogginApp")]
        [HttpPost("LoginApp")]
        public async Task<IActionResult> Post([FromBody] LoginModel model)
        {
            try
            {

                if (!ModelState.IsValid)
                    throw new Exception("Datos invalidos.");
                if (model == null)
                    throw new Exception("Los datos del modelo son necesarios para continuar.");
                if (String.IsNullOrEmpty(model.UserName) || String.IsNullOrEmpty(model.Password) || String.IsNullOrEmpty(model.Modulo))
                    throw new Exception("El usuario, contraseña y modulo son obligatorios.");

                /*Dictionary<string, object> response = new Dictionary<string, object>();
                var jsonSerialiser = new JavaScriptSerializer();*/
                var user = contextLoginModel.Select(new Dictionary<string, string>() { { "UserName",model.UserName }, { "Password", model.Password } }).FirstOrDefault();
                if (user == null)
                {
                    throw new Exception("Nombre de usuario o contraseña incorrectos.");
                }
                List<PermisoUser> permission = contextPermiso.PermissionByUserAndModule(model.UserName, model.Modulo);
                /*if (permission == null)
                {
                    throw new Exception($"No tiene permisos para acceder a esta aplicacion: {model.Modulo}");
                }
                else
                {*/
                    var responseData = new ResponseDataLoginApp
                    {
                        status = "success",
                        msj = "Permisos del usuario obtenidos correctamente.",
                        CodeError=0,
                        permissions = permission
                    };
                    return Ok(responseData);
                //}
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ResponseDataLoginApp
                {
                    status = "error",
                    msj = $"Error: {ex.Message}",
                    CodeError=1,
                    permissions = null
                });
            }
        }

        [HttpGet("GetUsers")]
        public async Task<IActionResult> Get()
        {
            try
            {
                var users = contextLoginModel.Select();
                return Ok(users);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("CreateUser")]
        public async Task<IActionResult> CreateUser([FromBody] User user)
        {
            try
            {

                if (!ModelState.IsValid)
                    throw new Exception("Datos invalidos.");
                if (user == null)
                    throw new Exception("Los datos del modelo son necesarios para continuar.");

                object id = contextLoginModel.Insert(user);

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

        [HttpGet("GetModules")]
        public async Task<IActionResult> GetModules()
        {
            try
            {
                var modules = contextModule.Select();
                return Ok(modules);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }

   
}
