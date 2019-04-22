Ext.define('App.view.tpm.client.DispatchDate', {
    extend: 'Ext.panel.Panel',
    alias: 'widget.dispatchdate',

    items: [{
        xtype: 'container',
        html: l10n.ns('tpm', 'Dispatch').value('DispatchTitle'),
        style: 'margin: 10px 0 10px 0;'
    }, {
        xtype: 'container',
        style: 'border: 1px solid #ffffff; padding: 10px 6px; margin-bottom: 10px;',

        items: [{
            xtype: 'fieldcontainer',
            itemId: 'fieldcontainerStart',
            fieldLabel: l10n.ns('tpm', 'Dispatch').value('DispatchStart'),
            labelSeparator: ' ',
            width: '100%',
            labelWidth: 160,
            items: [{
                xtype: 'container',
                width: '100%',
                layout: {
                    type: 'hbox',
                    align: 'middle',
                },
                items: [{
                    xtype: 'booleancombobox',
                    name: 'IsBeforeStart',
                    store: {
                        type: 'booleanstore',
                        data: [
                            { id: null, text: '\u00a0' },
                            { id: true, text: l10n.ns('tpm', 'Dispatch').value('Before') },
                            { id: false, text: l10n.ns('tpm', 'Dispatch').value('After') }
                        ]
                    },
                    flex: 2,
                    style: 'margin-right: 5px;',
                    allowBlank: true
                }, {
                    xtype: 'numberfield',
                    name: 'DaysStart',
                    minValue: 1,
                    flex: 1,
                    style: 'margin-right: 5px',
                    disabled: true,
                    allowDecimals: false,
                    allowBlank: false
                }, {
                    xtype: 'booleancombobox',
                    name: 'IsDaysStart',
                    store: {
                        type: 'booleanstore',
                        data: [
                            { id: true, text: l10n.ns('tpm', 'Dispatch').value('Days') },
                            { id: false, text: l10n.ns('tpm', 'Dispatch').value('Weeks') }
                        ],
                    },
                    flex: 2,
                    disabled: true,
                    allowBlank: false,
                }]
            }]
        }, {
            xtype: 'fieldcontainer',
            itemId: 'fieldcontainerEnd',
            fieldLabel: l10n.ns('tpm', 'Dispatch').value('DispatchEnd'),
            labelSeparator: ' ',
            width: '100%',
            labelWidth: 160,
            style: 'margin-bottom: 0', 
            items: [{
                xtype: 'container',
                layout: {
                    type: 'hbox',
                    align: 'middle'
                },
                items: [{
                    xtype: 'booleancombobox',
                    name: 'IsBeforeEnd',
                    store: {
                        type: 'booleanstore',
                        data: [
                            { id: null, text: '\u00a0' },
                            { id: true, text: l10n.ns('tpm', 'Dispatch').value('Before') },
                            { id: false, text: l10n.ns('tpm', 'Dispatch').value('After') }
                        ]
                    },
                    flex: 2,
                    style: 'margin-right: 5px',
                    allowBlank: true
                }, {
                    xtype: 'numberfield',
                    name: 'DaysEnd',
                    minValue: 1,
                    flex: 1,
                    style: 'margin-right: 5px;',
                    disabled: true,
                    allowDecimals: false,
                    allowBlank: false,
                }, {
                    xtype: 'booleancombobox',
                    name: 'IsDaysEnd',
                    store: {
                        type: 'booleanstore',
                        data: [
                            { id: true, text: l10n.ns('tpm', 'Dispatch').value('Days') },
                            { id: false, text: l10n.ns('tpm', 'Dispatch').value('Weeks') }
                        ]
                    },
                    flex: 2,
                    disabled: true,
                    allowBlank: false,
                }]
            }]
        }]
    }]
});