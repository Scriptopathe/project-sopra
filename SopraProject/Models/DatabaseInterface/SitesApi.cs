﻿using System;
using System.Linq;
using System.Collections.Generic;
using SopraProject.Models.Identifiers;

namespace SopraProject.Models.DatabaseInterface
{
	public class SitesApi : ISitesApi
	{
		
		public SitesApi ()
		{
		}
		/// <summary>
		/// Gets a list of SiteIdentifiers corresponding to all the sites in the database.
		/// </summary>
		/// <returns>The sites.</returns>
		public List<SiteIdentifier> GetSites()
		{
			List<SiteIdentifier> val;
			using (var ctx = new DatabaseContexts.MainContext())
			{
				var query = from site in ctx.Sites
					        select site.SiteID;

				val = query.ToList().ConvertAll(id => new SiteIdentifier(id.ToString()));
			}
			return val;
		}

        public int SitesCount()
        {
            int val;
            using (var ctx = new DatabaseContexts.MainContext())
            {
                var query = from site in ctx.Sites
                            select site.SiteID;
                val = query.Count();
            }
            return val;
        }

		/// <summary>
		/// Gets the name of the given site.
		/// </summary>
		/// <returns>The site name.</returns>
		/// <param name="siteId">Site identifier.</param>
		public string GetSiteName(SiteIdentifier siteId)
		{
			string val;
			using (var ctx = new DatabaseContexts.MainContext())
			{
				var query = from site in ctx.Sites
						    where site.SiteID.ToString().Equals(siteId.Value)
					        select site.Name;
				val = query.First();
			}
			return val;
		}
		/// <summary>
		/// Gets the given site address.
		/// </summary>
		/// <returns>The site address.</returns>
		/// <param name="siteId">Site identifier.</param>
		public string GetSiteAddress(SiteIdentifier siteId)
		{
			string val;
			using (var ctx = new DatabaseContexts.MainContext())
			{
				var query = from site in ctx.Sites
					    	where site.SiteID.ToString().Equals(siteId.Value)
					        select site.Address;
				val = query.First();
			}
			return val;
		}

        public List<ParticularityIdentifier> GetAllParticularities()
        {
            List<ParticularityIdentifier> val;
            using (var ctx = new DatabaseContexts.MainContext())
            {
                var query = from parts in ctx.Particularities
                            select parts.ParticularityID;
                val = query.ToList().ConvertAll(id => new ParticularityIdentifier(id.ToString()));
            }
            return val;
        }

