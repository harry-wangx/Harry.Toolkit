//using System;

//namespace Harry.Domain
//{
//    public abstract class Entity<TId> 
//    {
//        public TId Id { get; set; }

//        public int Version { get; set; }

//        public override bool Equals(object obj)
//        {
//            var entity = obj as Entity<TId>;
//            if (entity == null)
//            {
//                return false;
//            }
//            return this.Equals(entity);
//        }

//        public override int GetHashCode()
//        {
//            return this.Id.GetHashCode();
//        }

//        public virtual bool Equals(Entity<TId> other)
//        {
//            if (other == null)
//            {
//                return false;
//            }
//            return this.Id.Equals(other.Id);
//        }
//    }
//}
