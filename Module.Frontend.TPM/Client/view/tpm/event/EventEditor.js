Ext.define('App.view.tpm.event.EventEditor', {
    extend: 'App.view.core.common.EditorDetailWindow',
    alias: 'widget.eventeditor',
    width: 500,
    minWidth: 500,
    maxHeight: 500,

    afterWindowShow: function (scope, isCreating) {
        scope.down('field').focus(true, 10); // фокус на первом поле формы для корректной работы клавишных комбинаций

        if (scope.down('textfield[name=Name]').rawValue == 'Standard promo') {
            scope.down('textfield[name=Year]').setValue(0);
            scope.down('textfield[name=Name]').setReadOnly(true);
            scope.down('textfield[name=Year]').setReadOnly(true);
            scope.down('textfield[name=Period]').setReadOnly(true);
            scope.down('textfield[name=Description]').setReadOnly(true);
        }
    },

    items: {
        xtype: 'editorform',
        columnsCount: 1,

        items: [{
            xtype: 'textfield',
            name: 'Name',
            fieldLabel: l10n.ns('tpm', 'Event').value('Name')
        }, {
            xtype: 'numberfield',
            name: 'Year',
            allowDecimals: true,
            allowExponential: false,
            minValue: 0,
            maxValue: 9999,
            allowBlank: false,
            allowOnlyWhitespace: false,
            fieldLabel: l10n.ns('tpm', 'Event').value('Year'),
            listeners: {
                afterrender: function (field) {
                    if (!field.value) {
                        field.setValue(new Date().getFullYear());
                    }
                }
            }
        }, {
            xtype: 'combobox',
            name: 'Period',
            allowBlank: false,
            allowOnlyWhitespace: false,
            fieldLabel: l10n.ns('tpm', 'Event').value('Period'),
            store: {
                type: 'simplestore',
                id: 'pccase',
                fields: ['val'],
                data: [
                    { val: 'P01' }, { val: 'P02' }, { val: 'P03' },
                    { val: 'P04' }, { val: 'P05' }, { val: 'P06' },
                    { val: 'P07' }, { val: 'P08' }, { val: 'P09' },
                    { val: 'P10' }, { val: 'P11' }, { val: 'P12' },
                    { val: 'P13' }
                ]
            },
            valueField: 'val',
            displayField: 'val',
            queryMode: 'local'
        }, {
            xtype: 'textfield',
            name: 'Description',
            allowBlank: true,
            allowOnlyWhitespace: true,
            fieldLabel: l10n.ns('tpm', 'Event').value('Description')
        }]
    }
});




