Ext.define('App.view.tpm.promocalculating.CalculatingInfoLog', {
    extend: 'App.view.core.common.CombinedDirectoryPanel',
    alias: 'widget.calculatinginfolog',

    height: 325,
    maxHeight: 325,
    header: {},

    listeners: {
        resize: function (panel, w, h) {
            panel.down('customlogtoptoolbar').setWidth(w);
        },
        afterrender: function () {
            this.setLoading(true);
        }
    },

    dockedItems: [{}],

    customHeaderItems: [],
    systemHeaderItems: [{
        padding: '6 6 6 6',
        xtype: 'customlogtoptoolbar'
    }],

    items: [{
        xtype: 'directorygrid',
        selModel: 'App.model.tpm.promocalculating.CalculatingInfoLog',
        itemId: 'datatable',
        editorModel: 'Core.form.EditorDetailWindowModel',
        store: {
            model: 'App.model.tpm.promocalculating.CalculatingInfoLog',
            storeId: 'calculatinginfologstore',
            listeners: {
                filterchange: function () {
                    var store = Ext.ComponentQuery.query('calculatinginfolog grid')[0].getStore();
                    var displayItem = Ext.ComponentQuery.query('calculatinginfolog #displayItem')[0],
                        msg = Ext.String.format(l10n.ns('core', 'gridInfoToolbar').value('gridInfoMsg'), store.data.length);

                    if (displayItem) {
                        displayItem.setText(msg);
                    }
                }
            }
        },
        listeners: {
            viewready: function () {
                this.up('calculatinginfolog').setLoading(false);
            }
        },
        columns: {
            defaults: {
                plugins: ['sortbutton'],
                menuDisabled: true,
                filter: true
            },
            items: [{
                text: l10n.ns('tpm', 'CalculatingInfoLog').value('Type'),
                dataIndex: 'Type',
                flex: 1,
                minWidth: 50
            }, {
                text: l10n.ns('tpm', 'CalculatingInfoLog').value('Message'),
                dataIndex: 'Message',
                flex: 5,
                minWidth: 100
            }]
        }
    }]
})