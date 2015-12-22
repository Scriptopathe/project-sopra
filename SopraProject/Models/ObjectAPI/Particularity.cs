using System;
using System.Xml.Serialization;
using SopraProject.ObjectApi.Cache;
namespace SopraProject.ObjectApi
{
    public class Particularity
    {
        ParticularityIdentifier _identifier;
        string _name;
        object _lock = new object();

        #region Properties
        /// <summary>
        /// Gets the particularity's identifier.
        /// </summary>
        /// <value>The identifier.</value>
        [XmlIgnore()]
        public ParticularityIdentifier Identifier
        {
            get { return _identifier; }
            private set
            {
                lock(_lock)
                {
                    _identifier = value;
                    if (!ObjectApiProvider.Instance.SitesApi.ParticularityExists(_identifier))
                        throw new InvalidIdentifierException(this.GetType(), _identifier.Value.ToString());
                }
            }
        }
        /// <summary>
        /// Gets the particularity name.
        /// </summary>
        /// <value>The name.</value>
        [XmlIgnore()]
        public string Name
        {
            get 
            { 
                lock(_lock)
                {
                    if (_name == null)
                    {
                        _name = ObjectApiProvider.Instance.SitesApi.GetParticularityName(_identifier);
                    }
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

        private Particularity(ParticularityIdentifier id)
        {
            Identifier = id;
        }

        #region Cache
        private static ObjectCache<string, Particularity> s_cache = new ObjectCache<string, Particularity>();

        /// <summary>
        /// Gets the booking from the database with the given identifier.
        /// </summary>
        public static Particularity Get(ParticularityIdentifier id)
        {
            return s_cache.Get(id);
        }
        #endregion
    }
}

