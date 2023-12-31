﻿Ext.define('App.view.tpm.brand.DeletedBrand', {
    extend: 'App.view.core.common.CombinedDirectoryPanel',
    alias: 'widget.deletedbrand',
    title: l10n.ns('core', 'compositePanelTitles').value('deletedPanelTitle'),

    dockedItems: [{
        xtype: 'readonlydeleteddirectorytoolbar',
        dock: 'right'
    }],

    items: [{
        xtype: 'directorygrid',
        itemId: 'datatable',
		editorModel: 'Core.form.EditorDetailWindowModel',
        store: {
            type: 'directorystore',
            model: 'App.model.tpm.brand.DeletedBrand',
            storeId: 'deletedbrandstore',
            extendedFilter: {
                xclass: 'App.ExtFilterContext',
                supportedModels: [{
                    xclass: 'App.ExtSelectionFilterModel',
                    model: 'App.model.tpm.brand.DeletedBrand',
                    modelId: 'efselectionmodel'
                }, {
                    xclass: 'App.ExtTextFilterModel',
                    modelId: 'eftextmodel'
                }]
            },
            sorters: [{
                property: 'DeletedDate',
                direction: 'DESC'
            }]
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
                text: l10n.ns('core', 'BaseDeletedEntity').value('DeletedDate'),
                dataIndex: 'DeletedDate',
                xtype: 'datecolumn',
                renderer: Ext.util.Format.dateRenderer('d.m.Y H:i:s')
            }, {
                text: l10n.ns('tpm', 'Brand').value('Name'),
                dataIndex: 'Name'
            }, {
                text: l10n.ns('tpm', 'Brand').value('Brand_code'),
                dataIndex: 'Brand_code'
            }, {
                text: l10n.ns('tpm', 'Brand').value('Segmen_code'),
                dataIndex: 'Segmen_code'
            }]
        }
    }, {
        xtype: 'editabledetailform',
        itemId: 'detailform',
        model: 'App.model.tpm.brand.DeletedBrand',
        items: [{
            xtype: 'singlelinedisplayfield',
            name: 'DeletedDate',
            renderer: Ext.util.Format.dateRenderer('d.m.Y H:i:s'),
            fieldLabel: l10n.ns('core', 'BaseDeletedEntity').value('DeletedDate')
        }, {
            xtype: 'singlelinedisplayfield',
            name: 'Name',
            fieldLabel: l10n.ns('tpm', 'Brand').value('Name'),
        }, {
            xtype: 'singlelinedisplayfield',
            name: 'Brand_code',
            fieldLabel: l10n.ns('tpm', 'Brand').value('Brand_code'),
        }, {
            xtype: 'singlelinedisplayfield',
            name: 'Segmen_code',
            fieldLabel: l10n.ns('tpm', 'Brand').value('Segmen_code'),
        }]
    }]
});
