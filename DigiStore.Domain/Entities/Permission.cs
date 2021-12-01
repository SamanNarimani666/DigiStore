using System;
using System.Collections.Generic;

#nullable disable

namespace DigiStore.Domain.Entities
{
    public partial class Permission
    {
        public Permission()
        {
            InverseParent = new HashSet<Permission>();
            RolePermissions = new HashSet<RolePermission>();
        }

        public int PermissionId { get; set; }
        public string PermissionTitle { get; set; }
        public int? ParentId { get; set; }

        public virtual Permission Parent { get; set; }
        public virtual ICollection<Permission> InverseParent { get; set; }
        public virtual ICollection<RolePermission> RolePermissions { get; set; }
    }
}
