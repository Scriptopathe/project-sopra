using System;

namespace SopraProject.ObjectApi
{
    public class Particularity
    {
        ParticularityIdentifier _identifier;
        string _name;

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
                if (_name == String.Empty)
                {
                    _name = ObjectApiProvider.Instance.SitesApi.GetParticularityName(_identifier);
                }
                return _name;
            }
        }

        public Particularity(ParticularityIdentifier id)
        {
            _identifier = id;
        }
    }
}

