using System;
using System.Collections;
using System.Collections.Generic;
namespace SopraProject.Models.DatabaseContexts
{
    public static class DatabaseWorker
    {
        static Random s_rand = new Random();
        /// <summary>
        /// Creates the database.
        /// </summary>
        public static void CreateDatabase()
        {
            List<string> usernames = new List<string>();
            for (int i = 1; i < 10; i++)
                usernames.Add("User" + i);
            
            List<MCSite> allsites = new List<MCSite>();
            List<MCRoom> allrooms = new List<MCRoom>();

            using (var ctx = new MainContext())
            {

                List<MCParticularity> allparts = new List<MCParticularity>();
                allparts.Add(new MCParticularity() { Name = "Videoconf" });
                allparts.Add(new MCParticularity() { Name = "Digilab" });
                allparts.Add(new MCParticularity() { Name = "Telephone" });
                allparts.Add(new MCParticularity() { Name = "Secure" });



                for (int i = 1; i < 10; i++)
                {
                    List<MCRoom> rooms = new List<MCRoom>();
                    for (int j = 1; j < 10; j++)
                    {
                        rooms.Add(CreateRoom("Site" + i + "Room" + j, allparts));
                    }
                    allrooms.AddRange(rooms);
                    MCSite site = CreateSite(i, "Site" + i, rooms);
                    allsites.Add(site);
                }

                foreach (string username in usernames)
                {
                    ctx.UsersProfile.Add(CreateUserProfile(username, allsites));
                }
                ctx.SaveChanges();
                ctx.Particularities.AddRange(allparts);
                ctx.SaveChanges();
                ctx.Rooms.AddRange(allrooms);
                ctx.SaveChanges();
                ctx.Sites.AddRange(allsites);
                ctx.SaveChanges();
            }

            using (var ctx = new AuthenticationContext())
            {
                foreach (string username in usernames)
                {
                    ACUser user = new ACUser() { Username = username, Password = Tools.Security.Hash(username + "pass") };
                    ctx.Users.Add(user);
                }
                ctx.SaveChanges();
            }

            using (var ctx = new BookingContext())
            {
                for(int i = 1; i < 5000; i++)
                    ctx.Bookings.Add(CreateBooking(usernames, allrooms.Count, "Subject" + i));
                ctx.SaveChanges();
            }
        }



        static MCRoom CreateRoom(string name, List<MCParticularity> particularities)
        {
            List<MCParticularity> p = new List<MCParticularity>();
            foreach (var part in particularities)
            {
                if (s_rand.Next(4) != 0)
                    p.Add(part);
            }

            return new MCRoom()
            {
                Capacity = s_rand.Next(1, 10),
                Name = name,
                Particularities = p
            };
        }

        static MCSite CreateSite(int id, string name, List<MCRoom> rooms)
        {
            return new MCSite() { Name = name, Address = "10 allée des trucs machin chouettes,\nbâtiment P565,\n31400 Toulouse CEDEX 6", Rooms = rooms };
        }

        static ACUser CreateUser(string username)
        {
            return new ACUser() { Username = username, Password = "pass" };
        }

        static MCUserProfile CreateUserProfile(string username, List<MCSite> allsites)
        {
            return new MCUserProfile() { Username = username, SiteID = s_rand.Next(1, allsites.Count), IsAdmin = username.Contains("1") };
        }

        static BCBooking CreateBooking(List<string> usernames, int rooms, string subject)
        {
            string username = usernames[s_rand.Next(0, usernames.Count)];
            int room = s_rand.Next(1, rooms);
            int hour = s_rand.Next(8, 18);
            int minute = (new int[] { 0, 15, 30, 45 })[s_rand.Next(0, 4)];
            int day = s_rand.Next(1, 30);
            DateTime start = new DateTime(2015, 11, day, hour, minute, 0);
            minute = (new int[] { 0, 15, 30, 45, 60, 75, 90, 105, 120 })[s_rand.Next(0, 4)];
            DateTime end = start.AddMinutes(minute);
            int personCount = s_rand.Next(1, 5);
            return new BCBooking()
            {
                RoomID = room,
                Contacts = new List<string>() { "email@domain.com", "email2@domain.com" },
                StartDate = start,
                EndDate = end,
                PersonCount = personCount,
                Subject = subject,
                ParticipantsCount = s_rand.Next(0, 4)
                
            };
        }
        
    }
}

