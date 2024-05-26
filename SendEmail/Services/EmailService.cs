using SendEmail.Models;
using MailKit.Security;
using MimeKit.Text;
using MimeKit;
using MailKit.Net.Smtp;
using System.Text;

namespace SendEmail.Services
{
    public class EmailService : IEmailService
    {
        private readonly IConfiguration _config;
        public EmailService(IConfiguration config)
        {
            _config = config;
        }

        public void SenqEmail(EmailDTO request)
        {
            var email = new MimeMessage();
            email.From.Add(MailboxAddress.Parse(_config.GetSection("Email:UserName").Value));
            email.To.Add(MailboxAddress.Parse(request.Para));
            email.Subject = "Detalles de compra en MiPortal";

            //Cuerpo del correo:
            var bodyBuilder = new StringBuilder();
            bodyBuilder.AppendLine("<h1>Detalles de su compra:</h1>");
            bodyBuilder.AppendLine($"<p><strong>Dirección de envio: </strong> {request.DireccionEnvio}</p>");
            bodyBuilder.AppendLine("<h2>Productos:</h2>");
            bodyBuilder.AppendLine("<ul>");

            foreach (var producto in request.Productos)
            {
                bodyBuilder.AppendLine($"<li>{producto.Nombre} - Cantidad: {producto.Cantidad}" +
                    $", Precio: {producto.Precio:C}</li>");
            }
            bodyBuilder.AppendLine("</ul>");
            bodyBuilder.AppendLine($"<p><strong>Total del pago: </strong> {request.TotalPago:C}</p>");
            bodyBuilder.AppendLine($"<p><strong>Fecha del pedido: </strong> {request.FechaPedido:dd/MM/yyyy}</p>");

            email.Body = new TextPart(TextFormat.Html)
            {
                Text = bodyBuilder.ToString()
            };


            using var smtp = new SmtpClient();
            smtp.Connect(
                _config.GetSection("Email:Host").Value,
                Convert.ToInt32(_config.GetSection("Email:Port").Value),
                SecureSocketOptions.StartTls);

            smtp.Authenticate(_config.GetSection("Email:Username").Value,
                _config.GetSection("Email:PassWord").Value);

            smtp.Send(email);
            smtp.Disconnect(true);
        }
    }
}
