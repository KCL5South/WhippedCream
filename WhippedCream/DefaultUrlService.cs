using System.Linq;

namespace WhippedCream
{
	/// <summary>
	/// This object represents the default implementation of <see cref="IUrlService"/>.
	/// </summary>
	internal class DefaultUrlService : IUrlService
	{
		private System.Uri _baseUri;

		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="baseUri">The base Uri of all services.</param>
		/// <param name="servicePrefix">If we need a prefix to be appended to the base Uri.</param>
		public DefaultUrlService(System.Uri baseUri, string servicePrefix)
		{
			if(baseUri == null)
				throw new System.ArgumentNullException("baseUri");

			_baseUri = baseUri;
			ServicePrefix = servicePrefix;
		}

		/// <summary>
		/// Gets the Service Prefix that is appended to the Base Uri.
		/// </summary>
		public string ServicePrefix { get; private set; }

		#region IUrlService Members

		string IUrlService.BaseUrl { get { return _baseUri.ToString(); } }
		System.Uri IUrlService.BaseUri { get { return _baseUri; } }
		string IUrlService.GetUrl(string path)
		{
			if(path == null)
				return _baseUri.ToString();

			return string.Join("/", _baseUri.ToString().TrimEnd('/'), 
				     	 	path.Trim().TrimStart('/'));
		}
		System.Uri IUrlService.GetUri(string path)
		{
			if(path == null)
				return _baseUri;

			return new System.Uri(string.Join("/", _baseUri.ToString().TrimEnd('/'),
							       path.Trim().TrimStart('/')));
		}
		string IUrlService.GetServiceUrl(string path)
		{
			string baseUrl = _baseUri == null ? string.Empty : _baseUri.ToString().TrimEnd('/'); 
			string servicePrefix = string.IsNullOrWhiteSpace(ServicePrefix) ? string.Empty : "/" + ServicePrefix.Trim().Trim('/');
			string newPath = string.IsNullOrWhiteSpace(path) ? string.Empty : "/" + path.Trim().TrimStart('/');

			return string.Join("", baseUrl, servicePrefix, newPath);
		}
		System.Uri IUrlService.GetServiceUri(string path)
		{
			string baseUrl = _baseUri == null ? string.Empty : _baseUri.ToString().TrimEnd('/'); 
			string servicePrefix = string.IsNullOrWhiteSpace(ServicePrefix) ? string.Empty : "/" + ServicePrefix.Trim().Trim('/');
			string newPath = string.IsNullOrWhiteSpace(path) ? string.Empty : "/" + path.Trim().TrimStart('/');

			return new System.Uri(string.Join("", baseUrl, servicePrefix, newPath));
		}

		#endregion
	}
}
