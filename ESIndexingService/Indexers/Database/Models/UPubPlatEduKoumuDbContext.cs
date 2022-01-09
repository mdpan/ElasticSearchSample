namespace UPubPlat.EduKoumu.Models
{
    using log4net;
    using System;
    using System.Data.Entity;
    using System.Linq;
    using System.Reflection;

    public partial class UPubPlatEduKoumuDbContext : DbContext
    {

        /// <summary> ロガー（デバックログ用）</summary>
        private static readonly ILog logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public UPubPlatEduKoumuDbContext() : base("name=UPubPlatBaseDb3")
        {
            //SQL出力関数を登録
            base.Database.Log = SqlLogger;
        }

        /// <summary>
        /// SQLログの出力
        /// </summary>
        /// <param name="sql"></param>
        private void SqlLogger(string sql)
        {
            logger.Debug(sql);
        }


        /// <summary>
        /// SaveChangesの実行
        /// </summary>
        public override int SaveChanges()
        {

            var entities = ChangeTracker.Entries().Where(x => (x.State == EntityState.Added || x.State == EntityState.Deleted || x.State == EntityState.Modified));


            return base.SaveChanges();
        }

        #region DBSet


        public virtual DbSet<PS_PdfDoc> PS_PdfDoc { get; set; }
        public virtual DbSet<PS_PdfDocSub> PS_PdfDocSub { get; set; }

        #endregion // DBSet

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            #region modelCreating



            //Database.SetInitializer(new DropCreateDatabaseIfModelChanges<UPubPlatEduKoumuDbContext>());
            //------------------------------------------------------
            // アンケート
            //------------------------------------------------------

            modelBuilder.Entity<PS_PdfDoc>()
                .Property(e => e.TenantId)
                .IsUnicode(false);

            modelBuilder.Entity<PS_PdfDoc>()
                .Property(e => e.ApplicationCode)
                .IsUnicode(false);

            modelBuilder.Entity<PS_PdfDoc>()
                .Property(e => e.TIMESTAMP)
                .IsFixedLength();

            modelBuilder.Entity<PS_PdfDocSub>()
                .Property(e => e.TenantId)
                .IsUnicode(false);

            modelBuilder.Entity<PS_PdfDocSub>()
                .Property(e => e.ApplicationCode)
                .IsUnicode(false);

            modelBuilder.Entity<PS_PdfDocSub>()
                .Property(e => e.FilePath)
                .IsUnicode(false);

            modelBuilder.Entity<PS_PdfDocSub>()
                .Property(e => e.SPFilePath)
                .IsUnicode(false);

            modelBuilder.Entity<PS_PdfDocSub>()
                .Property(e => e.SPOnlineFilePath)
                .IsUnicode(false);

            modelBuilder.Entity<PS_PdfDocSub>()
                .Property(e => e.SPFileId)
                .IsUnicode(false);

            modelBuilder.Entity<PS_PdfDocSub>()
                .Property(e => e.UploadFileName)
                .IsUnicode(false);

            modelBuilder.Entity<PS_PdfDocSub>()
                .Property(e => e.FileName)
                .IsUnicode(false);

            modelBuilder.Entity<PS_PdfDocSub>()
                .Property(e => e.TIMESTAMP)
                .IsFixedLength();

            //Database.SetInitializer(new DropCreateDatabaseIfModelChanges<DbContextClass>());

            base.OnModelCreating(modelBuilder);
            #endregion // modelCreating
        }
    }
}
