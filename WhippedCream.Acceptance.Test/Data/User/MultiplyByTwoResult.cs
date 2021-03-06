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
    [System.Data.Services.Common.DataServiceKey("Id")]
    public partial class MultiplyByTwoResult : System.IEquatable<MultiplyByTwoResult>
    {
        public System.Guid Id { get; set; }
        public int Result { get; set; }
    
    	#region Members overriden from System.Object
    
    	public override bool Equals(object obj)
    	{
    		return obj is MultiplyByTwoResult ? Equals(obj as MultiplyByTwoResult) : false;
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
    			hash = (hash * 16777619) ^ Id.GetHashCode();
    			hash = (hash * 16777619) ^ Result.GetHashCode();
    		}
    		return hash;
    	}
    
    	public override string ToString()
    	{
    		return base.ToString();
    	}
    
    	#endregion
    
    	#region IEquatable<MultiplyByTwoResult> Members
    
    	public bool Equals(MultiplyByTwoResult rhs)
    	{
    		if(rhs == null)
    			return false;
    			
    		return System.Collections.Generic.EqualityComparer<MultiplyByTwoResult>.Equals(Id, rhs.Id) && System.Collections.Generic.EqualityComparer<MultiplyByTwoResult>.Equals(Result, rhs.Result);
    	}
    
    	#endregion
    }
}
