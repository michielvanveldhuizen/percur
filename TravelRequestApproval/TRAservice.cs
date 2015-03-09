// <copyright company=CSi Romania SRL>
// Copyright (c) 2014 All Rights Reserved
// </copyright>
// <author>Tim Lagerburg</author>
// <summary>Code for the Windows Service with all its methods</summary>

using Percurrentis.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Data.SqlClient;
using System.Diagnostics;
using System.DirectoryServices;
using System.Net;
using System.Net.Mail;
using Outlook = Microsoft.Office.Interop.Outlook;
using System.ServiceProcess;
using System.Timers;
using Percurrentis.Context;
using System.IO;
using System.Threading;
using Percurrentis.AD_classes;
using System.Xml;
using System.Xml.Linq;

namespace TravelRequestApproval
{
    public partial class TRAservice : ServiceBase
    {
        private System.Timers.Timer scheduleTimer = null;
        private static Mailer m = new Mailer();
        private static string travelAgent = "Peter Feddema";
        private static ADservices AD = new ADservices();

        public TRAservice()
        {
            InitializeComponent();

            CanHandlePowerEvent = false;
            CanHandleSessionChangeEvent = false;
            CanPauseAndContinue = true;
            CanShutdown = true;
            CanStop = true;
            AutoLog = false;

            scheduleTimer = new System.Timers.Timer();
            scheduleTimer.Interval = 60000;
            scheduleTimer.Elapsed += new ElapsedEventHandler(scheduleTimer_Elapsed);
        }

        protected override void OnStart(string[] args)
        {
            this.EventLog.Source = this.ServiceName;
            this.EventLog.Log = "Application";

            ((ISupportInitialize)(this.EventLog)).BeginInit();
            if (!EventLog.SourceExists(this.EventLog.Source))
            {
                EventLog.CreateEventSource(this.EventLog.Source, this.EventLog.Log);
            }
            ((ISupportInitialize)(this.EventLog)).EndInit();

            EventLog.WriteEntry("Service started", EventLogEntryType.Information, 1, 100);
            scheduleTimer.Start();
            
        }

        protected void scheduleTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            SetMethod();
        }

        public void SetMethod()
        {
            double todaysRate = Checks.checkAndUpdateConversionRate();

            using(DatabaseContext db = new DatabaseContext())
            {
                ExchangeRate current;
                if ((db.ExchangeRate.Count()).Equals(0))
                {
                    current = new ExchangeRate();
                }
                else
                {
                    current = db.ExchangeRate.Single();
                }

                current.EUR = 1;
                current.RON = todaysRate;
                current.LastUpdate = DateTime.Now;
                db.SaveChanges();
            }
            //Checks.checkInsuranceExpiration();           
                                              
            /* How to send a notification
             * Mailer mail = new Mailer();
            UserAC Peter = new UserAC();
            Peter.userName = "Peter Feddema";
            Peter.mail = "peterfeddema@csiweb.ro";
            mail.Send(mail.createNotification(Peter, "Travelrequest waiting", 1));*/
        }

        /// <summary>
        /// Class that contains methods that are to be run regularly to perform certain checks.
        /// </summary>
        public class Checks
        {
            // String which will be filled with table rows if there are expiring insurances
            public static string expiring = "";
            
            /// <summary>
            /// Checks all insurance instances in the database for expiry based on numDays variable
            /// </summary>
            public static void checkInsuranceExpiration()
            {
                int numDays = 7; // How many days in advance you want to receive a warning

                using (var db = new DatabaseContext())
                {
                    Insurance anois = new Insurance();
                    anois.InsuranceNumber = "12XBD485";
                    anois.InsureeGUID = "e579f611-3def-4a85-89c4-b7a010ac08db";
                    anois.StartDate = DateTime.Today.AddDays(-1000);
                    anois.ExpirationDate = DateTime.Today.AddDays(830);
                    db.Insurance.Add(anois);
                    db.SaveChanges();


                    foreach (Insurance i in db.Insurance)
                    {
                        DateTime today = DateTime.Today;
                        DateTime checkDate = today.AddDays(numDays);

                        if (((i.ExpirationDate.Date).CompareTo(checkDate).Equals(-1)))
                        {
                            string expDate = i.ExpirationDate.Date.ToShortDateString();
                            string UserName = AD.GetUserByGuid(i.InsureeGUID).userName;
                            expiring += "<tr><td>" + i.InsuranceNumber + "</td><td>" + UserName + "</td><td>" + expDate + "</td></tr>";
                        }
                    }
                    if (expiring.Length > 0)
                    {
                        UserAC p = new UserAC();
                        p = AD.GetUserByName(travelAgent);
                        MailMessage expireNotification = m.createNotification(p, "Insurance(s) expiring");
                        //m.Send(expireNotification);
                    }
                }
            }

