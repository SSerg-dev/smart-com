Ext.define('App.view.tpm.planpostpromoeffectreport.PlanPostPromoEffectReport', {
    extend: 'App.view.core.common.CombinedDirectoryPanel',
    alias: 'widget.planpostpromoeffectreport',
    title: l10n.ns('tpm', 'compositePanelTitles').value('PlanPostPromoEffectReport'),

    items: [{
        xtype: 'directorygrid',
        itemId: 'datatable',
        editorModel: 'Core.form.EditorDetailWindowModel',
        store: {
            type: 'directorystore',
            model: 'App.model.tpm.planpostpromoeffectreport.PlanPostPromoEffectReport',
            storeId: 'planpostpromoeffectreportstore',
            extendedFilter: {
                xclass: 'App.ExtFilterContext',
                supportedModels: [{
                    xclass: 'App.ExtSelectionFilterModel',
                    model: 'App.model.tpm.planpostpromoeffectreport.PlanPostPromoEffectReport',
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
            items: [
                { text: l10n.ns('tpm', 'PlanPostPromoEffectReport').value('ZREP'), dataIndex: 'ZREP' },
            { text: l10n.ns('tpm', 'PlanPostPromoEffectReport').value('DemandCode'), dataIndex: 'DemandCode' },
            { text: l10n.ns('tpm', 'PlanPostPromoEffectReport').value('PromoName'), dataIndex: 'PromoName' },
            { text: l10n.ns('tpm', 'PlanPostPromoEffectReport').value('PromoNameId'), dataIndex: 'PromoNameId' },
            { text: l10n.ns('tpm', 'PlanPostPromoEffectReport').value('LocApollo'), dataIndex: 'LocApollo' },
            { text: l10n.ns('tpm', 'PlanPostPromoEffectReport').value('TypeApollo'), dataIndex: 'TypeApollo' },
            { text: l10n.ns('tpm', 'PlanPostPromoEffectReport').value('ModelApollo'), dataIndex: 'ModelApollo' },
            { xtype: 'datecolumn', text: l10n.ns('tpm', 'PlanPostPromoEffectReport').value('WeekStartDate'), dataIndex: 'WeekStartDate', renderer: Ext.util.Format.dateRenderer('d.m.Y') },
            { xtype: 'numbercolumn', text: l10n.ns('tpm', 'PlanPostPromoEffectReport').value('PlanPostPromoEffectQty'), dataIndex: 'PlanPostPromoEffectQty' },
            { xtype: 'datecolumn', text: l10n.ns('tpm', 'PlanPostPromoEffectReport').value('StartDate'), dataIndex: 'StartDate', renderer: Ext.util.Format.dateRenderer('d.m.Y') },
            { xtype: 'datecolumn', text: l10n.ns('tpm', 'PlanPostPromoEffectReport').value('EndDate'), dataIndex: 'EndDate', renderer: Ext.util.Format.dateRenderer('d.m.Y') },
            { text: l10n.ns('tpm', 'PlanPostPromoEffectReport').value('Status'), dataIndex: 'Status' }]
        }
    }, {
        xtype: 'editabledetailform',
        itemId: 'detailform',
        model: 'App.model.tpm.planpostpromoeffectreport.PlanPostPromoEffectReport',
        items: [{ xtype: 'textfield', name: 'ZREP', fieldLabel: l10n.ns('tpm', 'PlanPostPromoEffectReport').value('ZREP') },
                { xtype: 'textfield', name: 'DemandCode', fieldLabel: l10n.ns('tpm', 'PlanPostPromoEffectReport').value('DemandCode') },
                { xtype: 'textfield', name: 'PromoName', fieldLabel: l10n.ns('tpm', 'PlanPostPromoEffectReport').value('PromoName') },
                { xtype: 'textfield', name: 'PromoNameId', fieldLabel: l10n.ns('tpm', 'PlanPostPromoEffectReport').value('PromoNameId') },
                { xtype: 'textfield', name: 'LocApollo', fieldLabel: l10n.ns('tpm', 'PlanPostPromoEffectReport').value('LocApollo') },
                { xtype: 'textfield', name: 'TypeApollo', fieldLabel: l10n.ns('tpm', 'PlanPostPromoEffectReport').value('TypeApollo') },
                { xtype: 'textfield', name: 'ModelApollo', fieldLabel: l10n.ns('tpm', 'PlanPostPromoEffectReport').value('ModelApollo') },
                { xtype: 'datefield', name: 'WeekStartDate', fieldLabel: l10n.ns('tpm', 'PlanPostPromoEffectReport').value('WeekStartDate') },
                { xtype: 'numberfield', name: 'PlanPostPromoEffectQty', fieldLabel: l10n.ns('tpm', 'PlanPostPromoEffectReport').value('PlanPostPromoEffectQty') },
                { xtype: 'datefield', name: 'StartDate', fieldLabel: l10n.ns('tpm', 'PlanPostPromoEffectReport').value('StartDate') },
                { xtype: 'datefield', name: 'EndDate', fieldLabel: l10n.ns('tpm', 'PlanPostPromoEffectReport').value('EndDate') },
                { xtype: 'textfield', name: 'Status', fieldLabel: l10n.ns('tpm', 'PlanPostPromoEffectReport').value('Status') }
        ]
    }]
});
