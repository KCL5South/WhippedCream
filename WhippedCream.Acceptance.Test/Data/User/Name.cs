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
    
    public partial class Name
    {
        public string First { get; set; }
        public string Middle { get; set; }
        public string Last { get; set; }
        public string Suffix { get; set; }
        public string Prefix { get; set; }
    
    	#region Members overriden from System.Object
    
    	public override bool Equals(object obj)
    	{
    		return obj is Name ? Equals(obj as Name) : false;
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
    			hash = (hash * 16777619) ^ (First == null ? 0 : First.GetHashCode());
    			hash = (hash * 16777619) ^ (Middle == null ? 0 : Middle.GetHashCode());
    			hash = (hash * 16777619) ^ (Last == null ? 0 : Last.GetHashCode());
    			hash = (hash * 16777619) ^ (Suffix == null ? 0 : Suffix.GetHashCode());
    			hash = (hash * 16777619) ^ (Prefix == null ? 0 : Prefix.GetHashCode());
    		}
    		return hash;
    	}
    
    	public override string ToString()
    	{
    		return base.ToString();
    	}
    
    	#endregion
    
    	#region IEquatable<Name> Members
    
    	public bool Equals(Name rhs)
    	{
    		if(rhs == null)
    			return false;
    
    		return System.Collections.Generic.EqualityComparer<Name>.Equals(First, rhs.First) && System.Collections.Generic.EqualityComparer<Name>.Equals(Middle, rhs.Middle) && System.Collections.Generic.EqualityComparer<Name>.Equals(Last, rhs.Last) && System.Collections.Generic.EqualityComparer<Name>.Equals(Suffix, rhs.Suffix) && System.Collections.Generic.EqualityComparer<Name>.Equals(Prefix, rhs.Prefix);
    	}
    
    	#endregion
    }
}