            public static double checkAndUpdateConversionRate()
            {
                string ecb_url = "http://www.ecb.europa.eu/stats/eurofxref/eurofxref-daily.xml";
                string yahoo_url = "https://query.yahooapis.com/v1/public/yql?q=select%20*%20from%20yahoo.finance.xchange%20where%20pair%20in%20(%22EURRON%22)&env=store%3A%2F%2Fdatatables.org%2Falltableswithkeys";
                float exchangeRate = 0;

                try
                {
                    XDocument doc = XDocument.Load(ecb_url);

                    XNamespace gesmes = "http://www.gesmes.org/xml/2002-08-01";
                    XNamespace ns = "http://www.ecb.int/vocabulary/2002-08-01/eurofxref";

                    var cubes = doc.Descendants(ns + "Cube")
                                   .Where(x => x.Attribute("currency") != null)
                                   .Select(x => new
                                   {
                                       Currency = (string)x.Attribute("currency"),
                                       Rate = (decimal)x.Attribute("rate")
                                   });

                    var RON = cubes.Single(x => x.Currency == "RON");
                    exchangeRate = (float)RON.Rate;
                }
                catch(WebException)
                {
                    try
                    {
                        XDocument doc = XDocument.Load(yahoo_url);

                        var cubes = doc.Descendants("rate");
                        var node = cubes.First();
                        var RON = node.Descendants("Rate");

                        exchangeRate = float.Parse(RON.First().Value);
                    }
                    catch (WebException)
                    {
                        Console.WriteLine("Connection could not be established");
                        exchangeRate = -1;
                    }
                }
                return exchangeRate;
            }
        }

        public class Mailer
        {
            SmtpClient smtp;
            string path;
            string baseUrl;

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
                path = "../../mailtemplates/";
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
            public MailMessage createNotification(UserAC recipient, string type, int id = 0)
            {
                if (File.Exists(path + type + ".html"))
                {
                    // Read all HTML from the correct file and replace the placeholders ({0} etc) with
                    // their actual values.
                    string templateBody = File.ReadAllText(path + type + ".html");
                    templateBody = templateBody.Replace("{0}", recipient.userName);
                    templateBody = templateBody.Replace("{1}", baseUrl + "Request/" + id);
                    templateBody = templateBody.Replace("{2}", baseUrl + "Itinerary/" + id);
                    templateBody = templateBody.Replace("{3}", Checks.expiring);
                    MailMessage notification = new MailMessage("peterfeddema@csiweb.ro",
                                                                recipient.mail,
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
                    finally
                    {
                        mm.Dispose();
                    }
                }
            }
        }

        protected override void OnStop()
        {
            EventLog.WriteEntry("Service stopped", EventLogEntryType.Information, 2, 100);
            scheduleTimer.Stop();
            EventLog.WriteEntry("Stopped");
        }
        protected override void OnPause()
        {
            EventLog.WriteEntry("Service paused", EventLogEntryType.Information, 3, 100);
            scheduleTimer.Stop();
            EventLog.WriteEntry("Paused");
        }
        protected override void OnContinue()
        {
            EventLog.WriteEntry("Service continued", EventLogEntryType.Information, 4, 100);
            scheduleTimer.Start(); ;
            EventLog.WriteEntry("Continuing");
        }
        protected override void OnShutdown()
        {
            EventLog.WriteEntry("Service shutdown", EventLogEntryType.Information, 5, 100);
            scheduleTimer.Stop();
            EventLog.WriteEntry("ShutDowned");
        }
    }    
}
