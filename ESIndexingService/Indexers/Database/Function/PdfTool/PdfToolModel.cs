using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace UPubPlat.EduKoumu.Functions
{

    /// <summary>
    /// PDF文書検索条件Model
    /// </summary>
    public class PdfDocSearchModel 
    {
        public string PdfDocId { get; set; }
        /// <summary>
        /// お客様ID
        /// </summary>
        public string TenantId { get; set; }

    }
    /// <summary>
    /// PDFDLLModel
    /// </summary>
    public class PdfDocBaseModel
    {
        public string TenantId { get; set; }
        public string GyoumuCd { get; set; }
        public string LoginId { get; set; }

    }

    /// <summary>
    /// PDF文書
    /// </summary>
    public class PdfDocModel
    {
        public string PdfDocId { get; set; }

        public string TenantId { get; set; }

        public string ApplicationCode { get; set; }
        public string ShareKey { get; set; }
        public string ShareMemo { get; set; }        

        public string Title { get; set; }

        public int? FileCount { get; set; }

        public int? PageCount { get; set; }

        public int? CrtUserId { get; set; }
        
        public string CrtUserName { get; set; }

        public DateTime? CrtDateTime { get; set; }

        public int? UpdUserId { get; set; }
        
        public string UpdUserName { get; set; }

        public DateTime? UpdDateTime { get; set; }

        public bool? DeleteFlg { get; set; }
        
        public byte[] TIMESTAMP { get; set; }
    }

    /// <summary>
    /// PDFサブ文書
    /// </summary>
    public class PdfSubDocModel 
    {
        public string PdfDocSubId { get; set; }

        public string PdfDocId { get; set; }

        public string TenantId { get; set; }

        public string ApplicationCode { get; set; }

        public string ShareKey { get; set; }

        public string ShareMemo { get; set; }

        public int? Hyojijun { get; set; }

        public string FilePath { get; set; }

        public string SPFilePath { get; set; }

        public string SPOnlineFilePath { get; set; }

        public string SPFileId { get; set; }

        public string UploadFileName { get; set; }

        public string FileName { get; set; }

        public bool PdfFlg { get; set; }

        public bool PdfAFlg { get; set; }

        public bool SignFlg { get; set; }

        public byte[] PdfData { get; set; }

        public byte[] PdfAData { get; set; }

        public int? SubPageCount { get; set; }

        public int? CrtUserId { get; set; }

        public string CrtUserName { get; set; }

        public DateTime? CrtDateTime { get; set; }

        public int? UpdUserId { get; set; }

        public string UpdUserName { get; set; }

        public DateTime? UpdDateTime { get; set; }

        public bool? DeleteFlg { get; set; }

        public byte[] TIMESTAMP { get; set; }
    }

    /// <summary>
    /// PDFサブ文書検索条件Model
    /// </summary>
    public class PdfSubDocSearchModel 
    {
        public string PdfDocSubId { get; set; }

        public string PdfDocId { get; set; }

        public int? Hyojijun { get; set; }

        public string TenantId { get; set; }

        public bool? PdfFlg { get; set; }

    }

}