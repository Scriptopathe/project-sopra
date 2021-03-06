﻿using System;
using System.Collections.Generic;
using SopraProject.Models.Identifiers;
namespace SopraProject.Models.DatabaseInterface
{
    /// <summary>
    /// This interfaces describes the messages needed to access the sites database.
    /// </summary>
    public interface ISitesApi
    {
        /// <summary>
        /// Gets a list of SiteIdentifiers corresponding to all the sites in the database.
        /// </summary>
        /// <returns>The sites.</returns>
        List<SiteIdentifier> GetSites();
        /// <summary>
        /// Gets all existing particularities.
        /// </summary>
        /// <returns>All existing particularities.</returns>
        List<ParticularityIdentifier> GetAllParticularities();
        /// <summary>
        /// The numbers of sites.
        /// </summary>
        /// <returns>The count.</returns>
        int SitesCount();
        /// <summary>
        /// Checks if the given site exists.
        /// </summary>
        /// <param name="siteId">Site identifier.</param>
        bool SiteExists(SiteIdentifier siteId);
        /// <summary>
        /// Checks if the given room exists.
        /// </summary>
        /// <param name="roomId">room identifier.</param>
        bool RoomExists(RoomIdentifier roomId);
        /// <summary>
        /// Checks if the given particularity exists.
        /// </summary>
        /// <param name="particularityId">particularity identifier.</param>
        bool ParticularityExists(ParticularityIdentifier particularityId);
        /// <summary>
        /// Gets the name of the given site.
        /// </summary>
        /// <returns>The site name.</returns>
        /// <param name="siteId">Site identifier.</param>
        string GetSiteName(SiteIdentifier siteId);
        /// <summary>
        /// Gets the given site address.
        /// </summary>
        /// <returns>The site address.</returns>
        /// <param name="siteId">Site identifier.</param>
        string GetSiteAddress(SiteIdentifier siteId);
        /// <summary>
        /// Gets a list of RoomIdentifier corresponding to all the rooms in the database.
        /// </summary>
        /// <returns>The rooms.</returns>
        List<RoomIdentifier> GetRooms();
        /// <summary>
        /// Gets a list of RoomIdentifier corresponding to the rooms in the given site.
        /// </summary>
        /// <returns>The rooms.</returns>
		List<RoomIdentifier> GetRooms(SiteIdentifier siteID);
        /// <summary>
        /// Gets the name of the given room.
        /// </summary>
        /// <returns>The room name.</returns>
        /// <param name="roomId">Room identifier.</param>
        string GetRoomName(RoomIdentifier roomId);
        /// <summary>
        /// Gets the room capacity.
        /// </summary>
        /// <returns>The room capacity.</returns>
        /// <param name="roomId">Room identifier.</param>
        int GetRoomCapacity(RoomIdentifier roomId);
        /// <summary>
        /// Gets the room particularities.
        /// </summary>
        /// <returns>The room particularities.</returns>
        /// <param name="roomId">Room identifier.</param>
        List<ParticularityIdentifier> GetRoomParticularities(RoomIdentifier roomId);
        /// <summary>
        /// Gets the name of the particularity.
        /// </summary>
        /// <returns>The particularity name.</returns>
        /// <param name="partId">Part identifier.</param>
        string GetParticularityName(ParticularityIdentifier partId);
        /// <summary>
        /// Gets the particularity description.
        /// </summary>
        /// <returns>The particularity description.</returns>
        /// <param name="partId">Part identifier.</param>
        string GetParticularityDescription(ParticularityIdentifier partId);
    }
}

