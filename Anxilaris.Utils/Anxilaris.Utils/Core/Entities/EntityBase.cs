namespace Anxilaris.Utils.Core.Entities
{
    using System;
    public class EntityBase : ValidatorBase, ICloneable
    {
        /// <summary>
        /// Base Id
        /// </summary>
        public long Id { get; set; }

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
        public T Clone<T>()
        {
            return (T)this.Clone();
        }
    }
}
