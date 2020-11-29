Ext.define('App.view.tpm.nonpromoequipment.DeletedNonPromoEquipment', {
    extend: 'App.view.core.common.CombinedDirectoryPanel',
    alias: 'widget.deletednonpromoequipment',
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
            model: 'App.model.tpm.nonpromoequipment.DeletedNonPromoEquipment',
            storeId: 'deletednonpromoequipmentstore',
            extendedFilter: {
                xclass: 'App.ExtFilterContext',
                supportedModels: [{
                    xclass: 'App.ExtSelectionFilterModel',
                    model: 'App.model.tpm.nonpromoequipment.DeletedNonPromoEquipment',
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
                text: l10n.ns('tpm', 'NonPromoEquipment').value('EquipmentType'),
                dataIndex: 'EquipmentType'
            }, {
                text: l10n.ns('tpm', 'NonPromoEquipment').value('Description_ru'),
                dataIndex: 'Description_ru'
            }]
        }
    }, {
        xtype: 'editabledetailform',
        itemId: 'detailform',
        model: 'App.model.tpm.nonpromoequipment.DeletedNonPromoEquipment',
        items: [{
            xtype: 'singlelinedisplayfield',
            name: 'DeletedDate',
            renderer: Ext.util.Format.dateRenderer('d.m.Y H:i:s'),
            fieldLabel: l10n.ns('core', 'BaseDeletedEntity').value('DeletedDate')
        }, {
            xtype: 'singlelinedisplayfield',
            name: 'EquipmentType',
            fieldLabel: l10n.ns('tpm', 'NonPromoEquipment').value('EquipmentType'),
        }, {
            xtype: 'singlelinedisplayfield',
            name: 'Description_ru',
            fieldLabel: l10n.ns('tpm', 'NonPromoEquipment').value('Description_ru'),
        }]
    }]
});
