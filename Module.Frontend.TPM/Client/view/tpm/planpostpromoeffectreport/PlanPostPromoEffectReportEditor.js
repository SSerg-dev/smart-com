Ext.define('App.view.tpm.planpostpromoeffectreport.PlanPostPromoEffectReportEditor', {
    extend: 'App.view.core.common.EditorDetailWindow',
    alias: 'widget.planpostpromoeffectreporteditor',
    width: 500,
    minWidth: 800,
    maxHeight: 500,
    cls: 'readOnlyFields',

    noChange: true,

    items: {
        xtype: 'editorform',
        columnsCount: 2,
        items: [
            { xtype: 'textfield', name: 'ZREP', fieldLabel: l10n.ns('tpm', 'PlanPostPromoEffectReport').value('ZREP') },
            { xtype: 'textfield', name: 'DemandCode', fieldLabel: l10n.ns('tpm', 'PlanPostPromoEffectReport').value('DemandCode') },
            { xtype: 'textfield', name: 'PromoNameId', fieldLabel: l10n.ns('tpm', 'PlanPostPromoEffectReport').value('PromoNameId') },
            { xtype: 'textfield', name: 'LocApollo', fieldLabel: l10n.ns('tpm', 'PlanPostPromoEffectReport').value('LocApollo') },
            { xtype: 'textfield', name: 'TypeApollo', fieldLabel: l10n.ns('tpm', 'PlanPostPromoEffectReport').value('TypeApollo') },
            { xtype: 'textfield', name: 'ModelApollo', fieldLabel: l10n.ns('tpm', 'PlanPostPromoEffectReport').value('ModelApollo') },
            { xtype: 'datefield', name: 'WeekStartDate', fieldLabel: l10n.ns('tpm', 'PlanPostPromoEffectReport').value('WeekStartDate') },
            {
                xtype: 'numberfield', name: 'PlanPostPromoEffectQty', fieldLabel: l10n.ns('tpm', 'PlanPostPromoEffectReport').value('PlanPostPromoEffectQty')
			},
			{ xtype: 'numberfield', name: 'PlanUplift', fieldLabel: l10n.ns('tpm', 'PlanPostPromoEffectReport').value('PlanUplift') },
            { xtype: 'datefield', name: 'StartDate', fieldLabel: l10n.ns('tpm', 'PlanPostPromoEffectReport').value('StartDate') },
			{ xtype: 'datefield', name: 'EndDate', fieldLabel: l10n.ns('tpm', 'PlanPostPromoEffectReport').value('EndDate') },
			{ xtype: 'textfield', name: 'Week', fieldLabel: l10n.ns('tpm', 'PlanPostPromoEffectReport').value('Week') },
			{ xtype: 'textfield', name: 'Status', fieldLabel: l10n.ns('tpm', 'PlanPostPromoEffectReport').value('Status') },
			{ xtype: 'numberfield', name: 'PlanProductBaselineCaseQty', fieldLabel: l10n.ns('tpm', 'PlanPostPromoEffectReport').value('PlanProductBaselineCaseQty') },
			{ xtype: 'numberfield', name: 'PlanProductPostPromoEffectLSV', fieldLabel: l10n.ns('tpm', 'PlanPostPromoEffectReport').value('PlanProductPostPromoEffectLSV') },
			{ xtype: 'numberfield', name: 'PlanProductBaselineLSV', fieldLabel: l10n.ns('tpm', 'PlanPostPromoEffectReport').value('PlanProductBaselineLSV') },
            {
                xtype: 'textfield',
                name: 'InOut',
                fieldLabel: l10n.ns('tpm', 'PlanPostPromoEffectReport').value('InOut'),
                listeners: {
                    afterrender: function (value) {
                        this.setValue(value.rawValue === 'true' ? l10n.ns('core', 'booleanValues').value('true') : l10n.ns('core', 'booleanValues').value('false'));
                    }
                }
            }
        ]
    }
});
