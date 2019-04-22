Ext.define('App.view.core.gridsetting.ColumnSettingsGrid', {
    extend: 'Ext.grid.Panel',
    alias: 'widget.columnsettingsgrid',
    frame: true,
    ui: 'grid-panel',
    cls: 'columnsettingsgrid',
    enableColumnHide: false,
    enableColumnMove: false,
    columnLines: true,

    viewConfig: {
        overflowY: 'scroll',
        trackOver: false,
        preserveScrollOnRefresh: true,
        markDirty: false
    },

    store: {
        fields: [
            { name: 'dataIndex', type: 'string' },
            { name: 'index', type: 'int' },
            { name: 'text', type: 'string' },
            { name: 'width', type: 'int', useNull: true },
            { name: 'sort', type: 'string' },
            { name: 'autoWidth', type: 'boolean', defaultValue: true }
        ]
    },

    columns: {
        defaults: {
            menuDisabled: true,
            resizable: false
        },

        items: [{
            text: l10n.ns('core', 'gridsettings', 'columns').value('Name'),
            dataIndex: 'text',
            flex: 1,
            renderer: function (value, metaData) {
                metaData.tdAttr = 'data-qtip="' + Ext.String.htmlEncode(value) + '"';
                return value;
            }
        }, {
            text: l10n.ns('core', 'gridsettings', 'columns').value('Width'),
            dataIndex: 'width',
            width: 70,
            renderer: function (value, metaData, record) {
                if (Ext.isEmpty(value) || record.get('autoWidth')) {
                    return l10n.ns('core', 'gridsettings').value('emptyWidthText');
                }
                return value;
            }
        }, {
            text: l10n.ns('core', 'gridsettings', 'columns').value('Sort'),
            dataIndex: 'sort',
            width: 120,
            renderer: function (value) {
                if (!Ext.isEmpty(value)) {
                    return l10n.ns('core', 'enums', 'SortMode').value(value);
                }
            }
        }]
    },

    setSelectedRecordSort: function (button, direction) {
        var selModel = this.getSelectionModel();

        if (!selModel.hasSelection()) {
            return false;
        }

        var record = selModel.getSelection()[0],
            sortCount = this.getStore().collect('sort', false).length;

        if (sortCount > 1 || (sortCount == 1 && Ext.isEmpty(record.get('sort')))) {
            Ext.Msg.show({
                title: l10n.ns('core').value('confirmTitle'),
                msg: l10n.ns('core', 'gridsettings').value('selectSortMsg'),
                icon: Ext.Msg.QUESTION,
                buttons: Ext.Msg.YESNO,
                scope: this,
                fn: function (buttonId) {
                    if (buttonId === 'yes') {
                        this.getStore().each(function (item) {
                            item.set('sort', null);
                        });
                        record.set('sort', direction);
                        button.toggle(true);
                    }
                }
            });
            return false;
        } else {
            selModel.getSelection()[0].set('sort', direction);
        }
    }
});