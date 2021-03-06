//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
namespace WhippedCream.Data.User
{
    
    [System.Data.Services.Common.DataServiceEntity]
    [System.Data.Services.Common.DataServiceKey("UserId", "EMailAddressId")]
    public partial class EMailAddress : System.IEquatable<EMailAddress>
    {
        public System.Guid UserId { get; set; }
        public System.Guid EMailAddressId { get; set; }
        public string Address { get; set; }
        public int Type { get; set; }
    
        public virtual User User { get; set; }
    
    	#region Members overriden from System.Object
    
    	public override bool Equals(object obj)
    	{
    		return obj is EMailAddress ? Equals(obj as EMailAddress) : false;
    	}
    
    	/// <summary>
    	/// The Implementation of this algorithm was taken
    	/// from [here](http://www.isthe.com/chongo/tech/comp/fnv/).
    	/// </summary>
    	public override int GetHashCode()
    	{
    		int hash = 13;
    		unchecked
    		{
    			hash = (hash * 16777619) ^ UserId.GetHashCode();
    			hash = (hash * 16777619) ^ EMailAddressId.GetHashCode();
    			hash = (hash * 16777619) ^ (Address == null ? 0 : Address.GetHashCode());
    			hash = (hash * 16777619) ^ Type.GetHashCode();
    		}
    		return hash;
    	}
    
    	public override string ToString()
    	{
    		return base.ToString();
    	}
    
    	#endregion
    
    	#region IEquatable<EMailAddress> Members
    
    	public bool Equals(EMailAddress rhs)
    	{
    		if(rhs == null)
    			return false;
    			
    		return System.Collections.Generic.EqualityComparer<EMailAddress>.Equals(UserId, rhs.UserId) && System.Collections.Generic.EqualityComparer<EMailAddress>.Equals(EMailAddressId, rhs.EMailAddressId) && System.Collections.Generic.EqualityComparer<EMailAddress>.Equals(Address, rhs.Address) && System.Collections.Generic.EqualityComparer<EMailAddress>.Equals(Type, rhs.Type);
    	}
    
    	#endregion
    }
}
