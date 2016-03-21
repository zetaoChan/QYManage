using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using Wy.Hr.Common;

namespace Wy.Hr.Data
{
    public class UserMap : EntityTypeConfiguration<User>
    {
        //public UserMap(){
        //    this.HasRequired(t => t.Roles)
        //    .WithRequiredDependent(t => t.User);
        //}
        
    }

}