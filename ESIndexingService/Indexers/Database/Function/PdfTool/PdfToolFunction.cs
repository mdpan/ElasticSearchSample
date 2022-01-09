using System;
using System.Collections.Generic;
using System.Linq;
using UPubPlat.EduKoumu.Models;

namespace UPubPlat.EduKoumu.Functions
{
    public class PdfToolFunction : IPdfToolFunction
    {
        private string ProcResult;                              // 成功 or 失敗
        private int ProcCount;                                  // 処理結果数

        /// <summary>
        /// DBコンテキストオブジェクト格納用プロパティ
        /// </summary>
        public UPubPlatEduKoumuDbContext KoumuDb { get; set; }



        /// <summary>
        /// 初期化
        /// </summary>
        private void Initialization()
        {
            ProcCount = 0;
        }


        ///PS_PdfDoc

        /// <summary>
        /// PDFファイル複数取得
        /// </summary>
        /// <param name="searchCond">検索条件Model</param>
        /// <returns>PDFファイル一覧Modelリスト</returns>
        public IList<PdfDocModel> GetPdfDocList(PdfDocSearchModel searchCond)
        {
            IList<PdfDocModel> modelList = new List<PdfDocModel>();


            try
            {
                // 処理結果を初期化
                Initialization();

                // メイン処理：Modelリスト取得
                modelList = GetPdfDocListProc(searchCond);
            }
            finally
            {
                
            }

            return modelList;
        }

        /// <summary>
        /// PDFファイル複数取得
        /// </summary>
        /// <param name="searchCond">検索条件Model</param>
        /// <returns>PDFファイル一覧Modelリスト</returns>
        private IList<PdfDocModel> GetPdfDocListProc(PdfDocSearchModel searchCond)
        {
            IList<PdfDocModel> modelList = new List<PdfDocModel>();

            IQueryable<PS_PdfDoc> entityList = KoumuDb.PS_PdfDoc;

            foreach (var entity in entityList)
            {
                PdfDocModel Models = new PdfDocModel();
                Models.PdfDocId = entity.PdfDocId.ToString();
                Models.TenantId = entity.TenantId;
                //Models.DocId = entity.DocId;
                Models.Title = entity.PdfDocTitle;
                Models.ApplicationCode = entity.ApplicationCode;
                Models.ShareKey = entity.ShareKey;
                Models.ShareMemo = entity.ShareMemo;

                Models.FileCount = entity.FileCount;
                Models.PageCount = entity.PageCount;
                Models.CrtUserId = entity.CrtUserId;
                Models.CrtUserName = entity.CrtUserName;
                Models.CrtDateTime = entity.CrtDateTime;
                Models.UpdUserId = entity.UpdUserId;
                Models.UpdUserName = entity.UpdUserName;
                Models.UpdDateTime = entity.UpdDateTime;
                Models.DeleteFlg = entity.DeleteFlg;
                Models.TIMESTAMP = entity.TIMESTAMP;

                modelList.Add(Models);
            }
            return modelList;
        }



        /// <summary>
        /// PDF_Subファイル複数取得
        /// </summary>
        /// <param name="searchCond">検索条件Model</param>
        /// <returns>PDF_Subファイル一覧Modelリスト</returns>
        public IList<PdfSubDocModel> GetPdfSubDocList(PdfSubDocSearchModel searchCond)
        {
            IList<PdfSubDocModel> modelList = new List<PdfSubDocModel>();


            try
            {
                // 処理結果を初期化
                Initialization();

                // メイン処理：Modelリスト取得
                modelList = GetPdfSubDocListProc(searchCond);
            }
            finally
            {
            }

            return modelList;
        }

        /// <summary>
        /// PDF_Subファイル複数取得
        /// </summary>
        /// <param name="searchCond">検索条件Model</param>
        /// <returns>PDF_Subファイル一覧Modelリスト</returns>
        private IList<PdfSubDocModel> GetPdfSubDocListProc(PdfSubDocSearchModel searchCond)
        {
            IList<PdfSubDocModel> modelList = new List<PdfSubDocModel>();

            IQueryable<PS_PdfDocSub> entityList = KoumuDb.PS_PdfDocSub;
            // Id
            
            entityList = entityList.OrderBy(x => x.Hyojijun);
            foreach (var entity in entityList)
            {
                PdfSubDocModel Models = new PdfSubDocModel();
                Models.PdfDocSubId = entity.PdfDocSubId.ToString();
                Models.PdfDocId = entity.PdfDocId.ToString();
                Models.TenantId = entity.TenantId;
                Models.ApplicationCode = entity.ApplicationCode;
                Models.ShareKey = entity.ShareKey;
                Models.ShareMemo = entity.ShareMemo;
                Models.Hyojijun = entity.Hyojijun;
                Models.FilePath = entity.FilePath;
                Models.SPFilePath = entity.SPFilePath;
                Models.SPOnlineFilePath = entity.SPOnlineFilePath;
                Models.SPFileId = entity.SPFileId;
                Models.UploadFileName = entity.UploadFileName;
                Models.FileName = entity.FileName;
                Models.PdfData = entity.PdfData;
                Models.PdfAData = entity.PdfAData;
                Models.PdfFlg = entity.PdfFlg;
                Models.PdfAFlg = entity.PdfAFlg;
                Models.SignFlg = entity.SignFlg;
                Models.SubPageCount = entity.SubPageCount;
                Models.CrtUserId = entity.CrtUserId;
                Models.CrtUserName = entity.CrtUserName;
                Models.CrtDateTime = entity.CrtDateTime;
                Models.UpdUserId = entity.UpdUserId;
                Models.UpdUserName = entity.UpdUserName;
                Models.UpdDateTime = entity.UpdDateTime;
                Models.DeleteFlg = entity.DeleteFlg;
                Models.TIMESTAMP = entity.TIMESTAMP;

                modelList.Add(Models);
            }
            return modelList;
        }

    }
}