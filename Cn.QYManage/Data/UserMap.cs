using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using Cn.QYManage.Common;

namespace Cn.QYManage.Data
{
    public class UserMap : EntityTypeConfiguration<User>
    {
        //public UserMap(){
        //    this.HasRequired(t => t.Roles)
        //    .WithRequiredDependent(t => t.User);
        //}
        
    }

}