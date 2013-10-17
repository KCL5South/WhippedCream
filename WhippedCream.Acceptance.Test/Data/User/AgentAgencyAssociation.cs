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
    [System.Data.Services.Common.DataServiceKey("AssociationId")]
    public partial class AgentAgencyAssociation : System.IEquatable<AgentAgencyAssociation>
    {
        public System.Guid AssociationId { get; set; }
        public string AgentId { get; set; }
        public string AgencyId { get; set; }
    
    	#region Members overriden from System.Object
    
    	public override bool Equals(object obj)
    	{
    		return obj is AgentAgencyAssociation ? Equals(obj as AgentAgencyAssociation) : false;
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
    			hash = (hash * 16777619) ^ AssociationId.GetHashCode();
    			hash = (hash * 16777619) ^ (AgentId == null ? 0 : AgentId.GetHashCode());
    			hash = (hash * 16777619) ^ (AgencyId == null ? 0 : AgencyId.GetHashCode());
    		}
    		return hash;
    	}
    
    	public override string ToString()
    	{
    		return base.ToString();
    	}
    
    	#endregion
    
    	#region IEquatable<AgentAgencyAssociation> Members
    
    	public bool Equals(AgentAgencyAssociation rhs)
    	{
    		if(rhs == null)
    			return false;
    			
    		return System.Collections.Generic.EqualityComparer<AgentAgencyAssociation>.Equals(AssociationId, rhs.AssociationId) && System.Collections.Generic.EqualityComparer<AgentAgencyAssociation>.Equals(AgentId, rhs.AgentId) && System.Collections.Generic.EqualityComparer<AgentAgencyAssociation>.Equals(AgencyId, rhs.AgencyId);
    	}
    
    	#endregion
    }
}
