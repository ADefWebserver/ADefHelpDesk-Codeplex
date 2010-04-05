// ADefwebserver.com
// Copyright (c) 2009
// by Michael Washington
//
// DotNetNuke® - http://www.dotnetnuke.com
// Copyright (c) 2002-2008
// by DotNetNuke Corporation
//
// Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated 
// documentation files (the "Software"), to deal in the Software without restriction, including without limitation 
// the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and 
// to permit persons to whom the Software is furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in all copies or substantial portions 
// of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED 
// TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL 
// THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF
// CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER 
// DEALINGS IN THE SOFTWARE.

using System;
using System.Data;
using System.Linq;
using System.Web;
using System.Xml.Linq;
using System.Net.Mail;
using System.Web.Mail;
using System.Text;

namespace ADefWebserver.Modules.ADefHelpDesk
{
    public class Email
    {
        public Email()
        {
        }

        #region SendMail(string MailTo, string Cc, string Bcc, string ReplyTo, string Subject, string Body)
        public static string SendMail(string MailTo, string Cc, string Bcc, string ReplyTo, string Subject, string Body)
        {
            GeneralSettings GeneralSettings = new GeneralSettings();
            string[] arrAttachments = new string[0];
            return SendMail(GeneralSettings.SMTPFromEmail, MailTo, Cc, Bcc, ReplyTo, System.Net.Mail.MailPriority.Normal, Subject, Encoding.UTF8, Body, arrAttachments, "", "", "", "", GeneralSettings.SMTPSecure);
        }
        #endregion

        #region SendMail(string MailFrom, string MailTo, string Cc, string Bcc, string ReplyTo, System.Net.Mail.MailPriority Priority, string Subject, Encoding BodyEncoding, string Body, string[] Attachment, string SMTPServer, string SMTPAuthentication, string SMTPUsername, string SMTPPassword, bool SMTPEnableSSL)
        public static string SendMail(string MailFrom, string MailTo, string Cc, string Bcc, string ReplyTo, System.Net.Mail.MailPriority Priority, string Subject, Encoding BodyEncoding, string Body,
    string[] Attachment, string SMTPServer, string SMTPAuthentication, string SMTPUsername, string SMTPPassword, bool SMTPEnableSSL)
        {
            string strSendMail = "";
            GeneralSettings GeneralSettings = new GeneralSettings();

            // SMTP server configuration
            if (SMTPServer == "")
            {
                SMTPServer = GeneralSettings.SMTPServer;
            }

            if (SMTPAuthentication == "")
            {
                SMTPAuthentication = GeneralSettings.SMTPAuthendication;
            }

            if (SMTPUsername == "")
            {
                SMTPUsername = GeneralSettings.SMTPUserName;
            }

            if (SMTPPassword == "")
            {
                SMTPPassword = GeneralSettings.SMTPPassword;
            }

            MailTo = MailTo.Replace(";", ",");
            Cc = Cc.Replace(";", ",");
            Bcc = Bcc.Replace(";", ",");

            System.Net.Mail.MailMessage objMail = null;
            try
            {
                objMail = new System.Net.Mail.MailMessage(MailFrom, MailTo);
                if (Cc != "")
                {
                    objMail.CC.Add(Cc);
                }
                if (Bcc != "")
                {
                    objMail.Bcc.Add(Bcc);
                }

                if (ReplyTo != string.Empty) objMail.ReplyTo = new System.Net.Mail.MailAddress(ReplyTo);
                objMail.Priority = (System.Net.Mail.MailPriority)Priority;
                objMail.IsBodyHtml = IsHTMLMail(Body);

                foreach (string myAtt in Attachment)
                {
                    if (myAtt != "") objMail.Attachments.Add(new System.Net.Mail.Attachment(myAtt));
                }

                // message
                objMail.SubjectEncoding = BodyEncoding;
                objMail.Subject = Subject.Trim();
                objMail.BodyEncoding = BodyEncoding;

                //added support for multipart html messages
                //add text part as alternate view
                //objMail.Body = Body
                System.Net.Mail.AlternateView PlainView = System.Net.Mail.AlternateView.CreateAlternateViewFromString(Utility.ConvertToText(Body), null, "text/plain");
                objMail.AlternateViews.Add(PlainView);

                //if body contains html, add html part
                if (IsHTMLMail(Body))
                {
                    System.Net.Mail.AlternateView HTMLView = System.Net.Mail.AlternateView.CreateAlternateViewFromString(Body, null, "text/html");
                    objMail.AlternateViews.Add(HTMLView);
                }
            }

            catch (Exception objException)
            {
                // Problem creating Mail Object
                strSendMail = MailTo + ": " + objException.Message;
            }

            if (objMail != null)
            {
                // external SMTP server alternate port
                int? SmtpPort = null;
                int portPos = SMTPServer.IndexOf(":");
                if (portPos > -1)
                {
                    SmtpPort = Int32.Parse(SMTPServer.Substring(portPos + 1, SMTPServer.Length - portPos - 1));
                    SMTPServer = SMTPServer.Substring(0, portPos);
                }

                System.Net.Mail.SmtpClient smtpClient = new System.Net.Mail.SmtpClient();

                if (SMTPServer != "")
                {
                    smtpClient.Host = SMTPServer;
                    smtpClient.Port = (SmtpPort == null) ? (int)25 : (Convert.ToInt32(SmtpPort));

                    switch (SMTPAuthentication)
                    {
                        case "":
                        case "0":
                            // anonymous
                            break;
                        case "1":
                            // basic
                            if (SMTPUsername != "" & SMTPPassword != "")
                            {
                                smtpClient.UseDefaultCredentials = false;
                                smtpClient.Credentials = new System.Net.NetworkCredential(SMTPUsername, SMTPPassword);
                            }

                            break;
                        case "2":
                            // NTLM
                            smtpClient.UseDefaultCredentials = true;
                            break;
                    }
                }
                smtpClient.EnableSsl = SMTPEnableSSL;

                try
                {
                    smtpClient.Send(objMail);
                    strSendMail = "";
                }
                catch (Exception objException)
                {
                    // mail configuration problem
                    if (!(objException.InnerException == null))
                    {
                        strSendMail = string.Concat(objException.Message, Environment.NewLine, objException.InnerException.Message);
                    }
                    else
                    {
                        strSendMail = objException.Message;
                    }
                }
                finally
                {
                    objMail.Dispose();
                }
            }

            return strSendMail;
        }
        #endregion

        #region IsHTMLMail
        public static bool IsHTMLMail(string Body)
        {
            return System.Text.RegularExpressions.Regex.IsMatch(Body, "<[^>]*>");
        }
        #endregion
    }
}