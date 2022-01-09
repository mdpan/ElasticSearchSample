namespace UPubPlat.EduKoumu.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class PS_PdfDocSub
    {
        [Key]
        public Guid PdfDocSubId { get; set; }

        public Guid PdfDocId { get; set; }

        [Required]
        [StringLength(10)]
        public string TenantId { get; set; }

        [Required]
        [StringLength(15)]
        public string ApplicationCode { get; set; }

        public string ShareKey { get; set; }

        public string ShareMemo { get; set; }

        public int Hyojijun { get; set; }

        [StringLength(400)]
        public string FilePath { get; set; }

        [StringLength(400)]
        public string SPFilePath { get; set; }

        [StringLength(400)]
        public string SPOnlineFilePath { get; set; }

        [StringLength(40)]
        public string SPFileId { get; set; }

        [Required]
        [StringLength(255)]
        public string UploadFileName { get; set; }

        [Required]
        [StringLength(255)]
        public string FileName { get; set; }

        public bool PdfFlg { get; set; }

        public bool PdfAFlg { get; set; }

        public bool SignFlg { get; set; }

        public byte[] PdfData { get; set; }

        public byte[] PdfAData { get; set; }

        public int SubPageCount { get; set; }

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
