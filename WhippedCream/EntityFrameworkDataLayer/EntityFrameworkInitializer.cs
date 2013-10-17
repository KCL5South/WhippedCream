using System.Data.EntityClient;
namespace WhippedCream.EntityFrameworkDataLayer
{
	/// <summary>
	/// This class hold all the data necessary to initialize an Entity Framework ObjectContext object.
	/// </summary>
    public struct EntityFrameworkInitializer : System.IEquatable<EntityFrameworkInitializer>
    {
		/// <summary>
		/// Represents the default.
		/// </summary>
        public static readonly EntityFrameworkInitializer Empty = new EntityFrameworkInitializer()
        {
            ConnectionString = null,
            Provider = null,
            DefaultContainerName = null,
            SSDLResource = null,
            CSDLResource = null,
            MSLResource = null
        };

		/// <summary>
		/// Gets or sets the Provider Connection String used to generate an Entity Framework Connection String.
		/// </summary>
		public string ConnectionString { get; set; }
		/// <summary>
		/// Gets or sets the Provider type used to generate an Entity Framework Connection String.
		/// </summary>
		public string Provider { get; set; }
		/// <summary>
		/// Gets or sets the Default Container Name used to initialize the Object Context.
		/// </summary>
		public string DefaultContainerName { get; set; }
		/// <summary>
		/// Gets or sets the SSDL resource used to generate an Entity Framework Connection String.
		/// </summary>
		public string SSDLResource { get; set; }
		/// <summary>
		/// Gets or sets the CSDL resource used to generate an Entity Framework Connection String.
		/// </summary>
		public string CSDLResource { get; set; }
		/// <summary>
		/// Gets or sets the MSL resource used to generate an Entity Framework Connection String.
		/// </summary>
        public string MSLResource { get; set; }

		/// <summary>
		/// Overriden from <see cref="System.Object"/>.
		/// </summary>
		/// <param name="obj">The object to compare against.</param>
		/// <returns>True if the passed object is equal to this object.</returns>
        public override bool Equals(object obj)
        {
            if (obj is EntityFrameworkInitializer)
                return Equals((EntityFrameworkInitializer)obj);
            else
                return false;
        }

		/// <summary>
		/// Overriden from <see cref="System.Object"/>.
		/// </summary>
		/// <returns>A hash that is unique to this object.</returns>
        public override int GetHashCode()
        {
            int hashCode = 0;
            unchecked
            {
                hashCode += 1000000007 * (ConnectionString ?? string.Empty).GetHashCode();
                hashCode += 1000000009 * (DefaultContainerName ?? string.Empty).GetHashCode();
                hashCode += 1000000021 * (SSDLResource ?? string.Empty).GetHashCode();
                hashCode += 1000000033 * (CSDLResource ?? string.Empty).GetHashCode();
                hashCode += 1000000037 * (MSLResource ?? string.Empty).GetHashCode();
                hashCode += 1000000041 * (Provider ?? string.Empty).GetHashCode();
            }

            return hashCode;
        }

		/// <summary>
		/// Overriden from <see cref="System.Object"/>.
		/// </summary>
		/// <returns>A string representation of this object.</returns>
        public override string ToString()
        {
            return ConnectionString ?? base.ToString();
        }

		/// <summary>
		/// Generates an Entity Framework Connection String.
		/// </summary>
		/// <returns>An Entity Framework Connection String.</returns>
		public string GenerateConnectionString()
		{
			EntityConnectionStringBuilder builder = new EntityConnectionStringBuilder();
			builder.Provider = this.Provider;
			builder.ProviderConnectionString = this.ConnectionString;
			builder.Metadata = string.Join("|", this.CSDLResource,
												this.MSLResource,
												this.SSDLResource);

			return builder.ConnectionString;
		}

        #region IEquatable<EntityFrameworkInitializer> Members

		/// <summary>
		/// Tests wether the passed instance is equal to this instance.
		/// </summary>
		/// <param name="other">An instance of <see cref="EntityFrameworkInitializer"/>.</param>
		/// <returns>True if the instances are equal, false otherwise.</returns>
        public bool Equals(EntityFrameworkInitializer other)
        {
            return object.Equals(ConnectionString, other.ConnectionString) &&
                   object.Equals(DefaultContainerName, other.DefaultContainerName) &&
                   object.Equals(SSDLResource, other.SSDLResource) &&
                   object.Equals(CSDLResource, other.CSDLResource) &&
                   object.Equals(MSLResource, other.MSLResource) &&
				   object.Equals(Provider, other.Provider);
        }

        #endregion
    }
}