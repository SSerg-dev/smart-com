MenuMgr.defineMenu([{
    text: l10n.ns('core', 'mainmenu').value('adminMenu'),
    tooltip: l10n.ns('core', 'mainmenu').value('adminMenu'),
    glyph: 0xf493,
    scale: 'medium',

    children: [{
        text: l10n.ns('core', 'mainmenu').value('loopHandlerItem'),
        tooltip: l10n.ns('core', 'mainmenu').value('loopHandlerItem'),
        glyph: 0xf51b,
        roles: ['Administrator', 'SupportAdministrator'],
        widget: 'loophandler'
    }, {
        text: l10n.ns('core', 'mainmenu').value('singleLoopHandlerItem'),
        tooltip: l10n.ns('core', 'mainmenu').value('singleLoopHandlerItem'),
        glyph: 0xf51b,
        roles: ['Administrator', 'SupportAdministrator', 'User'],
        exeptRoles: true,
        widget: 'singleloophandler'
    }, {
        text: l10n.ns('core', 'mainmenu').value('singleLoopHandlerItem'),
        tooltip: l10n.ns('core', 'mainmenu').value('singleLoopHandlerItem'),
        glyph: 0xf51b,
        roles: ['Administrator', 'SupportAdministrator'],
        widget: 'adminloophandler'
    }, {
        text: l10n.ns('core', 'mainmenu').value('UserItem'),
        tooltip: l10n.ns('core', 'mainmenu').value('UserItem'),
        glyph: 0xf004,
        roles: ['Administrator', 'SupportAdministrator'],
        itemId: 'user',
        widget: 'associateddbusercontainer'
    }, {
        text: l10n.ns('core', 'mainmenu').value('RoleItem'),
        tooltip: l10n.ns('core', 'mainmenu').value('RoleItem'),
        glyph: 0xf00e,
        roles: ['Administrator', 'SupportAdministrator'],
        widget: 'role'
    }, {
        text: l10n.ns('core', 'mainmenu').value('AccessPointItem'),
        tooltip: l10n.ns('core', 'mainmenu').value('AccessPointItem'),
        glyph: 0xf483,
        roles: ['Administrator', 'SupportAdministrator'],
        widget: 'associatedaccesspointcontainer'
    }, {
        text: l10n.ns('core', 'mainmenu').value('ConstraintItem'),
        tooltip: l10n.ns('core', 'mainmenu').value('ConstraintItem'),
        glyph: 0xf631,
        roles: ['Administrator', 'SupportAdministrator'],
        widget: 'compositeconstraint'
    }, {
        text: l10n.ns('core', 'mainmenu').value('SettingItem'),
        tooltip: l10n.ns('core', 'mainmenu').value('SettingItem'),
        glyph: 0xf494,
        roles: ['Administrator', 'SupportAdministrator'],
        widget: 'setting'
    }, {
        text: l10n.ns('core', 'mainmenu').value('MailNotificationSettingItem'),
        tooltip: l10n.ns('core', 'mainmenu').value('MailNotificationSettingItem'),
        glyph: 0xf1ee,
        roles: ['Administrator', 'SupportAdministrator'],
        widget: 'associatedmailnotificationsettingcontainer'
    }, {
        text: l10n.ns('core', 'mainmenu').value('RPASettingItem'),
        tooltip: l10n.ns('core','mainmenu').value('RPASettingItem'),
        glyph: 0xf494,
        roles: ['Administrator', 'SupportAdministrator'],
        widget: 'rpasetting'
    }, {
        text: l10n.ns('core', 'mainmenu').value('RPAItem'),
        tooltip: l10n.ns('core','mainmenu').value('RPAItem'),
        glyph: 0xf494,
        roles: ['Administrator', 'SupportAdministrator'],
        widget: 'rpa'
    }]
}, {
    text: l10n.ns('core', 'mainmenu').value('InterfaceItem'),
    tooltip: l10n.ns('core', 'mainmenu').value('InterfaceItem'),
    glyph: 0xf318,
    scale: 'medium',
    children: [{
        text: l10n.ns('core', 'mainmenu').value('InterfaceItem'),
        tooltip: l10n.ns('core', 'mainmenu').value('InterfaceItem'),
        glyph: 0xf497,
        roles: ['Administrator'],
        widget: 'interface'
    }, {
        text: l10n.ns('core', 'mainmenu').value('FileBufferItem'),
        tooltip: l10n.ns('core', 'mainmenu').value('FileBufferItem'),
        glyph: 0xf222,
        roles: ['Administrator'],
        widget: 'filebuffer'
    }, {
        text: l10n.ns('core', 'mainmenu').value('FileCollectInterfaceSettingItem'),
        tooltip: l10n.ns('core', 'mainmenu').value('FileCollectInterfaceSettingItem'),
        glyph: 0xf493,
        roles: ['Administrator'],
        widget: 'filecollectinterfacesetting'
    }, {
        text: l10n.ns('core', 'mainmenu').value('CSVProcessInterfaceSettingItem'),
        tooltip: l10n.ns('core', 'mainmenu').value('CSVProcessInterfaceSettingItem'),
        glyph: 0xf218,
        roles: ['Administrator'],
        widget: 'csvprocessinterfacesetting'
    }, {
        text: l10n.ns('core', 'mainmenu').value('XMLProcessInterfaceSettingItem'),
        tooltip: l10n.ns('core', 'mainmenu').value('XMLProcessInterfaceSettingItem'),
        glyph: 0xf22e,
        roles: ['Administrator'],
        widget: 'xmlprocessinterfacesetting'
    }, {
        text: l10n.ns('core', 'mainmenu').value('CSVExtractInterfaceSettingItem'),
        tooltip: l10n.ns('core', 'mainmenu').value('CSVExtractInterfaceSettingItem'),
        glyph: 0xf218,
        roles: ['Administrator'],
        widget: 'csvextractinterfacesetting'
    }, {
        text: l10n.ns('core', 'mainmenu').value('FileSendInterfaceSettingItem'),
        tooltip: l10n.ns('core', 'mainmenu').value('FileSendInterfaceSettingItem'),
        glyph: 0xf22a,
        roles: ['Administrator'],
        widget: 'filesendinterfacesetting'
    }]
}]);