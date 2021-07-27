using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Microsoft.AspNet.Identity.Owin;
using System.Net.Http;
using System.Web.Http;
using University.API.Models;
using System.Threading.Tasks;

namespace University.API.Controllers
{
    [RoutePrefix("api/AccountApi")]
    public class AccountApiController : ApiController
    {
        private ApplicationSignInManager _signInManager;
        private ApplicationUserManager _userManager;

        public ApplicationSignInManager SignInManager
        {
            get
            {
                return _signInManager ?? Request.GetOwinContext().Get<ApplicationSignInManager>();
            }
            private set
            {
                _signInManager = value;
            }
        }

        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? Request.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }

        }
        /// <summary>
        /// Metodo para autenticar usuarios
        /// </summary>
        /// <param name="model">Objeto de autenticacion </param>
        /// <returns></returns>
        /// <response code = "200">ok. devuelve el objeto solicitado </response>
        /// <response code = "400">badrequest. no se cumple la validacion del modelo </response>
        /// <response code = "401">unauthorized. no se encuentra autorizado </response>
        /// <response code = "500">internalserver Error. Se ha presentado un error </response>
        [HttpPost]
        [Route("Login")]
        public async Task<IHttpActionResult> Login(LoginViewModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);
                var result = await SignInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, shouldLockout: false);
                switch (result)
                {
                    case SignInStatus.Success:
                        var user = await UserManager.FindByNameAsync(model.Email);
                        return Ok(user);                                    
                    case SignInStatus.Failure:
                    default:                       
                        return Unauthorized();
                }
                
            }
            catch (Exception ex )
            {

                return InternalServerError(ex);
            }
            
        }
        /// <summary>
        /// Metodo para registrar usuarios
        /// </summary>
        /// <param name="model">Objeto del registro</param>
        /// <returns></returns>
        /// <response code = "200">ok. devuelve el objeto solicitado </response>
        /// <response code = "400">badrequest. no se cumple la validacion del modelo </response>
        /// <response code = "500">internalserver Error. Se ha presentado un error </response>
        [HttpPost]
        [Route("Register")]
        public async Task<IHttpActionResult> Register(RegisterViewModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);
                var user = new ApplicationUser { UserName = model.Email, Email = model.Email };
                var result = await UserManager.CreateAsync(user, model.Password);
                if (result.Succeeded)            

                    return Ok();
                
                else
                    return InternalServerError();

            }
            catch (Exception ex)
            {

                return InternalServerError(ex);
            }

        }



    }
}
