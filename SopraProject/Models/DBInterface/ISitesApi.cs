using System;
using System.Collections.Generic;
namespace SopraProject
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
        /// Gets the name of the given room.
        /// </summary>
        /// <returns>The room name.</returns>
        /// <param name="roomId">Room identifier.</param>
        string GetRoomName(RoomIdentifier roomId);
    }
}

