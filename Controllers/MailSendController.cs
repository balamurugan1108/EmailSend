using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MimeKit;
using System.Net.Mail;
using MailKit.Net.Smtp;
using SmtpClient = MailKit.Net.Smtp.SmtpClient;
using EmailSendWeb.Mailservice;
using System;
using EmailSendWeb.Model;
using MailKit;

namespace EmailSendWeb.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MailSendController : ControllerBase
    {
        private readonly IEmailService _emailService;
        public MailSendController(IEmailService emailService)
        {
            _emailService = emailService;
        }

        [HttpPost("EmailSend")]
        public async Task<IActionResult> MailSend([FromForm] EmailInfo source)
        {
            try
            {
                await _emailService.SendEmailAsync(source);
                return Ok("successfully Send");

            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }
    }
}
