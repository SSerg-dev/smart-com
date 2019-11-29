﻿Ext.define('App.view.tpm.promoproductcorrection.DeletedPromoProductCorrection', {
    extend: 'App.view.core.common.CombinedDirectoryPanel',
    alias: 'widget.deletedpromoproductcorrection',
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
            model: 'App.model.tpm.promoproductcorrection.DeletedPromoProductCorrection',
            storeId: 'deletedpromoproductcorrectionstore',
            extendedFilter: {
                xclass: 'App.ExtFilterContext',
                supportedModels: [{
                    xclass: 'App.ExtSelectionFilterModel',
                    model: 'App.model.tpm.promoproductcorrection.DeletedPromoProductCorrection',
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
                    text: l10n.ns('tpm', 'PromoProductCorrection').value('Number'),
                    dataIndex: 'Number'
            }, {
                    text: l10n.ns('tpm', 'PromoProductCorrection').value('ZREP'),
                    dataIndex: 'ZREP'
            },
            {
                xtype: 'numbercolumn',
                format: '0.00',
                text: l10n.ns('tpm', 'PromoProductCorrection').value('PlanProductUpliftPercentCorrected'),
                dataIndex: 'PlanProductUpliftPercentCorrected'
                }, {
                xtype: 'datecolumn',
                renderer: Ext.util.Format.dateRenderer('d.m.Y H:i:s'),
                text: l10n.ns('tpm', 'PromoProductCorrection').value('CreateDate'),
                dataIndex: 'CreateDate'
                }, {
                xtype: 'datecolumn',
                renderer: Ext.util.Format.dateRenderer('d.m.Y H:i:s'),
                text: l10n.ns('tpm', 'PromoProductCorrection').value('ChangeDate'),
                dataIndex: 'ChangeDate'
                }, {
                  
                text: l10n.ns('tpm', 'PromoProductCorrection').value('UserName'),
                dataIndex: 'UserName'
                }]
        }
    }, {
        xtype: 'editabledetailform',
        itemId: 'detailform',
            model: 'App.model.tpm.promoproductcorrection.DeletedPromoProductCorrection',
        items: [{
            xtype: 'singlelinedisplayfield',
            name: 'DeletedDate',
            renderer: Ext.util.Format.dateRenderer('d.m.Y H:i:s'),
            fieldLabel: l10n.ns('core', 'BaseDeletedEntity').value('DeletedDate')
        } ]
    }]
});