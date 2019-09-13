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
            
			{ xtype: 'numberfield', name: 'PlanUplift', fieldLabel: l10n.ns('tpm', 'PlanPostPromoEffectReport').value('PlanUplift') },
			{ xtype: 'datefield', name: 'DispatchesStart', fieldLabel: l10n.ns('tpm', 'PlanPostPromoEffectReport').value('DispatchesStart') },
			{ xtype: 'datefield', name: 'DispatchesEnd', fieldLabel: l10n.ns('tpm', 'PlanPostPromoEffectReport').value('DispatchesEnd') },
			{ xtype: 'textfield', name: 'Week', fieldLabel: l10n.ns('tpm', 'PlanPostPromoEffectReport').value('Week') },
			{ xtype: 'textfield', name: 'Status', fieldLabel: l10n.ns('tpm', 'PlanPostPromoEffectReport').value('Status') },
            { xtype: 'numberfield', name: 'PlanPostPromoEffectQtyW1', fieldLabel: l10n.ns('tpm', 'PlanPostPromoEffectReport').value('PlanPostPromoEffectQtyW1'), dataIndex: 'PlanPostPromoEffectQtyW1' },
            { xtype: 'numberfield', name: 'PlanProductBaselineCaseQtyW1', fieldLabel: l10n.ns('tpm', 'PlanPostPromoEffectReport').value('PlanProductBaselineCaseQtyW1'), dataIndex: 'PlanProductBaselineCaseQtyW1' },
            { xtype: 'numberfield', name: 'PlanProductPostPromoEffectLSVW1', fieldLabel: l10n.ns('tpm', 'PlanPostPromoEffectReport').value('PlanProductPostPromoEffectLSVW1'), dataIndex: 'PlanProductPostPromoEffectLSVW1' },
            { xtype: 'numberfield', name: 'PlanProductBaselineLSVW1', fieldLabel: l10n.ns('tpm', 'PlanPostPromoEffectReport').value('PlanProductBaselineLSVW1'), dataIndex: 'PlanProductBaselineLSVW1' },
            { xtype: 'numberfield', name: 'PlanPostPromoEffectQtyW2', fieldLabel: l10n.ns('tpm', 'PlanPostPromoEffectReport').value('PlanPostPromoEffectQtyW2'), dataIndex: 'PlanPostPromoEffectQtyW2' },
            { xtype: 'numberfield', name: 'PlanProductBaselineCaseQtyW2', fieldLabel: l10n.ns('tpm', 'PlanPostPromoEffectReport').value('PlanProductBaselineCaseQtyW2'), dataIndex: 'PlanProductBaselineCaseQtyW2' },
            { xtype: 'numberfield', name: 'PlanProductPostPromoEffectLSVW2', fieldLabel: l10n.ns('tpm', 'PlanPostPromoEffectReport').value('PlanProductPostPromoEffectLSVW2'), dataIndex: 'PlanProductPostPromoEffectLSVW2' },
            { xtype: 'numberfield', name: 'PlanProductBaselineLSVW2', fieldLabel: l10n.ns('tpm', 'PlanPostPromoEffectReport').value('PlanProductBaselineLSVW2'), dataIndex: 'PlanProductBaselineLSVW2' },

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
