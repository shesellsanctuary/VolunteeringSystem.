using System;

namespace VolunteeringSystem.Helpers.Email
{
    public static class SendMail
    {
        private static string email = "volunteering.system@gmail.com";
        private static string password = "volunteering.system";
        private static string host = "smtp.gmail.com";
        private static int port = 587;

        public static void SendNewVolunteerStatus(string receiver_email, int new_status)
        {
            System.Net.Mail.SmtpClient client = getClient();

            System.Net.Mail.MailMessage mailMessage = new System.Net.Mail.MailMessage();
            mailMessage.From = new System.Net.Mail.MailAddress(email);
            mailMessage.To.Add(receiver_email);
            mailMessage.Body = getVolunteerEmailBody(new_status);
            mailMessage.Subject = getVolunteerEmailSubject(new_status);
            client.Send(mailMessage);
        }

        //TODO: remove duplicated code kappa
        public static void SendNewEventStatus(string receiver_email, int new_status, string justification)
        {
            System.Net.Mail.SmtpClient client = getClient();

            System.Net.Mail.MailMessage mailMessage = new System.Net.Mail.MailMessage();
            mailMessage.From = new System.Net.Mail.MailAddress(email);
            mailMessage.To.Add(receiver_email);
            mailMessage.Body = getEventEmailBody(new_status, justification);
            mailMessage.Subject = getEventEmailSubject(new_status);
            client.Send(mailMessage);
        }

        private static System.Net.Mail.SmtpClient getClient()
        {
            System.Net.Mail.SmtpClient client = new System.Net.Mail.SmtpClient();
            client.UseDefaultCredentials = false;
            client.Host = host;
            client.Port = port;
            client.EnableSsl = true;
            client.Credentials = new System.Net.NetworkCredential(email, password);

            return client;
        }

        private static string getVolunteerEmailBody(int status)
        {

            if (status == 1) return @"
                                Olá,
                                A sua conta no sistema de voluntariado de orfanatos foi aprovada !
                                Agradecemos o seu cadastro e aguardamos ansiosamente o envio de suas primeiras propostas.

                                Até breve,
                                Equipe do sistema de voluntariado de orfanatos.";

            if (status == 2) return @"
                                Olá,
                                A sua conta no sistema de voluntariados não foi aprovada e está agora bloqueada.
                                Qualquer dúvidas você pode entrar em contato com nós através de volunteering.system@gmail.com

                                Atenciosamente,
                                Equipe do sistema de voluntariado de orfanatos.";
            return @"
                    Olá,
                    Agradecemos o seu cadastro na nossa plataforma de ajuda para voluntários.
                    No momento a aprovação do seu cadastro está em andamento.
                            
                    Agradecemos a compreensão,
                    Equipe do sistema de voluntariado de orfanatos.";
        }

        private static string getVolunteerEmailSubject(int status)
        {
            if (status == 1) return "Aprovação confirmada";
            if (status == 2) return "Conta bloqueada";

            return "Aprovação em progresso";
        }

        private static string getEventEmailBody(int status, string justification)
        {

            if (status == 1) return @"
                                Olá,
                                O seu evento no sistema de voluntariado de orfanatos foi aprovada !
                                Agradecemos o seu evento e aguardamos sua presença.

                                Até breve,
                                Equipe do sistema de voluntariado de orfanatos.";

            if (status == 2) return @"
                                Olá,
                                O seu evento no sistema de voluntariados não foi aprovado e está agora bloqueada e a justificativa é: 
                                " +
                                justification +
                                @"

                                Qualquer dúvidas você pode entrar em contato com nós através de volunteering.system@gmail.com

                                Atenciosamente,
                                Equipe do sistema de voluntariado de orfanatos.";
            return @"
                    Olá,
                    Agradecemos a criação do seu evento na nossa plataforma de ajuda para voluntários.
                    No momento a aprovação do seu evento está em andamento.
                            
                    Agradecemos a compreensão,
                    Equipe do sistema de voluntariado de orfanatos.";
        }

        private static string getEventEmailSubject(int status)
        {
            if (status == 1) return "Evento aprovado";
            if (status == 2) return "Evento bloqueado";

            return "Evento em aprovação";
        }

    }
}