using System;
using System.Collections.Generic;

namespace KloudCodingChallenge.Model.Interfaces
{
    public interface IOwner
    {
        string Name { get; set; }
        List<ICar> Cars { get; set; }
    }
}
