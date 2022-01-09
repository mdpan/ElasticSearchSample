namespace UPubPlat.EduKoumu.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class PS_PdfDoc
    {
        [Key]
        public Guid PdfDocId { get; set; }

        [Required]
        [StringLength(10)]
        public string TenantId { get; set; }

        [Required]
        [StringLength(15)]
        public string ApplicationCode { get; set; }

        [StringLength(int.MaxValue)]
        public string ShareKey { get; set; }

        [StringLength(int.MaxValue)]
        public string ShareMemo { get; set; }

        [StringLength(255)]
        public string PdfDocTitle { get; set; }

        public int? FileCount { get; set; }

        public int? PageCount { get; set; }

        public int CrtUserId { get; set; }

        [StringLength(205)]
        public string CrtUserName { get; set; }

        public DateTime CrtDateTime { get; set; }

        public int UpdUserId { get; set; }

        [StringLength(205)]
        public string UpdUserName { get; set; }

        public DateTime UpdDateTime { get; set; }

        public bool DeleteFlg { get; set; }

        [Column(TypeName = "timestamp")]
        [MaxLength(8)]
        [Timestamp]
        public byte[] TIMESTAMP { get; set; }
    }
}
