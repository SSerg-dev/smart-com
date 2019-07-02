Ext.define('App.view.tpm.planincrementalreport.PlanIncrementalReportEditor', {
    extend: 'App.view.core.common.EditorDetailWindow',
    alias: 'widget.planincrementalreporteditor',
    width: 500,
    minWidth: 500,
    maxHeight: 500,
    cls: 'readOnlyFields',

    noChange: true,

    items: {
        xtype: 'editorform',
        columnsCount: 1,
        items: [{ xtype: 'textfield', name: 'ZREP', fieldLabel: l10n.ns('tpm', 'PlanIncrementalReport').value('ZREP') },
                { xtype: 'textfield', name: 'DemandCode', fieldLabel: l10n.ns('tpm', 'PlanIncrementalReport').value('DemandCode') },
                { xtype: 'textfield', name: 'PromoName', fieldLabel: l10n.ns('tpm', 'PlanIncrementalReport').value('PromoName') },
                { xtype: 'textfield', name: 'PromoNameId', fieldLabel: l10n.ns('tpm', 'PlanIncrementalReport').value('PromoNameId') },
                { xtype: 'textfield', name: 'LocApollo', fieldLabel: l10n.ns('tpm', 'PlanIncrementalReport').value('LocApollo') },
                { xtype: 'textfield', name: 'TypeApollo', fieldLabel: l10n.ns('tpm', 'PlanIncrementalReport').value('TypeApollo') },
                { xtype: 'textfield', name: 'ModelApollo', fieldLabel: l10n.ns('tpm', 'PlanIncrementalReport').value('ModelApollo') },
                { xtype: 'datefield', name: 'WeekStartDate', fieldLabel: l10n.ns('tpm', 'PlanIncrementalReport').value('WeekStartDate') },
            { xtype: 'numberfield', name: 'PlanProductCaseQty', fieldLabel: l10n.ns('tpm', 'PlanIncrementalReport').value('PlanProductCaseQty') },
                { xtype: 'numberfield', name: 'PlanUplift', fieldLabel: l10n.ns('tpm', 'PlanIncrementalReport').value('PlanUplift') },
                { xtype: 'datefield', name: 'StartDate', fieldLabel: l10n.ns('tpm', 'PlanIncrementalReport').value('StartDate') },
                { xtype: 'datefield', name: 'EndDate', fieldLabel: l10n.ns('tpm', 'PlanIncrementalReport').value('EndDate') },
            { xtype: 'textfield', name: 'Status', fieldLabel: l10n.ns('tpm', 'PlanIncrementalReport').value('Status') },
            { xtype: 'textfield', name: 'InOut', fieldLabel: l10n.ns('tpm', 'PlanIncrementalReport').value('InOut') }
        ]
    }
});
