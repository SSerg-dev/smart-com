Ext.define('App.view.core.main.TopToolbar', {
    extend: 'Ext.toolbar.Toolbar',
    alias: 'widget.toptoolbar',
    ui: 'marengo-toolbar',
    padding: '0 9 0 9',

    items: [
        {
            xtype: 'expandbutton',
            ui: 'white-button-toolbar',
            glyph: 0xf35c,
            glyph1: 0xf06d,
            target: '#drawer'
        },
        {
            xtype: 'tbtext',
            ui: 'menu-path',
            itemId: 'breadcrumbs'
        },
        '->',
        {
            xtype: 'modelswitcher',
        },
        {
            xtype: 'label',
            text: 'Rolling Scenario mode',
            margin: '0 0 0 5'
        },
        '->',
        {
            xtype: 'securitybutton',
            cls: 'rolebutton',
            glyph: 0xf019,
            itemId: 'rolebutton',
            text: l10n.ns('core').value('defaultRoleButtonText')
        },
        {
            xtype: 'securitybutton',
            cls: 'userbutton',
            glyph: 0xf004,
            itemId: 'userbutton'
        },
        {
            xtype: 'button',
            ui: 'light-blue-button-toolbar',
            //hidden: true,
            glyph: 0xf206,
            itemId: 'exitbutton',
            tooltip: l10n.ns('core').value('exitButtonText'),
            disabled: true
        },
        {
            xtype: 'tbspacer',
            width: 5
        }
    ]
});