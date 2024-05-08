// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
#nullable disable

using System;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using EFood.AccesoDatos.Data;
using EFood.Modelos;
using EFood.Utilidades;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;

namespace E_Food.Areas.Identity.Pages.Account
{
    public class ForgotPasswordQuestionModel : PageModel
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly ApplicationDbContext _db;

        public ForgotPasswordQuestionModel(UserManager<IdentityUser> userManager, ApplicationDbContext db)
        {
            _userManager = userManager;
            _db = db;
        }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        [BindProperty]
        public InputModel Input { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public class InputModel
        {
            /// <summary>
            ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
            ///     directly from your code. This API may change or be removed in future releases.
            /// </summary>
            [Required]
            [EmailAddress]
            public string Email { get; set; }

            [Required]
            [StringLength(45, ErrorMessage = "La {0} debe ser de máximo 45 carácteres.")]
            public string RespuestaSeguridad { get; set; }

            [Required]
            [StringLength(45, ErrorMessage = "La {0} debe ser de máximo 45 carácteres.")]
            public string PreguntaSeguridad { get; set; }
        }

        public IActionResult OnGet(string email = null)
        {
            if (email == null)
            {
                return BadRequest("Se debe ingresar un email para cambiar contraseña.");
            }
            else
            {
                Usuario usuario = _db.Usuarios.FirstOrDefault(u => u.Email == email);
                if (usuario != null)
                {
                    Input = new InputModel
                    {
                        Email = email,
                        PreguntaSeguridad = usuario.PreguntaSolicitud
                };
                    return Page();
                }
                else
                {
                    return BadRequest("Email no encontrado.");
                }
            }
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(Input.Email);
                var respuestaUsuario = _db.Usuarios.FirstOrDefault(u => u.Email == Input.Email).RespuestaSeguridad;
                if (user == null || (!(await _userManager.IsEmailConfirmedAsync(user)) && Input.RespuestaSeguridad == respuestaUsuario))
                {
                    string resetUrl = Url.Page("./ResetPassword", pageHandler: null, values: new { Input.Email, Opcion = "olvido" }, protocol: Request.Scheme);
                    return Redirect(resetUrl);
                } else
                {
                    TempData[DS.Error] = "Respuesta de seguridad incorrecta.";
                    return Page();
                }
            }

            return Page();
        }
    }
}
