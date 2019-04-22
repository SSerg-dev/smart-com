Ext.define('App.view.core.main.DummyDirectory', {
    extend: 'App.view.core.common.CombinedDirectoryPanel',
    alias: 'widget.dummydirectory',
    title: 'Заголовок',
    margin: '20 7 20 20',
    minHeight: null,

    suppressSelection: true,

    dockedItems: [{
        xtype: 'readonlydirectorytoolbar',
        dock: 'right'
    }],

    systemHeaderItems: [{
        glyph: 0xf2c1,
        itemId: 'detail',
        tooltip: l10n.ns('core', 'selectablePanelButtons').value('detail'),
        disabled: true
    }, {
        glyph: 0xf4e6,
        itemId: 'update',
        tooltip: l10n.ns('core', 'selectablePanelButtons').value('update')
    }],

    items: [{
        xtype: 'directorygrid',
        itemId: 'datatable',

        store: {
            storeId: 'simpsonsStore',
            fields: ['f1', 'f2', 'f3', 'f4', 'f5', 'f6', 'f7', 'f8', 'f9'],

            proxy: {
                type: 'memory',
                reader: {
                    type: 'json',
                    root: 'items'
                }
            }

        },

        columns: {
            defaults: {
                plugins: ['sortbutton'],
                menuDisabled: true,
                filter: true,
                flex: 1,
                minWidth: 100
            },

            items: [
                { text: 'Код', dataIndex: 'f1' },
                { text: 'Продукт', dataIndex: 'f2' },
                { text: 'Цена', dataIndex: 'f3' },
                { text: 'НДС', dataIndex: 'f4' },
                { text: 'Вес кор.', dataIndex: 'f5' },
                { text: 'Кол-во кор.', dataIndex: 'f6' },
                { text: 'Всего паллет', dataIndex: 'f7' },
                { text: 'Вес, кг', dataIndex: 'f8' },
                { text: 'Стоимость', dataIndex: 'f9' }
            ]
        }
    }, {
        xtype: 'detailform',
        itemId: 'detailform',

        items: [
            { fieldLabel: 'Код', name: 'f1' },
            { fieldLabel: 'Продукт', name: 'f2' },
            { fieldLabel: 'Цена', name: 'f3' },
            { fieldLabel: 'НДС', name: 'f4' },
            { fieldLabel: 'Вес кор.', name: 'f5' },
            { fieldLabel: 'Кол-во кор.', name: 'f6' },
            { fieldLabel: 'Всего паллет', name: 'f7' },
            { fieldLabel: 'Вес, кг', name: 'f8' },
            { fieldLabel: 'Стоимость', name: 'f9' }
        ]
    }],

    initComponent: function () {
        this.callParent(arguments);

        // Workaround в связи с багом в loadmask
        this.down('directorygrid').getView().loadMask = {
            maskOnDisable: false,
            fixedZIndex: 3
        };
    }
});