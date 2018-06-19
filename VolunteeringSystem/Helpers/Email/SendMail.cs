namespace VolunteeringSystem.Helpers.Email
{
    public static class SendMail
    {
        private const string ApprovedVolunteerEmailBody = @"Olá,
A sua conta no sistema de voluntariado de orfanatos foi aprovada!
Agradecemos o seu cadastro e aguardamos ansiosamente o envio de suas primeiras propostas.

Até breve,
Equipe do sistema de voluntariado de orfanatos.";

        private const string BlockedVolunteerEmailBody = @"Olá,
A sua conta no sistema de voluntariados não foi aprovada e está agora bloqueada.
Quaisquer dúvidas você pode entrar em contato conosco através de volunteering.system@gmail.com.

Atenciosamente,
Equipe do sistema de voluntariado de orfanatos.";

        private const string UnderReviewVolunteerEmailBody = @"Olá,
Agradecemos o seu cadastro na nossa plataforma de ajuda para voluntários.
No momento a aprovação do seu cadastro está em andamento.
                            
Agradecemos a compreensão,
Equipe do sistema de voluntariado de orfanatos.";

        private const string ApprovedEventEmailBody = @"Olá,
O seu evento no sistema de voluntariado de orfanatos foi aprovado!
Agradecemos o seu evento e aguardamos sua presença.

Até breve,
Equipe do sistema de voluntariado de orfanatos.";

        private const string BlockedEventEmailBodyStart = @"Olá,
O seu evento no sistema de voluntariados não foi aprovado e está agora bloqueada e a justificativa é:
";

        private const string BlockedEventEmailBodyEnd = @"
Qualquer dúvida você pode entrar em contato conosco através de volunteering.system@gmail.com

Atenciosamente,
Equipe do sistema de voluntariado de orfanatos.";

        private const string UnderReviewEventEmailBody = @"Olá,
Agradecemos a criação do seu evento na nossa plataforma de ajuda para voluntários.
No momento a aprovação do seu evento está em andamento.
                            
Agradecemos a compreensão,
Equipe do sistema de voluntariado de orfanatos.";

        private static string email = "volunteering.system@gmail.com";
        private static string password = "volunteering.system";
        private static string host = "smtp.gmail.com";
        private static int port = 587;

        private static void SendEmail(string to, string body, string subject)
        {
            var client = GetClient();
            var mailMessage = new System.Net.Mail.MailMessage {From = new System.Net.Mail.MailAddress(email)};
            mailMessage.To.Add(to);
            mailMessage.Body = body;
            mailMessage.Subject = subject;
            client.Send(mailMessage);
        }

        public static void SendNewVolunteerStatus(string receiverEmail, int newStatus)
        {
            SendEmail(receiverEmail, GetVolunteerEmailBody(newStatus), GetVolunteerEmailSubject(newStatus));
        }

        public static void SendNewEventStatus(string receiverEmail, int newStatus, string justification)
        {
            SendEmail(receiverEmail, GetEventEmailBody(newStatus, justification), GetEventEmailSubject(newStatus));
        }

        private static System.Net.Mail.SmtpClient GetClient()
        {
            var client = new System.Net.Mail.SmtpClient
            {
                UseDefaultCredentials = false,
                Host = host,
                Port = port,
                EnableSsl = true,
                Credentials = new System.Net.NetworkCredential(email, password)
            };

            return client;
        }

        private static string GetVolunteerEmailBody(int status)
        {
            if (status == 1) return ApprovedVolunteerEmailBody;
            if (status == 2) return BlockedVolunteerEmailBody;
            return UnderReviewVolunteerEmailBody;
        }

        private static string GetVolunteerEmailSubject(int status)
        {
            if (status == 1) return "Aprovação confirmada";
            if (status == 2) return "Conta bloqueada";
            return "Aprovação em progresso";
        }

        private static string GetEventEmailBody(int status, string justification)
        {
            if (status == 1) return ApprovedEventEmailBody;
            if (status == 2) return BlockedEventEmailBodyStart + justification + BlockedEventEmailBodyEnd;
            return UnderReviewEventEmailBody;
        }

        private static string GetEventEmailSubject(int status)
        {
            if (status == 1) return "Evento aprovado";
            if (status == 2) return "Evento bloqueado";
            return "Evento em aprovação";
        }
    }
}