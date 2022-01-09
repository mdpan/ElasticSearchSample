using System;
using System.Collections.Generic;
using UPubPlat.EduKoumu.Functions;
using UPubPlat.EduKoumu.Models;

namespace ESIndexingService.Indexers.Database
{
    public class DBClient
    {
        private readonly IPdfToolFunction pdfFunc = null;

        public DBClient()
        {
            string dataDir = AppDomain.CurrentDomain.BaseDirectory;
            if (dataDir.EndsWith(@"\bin\Debug\")
            || dataDir.EndsWith(@"\bin\Release\"))
            {
                dataDir = System.IO.Directory.GetParent(dataDir).Parent.Parent.FullName + "\\App_Data";
                AppDomain.CurrentDomain.SetData("DataDirectory", dataDir);
            }

            this.pdfFunc = new PdfToolFunction();

            var db = new UPubPlatEduKoumuDbContext();

            pdfFunc.KoumuDb = db;
        }

        public IList<PdfSubDocModel> GetPdfSubDocs()
        {
            var pdfDoclist = this.pdfFunc.GetPdfDocList(null);

            var pdfSubDoclist = this.pdfFunc.GetPdfSubDocList(null);

            return pdfSubDoclist;
        }

        public string[] GetUserRoles(int userId)
        {
            return new[] { "role1", "role2" };
        }
    }
}
