using System;
using System.Collections.Generic;

#nullable disable

namespace DigiStore.Domain.Entities
{
    public partial class User
    {
        public User()
        {
            Addresses = new HashSet<Address>();
            FavoriteProductUsers = new HashSet<FavoriteProductUser>();
            ProductDiscountUses = new HashSet<ProductDiscountUse>();
            ProductVisiteds = new HashSet<ProductVisited>();
            Productcomments = new HashSet<Productcomment>();
            SalesInforamtions = new HashSet<SalesInforamtion>();
            SalesOrderHeaders = new HashSet<SalesOrderHeader>();
            Sellers = new HashSet<Seller>();
            TicketMessages = new HashSet<TicketMessage>();
            Tickets = new HashSet<Ticket>();
            UserRoles = new HashSet<UserRole>();
        }

        public int UserId { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string PassWord { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string FullName { get; set; }
        public string ActiveCode { get; set; }
        public string UserAvatar { get; set; }
        public string Mobile { get; set; }
        public bool IsActive { get; set; }
        public bool IsBlock { get; set; }
        public bool IsDelete { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime ModifiedDate { get; set; }
        public Guid Rowguid { get; set; }

        public virtual ICollection<Address> Addresses { get; set; }
        public virtual ICollection<FavoriteProductUser> FavoriteProductUsers { get; set; }
        public virtual ICollection<ProductDiscountUse> ProductDiscountUses { get; set; }
        public virtual ICollection<ProductVisited> ProductVisiteds { get; set; }
        public virtual ICollection<Productcomment> Productcomments { get; set; }
        public virtual ICollection<SalesInforamtion> SalesInforamtions { get; set; }
        public virtual ICollection<SalesOrderHeader> SalesOrderHeaders { get; set; }
        public virtual ICollection<Seller> Sellers { get; set; }
        public virtual ICollection<TicketMessage> TicketMessages { get; set; }
        public virtual ICollection<Ticket> Tickets { get; set; }
        public virtual ICollection<UserRole> UserRoles { get; set; }
    }
}
