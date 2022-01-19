using Microsoft.AspNetCore.ApiAuthorization.IdentityServer;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace Blog.Server.Controllers;
public class OidcConfigurationController : Controller
{
	private readonly ILogger<OidcConfigurationController> _logger;

	public OidcConfigurationController(IClientRequestParametersProvider clientRequestParametersProvider, ILogger<OidcConfigurationController> logger)
	{
		ClientRequestParametersProvider = clientRequestParametersProvider;
		_logger = logger;
	}

	public IClientRequestParametersProvider ClientRequestParametersProvider { get; }

	[HttpGet("_configuration/{clientId}")]
	public IActionResult GetClientRequestParameters([FromRoute] string clientId)
	{
		var parameters = ClientRequestParametersProvider.GetClientParameters(HttpContext, clientId);
		_logger.LogDebug("GetClientRequestParameters(...) - parameters: {params}", JsonSerializer.Serialize(parameters));
		return Ok(parameters);
	}
}
