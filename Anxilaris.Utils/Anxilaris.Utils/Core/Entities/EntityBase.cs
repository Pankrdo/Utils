namespace Anxilaris.Utils.Core.Entities
{
    using System;
    public class EntityBase<T> : ValidatorBase, ICloneable
    {
        /// <summary>
        /// Base Id
        /// </summary>
        public T Id { get; set; }

        /// <summary>
        /// Item Activw
        /// </summary>
        public bool Active { get; set; }

        /// <summary>
        /// Clone object
        /// </summary>
        /// <returns></returns>
        public object Clone()
        {
            return this.MemberwiseClone();
        }

        /// <summary>
        /// Clone object to specific type
        /// </summary>
        /// <typeparam name="T">return type</typeparam>
        /// <returns></returns>
        public UType Clone<UType>()
        {
            return (UType)this.Clone();
        }
    }
}
