using System;

namespace VolunteeringSystem.Helpers.Email
{
    public static class SendMail
    {
        private static string email = "volunteering.system@gmail.com";
        private static string password = "volunteering.system";
        private static string host = "smtp.gmail.com";
        private static int port = 587;

        public static void SendNewStatus(string receiver_email, int new_status)
        {
            System.Net.Mail.SmtpClient smtp = new System.Net.Mail.SmtpClient();
            smtp.UseDefaultCredentials = false;
            smtp.Host = host;
            smtp.Port = port;
            smtp.EnableSsl = true;
            smtp.Credentials = new System.Net.NetworkCredential(email, password);

            System.Net.Mail.MailMessage mailMessage = new System.Net.Mail.MailMessage();
            mailMessage.From = new System.Net.Mail.MailAddress(email);
            mailMessage.To.Add(receiver_email);
            mailMessage.Body = getEmailBody(new_status);
            mailMessage.Subject = getEmailSubject(new_status);
            smtp.Send(mailMessage);
        }

        private static string getEmailBody(int status)
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

        private static string getEmailSubject(int status)
        {
            if (status == 1) return "Aprovação confirmada";
            if (status == 2) return "Conta bloqueada";

            return "Aprovação em progresso";
        }

    }
}