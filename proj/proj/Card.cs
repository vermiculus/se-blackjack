using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace proj
{
    internal class Card
    {
        // override object.Equals
        public override bool Equals (object obj)
        {
            if (obj == null || GetType() != obj.GetType()) 
            {
                return false;
            }
            // TODO: write your implementation of Equals() here
            throw new NotImplementedException();
            return base.Equals(obj);    
        }
    
        // override object.GetHashCode
        public override int GetHashCode()
        {
            // TODO: write your implementation of GetHashCode() here
            throw new NotImplementedException();
            return base.GetHashCode();
        }
        }
}
