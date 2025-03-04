namespace GestionSchoolApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ma_premiere_migration : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Classes",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        nomClasse = c.String(),
                    })
                .PrimaryKey(t => t.id);
            
            CreateTable(
                "dbo.Cours",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        nomCours = c.String(),
                        description = c.String(),
                    })
                .PrimaryKey(t => t.id);
            
            CreateTable(
                "dbo.Etudiants",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        matricule = c.String(),
                        nom = c.String(),
                        prenom = c.String(),
                        datenaiss = c.DateTime(nullable: false),
                        sexe = c.String(),
                        adresse = c.String(),
                        tel = c.String(),
                        email = c.String(),
                        idC = c.Int(nullable: false),
                        classe_id = c.Int(),
                    })
                .PrimaryKey(t => t.id)
                .ForeignKey("dbo.Classes", t => t.classe_id)
                .Index(t => t.classe_id);
            
            CreateTable(
                "dbo.Matieres",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        nomMatiere = c.String(),
                    })
                .PrimaryKey(t => t.id);
            
            CreateTable(
                "dbo.Notes",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        idE = c.Int(nullable: false),
                        idM = c.Int(nullable: false),
                        note = c.Single(nullable: false),
                        etudiant_id = c.Int(),
                        matiere_id = c.Int(),
                    })
                .PrimaryKey(t => t.id)
                .ForeignKey("dbo.Etudiants", t => t.etudiant_id)
                .ForeignKey("dbo.Matieres", t => t.matiere_id)
                .Index(t => t.etudiant_id)
                .Index(t => t.matiere_id);
            
            CreateTable(
                "dbo.OTPCodes",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        idU = c.Int(nullable: false),
                        code = c.String(),
                        dateExp = c.DateTime(nullable: false),
                        utilisateur_id = c.Int(),
                    })
                .PrimaryKey(t => t.id)
                .ForeignKey("dbo.Utilisateurs", t => t.utilisateur_id)
                .Index(t => t.utilisateur_id);
            
            CreateTable(
                "dbo.Utilisateurs",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        nomUser = c.String(),
                        mdp = c.String(),
                        role = c.String(),
                        tel = c.String(),
                    })
                .PrimaryKey(t => t.id);
            
            CreateTable(
                "dbo.Professeurs",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        nom = c.String(),
                        prenom = c.String(),
                        email = c.String(),
                        tel = c.String(),
                    })
                .PrimaryKey(t => t.id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.OTPCodes", "utilisateur_id", "dbo.Utilisateurs");
            DropForeignKey("dbo.Notes", "matiere_id", "dbo.Matieres");
            DropForeignKey("dbo.Notes", "etudiant_id", "dbo.Etudiants");
            DropForeignKey("dbo.Etudiants", "classe_id", "dbo.Classes");
            DropIndex("dbo.OTPCodes", new[] { "utilisateur_id" });
            DropIndex("dbo.Notes", new[] { "matiere_id" });
            DropIndex("dbo.Notes", new[] { "etudiant_id" });
            DropIndex("dbo.Etudiants", new[] { "classe_id" });
            DropTable("dbo.Professeurs");
            DropTable("dbo.Utilisateurs");
            DropTable("dbo.OTPCodes");
            DropTable("dbo.Notes");
            DropTable("dbo.Matieres");
            DropTable("dbo.Etudiants");
            DropTable("dbo.Cours");
            DropTable("dbo.Classes");
        }
    }
}
