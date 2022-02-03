Ext.define('App.view.core.rpa.CustomRPAEditor', {
    extend: 'Ext.window.Window',
    alias: 'widget.customrpaeditor',
    title: l10n.ns('core', 'compositePanelTitles').value('RPATitle'),
    width: 450,
    height: 350,
    minWidth: 450,
    minHeight: 350,
    layout: 'fit',
    plain: true,
    modal: true,
    ghost: false,
    closable: false,

    items: [{
        xtype: 'panel',
        layout: 'fit',

        defaults: {
            padding: '10 15 15 15'
        },

        dockedItems: [{
            xtype: 'rpaformtoolbar',
            dock: 'bottom'
        }],

        items: [{
            xtype: 'panel',
            layout: 'fit',
            style: {
                background: '#829cb8',
            },

            items: [{
                xtype: 'container',
                itemId: 'customRPAEditorContainer',
                flex: 1,
                height: '100%',
                padding: '5 5 0 5',
                cls: 'promo-support-custom-promo-panel-container',
                layout: {
                    type: 'vbox',
                    align: 'stretch'
                },
                items: [{
                    xtype: 'rpaform',
                    height: 'auto',
                }]
            }]
        }]
    }],
});