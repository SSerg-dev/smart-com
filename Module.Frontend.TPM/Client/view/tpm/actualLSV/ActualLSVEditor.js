Ext.define('App.view.tpm.actualLSV.ActualLSVEditor', {
    extend: 'App.view.core.common.EditorDetailWindow',
    alias: 'widget.actuallsveditor',
    //width: 500,
    minWidth: 800,
    maxHeight: 600,
    cls: 'readOnlyFields',

    afterWindowShow: function (scope, isCreating) {
        scope.down('numberfield[name=ActualPromoBaselineLSV]').focus(true, 10);
    },

    items: {
        xtype: 'editorform',
        columnsCount: 2,
        items: [{
            xtype: 'singlelinedisplayfield',
            fieldLabel: l10n.ns('tpm', 'ActualLSV').value('Number'),
            name: 'Number',
        }, {
            xtype: 'singlelinedisplayfield',
            fieldLabel: l10n.ns('tpm', 'ActualLSV').value('ClientHierarchy'),
            name: 'ClientHierarchy',
        }, {
            xtype: 'singlelinedisplayfield',
            fieldLabel: l10n.ns('tpm', 'ActualLSV').value('Name'),
            name: 'Name',
        }, {
            xtype: 'singlelinedisplayfield',
            fieldLabel: l10n.ns('tpm', 'ActualLSV').value('BrandTech'),
            name: 'BrandTech',
        }, {
            xtype: 'singlelinedisplayfield',
            fieldLabel: l10n.ns('tpm', 'ActualLSV').value('Event'),
            name: 'Event',
        }, {
            xtype: 'singlelinedisplayfield',
            fieldLabel: l10n.ns('tpm', 'ActualLSV').value('Mechanic'),
            name: 'Mechanic',
        }, {
            xtype: 'singlelinedisplayfield',
            fieldLabel: l10n.ns('tpm', 'ActualLSV').value('MechanicIA'),
            name: 'MechanicIA',
        }, {
            xtype: 'singlelinedisplayfield',
            format: 'd.m.Y',
            fieldLabel: l10n.ns('tpm', 'ActualLSV').value('StartDate'),
            name: 'StartDate',
        }, {
            xtype: 'singlelinedisplayfield',
            fieldLabel: l10n.ns('tpm', 'ActualLSV').value('MarsStartDate'),
            name: 'MarsStartDate',
        }, {
            xtype: 'singlelinedisplayfield',
            format: 'd.m.Y',
            fieldLabel: l10n.ns('tpm', 'ActualLSV').value('EndDate'),
            name: 'EndDate',
        }, {
            xtype: 'singlelinedisplayfield',
            fieldLabel: l10n.ns('tpm', 'ActualLSV').value('MarsEndDate'),
            name: 'MarsEndDate',
        }, {
            xtype: 'singlelinedisplayfield',
            fieldLabel: l10n.ns('tpm', 'ActualLSV').value('DispatchesStart'),
            name: 'DispatchesStart',
        }, {
            xtype: 'singlelinedisplayfield',
            fieldLabel: l10n.ns('tpm', 'ActualLSV').value('MarsDispatchesStart'),
            name: 'MarsDispatchesStart',
        }, {
            xtype: 'singlelinedisplayfield',
            fieldLabel: l10n.ns('tpm', 'ActualLSV').value('DispatchesEnd'),
            name: 'DispatchesEnd',
        }, {
            xtype: 'singlelinedisplayfield',
            fieldLabel: l10n.ns('tpm', 'ActualLSV').value('MarsDispatchesEnd'),
            name: 'MarsDispatchesEnd',
        }, {
            xtype: 'singlelinedisplayfield',
            fieldLabel: l10n.ns('tpm', 'ActualLSV').value('Status'),
            name: 'Status',
        },
        {
            xtype: 'singlelinedisplayfield',
            fieldLabel: l10n.ns('tpm', 'ActualLSV').value('ActualInStoreDiscount'),
            name: 'ActualInStoreDiscount'
        },
        {
            xtype: 'singlelinedisplayfield',
            fieldLabel: l10n.ns('tpm', 'ActualLSV').value('PlanPromoUpliftPercent'),
            name: 'PlanPromoUpliftPercent'
        },
        {
            xtype: 'singlelinedisplayfield',
            fieldLabel: l10n.ns('tpm', 'ActualLSV').value('ActualPromoUpliftPercent'),
            name: 'ActualPromoUpliftPercent'
        },
        {
            xtype: 'singlelinedisplayfield',
            fieldLabel: l10n.ns('tpm', 'ActualLSV').value('PlanPromoBaselineLSV'),
            name: 'PlanPromoBaselineLSV'
        },
        {
            xtype: 'numberfield',
            fieldLabel: l10n.ns('tpm', 'ActualLSV').value('ActualPromoBaselineLSV'),
            name: 'ActualPromoBaselineLSV',
            minValue: 0,
            maxValue: 100000000000000000000,
            allowDecimals: true,
            allowExponential: false,
            allowOnlyWhitespace: true,
            allowBlank: true,
        },
        {
            xtype: 'singlelinedisplayfield',
            fieldLabel: l10n.ns('tpm', 'ActualLSV').value('PlanPromoIncrementalLSV'),
            name: 'PlanPromoIncrementalLSV'
        },
        {
            xtype: 'singlelinedisplayfield',
            fieldLabel: l10n.ns('tpm', 'ActualLSV').value('ActualPromoIncrementalLSV'),
            name: 'ActualPromoIncrementalLSV'
        },
        {
            xtype: 'singlelinedisplayfield',
            fieldLabel: l10n.ns('tpm', 'ActualLSV').value('PlanPromoLSV'),
            name: 'PlanPromoLSV'
        },
        {
            xtype: 'singlelinedisplayfield',
            fieldLabel: l10n.ns('tpm', 'ActualLSV').value('ActualPromoLSVByCompensation'),
            name: 'ActualPromoLSVByCompensation'
        },
        {
            xtype: 'numberfield',
            fieldLabel: l10n.ns('tpm', 'ActualLSV').value('ActualPromoLSV'),
            name: 'ActualPromoLSV',
            minValue: 0,
            maxValue: 100000000000000000000,
            allowDecimals: true,
            allowExponential: false,
            allowOnlyWhitespace: true,
            allowBlank: true,
        },
        {
            xtype: 'singlelinedisplayfield',
            fieldLabel: l10n.ns('tpm', 'ActualLSV').value('PlanPromoPostPromoEffectLSVW1'),
            name: 'PlanPromoPostPromoEffectLSVW1'
        },
        {
            xtype: 'numberfield',
            fieldLabel: l10n.ns('tpm', 'ActualLSV').value('ActualPromoPostPromoEffectLSVW1'),
            name: 'ActualPromoPostPromoEffectLSVW1',
            allowDecimals: true,
            allowExponential: false,
            allowOnlyWhitespace: true,
            allowBlank: true,
        },
        {
            xtype: 'singlelinedisplayfield',
            fieldLabel: l10n.ns('tpm', 'ActualLSV').value('PlanPromoPostPromoEffectLSVW2'),
            name: 'PlanPromoPostPromoEffectLSVW2'
        },
        {
            xtype: 'numberfield',
            fieldLabel: l10n.ns('tpm', 'ActualLSV').value('ActualPromoPostPromoEffectLSVW2'),
            name: 'ActualPromoPostPromoEffectLSVW2',
            allowDecimals: true,
            allowExponential: false,
            allowOnlyWhitespace: true,
            allowBlank: true,
        },
        {
            xtype: 'singlelinedisplayfield',
            fieldLabel: l10n.ns('tpm', 'ActualLSV').value('PlanPromoPostPromoEffectLSV'),
            name: 'PlanPromoPostPromoEffectLSV'
        },
        {
            xtype: 'singlelinedisplayfield',
            fieldLabel: l10n.ns('tpm', 'ActualLSV').value('ActualPromoPostPromoEffectLSV'),
            name: 'ActualPromoPostPromoEffectLSV'
        }]
    }
});     