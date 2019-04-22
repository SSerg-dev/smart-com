Ext.define('App.view.tpm.demand.DemandEditor', {
    extend: 'App.view.core.common.EditorDetailWindow',
    alias: 'widget.demandeditor',
    width: 800,
    minWidth: 800,
    maxHeight: 600,

    afterWindowShow: function () {
        this.down('numberfield[name=PlanBaseline]').focus(true, 10);
    },

    items: {
        xtype: 'editorform',
        items: [{
            xtype: 'singlelinedisplayfield',
            fieldLabel: l10n.ns('tpm', 'Demand').value('Number'),
            name: 'Number'
        }, {
            xtype: 'singlelinedisplayfield',
            name: 'Name',
            fieldLabel: l10n.ns('tpm', 'Demand').value('Name'),
        }, {
            xtype: 'singlelinedisplayfield',
            fieldLabel: l10n.ns('tpm', 'Demand').value('ClientCommercialSubnetCommercialNetName'),
            name: 'ClientCommercialSubnetCommercialNetName'
        }, {
            xtype: 'singlelinedisplayfield',
            fieldLabel: l10n.ns('tpm', 'Demand').value('BrandName'),
            name: 'BrandName'
        }, {
            xtype: 'singlelinedisplayfield',
            fieldLabel: l10n.ns('tpm', 'Demand').value('BrandTechName'),
            name: 'BrandTechName'
        }, {
            xtype: 'singlelinedisplayfield',
            renderer: Ext.util.Format.dateRenderer('d.m.Y'),
            name: 'StartDate',
            fieldLabel: l10n.ns('tpm', 'Demand').value('StartDate'),
        }, {
            xtype: 'singlelinedisplayfield',
            renderer: Ext.util.Format.dateRenderer('d.m.Y'),
            name: 'EndDate',
            fieldLabel: l10n.ns('tpm', 'Demand').value('EndDate'),
        }, {
            xtype: 'singlelinedisplayfield',
            renderer: Ext.util.Format.dateRenderer('d.m.Y'),
            name: 'DispatchesStart',
            fieldLabel: l10n.ns('tpm', 'Demand').value('DispatchesStart'),
        }, {
            xtype: 'singlelinedisplayfield',
            renderer: Ext.util.Format.dateRenderer('d.m.Y'),
            name: 'DispatchesEnd',
            fieldLabel: l10n.ns('tpm', 'Demand').value('DispatchesEnd'),
        }, {
            xtype: 'numberfield', allowDecimals: false, allowExponential: false, allowBlank: true, allowOnlyWhitespace: true,
            name: 'PlanBaseline',
            minValue: 0,
            fieldLabel: l10n.ns('tpm', 'Demand').value('PlanBaseline'),
        }, {
            xtype: 'numberfield', allowDecimals: false, allowExponential: false, allowBlank: true, allowOnlyWhitespace: true,
            name: 'PlanDuration',
            minValue: 0,
            fieldLabel: l10n.ns('tpm', 'Demand').value('PlanDuration'),
        }, {
            xtype: 'numberfield', allowDecimals: false, allowExponential: false, allowBlank: true, allowOnlyWhitespace: true,
            name: 'PlanUplift',
            minValue: 0,
            fieldLabel: l10n.ns('tpm', 'Demand').value('PlanUplift'),
        }, {
            xtype: 'numberfield', allowDecimals: false, allowExponential: false, allowBlank: true, allowOnlyWhitespace: true,
            name: 'PlanIncremental',
            minValue: 0,
            fieldLabel: l10n.ns('tpm', 'Demand').value('PlanIncremental'),
        }, {
            xtype: 'numberfield', allowDecimals: false, allowExponential: false, allowBlank: true, allowOnlyWhitespace: true,
            name: 'PlanActivity',
            minValue: 0,
            fieldLabel: l10n.ns('tpm', 'Demand').value('PlanActivity'),
        }, {
            xtype: 'numberfield', allowDecimals: false, allowExponential: false, allowBlank: true, allowOnlyWhitespace: true,
            name: 'PlanSteal',
            minValue: 0,
            fieldLabel: l10n.ns('tpm', 'Demand').value('PlanSteal'),
        }, {
            xtype: 'numberfield', allowDecimals: false, allowExponential: false, allowBlank: true, allowOnlyWhitespace: true,
            name: 'FactBaseline',
            minValue: 0,
            fieldLabel: l10n.ns('tpm', 'Demand').value('FactBaseline'),
        }, {
            xtype: 'numberfield', allowDecimals: false, allowExponential: false, allowBlank: true, allowOnlyWhitespace: true,
            name: 'FactDuration',
            minValue: 0,
            fieldLabel: l10n.ns('tpm', 'Demand').value('FactDuration'),
        }, {
            xtype: 'numberfield', allowDecimals: false, allowExponential: false, allowBlank: true, allowOnlyWhitespace: true,
            name: 'FactUplift',
            minValue: 0,
            fieldLabel: l10n.ns('tpm', 'Demand').value('FactUplift'),
        }, {
            xtype: 'numberfield', allowDecimals: false, allowExponential: false, allowBlank: true, allowOnlyWhitespace: true,
            name: 'FactIncremental',
            minValue: 0,
            fieldLabel: l10n.ns('tpm', 'Demand').value('FactIncremental'),
        }, {
            xtype: 'numberfield', allowDecimals: false, allowExponential: false, allowBlank: true, allowOnlyWhitespace: true,
            name: 'FactActivity',
            minValue: 0,
            fieldLabel: l10n.ns('tpm', 'Demand').value('FactActivity'),
        }, {
            xtype: 'numberfield', allowDecimals: false, allowExponential: false, allowBlank: true, allowOnlyWhitespace: true,
            name: 'FactSteal',
            minValue: 0,
            fieldLabel: l10n.ns('tpm', 'Demand').value('FactSteal'),
        }]
    }
});
