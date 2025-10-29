using MailKit.Security;
using MimeKit;
using MimeKit.Text;
using MailKit.Net.Smtp;
using UserManagement.Application.Interfaces;

namespace UserManagement.Application.Services
{
    public class EmailService : IEmailService
    {
        private readonly IConfiguration _configuration;


        public EmailService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task SendEmailAsync(string toEmail, string subject, string message)
        {
            var email = new MimeMessage();
            email.From.Add(MailboxAddress.Parse(_configuration["Smtp:From"]));
            email.To.Add(MailboxAddress.Parse(toEmail));
            email.Subject = subject;
            email.Body = new TextPart(TextFormat.Html) { Text = message };

            using var smtp = new SmtpClient();

            await smtp.ConnectAsync(
                _configuration["Smtp:Host"],
                int.Parse(_configuration["Smtp:Port"]),
                SecureSocketOptions.StartTls
            );

            await smtp.AuthenticateAsync( //username, password);
            _configuration["Smtp:Username"],
            _configuration["Smtp:Password"]
            );

            await smtp.SendAsync(email);
            await smtp.DisconnectAsync(true);
        }


        public async Task SendEmailsAsync(List<string> toEmails, string subject, string message)
        {
            var email = new MimeMessage();
            email.From.Add(MailboxAddress.Parse(_configuration["Smtp:From"]));

            foreach (var toEmail in toEmails)
            {
                email.Bcc.Add(MailboxAddress.Parse(toEmail));
            }

            email.Subject = subject;
            email.Body = new TextPart(TextFormat.Html) { Text = message };

            using var smtp = new SmtpClient();

            await smtp.ConnectAsync(
                _configuration["Smtp:Host"],
                int.Parse(_configuration["Smtp:Port"]),
                SecureSocketOptions.StartTls
            );

            await smtp.AuthenticateAsync(
            _configuration["Smtp:Username"],
            _configuration["Smtp:Password"]
            );

            await smtp.SendAsync(email);
            await smtp.DisconnectAsync(true);

            await Task.Delay(1000);
        }


        public async Task SendProfileCompletionEmail(string toEmail, string firstName)
        {
            var subject = "Profile Completed — Welcome aboard!";

            var message = $@"
            <html>
              <body style='margin:0; padding:0; font-family: Arial, sans-serif; background-color:#e8f3fa;  border-radius:8px;'>
                <table align='center' width='600' cellpadding='0' cellspacing='0' style='background-color:#dff1f5; border-radius:8px; box-shadow:0 2px 8px rgba(0,0,0,0.05);'>
          
                  <!-- Body -->
                  <tr>
                    <td style='padding:30px; color:#333333;'>
                      <h3 style='color:#8badc4; margin-top:0;'>Welcome, {firstName}!</h3>

                      <p style='font-size:16px; line-height:1.6;'>
                        Your details have been successfully saved and now part of a company that values growth, collaboration, and excellence.
                      </p>

                      <p style='font-size:15px; line-height:1.6; margin:0 0 12px 0;'>
                        You have full access to your dashboard and all employee resources. If anything needs updating later, you can edit your profile from the dashboard at any time.
                      </p>

                      <p style='font-size:16px; line-height:1.6;'>
                        We’re here to support you every step of the way.
                      </p>

                  <!-- Footer --> 
                  <tr>
                    <td align='center' style='padding:20px; background-color:#c4c9cc; color:#777777; font-size:12px; border-bottom-left-radius:8px; border-bottom-right-radius:8px;'>
                      <p style='margin:0;'>© {DateTime.UtcNow.Year} DEB fit'n'mill. All rights reserved.</p>
                      <p style='margin:0;'>17 Business Street, Oslo, Norway</p>
                    </td>
                  </tr>  
                    
                </table>
              </body>
            </html>";

            await SendEmailAsync(toEmail, subject, message);
        }

        public async Task SendUserRegistrationEmail(string toEmail)
        {
            var subject = "Welcome to DEB fit'n'mill!";
            var message = $@"
             <html>
                <body style='margin:0; padding:0; font-family:Arial, sans-serif; background-color:#e8f3fa;'>
                   <table align='center' width='600' cellpadding='0' cellspacing='0' style='background-color:#ffffff; border-radius:8px; box-shadow:0 2px 8px rgba(0,0,0,0.08); margin-top:40px;'>
          
                        <!-- Header -->
                        <tr>
                          <td align='center' style='background-color:#8badc4; padding:20px 0; border-top-left-radius:8px; border-top-right-radius:8px;'>
                            <h1 style='color:#ffffff; font-size:22px; margin:0;'>Welcome to DEB fit'n'mill!</h1>
                          </td>
                        </tr>

                        <!-- Body -->
                        <tr>
                          <td style='padding:30px; color:#333333; background-color:#f9fdff;'>
                            <p style='font-size:16px; line-height:1.6; margin-top:0;'>
                              Thanks for registering with us at <b>DEB fit'n'mill</b>!
                            </p>
                            <p style='font-size:16px; line-height:1.6;'>
                              To complete your registration, please go to your dashboard and navigate to the 
                              <b>Create Employee</b> page to fill in your employee details.
                            </p>
                            <p style='font-size:16px; line-height:1.6;'>
                             We’re thrilled to have you on board and can’t wait to see you grow with us.
                            </p>
                            <p style='font-size:16px; line-height:1.6; margin-bottom:30px;'>
                              If you have any issues getting started, feel free to reach out to our support team.
                            </p>
                            <a href='#' style='display:inline-block; background-color:#8badc4; color:#ffffff; text-decoration:none; padding:12px 24px; border-radius:4px; font-size:15px;'>
                                Complete Your Profile
                            </a>
                          </td>
                        </tr>

                        <!-- Footer --> 
                        <tr>
                          <td align='center' style='padding:20px; background-color:#c4c9cc; color:#555555; font-size:12px; border-bottom-left-radius:8px; border-bottom-right-radius:8px;'>
                            <p style='margin:0;'>© {DateTime.UtcNow.Year} DEB fit'n'mill. All rights reserved.</p>
                            <p style='margin:0;'>17 Business Street, Oslo, Norway</p>
                         </td>
                       </tr>  
          
                    </table>
                </body>
            </html>";
            await SendEmailAsync(toEmail, subject, message);
        }
    }
}

