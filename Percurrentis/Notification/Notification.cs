using Percurrentis.Model;
using Percurrentis.AD_classes;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading;
using System.Web;
using System.Web.Hosting;

namespace Percurrentis.NotificationCenter
{
    public static class Notification
    {
        public static ADservices AD = ADservices.InstanceCreation();
        public static Mailer mail = new Mailer();

        /// <summary>
        /// Sends an email notification to the manager
        /// </summary>
        /// <param name="tr"></param>
        public static void requestManagerApproval(TravelRequest tr)
        {
            UserAC manager = AD.GetUserByGuid(tr.SuperiorID);
            MailMessage m = mail.createNotification(manager, "Travelrequest waiting", tr);
            mail.Send(m);
        }

        /// <summary>
        /// Notifies the recipient of an approved travelRequest
        /// </summary>
        /// <param name="recipient">UserAC person to send to</param>
        /// <param name="tr">The related TravelRequest object</param>
        public static void notifyOfApproval(UserAC recipient, TravelRequest tr)
        {
            MailMessage m = mail.createNotification(recipient, "Travelrequest approved", tr);
            mail.Send(m);
        }

        /// <summary>
        /// Notifies the recipient of an rejected travelRequest
        /// </summary>
        /// <param name="recipient">UserAC person to send to</param>
        /// <param name="tr">The related TravelRequest object</param>
        public static void notifyOfDenial(UserAC recipient, TravelRequest tr)
        {
            MailMessage m = mail.createNotification(recipient, "Travelrequest rejected", tr);
            mail.Send(m);
        }
    }

    public class Mailer
    {
        SmtpClient smtp;
        private string path;
        private string baseUrl;

        Boolean shutup = true;

        public Mailer()
        {
            // Create SmtpClient with hardcoded default values.
            smtp = new SmtpClient
            {
                Host = "smtp.gmail.com",
                Port = 587,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential("testerhenkie@gmail.com", "Wachtw00rd")
            };
            // Set the path for the mailtemplates
            path = "/Notification/mailtemplates/";
            baseUrl = "http://localhost:5583/TravelAgency/#/";
        }

        /// <summary>
        /// Set logincredentials
        /// </summary>
        /// <param name="senderEmail">String email</param>
        /// <param name="password">String password</param>
        public void setCredentials(string senderEmail, string password)
        {
            smtp.Credentials = new NetworkCredential(senderEmail, password);
        }

        public NetworkCredential getCredentials()
        {
            return (NetworkCredential)smtp.Credentials;
        }


        /// <summary>
        /// Returns a complete notification message
        /// </summary>
        /// <param name="recipient">UserAC recipient</param>
        /// <param name="type">String that declares the type of notification</param>
        /// <param name="id">The id of the request (optional)</param>
        /// <returns>Mailmessage</returns>
        public MailMessage createNotification(UserAC recipient, string type, TravelRequest tr)
        {
            string fullpath = HostingEnvironment.ApplicationPhysicalPath + path + type + ".html";
            if (File.Exists(fullpath))
            {
                // Read all HTML from the correct file and replace the placeholders ({0} etc) with
                // their actual values.
            
                string templateBody = File.ReadAllText(fullpath);
                Console.WriteLine();
                templateBody = templateBody.Replace("{0}", recipient.userName);
                templateBody = templateBody.Replace("{1}", baseUrl + "Request/" + tr.Hash);
                templateBody = templateBody.Replace("{2}", baseUrl + "Itinerary/" + tr.Hash);
                if (tr.TravelRequestApproval != null)
                {
                    templateBody = templateBody.Replace("{3}", tr.TravelRequestApproval.Note);
                }
                MailMessage notification = new MailMessage("peterfeddema@csiweb.ro",
                                                            "peterfeddema@csiweb.ro",
                                                            type + ".",
                                                            templateBody);
                notification.IsBodyHtml = true;
                return notification;
            }
            else
            {
                MailMessage fail = new MailMessage();
                fail.Body = "Template doesn't exist.";
                return fail;
            }
        }

        /// <summary>
        /// Send method with extra try
        /// </summary>
        /// <param name="mm">Mailmessage to be sent.</param>
        /// <returns>bool true when sent</returns>
        public bool Send(MailMessage mm)
        {
            if (shutup == false)
            {
                if ((mm.Body).Equals("Template doesn't exist."))
                {
                    return false;
                }
                else
                {
                    try
                    {
                        smtp.Send(mm);
                        return true;
                    }
                    catch (SmtpFailedRecipientException ex)
                    {
                        SmtpStatusCode statusCode = ex.StatusCode;

                        if (statusCode == SmtpStatusCode.MailboxBusy ||
                        statusCode == SmtpStatusCode.MailboxUnavailable ||
                        statusCode == SmtpStatusCode.TransactionFailed)
                        {
                            // Retry sending mail after 3 seconds
                            Thread.Sleep(5000);
                            smtp.Send(mm);
                            return true;
                        }
                        else
                        {
                            return false;
                            throw;
                        }
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.InnerException);
                        return false;
                    }
                    finally
                    {
                        mm.Dispose();
                    }
                }
            }
            else
            {
                return false;
            }
        }
    }
}