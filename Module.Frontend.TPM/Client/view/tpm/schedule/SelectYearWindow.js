Ext.define('App.view.tpm.schedule.SelectYearWindow', {
    extend: 'App.view.core.common.EditorWindow',
    alias: 'widget.selectyearwindow',
    title: 'Select year',

    width: 230,
    height: 130,
    minWidth: 230,
    minHeight: 130,
    maxWidth: 230,
    maxHeight: 130,

    items: [{
        xtype: 'numberfield',
        name: 'year',
        allowBlank: false,
        allowOnlyWhiteSpace: false,
        listeners: {
            afterrender: function (field) {
                var currentDate = new Date();
                field.setValue(currentDate.getFullYear());
            }
        }
    }],
    buttons: [{
        text: l10n.ns('core', 'buttons').value('cancel'),
        itemId: 'cancel'
    }, {
        text: l10n.ns('core', 'buttons').value('ok'),
        itemId: 'exportforyear',
        ui: 'green-button-footer-toolbar',
    }]
})