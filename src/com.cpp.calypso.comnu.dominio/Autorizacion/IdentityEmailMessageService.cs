using Abp.Dependency;
using Abp.Net.Mail;
using Microsoft.AspNet.Identity;
using System.Net.Mail;
using System.Threading.Tasks;

namespace com.cpp.calypso.comun.dominio
{
    /// <summary>
    /// Enviar mensajes por correo electronico
    /// </summary>
    public class IdentityEmailMessageService : IIdentityMessageService, ITransientDependency
    {
        private readonly IEmailSender _emailSender;

        public IdentityEmailMessageService(IEmailSender emailSender)
        {
            _emailSender = emailSender;
        }

        public virtual async Task SendAsync(IdentityMessage message)
        {
             await _emailSender.SendAsync(message.Destination, message.Subject, message.Body,true);
        }
        public virtual async Task SendWithFilesAsync(MailMessage mail)
        {
            await _emailSender.SendAsync(mail, true);
        }

    }
}
