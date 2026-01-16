namespace Core.Models
{
    public abstract class BaseEntity<T>
    {
            public T Id { get; set; }
            public BaseEntity()
            {
                Deleted = false;
                DateCreated = DateTime.Now;
            }
            public bool Deleted { get; set; }
            public DateTime DateCreated { get; set; }
    }
        /// <summary>
        /// Since most of our models have a key of <see cref="long"/>, it will be nice if we create a base for all of them.
        /// </summary>
        public abstract class BaseEntity : BaseEntity<long>
        {

        }
}
