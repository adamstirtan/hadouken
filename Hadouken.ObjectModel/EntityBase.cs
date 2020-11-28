using System;

namespace Hadouken.ObjectModel
{
    public abstract class EntityBase
    {
        public int Id { get; set; }

        public DateTime Created { get; set; }
    }
}