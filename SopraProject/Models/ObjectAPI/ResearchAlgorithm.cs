using System;
using SopraProject.ObjectApi;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Xml.Serialization;
using System.Web.Security;

namespace SopraProject.ObjectApi
{
    public class ResearchAlgorithm
    {
        public ResearchAlgorithm()
        {
        }

        public List<Room> research(int siteId = -1, int personCount=-1, List<ParticularityIdentifier> particularities=null, DateTime? startDate = null, DateTime? endDate = null)
        {
            var rooms = SopraProject.ObjectApi.Room.GetAllRooms();
            if (siteId != -1)
            {
                //List<SopraProject.ObjectApi.Site> sites = null;
                for (int i = 0; i < SopraProject.ObjectApi.Site.GetSitesCount(); i++)
                {
                    if (i != siteId)
                    {
                        var sites =  SopraProject.ObjectApi.Site.Get(new SiteIdentifier(i.ToString()));
                        for (int j = 0; j < sites.Rooms.Count; j++)
                        {
                            rooms.Remove(sites.Rooms[j]);
                        }
                        //rooms.RemoveAll(new SopraProject.ObjectApi.Site(new SiteIdentifier(i.ToString())).Rooms);
                    }
                }
                //rooms.RemoveAll(sites.Rooms);
            }
            if (personCount != -1)
            {
                for (int i = 0; i < rooms.Count; i++)
                {
                    if (rooms[i].Capacity > personCount)
                    {
                        rooms.Remove(rooms[i]);
                    }
                }
            }
            if (particularities != null)
            {
                for (int i = 0; i < rooms.Count; i++)
                {
                    //rooms.ElementAt(i).Particularities.Count();
                    // rooms[i].GetParticularities().Union(particularities);
                    //var part = rooms[i].Particularities.
                    var parts = rooms[i].Particularities.ToList().ConvertAll(p => p.Identifier);
                    if (parts.Union(particularities).Count() != rooms[i].Particularities.Count())
                    {
                        rooms.Remove(rooms[i]);
                    }
                }
            }

            if (startDate != null && endDate != null)
            {
                for (int i = 0; i < rooms.Count; i++)
                {
                    if (!(ObjectApiProvider.Instance.BookingsApi.GetBookings(rooms[i].Identifier, startDate.Value, endDate.Value) == null))
                    {
                        rooms.Remove(rooms[i]);
                    }
                }
            }

            return rooms;
        }
    }
}

