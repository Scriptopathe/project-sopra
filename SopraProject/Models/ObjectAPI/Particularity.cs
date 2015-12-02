﻿using System;
using System.Xml.Serialization;
namespace SopraProject.ObjectApi
{
    public class Particularity
    {
        ParticularityIdentifier _identifier;
        string _name;

        #region Properties
        /// <summary>
        /// Gets the particularity's identifier.
        /// </summary>
        /// <value>The identifier.</value>
        public ParticularityIdentifier Identifier
        {
            get { return _identifier; }
        }
        /// <summary>
        /// Gets the particularity name.
        /// </summary>
        /// <value>The name.</value>
        public string Name
        {
            get 
            { 
                if (_name == null)
                {
                    _name = ObjectApiProvider.Instance.SitesApi.GetParticularityName(_identifier);
                }
                return _name;
            }
        }
        #endregion

        #region XML
        [XmlAttribute("id")]
        public string XMLIdentifier
        {
            get { return Identifier.Value; }
            set { _identifier = new ParticularityIdentifier(value);}
        }
        [XmlElement("Name")]
        public string XMLName
        {
            get { return Name; }
            set { }
        }
        public Particularity() { }
        #endregion

        public Particularity(ParticularityIdentifier id)
        {
            _identifier = id;
        }
    }
}

