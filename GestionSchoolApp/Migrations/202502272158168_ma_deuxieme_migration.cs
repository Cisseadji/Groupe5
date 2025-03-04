namespace GestionSchoolApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ma_deuxieme_migration : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ProfesseursClasses",
                c => new
                    {
                        ProfesseurId = c.Int(nullable: false),
                        ClasseId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.ProfesseurId, t.ClasseId })
                .ForeignKey("dbo.Professeurs", t => t.ProfesseurId, cascadeDelete: true)
                .ForeignKey("dbo.Classes", t => t.ClasseId, cascadeDelete: true)
                .Index(t => t.ProfesseurId)
                .Index(t => t.ClasseId);
            
            CreateTable(
                "dbo.ProfesseursMatieres",
                c => new
                    {
                        ProfesseurId = c.Int(nullable: false),
                        MatiereId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.ProfesseurId, t.MatiereId })
                .ForeignKey("dbo.Professeurs", t => t.ProfesseurId, cascadeDelete: true)
                .ForeignKey("dbo.Matieres", t => t.MatiereId, cascadeDelete: true)
                .Index(t => t.ProfesseurId)
                .Index(t => t.MatiereId);
            
            CreateTable(
                "dbo.CoursMatieres",
                c => new
                    {
                        CoursId = c.Int(nullable: false),
                        MatiereId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.CoursId, t.MatiereId })
                .ForeignKey("dbo.Cours", t => t.CoursId, cascadeDelete: true)
                .ForeignKey("dbo.Matieres", t => t.MatiereId, cascadeDelete: true)
                .Index(t => t.CoursId)
                .Index(t => t.MatiereId);
            
            CreateTable(
                "dbo.ClasseCours",
                c => new
                    {
                        ClasseId = c.Int(nullable: false),
                        CoursId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.ClasseId, t.CoursId })
                .ForeignKey("dbo.Classes", t => t.ClasseId, cascadeDelete: true)
                .ForeignKey("dbo.Cours", t => t.CoursId, cascadeDelete: true)
                .Index(t => t.ClasseId)
                .Index(t => t.CoursId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ClasseCours", "CoursId", "dbo.Cours");
            DropForeignKey("dbo.ClasseCours", "ClasseId", "dbo.Classes");
            DropForeignKey("dbo.CoursMatieres", "MatiereId", "dbo.Matieres");
            DropForeignKey("dbo.CoursMatieres", "CoursId", "dbo.Cours");
            DropForeignKey("dbo.ProfesseursMatieres", "MatiereId", "dbo.Matieres");
            DropForeignKey("dbo.ProfesseursMatieres", "ProfesseurId", "dbo.Professeurs");
            DropForeignKey("dbo.ProfesseursClasses", "ClasseId", "dbo.Classes");
            DropForeignKey("dbo.ProfesseursClasses", "ProfesseurId", "dbo.Professeurs");
            DropIndex("dbo.ClasseCours", new[] { "CoursId" });
            DropIndex("dbo.ClasseCours", new[] { "ClasseId" });
            DropIndex("dbo.CoursMatieres", new[] { "MatiereId" });
            DropIndex("dbo.CoursMatieres", new[] { "CoursId" });
            DropIndex("dbo.ProfesseursMatieres", new[] { "MatiereId" });
            DropIndex("dbo.ProfesseursMatieres", new[] { "ProfesseurId" });
            DropIndex("dbo.ProfesseursClasses", new[] { "ClasseId" });
            DropIndex("dbo.ProfesseursClasses", new[] { "ProfesseurId" });
            DropTable("dbo.ClasseCours");
            DropTable("dbo.CoursMatieres");
            DropTable("dbo.ProfesseursMatieres");
            DropTable("dbo.ProfesseursClasses");
        }
    }
}
