Ext.define('App.view.core.gridsetting.GridSettingsWindow', {
    extend: 'App.view.core.base.BaseModalWindow',
    alias: 'widget.gridsettingswindow',
    title: l10n.ns('core', 'gridsettings').value('gridSettingsTitle'),

    width: 900,
    minWidth: 700,
    height: 450,
    minHeight: 450,

    items: [{
        xtype: 'panel',
        frame: true,
        ui: 'light-gray-panel',
        cls: 'gridsettings-panel',
        bodyPadding: '10 0 10 10',

        layout: {
            type: 'hbox',
            align: 'stretch'
        },

        items: [{
            xtype: 'container',
            flex: 1,

            layout: {
                type: 'vbox',
                align: 'stretch'
            },

            items: [{
                xtype: 'tbtext',
                text: l10n.ns('core', 'gridsettings').value('hiddenСolumnsTitle'),
                padding: '0 0 5 5'
            }, {
                xtype: 'columnsettingsgrid',
                itemId: 'hiddencolumns',
                flex: 1
            }]
        }, {
            xtype: 'colsettingstoolbar',
            itemId: 'movingToolbar',

            items: [{
                glyph: 0xf054,
                itemId: 'add',
                tooltip: l10n.ns('core', 'gridsettings', 'buttons').value('add')
            }, {
                glyph: 0xf04d,
                itemId: 'remove',
                tooltip: l10n.ns('core', 'gridsettings', 'buttons').value('remove')
            }]
        }, {
            xtype: 'container',
            flex: 1,

            layout: {
                type: 'vbox',
                align: 'stretch'
            },

            items: [{
                xtype: 'tbtext',
                text: l10n.ns('core', 'gridsettings').value('visibleСolumnsTitle'),
                padding: '0 0 5 5'
            }, {
                xtype: 'columnsettingsgrid',
                itemId: 'visiblecolumns',
                flex: 1
            }]
        }, {
            xtype: 'colsettingstoolbar',
            itemId: 'visibleColumnsToolbar',

            items: [{
                glyph: 0xf05d,
                itemId: 'up',
                tooltip: l10n.ns('core', 'gridsettings', 'buttons').value('up'),
                margin: 10
            }, {
                glyph: 0xf045,
                itemId: 'down',
                tooltip: l10n.ns('core', 'gridsettings', 'buttons').value('down')
            }, {
                xtype: 'tbspacer',
                height: 10,
                disabled: false
            }, {
                glyph: 0xf4bc,
                itemId: 'sortAsc',
                toggleGroup: 'sortBtn',
                tooltip: l10n.ns('core', 'gridsettings', 'buttons').value('sortAsc')
            }, {
                glyph: 0xf4bd,
                itemId: 'sortDesc',
                toggleGroup: 'sortBtn',
                tooltip: l10n.ns('core', 'gridsettings', 'buttons').value('sortDesc')
            }, {
                glyph: 0xf4ba,
                itemId: 'clearSort',
                toggleGroup: 'sortBtn',
                tooltip: l10n.ns('core', 'gridsettings', 'buttons').value('clearSort')
            }, {
                xtype: 'tbspacer',
                height: 10,
                disabled: false
            }, {
                xtype: 'numberfield',
                itemId: 'width',
                fieldLabel: l10n.ns('core', 'gridsettings').value('widthFieldLabel'),
                labelAlign: 'top',
                labelSeparator: '',
                width: 70,
                height: null,
                minValue: 100,
                allowDecimals: false,
                allowExponential: false
            }]
        }]
    }],

    buttons: [{
        text: l10n.ns('core', 'gridsettings', 'windowButtons').value('cancel'),
        itemId: 'cancel'
    }, {
        text: l10n.ns('core', 'gridsettings', 'windowButtons').value('applydefault'),
        itemId: 'applydefault'
    }, {
        text: l10n.ns('core', 'gridsettings', 'windowButtons').value('save'),
        ui: 'green-button-footer-toolbar',
        itemId: 'save'
    }],

    initComponent: function () {
        this.callParent(arguments);

        var btnSortAsc = this.down('#sortAsc'),
            btnSortDesc = this.down('#sortDesc');

        btnSortAsc.onClick = Ext.Function.createInterceptor(btnSortAsc.onClick, function () {
            var visibleColumnsGrid = this.up('window').down('#visiblecolumns');
            return visibleColumnsGrid.setSelectedRecordSort(this, 'ASC');
        }, btnSortAsc);

        btnSortDesc.onClick = Ext.Function.createInterceptor(btnSortDesc.onClick, function () {
            var visibleColumnsGrid = this.up('window').down('#visiblecolumns');
            return visibleColumnsGrid.setSelectedRecordSort(this, 'DESC');
        }, btnSortDesc);
    }
});