		/// <summary>
		/// Gets a list of RoomIdentifier corresponding to all the rooms in the database.
		/// </summary>
		/// <returns>The rooms.</returns>
		public List<RoomIdentifier> GetRooms()
		{
			List<RoomIdentifier> val;
			using (var ctx = new DatabaseContexts.MainContext())
			{
				var query = from room in ctx.Rooms
					        select room.RoomID;

				val = query.ToList().ConvertAll(id => new RoomIdentifier(id.ToString()));
			}
			return val;
		}
		/// <summary>
		/// Gets a list of RoomIdentifier corresponding to the rooms in the given site.
		/// </summary>
		/// <returns>The rooms.</returns>
		public List<RoomIdentifier> GetRooms(SiteIdentifier siteID)
		{
			List<RoomIdentifier> val;
			using (var ctx = new DatabaseContexts.MainContext())
			{
				var query = from site in ctx.Sites
                            where site.SiteID.ToString().Equals(siteID.Value.ToString())
                            select site.Rooms;

				val = query.First().ToList().ConvertAll(room => new RoomIdentifier(room.RoomID.ToString()));
			}
			return val;
		}
		/// <summary>
		/// Gets the name of the given room.
		/// </summary>
		/// <returns>The room name.</returns>
		/// <param name="roomId">Room identifier.</param>
		public string GetRoomName(RoomIdentifier roomId)
		{
			string val;
			using (var ctx = new DatabaseContexts.MainContext())
			{
				var query = from room in ctx.Rooms
						    where room.RoomID.ToString().Equals(roomId.Value)
					        select room.Name;
				val = query.First();
			}
			return val;			
		}
		/// <summary>
		/// Gets the room capacity.
		/// </summary>
		/// <returns>The room capacity.</returns>
		/// <param name="roomId">Room identifier.</param>
		public int GetRoomCapacity(RoomIdentifier roomId)
		{
			int val;
			using (var ctx = new DatabaseContexts.MainContext())
			{
				var query = from room in ctx.Rooms
						    where room.RoomID.ToString().Equals(roomId.Value)
					        select room.Capacity;
				val = query.First();
			}
			return val;	
		}
		/// <summary>
		/// Gets the room particularities.
		/// </summary>
		/// <returns>The room particularities.</returns>
		/// <param name="roomId">Room identifier.</param>
		public List<ParticularityIdentifier> GetRoomParticularities(RoomIdentifier roomId)
		{
			List<ParticularityIdentifier> val;
			using (var ctx = new DatabaseContexts.MainContext())
			{
				var query = from room in ctx.Rooms
						    where room.RoomID.ToString().Equals(roomId.Value)
                            select room.Particularities;
                val = query.First().ToList().ConvertAll(part => new ParticularityIdentifier(part.ParticularityID.ToString()));
			}
			return val;	
		}
		/// <summary>
		/// Gets the name of the particularity.
		/// </summary>
		/// <returns>The particularity name.</returns>
		/// <param name="partId">Part identifier.</param>
		public string GetParticularityName(ParticularityIdentifier partId)
		{
			string val;
			using (var ctx = new DatabaseContexts.MainContext())
			{
				var query = from particularity in ctx.Particularities
						    where particularity.ParticularityID.ToString().Equals(partId.Value)
					        select particularity.Name;
				val = query.First();
			}
			return val;	
		}
		/// <summary>
		/// Gets the particularity description.
		/// </summary>
		/// <returns>The particularity description.</returns>
		/// <param name="partId">Part identifier.</param>
		public string GetParticularityDescription(ParticularityIdentifier partId)
		{
			string val;
			using (var ctx = new DatabaseContexts.MainContext())
			{
				var query = from particularity in ctx.Particularities
						    where particularity.ParticularityID.ToString().Equals(partId.Value)
					        select particularity.Description;
				val = query.First();
			}
			return val;	
		}

        /// <summary>
        /// Checks if the given site exists.
        /// </summary>
        /// <param name="siteId">Site identifier.</param>
        public bool SiteExists(SiteIdentifier siteId)
        {
            bool val;
            using (var ctx = new DatabaseContexts.MainContext())
            {
                var query = from site in ctx.Sites
                            where site.SiteID.ToString().Equals(siteId.Value.ToString())
                            select site;
                val = query.Count() > 0;
            }
            return val;
        }
        /// <summary>
        /// Checks if the given room exists.
        /// </summary>
        /// <param name="roomId">room identifier.</param>
        public bool RoomExists(RoomIdentifier roomId)
        {
            bool val;
            using (var ctx = new DatabaseContexts.MainContext())
            {
                var query = from room in ctx.Rooms
                            where room.RoomID.ToString().Equals(roomId.Value.ToString())
                            select room;
                val = query.Count() > 0;
            }
            return val;
        }
        /// <summary>
        /// Checks if the given particularity exists.
        /// </summary>
        /// <param name="particularityId">particularity identifier.</param>
        public bool ParticularityExists(ParticularityIdentifier particularityId)
        {
            bool val;
            using (var ctx = new DatabaseContexts.MainContext())
            {
                var query = from particularity in ctx.Particularities
                            where particularity.ParticularityID.ToString().Equals(particularityId.Value.ToString())
                            select particularity;
                val = query.Count() > 0;
            }
            return val;
        }
	}
}

