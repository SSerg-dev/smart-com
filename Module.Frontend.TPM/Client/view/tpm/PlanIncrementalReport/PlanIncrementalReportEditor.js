Ext.define('App.view.tpm.planincrementalreport.PlanIncrementalReportEditor', {
    extend: 'App.view.core.common.EditorDetailWindow',
    alias: 'widget.planincrementalreporteditor',
    width: 500,
    minWidth: 800,
    maxHeight: 500,
    cls: 'readOnlyFields',

    noChange: true,

    items: {
        xtype: 'editorform',
        columnsCount: 2,
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
                listeners: {
                    afterrender: function (value) {
                        this.setValue(value.rawValue === 'true' ? l10n.ns('core', 'booleanValues').value('true') : l10n.ns('core', 'booleanValues').value('false'));
                    }
                }
            }
        ]
    }
});
