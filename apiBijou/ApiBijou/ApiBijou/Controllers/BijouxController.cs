using Microsoft.AspNetCore.Mvc;
using API_SAE.Model;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Cors;
using ApiBijou.Model.formModel;
using ApiBijou.Model.SurMesure;

namespace API_SAE.Controllers
{
    /// <summary>
    /// Controller de l'api
    /// </summary>
    [ApiController]
    [Route("Bijoux")]
    public class BijouxController : ControllerBase
    {
        public BijouxController() 
        {

        }

        /// <summary>
        /// V�rifie la validit� d'un ID et renvoie un bijou correspondant s'il existe.
        /// </summary>
        /// <param name="id">L'ID du bijou � v�rifier.</param>
        /// <returns>
        /// Un ActionResult contenant le bijou s'il existe, NotFound si l'ID est valide mais ne correspond � aucun bijou,
        /// ou BadRequest si aucun ID n'est sp�cifi�.
        /// </returns>
        [HttpGet("GetBijouWithId", Name = "Check")]
        public ActionResult<Bijou> Check(int? id)
        {
            ActionResult<Bijou> result = BadRequest("No id specified"); 

            if (id.HasValue)
            {
                result = NotFound(); 

                Bijou? user = BijouManager.Instance.GetBijouById(id.Value);

                if (user != null)
                {
                    result = Ok(user); // Si le bijou existe, change la r�ponse en Ok avec le bijou
                }
            }

            return result; 
        }

        /// <summary>
        /// R�cup�re la liste de tous les bijoux.
        /// </summary>
        /// <returns>
        /// Un ActionResult contenant la liste de tous les bijoux, 
        /// ou BadRequest si une erreur survient lors de la r�cup�ration.
        /// </returns>
        [HttpGet("GetAllBijoux")]
        public ActionResult<IEnumerable<Bijou>> GetAllBijoux()
        {
            ActionResult<IEnumerable<Bijou>> reponse = BadRequest(); 

            IEnumerable<Bijou> users = BijouManager.Instance.GetAllBijoux();

            if (users != null)
            {
                reponse = Ok(users); // Si la liste de bijoux est valide, change la r�ponse en Ok avec la liste
            }

            return reponse; 
        }


        [HttpPost("EnvoyerFormulaireSurMesure")]
        public IActionResult EnvoyerFormulaireSurMesure([FromBody] FormulaireSurMesureModel formulaire)
        {
            ActionResult result = BadRequest();
            if (ModelState.IsValid)
            {
                try
                {
                    MailBuilder mailBuilder = new MailBuilder(formulaire);
                    // Utilisez mailBuilder pour g�n�rer et envoyer le mail ici

                    return Ok("Formulaire soumis avec succ�s !");
                }
                catch (Exception ex)
                {
                    // G�rez l'exception ici, par exemple, en enregistrant les d�tails de l'erreur dans un journal.
                    return StatusCode(500, "Une erreur s'est produite lors de la tentative d'envoi de l'e-mail.");
                }
            }
            else
            {
                return BadRequest(ModelState);
            }
        }

    }
}