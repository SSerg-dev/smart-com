Ext.define('App.view.tpm.planincrementalreport.PlanIncrementalReport', {
    extend: 'App.view.core.common.CombinedDirectoryPanel',
    alias: 'widget.planincrementalreport',
    title: l10n.ns('tpm', 'compositePanelTitles').value('PlanIncrementalReport'),

    items: [{
        xtype: 'directorygrid',
        itemId: 'datatable',
        editorModel: 'Core.form.EditorDetailWindowModel',
        store: {
            type: 'directorystore',
            model: 'App.model.tpm.planincrementalreport.PlanIncrementalReport',
            storeId: 'planincrementalreportstore',
            extendedFilter: {
                xclass: 'App.ExtFilterContext',
                supportedModels: [{
                    xclass: 'App.ExtSelectionFilterModel',
                    model: 'App.model.tpm.planincrementalreport.PlanIncrementalReport',
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
                { text: l10n.ns('tpm', 'PlanIncrementalReport').value('ZREP'), dataIndex: 'ZREP' },
                { text: l10n.ns('tpm', 'PlanIncrementalReport').value('DemandCode'), dataIndex: 'DemandCode' },
                { text: l10n.ns('tpm', 'PlanIncrementalReport').value('PromoNameId'), dataIndex: 'PromoNameId' },
                { text: l10n.ns('tpm', 'PlanIncrementalReport').value('LocApollo'), dataIndex: 'LocApollo' },
                { text: l10n.ns('tpm', 'PlanIncrementalReport').value('TypeApollo'), dataIndex: 'TypeApollo' },
                { text: l10n.ns('tpm', 'PlanIncrementalReport').value('ModelApollo'), dataIndex: 'ModelApollo' },
                { xtype: 'datecolumn', text: l10n.ns('tpm', 'PlanIncrementalReport').value('WeekStartDate'), dataIndex: 'WeekStartDate', renderer: Ext.util.Format.dateRenderer('d.m.Y') },
                { xtype: 'numbercolumn', text: l10n.ns('tpm', 'PlanIncrementalReport').value('PlanProductCaseQty'), dataIndex: 'PlanProductCaseQty' },
                { xtype: 'numbercolumn', text: l10n.ns('tpm', 'PlanIncrementalReport').value('PlanUplift'), dataIndex: 'PlanUplift' },
				{ xtype: 'datecolumn', text: l10n.ns('tpm', 'PlanIncrementalReport').value('DispatchesStart'), dataIndex: 'DispatchesStart', renderer: Ext.util.Format.dateRenderer('d.m.Y') },
				{ xtype: 'datecolumn', text: l10n.ns('tpm', 'PlanIncrementalReport').value('DispatchesEnd'), dataIndex: 'DispatchesEnd', renderer: Ext.util.Format.dateRenderer('d.m.Y') },
				{ text: l10n.ns('tpm', 'PlanIncrementalReport').value('Week'), dataIndex: 'Week' },
				{ text: l10n.ns('tpm', 'PlanIncrementalReport').value('Status'), dataIndex: 'Status' },
				{ xtype: 'numbercolumn', text: l10n.ns('tpm', 'PlanIncrementalReport').value('PlanProductBaselineCaseQty'), dataIndex: 'PlanProductBaselineCaseQty' },
				{ xtype: 'numbercolumn', text: l10n.ns('tpm', 'PlanIncrementalReport').value('PlanProductIncrementalLSV'), dataIndex: 'PlanProductIncrementalLSV' },
				{ xtype: 'numbercolumn', text: l10n.ns('tpm', 'PlanIncrementalReport').value('PlanProductBaselineLSV'), dataIndex: 'PlanProductBaselineLSV' },
                {
                    text: l10n.ns('tpm', 'PlanIncrementalReport').value('InOut'),
                    dataIndex: 'InOut',
                    renderer: function (value) {
                        return value ? l10n.ns('core', 'booleanValues').value('true') : l10n.ns('core', 'booleanValues').value('false');
                    }
                }
            ]
        }
    }, {
        xtype: 'editabledetailform',
        itemId: 'detailform',
        model: 'App.model.tpm.planincrementalreport.PlanIncrementalReport',
            items: [
                { xtype: 'textfield', name: 'ZREP', fieldLabel: l10n.ns('tpm', 'PlanIncrementalReport').value('ZREP') },
                { xtype: 'textfield', name: 'DemandCode', fieldLabel: l10n.ns('tpm', 'PlanIncrementalReport').value('DemandCode') },
                { xtype: 'textfield', name: 'PromoNameId', fieldLabel: l10n.ns('tpm', 'PlanIncrementalReport').value('PromoNameId') },
                { xtype: 'textfield', name: 'LocApollo', fieldLabel: l10n.ns('tpm', 'PlanIncrementalReport').value('LocApollo') },
                { xtype: 'textfield', name: 'TypeApollo', fieldLabel: l10n.ns('tpm', 'PlanIncrementalReport').value('TypeApollo') },
                { xtype: 'textfield', name: 'ModelApollo', fieldLabel: l10n.ns('tpm', 'PlanIncrementalReport').value('ModelApollo') },
                { xtype: 'datefield', name: 'WeekStartDate', fieldLabel: l10n.ns('tpm', 'PlanIncrementalReport').value('WeekStartDate') },
                { xtype: 'numberfield', name: 'PlanProductCaseQty', fieldLabel: l10n.ns('tpm', 'PlanIncrementalReport').value('PlanProductCaseQty') },
                { xtype: 'numberfield', name: 'PlanUplift', fieldLabel: l10n.ns('tpm', 'PlanIncrementalReport').value('PlanUplift') },
				{ xtype: 'datefield', name: 'DispatchesStart', fieldLabel: l10n.ns('tpm', 'PlanIncrementalReport').value('DispatchesStart') },
				{ xtype: 'datefield', name: 'DispatchesEnd', fieldLabel: l10n.ns('tpm', 'PlanIncrementalReport').value('DispatchesEnd') },
				{ xtype: 'textfield', name: 'Week', fieldLabel: l10n.ns('tpm', 'PlanIncrementalReport').value('Week') },
				{ xtype: 'textfield', name: 'Status', fieldLabel: l10n.ns('tpm', 'PlanIncrementalReport').value('Status') },
				{ xtype: 'numberfield', name: 'PlanProductBaselineCaseQty', fieldLabel: l10n.ns('tpm', 'PlanIncrementalReport').value('PlanProductBaselineCaseQty') },
				{ xtype: 'numberfield', name: 'PlanProductIncrementalLSV', fieldLabel: l10n.ns('tpm', 'PlanIncrementalReport').value('PlanProductIncrementalLSV') },
				{ xtype: 'numberfield', name: 'PlanProductBaselineLSV', fieldLabel: l10n.ns('tpm', 'PlanIncrementalReport').value('PlanProductBaselineLSV') },
                {
                    xtype: 'textfield',
                    name: 'InOut',
                    fieldLabel: l10n.ns('tpm', 'PlanIncrementalReport').value('InOut'),
                }
        ]
    }]
});
