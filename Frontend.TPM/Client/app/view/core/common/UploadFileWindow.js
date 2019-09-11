Ext.define('App.view.core.common.UploadFileWindow', {
    extend: 'App.view.core.base.BaseModalWindow',
    alias: 'widget.uploadfilewindow',
    layout: 'auto',
    autoScroll: true,
    width: 560,
    minWidth: 560,
    maxHeight: 700,
    resizeHandles: 'w e',

    items: [{
        xtype: 'form',
        itemId: 'importform',
        ui: 'transparent-panel',
        margin: 0,

        layout: {
            type: 'vbox',
            align: 'stretch'
        },

        items: [{
            xtype: 'editorform',
            margin: '10 15 15 15',

            items: [{
                xtype: 'filefield',
                name: 'File',
                msgTarget: 'side',
                buttonText: l10n.ns('core', 'buttons').value('browse'),
                forceValidation: true,
                allowOnlyWhitespace: false,
                allowBlank: false,
                fieldLabel: l10n.ns('core').value('uploadFileLabelText'),
                vtype: 'filePass',
                ui: 'default',
                labelWidth: '10%'
            }]
        }]
    }],

    buttons: [{
        text: l10n.ns('core', 'buttons').value('cancel'),
        itemId: 'cancel'
    }, {
        text: l10n.ns('core', 'buttons').value('upload'),
        ui: 'green-button-footer-toolbar',
        itemId: 'ok'
    }]
});