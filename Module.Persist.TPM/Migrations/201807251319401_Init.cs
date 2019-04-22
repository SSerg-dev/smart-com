namespace Module.Persist.TPM.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Init : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.AccessPointRole",
                c => new
                    {
                        Id = c.Guid(nullable: false, identity: true),
                        RoleId = c.Guid(nullable: false),
                        AccessPointId = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AccessPoint", t => t.AccessPointId)
                .ForeignKey("dbo.Role", t => t.RoleId)
                .Index(t => t.RoleId)
                .Index(t => t.AccessPointId);
            
            CreateTable(
                "dbo.AccessPoint",
                c => new
                    {
                        Id = c.Guid(nullable: false, identity: true),
                        Disabled = c.Boolean(nullable: false),
                        DeletedDate = c.DateTimeOffset(precision: 7),
                        Resource = c.String(nullable: false, maxLength: 256),
                        Action = c.String(nullable: false, maxLength: 256),
                        Description = c.String(maxLength: 256),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Role",
                c => new
                    {
                        Id = c.Guid(nullable: false, identity: true),
                        Disabled = c.Boolean(nullable: false),
                        DeletedDate = c.DateTimeOffset(precision: 7),
                        SystemName = c.String(nullable: false, maxLength: 50),
                        DisplayName = c.String(nullable: false, maxLength: 50),
                        IsAllow = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.ConstraintPrefix",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Description = c.String(maxLength: 256),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Constraint",
                c => new
                    {
                        Id = c.Guid(nullable: false, identity: true),
                        UserRoleId = c.Guid(nullable: false),
                        Prefix = c.String(maxLength: 400),
                        Value = c.String(maxLength: 400),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.UserRole", t => t.UserRoleId)
                .Index(t => t.UserRoleId)
                .Index(t => t.Prefix)
                .Index(t => t.Value);
            
            CreateTable(
                "dbo.UserRole",
                c => new
                    {
                        Id = c.Guid(nullable: false, identity: true),
                        UserId = c.Guid(nullable: false),
                        RoleId = c.Guid(nullable: false),
                        IsDefault = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Role", t => t.RoleId)
                .ForeignKey("dbo.User", t => t.UserId)
                .Index(t => t.UserId)
                .Index(t => t.RoleId);
            
            CreateTable(
                "dbo.User",
                c => new
                    {
                        Id = c.Guid(nullable: false, identity: true),
                        Disabled = c.Boolean(nullable: false),
                        DeletedDate = c.DateTimeOffset(precision: 7),
                        Sid = c.String(maxLength: 184),
                        Name = c.String(nullable: false, maxLength: 129),
                        Password = c.String(maxLength: 128),
                        PasswordSalt = c.String(maxLength: 128),
                        Email = c.String(maxLength: 129),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.CSVExtractInterfaceSetting",
                c => new
                    {
                        Id = c.Guid(nullable: false, identity: true),
                        InterfaceId = c.Guid(nullable: false),
                        FileNameMask = c.String(nullable: false, maxLength: 400),
                        Delimiter = c.String(maxLength: 400),
                        UseQuoting = c.Boolean(nullable: false),
                        QuoteChar = c.String(maxLength: 400),
                        ExtractHandler = c.String(nullable: false, maxLength: 400),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Interface", t => t.InterfaceId)
                .Index(t => t.InterfaceId)
                .Index(t => t.FileNameMask)
                .Index(t => t.Delimiter)
                .Index(t => t.QuoteChar)
                .Index(t => t.ExtractHandler);
            
            CreateTable(
                "dbo.Interface",
                c => new
                    {
                        Id = c.Guid(nullable: false, identity: true),
                        Name = c.String(maxLength: 400),
                        Direction = c.String(maxLength: 400),
                        Description = c.String(maxLength: 400),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Name)
                .Index(t => t.Direction)
                .Index(t => t.Description);
            
            CreateTable(
                "dbo.CSVProcessInterfaceSetting",
                c => new
                    {
                        Id = c.Guid(nullable: false, identity: true),
                        InterfaceId = c.Guid(nullable: false),
                        Delimiter = c.String(nullable: false, maxLength: 400),
                        UseQuoting = c.Boolean(nullable: false),
                        QuoteChar = c.String(maxLength: 400),
                        ProcessHandler = c.String(nullable: false, maxLength: 400),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Interface", t => t.InterfaceId)
                .Index(t => t.InterfaceId)
                .Index(t => t.Delimiter)
                .Index(t => t.QuoteChar)
                .Index(t => t.ProcessHandler);
            
            CreateTable(
                "dbo.FileBuffer",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        CreateDate = c.DateTimeOffset(nullable: false, precision: 7),
                        InterfaceId = c.Guid(nullable: false),
                        UserId = c.Guid(),
                        HandlerId = c.Guid(),
                        FileName = c.String(nullable: false, maxLength: 400),
                        Status = c.String(nullable: false, maxLength: 400),
                        ProcessDate = c.DateTimeOffset(precision: 7),
                    })
                .PrimaryKey(t => new { t.Id, t.CreateDate })
                .ForeignKey("dbo.Interface", t => t.InterfaceId)
                .Index(t => t.CreateDate)
                .Index(t => t.InterfaceId)
                .Index(t => t.FileName)
                .Index(t => t.Status)
                .Index(t => t.ProcessDate);
            
            CreateTable(
                "dbo.FileCollectInterfaceSetting",
                c => new
                    {
                        Id = c.Guid(nullable: false, identity: true),
                        InterfaceId = c.Guid(nullable: false),
                        SourcePath = c.String(nullable: false),
                        SourceFileMask = c.String(nullable: false),
                        CollectHandler = c.String(nullable: false, maxLength: 400),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Interface", t => t.InterfaceId)
                .Index(t => t.InterfaceId)
                .Index(t => t.CollectHandler);
            
            CreateTable(
                "dbo.FileSendInterfaceSetting",
                c => new
                    {
                        Id = c.Guid(nullable: false, identity: true),
                        InterfaceId = c.Guid(nullable: false),
                        TargetPath = c.String(nullable: false, maxLength: 400),
                        TargetFileMask = c.String(nullable: false, maxLength: 400),
                        SendHandler = c.String(nullable: false, maxLength: 400),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Interface", t => t.InterfaceId)
                .Index(t => t.InterfaceId)
                .Index(t => t.TargetPath)
                .Index(t => t.TargetFileMask)
                .Index(t => t.SendHandler);
            
            CreateTable(
                "dbo.GridSetting",
                c => new
                    {
                        Id = c.Guid(nullable: false, identity: true),
                        Key = c.String(nullable: false, maxLength: 512),
                        Value = c.String(nullable: false),
                        UserRoleId = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.UserRole", t => t.UserRoleId)
                .Index(t => t.UserRoleId);
            
            CreateTable(
                "dbo.Import",
                c => new
                    {
                        Id = c.Guid(nullable: false, identity: true),
                        ImportType = c.String(),
                        UserId = c.Guid(nullable: false),
                        RoleId = c.Guid(nullable: false),
                        ImportDate = c.DateTimeOffset(nullable: false, precision: 7),
                        Status = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Role", t => t.RoleId)
                .ForeignKey("dbo.User", t => t.UserId)
                .Index(t => t.UserId)
                .Index(t => t.RoleId);
            
            CreateTable(
                "dbo.LoopHandlerConstraint",
                c => new
                    {
                        Id = c.Guid(nullable: false, identity: true),
                        LoopHandlerId = c.Guid(nullable: false),
                        Prefix = c.String(maxLength: 400),
                        Value = c.String(maxLength: 400),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.LoopHandler", t => t.LoopHandlerId)
                .Index(t => t.LoopHandlerId)
                .Index(t => t.Prefix)
                .Index(t => t.Value);
            
            CreateTable(
                "dbo.LoopHandler",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Description = c.String(maxLength: 400),
                        Name = c.String(maxLength: 400),
                        ExecutionPeriod = c.Long(),
                        ExecutionMode = c.String(maxLength: 400),
                        CreateDate = c.DateTimeOffset(precision: 7),
                        LastExecutionDate = c.DateTimeOffset(precision: 7),
                        NextExecutionDate = c.DateTimeOffset(precision: 7),
                        ConfigurationName = c.String(nullable: false, maxLength: 400),
                        Status = c.String(maxLength: 400),
                        RunGroup = c.String(maxLength: 400),
                        UserId = c.Guid(),
                        RoleId = c.Guid(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Role", t => t.RoleId)
                .ForeignKey("dbo.User", t => t.UserId)
                .Index(t => t.Description)
                .Index(t => t.Name)
                .Index(t => t.ExecutionPeriod)
                .Index(t => t.ExecutionMode)
                .Index(t => t.CreateDate)
                .Index(t => t.LastExecutionDate)
                .Index(t => t.NextExecutionDate)
                .Index(t => t.ConfigurationName)
                .Index(t => t.Status)
                .Index(t => t.RunGroup)
                .Index(t => t.UserId)
                .Index(t => t.RoleId);
            
            CreateTable(
                "dbo.MailNotificationSetting",
                c => new
                    {
                        Id = c.Guid(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 50),
                        Description = c.String(maxLength: 256),
                        Subject = c.String(),
                        Body = c.String(),
                        IsDisabled = c.Boolean(nullable: false),
                        To = c.String(),
                        CC = c.String(),
                        BCC = c.String(),
                        Disabled = c.Boolean(nullable: false),
                        DeletedDate = c.DateTimeOffset(precision: 7),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Disabled)
                .Index(t => t.DeletedDate);
            
            CreateTable(
                "dbo.Recipient",
                c => new
                    {
                        Id = c.Guid(nullable: false, identity: true),
                        MailNotificationSettingId = c.Guid(nullable: false),
                        Type = c.String(maxLength: 20),
                        Value = c.String(maxLength: 400),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.MailNotificationSetting", t => t.MailNotificationSettingId)
                .Index(t => t.MailNotificationSettingId)
                .Index(t => t.Type)
                .Index(t => t.Value);
            
            CreateTable(
                "dbo.Setting",
                c => new
                    {
                        Id = c.Guid(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 50),
                        Type = c.String(nullable: false, maxLength: 50),
                        Value = c.String(maxLength: 256),
                        Description = c.String(maxLength: 256),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.XMLProcessInterfaceSetting",
                c => new
                    {
                        Id = c.Guid(nullable: false, identity: true),
                        InterfaceId = c.Guid(nullable: false),
                        RootElement = c.String(maxLength: 400),
                        ProcessHandler = c.String(nullable: false, maxLength: 400),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Interface", t => t.InterfaceId)
                .Index(t => t.InterfaceId)
                .Index(t => t.RootElement)
                .Index(t => t.ProcessHandler);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.XMLProcessInterfaceSetting", "InterfaceId", "dbo.Interface");
            DropForeignKey("dbo.Recipient", "MailNotificationSettingId", "dbo.MailNotificationSetting");
            DropForeignKey("dbo.LoopHandler", "UserId", "dbo.User");
            DropForeignKey("dbo.LoopHandler", "RoleId", "dbo.Role");
            DropForeignKey("dbo.LoopHandlerConstraint", "LoopHandlerId", "dbo.LoopHandler");
            DropForeignKey("dbo.Import", "UserId", "dbo.User");
            DropForeignKey("dbo.Import", "RoleId", "dbo.Role");
            DropForeignKey("dbo.GridSetting", "UserRoleId", "dbo.UserRole");
            DropForeignKey("dbo.FileSendInterfaceSetting", "InterfaceId", "dbo.Interface");
            DropForeignKey("dbo.FileCollectInterfaceSetting", "InterfaceId", "dbo.Interface");
            DropForeignKey("dbo.FileBuffer", "InterfaceId", "dbo.Interface");
            DropForeignKey("dbo.CSVProcessInterfaceSetting", "InterfaceId", "dbo.Interface");
            DropForeignKey("dbo.CSVExtractInterfaceSetting", "InterfaceId", "dbo.Interface");
            DropForeignKey("dbo.Constraint", "UserRoleId", "dbo.UserRole");
            DropForeignKey("dbo.UserRole", "UserId", "dbo.User");
            DropForeignKey("dbo.UserRole", "RoleId", "dbo.Role");
            DropForeignKey("dbo.AccessPointRole", "RoleId", "dbo.Role");
            DropForeignKey("dbo.AccessPointRole", "AccessPointId", "dbo.AccessPoint");
            DropIndex("dbo.XMLProcessInterfaceSetting", new[] { "ProcessHandler" });
            DropIndex("dbo.XMLProcessInterfaceSetting", new[] { "RootElement" });
            DropIndex("dbo.XMLProcessInterfaceSetting", new[] { "InterfaceId" });
            DropIndex("dbo.Recipient", new[] { "Value" });
            DropIndex("dbo.Recipient", new[] { "Type" });
            DropIndex("dbo.Recipient", new[] { "MailNotificationSettingId" });
            DropIndex("dbo.MailNotificationSetting", new[] { "DeletedDate" });
            DropIndex("dbo.MailNotificationSetting", new[] { "Disabled" });
            DropIndex("dbo.LoopHandler", new[] { "RoleId" });
            DropIndex("dbo.LoopHandler", new[] { "UserId" });
            DropIndex("dbo.LoopHandler", new[] { "RunGroup" });
            DropIndex("dbo.LoopHandler", new[] { "Status" });
            DropIndex("dbo.LoopHandler", new[] { "ConfigurationName" });
            DropIndex("dbo.LoopHandler", new[] { "NextExecutionDate" });
            DropIndex("dbo.LoopHandler", new[] { "LastExecutionDate" });
            DropIndex("dbo.LoopHandler", new[] { "CreateDate" });
            DropIndex("dbo.LoopHandler", new[] { "ExecutionMode" });
            DropIndex("dbo.LoopHandler", new[] { "ExecutionPeriod" });
            DropIndex("dbo.LoopHandler", new[] { "Name" });
            DropIndex("dbo.LoopHandler", new[] { "Description" });
            DropIndex("dbo.LoopHandlerConstraint", new[] { "Value" });
            DropIndex("dbo.LoopHandlerConstraint", new[] { "Prefix" });
            DropIndex("dbo.LoopHandlerConstraint", new[] { "LoopHandlerId" });
            DropIndex("dbo.Import", new[] { "RoleId" });
            DropIndex("dbo.Import", new[] { "UserId" });
            DropIndex("dbo.GridSetting", new[] { "UserRoleId" });
            DropIndex("dbo.FileSendInterfaceSetting", new[] { "SendHandler" });
            DropIndex("dbo.FileSendInterfaceSetting", new[] { "TargetFileMask" });
            DropIndex("dbo.FileSendInterfaceSetting", new[] { "TargetPath" });
            DropIndex("dbo.FileSendInterfaceSetting", new[] { "InterfaceId" });
            DropIndex("dbo.FileCollectInterfaceSetting", new[] { "CollectHandler" });
            DropIndex("dbo.FileCollectInterfaceSetting", new[] { "InterfaceId" });
            DropIndex("dbo.FileBuffer", new[] { "ProcessDate" });
            DropIndex("dbo.FileBuffer", new[] { "Status" });
            DropIndex("dbo.FileBuffer", new[] { "FileName" });
            DropIndex("dbo.FileBuffer", new[] { "InterfaceId" });
            DropIndex("dbo.FileBuffer", new[] { "CreateDate" });
            DropIndex("dbo.CSVProcessInterfaceSetting", new[] { "ProcessHandler" });
            DropIndex("dbo.CSVProcessInterfaceSetting", new[] { "QuoteChar" });
            DropIndex("dbo.CSVProcessInterfaceSetting", new[] { "Delimiter" });
            DropIndex("dbo.CSVProcessInterfaceSetting", new[] { "InterfaceId" });
            DropIndex("dbo.Interface", new[] { "Description" });
            DropIndex("dbo.Interface", new[] { "Direction" });
            DropIndex("dbo.Interface", new[] { "Name" });
            DropIndex("dbo.CSVExtractInterfaceSetting", new[] { "ExtractHandler" });
            DropIndex("dbo.CSVExtractInterfaceSetting", new[] { "QuoteChar" });
            DropIndex("dbo.CSVExtractInterfaceSetting", new[] { "Delimiter" });
            DropIndex("dbo.CSVExtractInterfaceSetting", new[] { "FileNameMask" });
            DropIndex("dbo.CSVExtractInterfaceSetting", new[] { "InterfaceId" });
            DropIndex("dbo.UserRole", new[] { "RoleId" });
            DropIndex("dbo.UserRole", new[] { "UserId" });
            DropIndex("dbo.Constraint", new[] { "Value" });
            DropIndex("dbo.Constraint", new[] { "Prefix" });
            DropIndex("dbo.Constraint", new[] { "UserRoleId" });
            DropIndex("dbo.AccessPointRole", new[] { "AccessPointId" });
            DropIndex("dbo.AccessPointRole", new[] { "RoleId" });
            DropTable("dbo.XMLProcessInterfaceSetting");
            DropTable("dbo.Setting");
            DropTable("dbo.Recipient");
            DropTable("dbo.MailNotificationSetting");
            DropTable("dbo.LoopHandler");
            DropTable("dbo.LoopHandlerConstraint");
            DropTable("dbo.Import");
            DropTable("dbo.GridSetting");
            DropTable("dbo.FileSendInterfaceSetting");
            DropTable("dbo.FileCollectInterfaceSetting");
            DropTable("dbo.FileBuffer");
            DropTable("dbo.CSVProcessInterfaceSetting");
            DropTable("dbo.Interface");
            DropTable("dbo.CSVExtractInterfaceSetting");
            DropTable("dbo.User");
            DropTable("dbo.UserRole");
            DropTable("dbo.Constraint");
            DropTable("dbo.ConstraintPrefix");
            DropTable("dbo.Role");
            DropTable("dbo.AccessPoint");
            DropTable("dbo.AccessPointRole");
        }
    }
}
