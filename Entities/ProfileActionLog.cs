namespace Ames.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("ProfileActionLog")]
    public partial class ProfileActionLog
    {
        [Key]
        public int ProfileID { get; set; }

        public DateTime? DateTime { get; set; }

        [StringLength(30)]
        public string UserName { get; set; }

        [StringLength(150)]
        public string Module { get; set; }

        [StringLength(150)]
        public string Controller { get; set; }

        [StringLength(150)]
        public string Action { get; set; }

        [StringLength(150)]
        public string BrowserType { get; set; }

        [StringLength(300)]
        public string MachineName { get; set; }

        [StringLength(2000)]
        public string AbsoluteURL { get; set; }

        public decimal? ActionDuration { get; set; }

        public decimal? ResultDuration { get; set; }

        public string ErrorMessage { get; set; }
    }
}
