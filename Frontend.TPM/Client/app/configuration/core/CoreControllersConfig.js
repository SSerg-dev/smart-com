ResourceMgr.defineControllers('core', [
    'core.Main',
    'core.CombinedDirectory',
    'core.ExtendedFilter',
    'core.ExtendedFilterSettings',
    'core.ExtendedDetailFilter',
    'core.RecipientSettings',
    'core.ImportExportLogic',

    // LoopHandler
    'core.loophandler.LoopHandler',
    'core.loophandler.HistoricalLoopHandler',
    'core.loophandler.AdminLoopHandler',
    'core.loophandler.UserLoopHandler',
    'core.loophandler.SingleLoopHandler',

    'core.ReportController',

    // Security
    'core.security.Security',

    // AssociatedAccessPoint
    'core.associatedaccesspoint.accesspoint.AssociatedAccessPoint',
    'core.associatedaccesspoint.accesspoint.DeletedAssociatedAccessPoint',
    'core.associatedaccesspoint.accesspoint.HistoricalAssociatedAccessPoint',
    'core.associatedaccesspoint.accesspointrole.AssociatedAccessPointRole',

    // AssociatedUser
    'core.associateduser.aduser.AdUser',
    'core.associateduser.aduser.AssociatedAdUser',
    'core.associateduser.aduser.HistoricalAssociatedAdUser',
    'core.associateduser.aduser.DeletedAssociatedAdUser',

    'core.associateduser.dbuser.AssociatedDbUser',
    'core.associateduser.dbuser.HistoricalAssociatedDbUser',
    'core.associateduser.dbuser.DeletedAssociatedDbUser',

    'core.associateduser.userrole.AssociatedUserRole',

    // Role
    'core.role.Role',
    'core.role.DeletedRole',
    'core.role.HistoricalRole',

    // Setting
    'core.setting.Setting',
    'core.setting.HistoricalSetting',

    // GridSetting
    'core.gridsetting.GridSetting',

    //FileBuffer
    'core.filebuffer.FileBuffer',
    'core.filebuffer.HistoricalFileBuffer',

    // AssociatedMailNotificationSetting
    'core.associatedmailnotificationsetting.mailnotificationsetting.AssociatedMailNotificationSetting',
    'core.associatedmailnotificationsetting.mailnotificationsetting.HistoricalAssociatedMailNotificationSetting',
    'core.associatedmailnotificationsetting.mailnotificationsetting.DeletedAssociatedMailNotificationSetting',

    // AssociatedRecipient
    'core.associatedmailnotificationsetting.recipient.AssociatedRecipient',
    'core.associatedmailnotificationsetting.recipient.HistoricalAssociatedRecipient',

    // Interface
    'core.interface.Interface',
    'core.interface.HistoricalInterface',

    //CSVProcessInterfaceSetting
    'core.csvprocessinterfacesetting.CSVProcessInterfaceSetting',
    'core.csvprocessinterfacesetting.HistoricalCSVProcessInterfaceSetting',
    //CSVExtractInterfaceSetting
    'core.csvextractinterfacesetting.CSVExtractInterfaceSetting',
    'core.csvextractinterfacesetting.HistoricalCSVExtractInterfaceSetting',
    //FileSendInterfaceSetting
    'core.filesendinterfacesetting.FileSendInterfaceSetting',
    'core.filesendinterfacesetting.HistoricalFileSendInterfaceSetting',

    // FileCollectInterfaceSetting
    'core.filecollectinterfacesetting.FileCollectInterfaceSetting',
    'core.filecollectinterfacesetting.HistoricalFileCollectInterfaceSetting',

    // XMLProcessInterfaceSetting
    'core.xmlprocessinterfacesetting.XMLProcessInterfaceSetting',
    'core.xmlprocessinterfacesetting.HistoricalXMLProcessInterfaceSetting',

    // Constraint
    'core.associatedconstraint.constraint.AssociatedConstraint',
    'core.associatedconstraint.userrole.AssociatedUserRoleMain'

]);