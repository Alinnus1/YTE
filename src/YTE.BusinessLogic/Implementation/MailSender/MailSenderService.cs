using MailKit.Net.Smtp;
using Microsoft.Extensions.Options;
using MimeKit;
using System;
using System.Linq;
using System.Threading.Tasks;
using YTE.BusinessLogic.Base;
using YTE.Common;

namespace YTE.BusinessLogic.Implementation.MailSender
{
    public class MailSenderService : BaseService
    {
        private readonly MailSenderConfig MailSenderConfig;
        public MailSenderService(ServiceDependencies serviceDependencies, IOptions<MailSenderConfig> options) : base(serviceDependencies)
        {
            this.MailSenderConfig = options.Value;
        }

        public async Task SendFollowNotification(Guid followerId, Guid followedId)
        {
            var follower = UnitOfWork.Users.Get()
                            .FirstOrDefault(u => u.Id == followerId);
            var followed = UnitOfWork.Users.Get()
                            .FirstOrDefault(u => u.Id == followedId);

            if (follower == null || followed == null)
            {
                //error handling.
                return;
            }
            if (!followed.WantsNotifications)
            {
                return;
            }
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress(MailSenderConfig.Displayname, MailSenderConfig.Sender));

            message.To.Add(MailboxAddress.Parse(followed.Email));

            message.Subject = "You've got a new follower!";
            message.Body = new TextPart("html")
            {
                Text = $"Hello {followed.UserName}. We are happy to tell you that {follower.UserName} started following you! If you want to stop receiving notifications just click <a href='https://localhost:44383/UserAccount/Edit'>here</a> and edit your account settings."
            };
            using (var sender = new SmtpClient())
            {
                await sender.ConnectAsync(MailSenderConfig.Host, MailSenderConfig.Port, MailSenderConfig.EnableSsl ? MailKit.Security.SecureSocketOptions.StartTls : MailKit.Security.SecureSocketOptions.None);
                await sender.AuthenticateAsync(MailSenderConfig.Sender, MailSenderConfig.Password);
                await sender.SendAsync(message);
                await sender.DisconnectAsync(quit: true);
            }
        }

        public async Task SendUnFollowNotification(Guid followerId, Guid followedId)
        {
            var follower = UnitOfWork.Users.Get()
                            .FirstOrDefault(u => u.Id == followerId);
            var followed = UnitOfWork.Users.Get()
                            .FirstOrDefault(u => u.Id == followedId);

            if (follower == null || followed == null)
            {
                //error handling.
                return;
            }
            if (!followed.WantsNotifications)
            {
                return;
            }
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress(MailSenderConfig.Displayname, MailSenderConfig.Sender));

            message.To.Add(MailboxAddress.Parse(followed.Email));

            message.Subject = "You've lost a follower!";
            message.Body = new TextPart("html")
            {
                Text = $"Hello {followed.UserName}. We are sad to tell you that {follower.UserName} stopped following you! If you want to stop receiving notifications just click <a href='https://localhost:44383/UserAccount/Edit'>here</a> and edit your account settings."
            };
            using (var sender = new SmtpClient())
            {
                await sender.ConnectAsync(MailSenderConfig.Host, MailSenderConfig.Port, MailSenderConfig.EnableSsl ? MailKit.Security.SecureSocketOptions.StartTls : MailKit.Security.SecureSocketOptions.None);
                await sender.AuthenticateAsync(MailSenderConfig.Sender, MailSenderConfig.Password);
                await sender.SendAsync(message);
                await sender.DisconnectAsync(quit: true);
            }
        }

        public async Task SendConfirmationEmail(Guid userId, string email, string tokenId)
        {
            if (tokenId == null)
            {
                return;
            }
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress(MailSenderConfig.Displayname, MailSenderConfig.Sender));

            message.To.Add(MailboxAddress.Parse(email));

            message.Subject = "Confirmation for your email!";
            message.Body = new TextPart("html")
            {
                Text = $"Hello there. Press <a href='https://localhost:44383/UserAccount/ConfirmationEmail/{userId}/{tokenId}'>here</a> to confirm your account. This link is available for 24 hours."
            };
            using (var sender = new SmtpClient())
            {
                await sender.ConnectAsync(MailSenderConfig.Host, MailSenderConfig.Port, MailSenderConfig.EnableSsl ? MailKit.Security.SecureSocketOptions.StartTls : MailKit.Security.SecureSocketOptions.None);
                await sender.AuthenticateAsync(MailSenderConfig.Sender, MailSenderConfig.Password);
                await sender.SendAsync(message);
                await sender.DisconnectAsync(quit: true);
            }
        }

        public async Task SendForgotPassEmail(Guid userId, string email, string tokenId)
        {
            if (tokenId == null)
            {
                return;
            }
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress(MailSenderConfig.Displayname, MailSenderConfig.Sender));

            message.To.Add(MailboxAddress.Parse(email));

            message.Subject = "Update your account!";
            message.Body = new TextPart("html")
            {
                Text = $"Hello there. Press <a href='https://localhost:44383/UserAccount/ResetPassword/{userId}/{tokenId}'>here</a> to change your account's password. This link is available for 24 hours. This mail is automatically generated!"
            };
            using (var sender = new SmtpClient())
            {
                await sender.ConnectAsync(MailSenderConfig.Host, MailSenderConfig.Port, MailSenderConfig.EnableSsl ? MailKit.Security.SecureSocketOptions.StartTls : MailKit.Security.SecureSocketOptions.None);
                await sender.AuthenticateAsync(MailSenderConfig.Sender, MailSenderConfig.Password);
                await sender.SendAsync(message);
                await sender.DisconnectAsync(quit: true);
            }
        }

        public async Task SendFollowingLogicEmail(string email, string text)
        {
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress(MailSenderConfig.Displayname, MailSenderConfig.Sender));
            message.To.Add(MailboxAddress.Parse(email));
            message.Subject = "Your followees's reviews are here!";
            message.Body = new TextPart("html")
            {
                Text = text
            };

            using (var sender = new SmtpClient())
            {
                await sender.ConnectAsync(MailSenderConfig.Host, MailSenderConfig.Port, MailSenderConfig.EnableSsl ? MailKit.Security.SecureSocketOptions.StartTls : MailKit.Security.SecureSocketOptions.None);
                await sender.AuthenticateAsync(MailSenderConfig.Sender, MailSenderConfig.Password);
                await sender.SendAsync(message);
                await sender.DisconnectAsync(quit: true);
            }
        }
    }
}
