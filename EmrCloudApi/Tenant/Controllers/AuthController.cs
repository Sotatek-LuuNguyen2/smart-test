﻿using DevExpress.XtraPrinting;
using DevExpress.XtraRichEdit;
using DevExpress.XtraRichEdit.Import;
using EmrCloudApi.Configs.Options;
using EmrCloudApi.Tenant.Constants;
using EmrCloudApi.Tenant.Requests.Auth;
using EmrCloudApi.Tenant.Responses;
using EmrCloudApi.Tenant.Responses.Auth;
using EmrReporting.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using System.Text;
using UseCase.Core.Sync;
using UseCase.User.GetByLoginId;

namespace EmrCloudApi.Tenant.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly UseCaseBus _bus;
    private readonly JwtOptions _jwtOptions;

    public AuthController(UseCaseBus bus, IOptions<JwtOptions> jwtOptionsAccessor)
    {
        _bus = bus;
        _jwtOptions = jwtOptionsAccessor.Value;
    }

    public IActionResult Index()
    {
        var service = new Karte2ReportingService();
        service.ExportToPdf();
        return Ok();
    }

    //public IActionResult Index()
    //{
    //    using var wordProcessor = new RichEditDocumentServer();
    //    wordProcessor.BeforeImport += (s, e) =>
    //    {
    //        if (e.DocumentFormat == DocumentFormat.Html)
    //        {
    //            var options = (HtmlDocumentImporterOptions)e.Options;
    //            // Specify encoding.
    //            options.AutoDetectEncoding = false;
    //            options.Encoding = Encoding.UTF8;
    //            // Skip media rules.
    //            options.IgnoreMediaQueries = true;
    //            // Load images synchronously with HTML documents.
    //            options.AsyncImageLoading = false;
    //            // Preserve image resolution.
    //            options.ImageScalingDpi = 96;
    //        }
    //    };
    //    using var client = new HttpClient();
    //    var html = client.GetStreamAsync("https://docs.devexpress.com/WindowsForms/9610/").Result;
    //    //wordProcessor.HtmlText = html;
    //    wordProcessor.LoadDocument(html, DocumentFormat.Html);
    //    var doc = wordProcessor.Document;
    //    //var html = Encoding.UTF8.GetBytes(@"<h1>Hello world</h1>");
    //    //wordProcessor.LoadDocument(html);
    //    //using var pdfStream = new MemoryStream();
    //    //wordProcessor.ExportToPdf(pdfStream);
    //    wordProcessor.ExportToPdf("Document_PDF.pdf");
    //    return Ok();
    //}

    [HttpPost("ExchangeToken"), Produces("application/json")]
    public ActionResult<Response<ExchangeTokenResponse>> ExchangeToken([FromBody] ExchangeTokenRequest req)
    {
        var getUserInput = new GetUserByLoginIdInputData(req.LoginId);
        var getUserOutput = _bus.Handle(getUserInput);
        var user = getUserOutput.User;
        if (user is null)
        {
            var errorResult = GetErrorResult("The loginId is invalid.");
            return BadRequest(errorResult);
        }

        if (req.Password != user.LoginPass)
        {
            var errorResult = GetErrorResult("The password is invalid.");
            return BadRequest(errorResult);
        }

        // The claims that will be persisted in the tokens.
        var claims = new Claim[]
        {
            new(ClaimTypes.NameIdentifier, user.UserId.ToString()),
            new(ClaimTypes.Name, user.LoginId),
            new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };
        var token = CreateToken(claims);
        var successResult = GetSuccessResult(token);
        return Ok(successResult);

        #region Helper methods

        Response<ExchangeTokenResponse> GetErrorResult(string errorMessage)
        {
            return new Response<ExchangeTokenResponse>
            {
                Data = new ExchangeTokenResponse(string.Empty),
                Status = 0,
                Message = errorMessage
            };
        }

        Response<ExchangeTokenResponse> GetSuccessResult(string token)
        {
            return new Response<ExchangeTokenResponse>
            {
                Data = new ExchangeTokenResponse(token),
                Status = 1,
                Message = ResponseMessage.Success
            };
        }

        #endregion
    }

    private string CreateToken(IEnumerable<Claim> claims)
    {
        var key = Encoding.UTF8.GetBytes(_jwtOptions.Secret);
        var signingKey = new SymmetricSecurityKey(key);
        var token = new JwtSecurityToken(
            expires: DateTime.UtcNow.AddHours(_jwtOptions.TokenLifetime),
            claims: claims,
            signingCredentials: new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256)
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
