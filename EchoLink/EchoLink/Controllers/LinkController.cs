using System.Web.Http;

namespace EchoLink.Controllers
{
    using Slight.Alexa.Framework.Models.Requests;
    using Slight.Alexa.Framework.Models.Requests.RequestTypes;
    using Slight.Alexa.Framework.Models.Responses;
    using Slight.Alexa.Framework.Models.Types;

    [RoutePrefix("")]
    public class LinkController : ApiController
    {
        [Route(""), HttpPost]
        public IHttpActionResult IntentRequest(SkillRequest request)
        {
            var requestType = request.GetRequestType();

            if (requestType == typeof(IIntentRequest))
            {
                return IntentRequest(request.Request);
            }
            else if (requestType == typeof(ILaunchRequest))
            {
                return LaunchRequest(request.Request);
            }
            else if (requestType == typeof(ISessionEndedRequest))
            {
                return SessionEndedRequest(request.Request);
            }

            return BadRequest();
        }

        private IHttpActionResult IntentRequest(IIntentRequest intentRequest)
        {
            var skillResponse = new SkillResponse
            {
                Version = "1.0",
                Response = new Response
                {
                    OutputSpeech = new PlainTextOutputSpeech
                    {
                        Text = $"This was a Intent Request for {intentRequest.Intent.Name}."
                    },
                    ShouldEndSession = intentRequest.Intent.Name == BuiltInIntent.Stop || intentRequest.Intent.Name == BuiltInIntent.Cancel
                }
            };

            return Ok(skillResponse);
        }

        private IHttpActionResult LaunchRequest(ILaunchRequest intentRequest)
        {
            var skillResponse = new SkillResponse
            {
                Version = "1.0",
                Response = new Response
                {
                    OutputSpeech = new PlainTextOutputSpeech
                    {
                        Text = "This was a launch Request. Ready for input."
                    },
                    ShouldEndSession = false
                }
            };

            return Ok(skillResponse);
        }

        private IHttpActionResult SessionEndedRequest(ISessionEndedRequest intentRequest)
        {
            var skillResponse = new SkillResponse
            {
                Version = "1.0",
                Response = new Response
                {
                    OutputSpeech = new PlainTextOutputSpeech
                    {
                        Text = "This was a session ended Request."
                    },
                    ShouldEndSession = true
                }
            };

            return Ok(skillResponse);
        }
    }
}
