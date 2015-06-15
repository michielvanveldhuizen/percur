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
using Percurrentis.Context;

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

        Boolean shutup = false;
        public ADservices AD = ADservices.InstanceCreation();

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
                Credentials = new NetworkCredential("testerhenkie@gmail.com", "Wachtw00rd"),
                /*Host = "MAILRO.CSiRO.local",
                //Port = 465,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential("MichielvanVeldhuizen@csiweb.ro", "csimichielv")*/
            };

            /*smtp = new SmtpClient("MAILRO.CSiRO.local");
            smtp.UseDefaultCredentials = false;
            ///////////////////////////////////////
            // TODO MAIL
            // EDIT THE ROW BELOW IN ORDER TO GET THE E-MAL FUNCTAIONALITY WORKING
            ///////////////////////////////////////
            smtp.Credentials = new NetworkCredential("MichielvanVeldhuizen@csiweb.ro", "csimichielv");
            //smtp.Credentials = new NetworkCredential("michielv", "csimichielv");
            smtp.DeliveryMethod = SmtpDeliveryMethod.Network;*/

            // Set the path for the mailtemplates
            path = "/Notification/mailtemplates/";
            baseUrl = "http://appro.csiro.local/TravelAgency/#/";
        }

        /// <summary>
        /// To save a mail error
        /// </summary>
        /// <param name="message"></param>
        /// <param name="toEmail"></param>
        /// <param name="typeOfMail"></param>
        public void saveError(string message,string toEmail,string typeOfMail)
        {
            MailError mError    = new MailError();
            mError.Message      = message;
            mError.ToEmail      = toEmail;
            mError.TypeOfEmail  = typeOfMail;
            mError.DateTime     = DateTime.Now;

            using (var ctx = new DatabaseContext())
            {
                ctx.MailError.Add(mError);
                ctx.SaveChanges();
            }
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
                templateBody = templateBody.Replace("{name}", recipient.userName);
                templateBody = templateBody.Replace("{detailLink}", baseUrl + "Request/" + tr.Hash);
                templateBody = templateBody.Replace("{2}", baseUrl + "Itinerary/" + tr.Hash);

                string supervisor = "";

                //Special case for Romania
                if (tr.Country.Name.Equals("Romania"))
                {
                    //Romania approved
                    if (type.Equals("Travelrequest approved"))
                    {
                        supervisor = AD.GetUserByGuid(tr.SuperiorID).userName + " & " + AD.GetUserByGuid(GlobalVar.COOGuid).userName;
                    }
                    //Romania rejected
                    if (type.Equals("Travelrequest rejected"))
                    {
                        if (tr.TravelRequestApproval.COOApproved == 1)
                        {
                            supervisor = AD.GetUserByGuid(GlobalVar.COOGuid).userName;
                        }
                        
                        if(tr.TravelRequestApproval.HasApproved == 1)
                        {
                            supervisor = AD.GetUserByGuid(tr.SuperiorID).userName;   
                        }
                    }
                }
                else
                {
                    supervisor = AD.GetUserByGuid(tr.SuperiorID).userName;
                }

                templateBody = templateBody.Replace("{supervisor}", supervisor);

                templateBody = templateBody.Replace("{departureDate}", tr.DepartureDate.ToString("yyyy-MM-dd"));
                templateBody = templateBody.Replace("{returnDate}", tr.ReturnDate.ToString("yyyy-MM-dd"));
                

                if (tr.TravelRequestApproval != null)
                {
                    templateBody = templateBody.Replace("{reason}", tr.TravelRequestApproval.Note);
                }

                if (tr.Country != null)
                {
                    templateBody = templateBody.Replace("{country}", tr.Country.Name);
                }

                string travellerString = "";

                if (tr.TravelRequest_RequestTravellers != null)
                {
                    foreach (TravelRequest_RequestTraveller TR_RT in tr.TravelRequest_RequestTravellers)
                    {
                        travellerString += TR_RT.RequestTraveller.FullName+" & ";
                    }
                    travellerString = travellerString.Substring(0, travellerString.Length - 2);
                    templateBody = templateBody.Replace("{travellers}", travellerString);
                }


                string mailReceiver = "MichielvanVeldhuizen@csiweb.ro";
                if (!recipient.mail.Equals("KeesOosting@csiweb.ro"))
                {
                    //mailReceiver = recipient.mail;
                }

                MailMessage notification = new MailMessage("TravelAgency@csiweb.ro",
                                                            mailReceiver,
                                                            type + ".",
                                                            templateBody);

                MailMessage notificationToMe = new MailMessage("TravelAgency@csiweb.ro",
                                                            "MichielvanVeldhuizen@csiweb.ro",
                                                            "copy mail - "+type + ".",
                                                            templateBody);
                notificationToMe.IsBodyHtml = true;
                Send(notificationToMe);


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
                    string to = mm.To.ToString();
                    saveError("Template doesn't exist.", to, mm.Subject);
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

                        string to = mm.To.ToString();
                        saveError(statusCode+"---"+ex.Message, to, mm.Subject);

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
                        string to = mm.To.ToString();
                        saveError(e.Message + "___" + e.InnerException, to, mm.Subject);
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