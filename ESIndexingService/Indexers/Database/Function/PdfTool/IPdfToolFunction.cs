using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UPubPlat.EduKoumu.Models;

namespace UPubPlat.EduKoumu.Functions
{
    public interface IPdfToolFunction 
    {
        UPubPlatEduKoumuDbContext KoumuDb { get; set; }
        /// <summary>
        /// PDFファイル取得
        /// </summary>
        /// <param name="searchCond">検索条件Model</param>
        /// <returns>PDFファイル一覧Modelリスト</returns>
        IList<PdfDocModel> GetPdfDocList(PdfDocSearchModel searchCond);

        /// <summary>
        /// PDF_Subファイル取得
        /// </summary>
        /// <param name="searchCond">検索条件Model</param>
        /// <returns>PDF_Subファイル一覧Modelリスト</returns>
        IList<PdfSubDocModel> GetPdfSubDocList(PdfSubDocSearchModel searchCond);
    }
}
