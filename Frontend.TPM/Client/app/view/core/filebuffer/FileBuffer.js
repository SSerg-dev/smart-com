Ext.define('App.view.core.filebuffer.FileBuffer', {
    extend: 'App.view.core.common.CombinedDirectoryPanel',
    alias: 'widget.filebuffer',
    title: l10n.ns('core', 'compositePanelTitles').value('FileBufferTitle'),

    dockedItems: [{
        xtype: 'harddeletedirectorytoolbar',
        dock: 'right'
    }],

    customHeaderItems: [
        ResourceMgr.getAdditionalMenu('core').filebuffer
    ],

    items: [{
        xtype: 'directorygrid',
        itemId: 'datatable',
        editorModel: 'Core.form.EditorDetailWindowModel',

        store: {
            type: 'directorystore',
            model: 'App.model.core.filebuffer.FileBuffer',
            storeId: 'filebufferstore',
            extendedFilter: {
                xclass: 'App.ExtFilterContext',
                supportedModels: [{
                    xclass: 'App.ExtSelectionFilterModel',
                    model: 'App.model.core.filebuffer.FileBuffer',
                    modelId: 'efselectionmodel'
                }, {
                    xclass: 'App.ExtTextFilterModel',
                    modelId: 'eftextmodel'
                }]
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
            items: [{
                text: l10n.ns('core', 'FileBuffer').value('InterfaceName'),
                dataIndex: 'InterfaceName',
            }, {
                text: l10n.ns('core', 'FileBuffer').value('InterfaceDirection'),
                dataIndex: 'InterfaceDirection',
            }, {
                text: l10n.ns('core', 'FileBuffer').value('CreateDate'),
                dataIndex: 'CreateDate',
                xtype: 'datecolumn',
                renderer: Ext.util.Format.dateRenderer('d.m.Y H:i:s')
            }, {
                text: l10n.ns('core', 'FileBuffer').value('FileName'),
                dataIndex: 'FileName'
            }, {
                text: l10n.ns('core', 'FileBuffer').value('Status'),
                dataIndex: 'Status'
            }, {
                text: l10n.ns('core', 'FileBuffer').value('ProcessDate'),
                dataIndex: 'ProcessDate',
                xtype: 'datecolumn',
                renderer: Ext.util.Format.dateRenderer('d.m.Y H:i:s')
            }]
        }
    }, {
        xtype: 'editabledetailform',
        itemId: 'detailform',
        model: 'App.model.core.filebuffer.FileBuffer',
        items: [{
            xtype: 'searchfield',
            name: 'InterfaceId',
            fieldLabel: l10n.ns('core', 'FileBuffer').value('InterfaceId'),
            selectorWidget: 'interface',
            valueField: 'Id',
            displayField: 'Name',
            store: {
                type: 'directorystore',
                model: 'App.model.core.interface.Interface',
                extendedFilter: {
                    xclass: 'App.ExtFilterContext',
                    supportedModels: [{
                        xclass: 'App.ExtSelectionFilterModel',
                        model: 'App.model.core.interface.Interface',
                        modelId: 'efselectionmodel'
                    }, {
                        xclass: 'App.ExtTextFilterModel',
                        modelId: 'eftextmodel'
                    }]
                }
            },
            mapping: [{
                from: 'Name',
                to: 'InterfaceName'
            }, {
                from: 'Direction',
                to: 'InterfaceDirection'
            }]
        }, {
            xtype: 'singlelinedisplayfield',
            name: 'InterfaceDirection',
            fieldLabel: l10n.ns('core', 'FileBuffer').value('InterfaceDirection')
        }, {
            xtype: 'singlelinedisplayfield',
            name: 'CreateDate',
            renderer: Ext.util.Format.dateRenderer('d.m.Y H:i:s'),
            fieldLabel: l10n.ns('core', 'FileBuffer').value('CreateDate')
        }, {
            xtype: 'textfield',
            name: 'FileName',
            fieldLabel: l10n.ns('core', 'FileBuffer').value('FileName')
        }, {
            xtype: 'textfield',
            name: 'Status',
            fieldLabel: l10n.ns('core', 'FileBuffer').value('Status')
        }, {
            xtype: 'singlelinedisplayfield',
            name: 'ProcessDate',
            renderer: Ext.util.Format.dateRenderer('d.m.Y H:i:s'),
            fieldLabel: l10n.ns('core', 'FileBuffer').value('ProcessDate')
        }]
    }]

});