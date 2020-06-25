Ext.define('App.view.tpm.rollingvolume.RollingVolumeEditor', {
    extend: 'App.view.core.common.EditorDetailWindow',
    alias: 'widget.rollingvolumeeditor',
    width: 700,
    minWidth: 700,
    maxHeight: 700, 

    items: {
        xtype: 'editorform',
        columnsCount: 2,
        items: [{
            xtype: 'singlelinedisplayfield',
            fieldLabel: l10n.ns('tpm', 'RollingVolume').value('ZREP'),
            name: 'ZREP',
        }, {
             xtype: 'singlelinedisplayfield',
             fieldLabel: l10n.ns('tpm', 'RollingVolume').value('SKU'),
             name: 'SKU',
        }, {
            xtype: 'singlelinedisplayfield',
            fieldLabel: l10n.ns('tpm', 'RollingVolume').value('BrandTech'),
            name: 'BrandTech',
        }, {
             xtype: 'singlelinedisplayfield',             name: 'DemandGroup',             fieldLabel: l10n.ns('tpm', 'RollingVolume').value('DemandGroup'),
        }, {
            xtype: 'singlelinedisplayfield',            name: 'Week',            fieldLabel: l10n.ns('tpm', 'RollingVolume').value('Week'),
        }, {
            xtype: 'numberfield',            name: 'Baseline',
            readOnly: true,            cls: 'field-for-read-only',            name: 'PlanProductIncrementalQTY',            fieldLabel: l10n.ns('tpm', 'RollingVolume').value('PlanProductIncrementalQTY'),
        },{
            xtype: 'numberfield',            name: 'Baseline',
            readOnly: true,            cls: 'field-for-read-only',            name: 'Actuals',            fieldLabel: l10n.ns('tpm', 'RollingVolume').value('Actuals'),
        }, {
            xtype: 'numberfield',            name: 'Baseline',
            readOnly: true,            cls: 'field-for-read-only',            name: 'OpenOrders',            fieldLabel: l10n.ns('tpm', 'RollingVolume').value('OpenOrders'),
        }, {
            xtype: 'numberfield',            name: 'Baseline',
            readOnly: true,            cls: 'field-for-read-only',            name: 'ActualOO',            fieldLabel: l10n.ns('tpm', 'RollingVolume').value('ActualOO'),
        }, {
            xtype: 'numberfield',            name: 'Baseline',
            readOnly: true,            cls: 'field-for-read-only',            fieldLabel: l10n.ns('tpm', 'RollingVolume').value('Baseline'),
        }, {
            xtype: 'numberfield',            name: 'Baseline',
            readOnly: true,            cls: 'field-for-read-only',            name: 'ActualIncremental',            fieldLabel: l10n.ns('tpm', 'RollingVolume').value('ActualIncremental'),
        }, {
            xtype: 'numberfield',            name: 'Baseline',
            readOnly: true,            cls: 'field-for-read-only',            name: 'PreliminaryRollingVolumes',            fieldLabel: l10n.ns('tpm', 'RollingVolume').value('PreliminaryRollingVolumes'),
        }, {
            xtype: 'numberfield',            name: 'Baseline',
            readOnly: true,            cls: 'field-for-read-only',            name: 'PreviousRollingVolumes',            fieldLabel: l10n.ns('tpm', 'RollingVolume').value('PreviousRollingVolumes'),
        }, {
            xtype: 'numberfield',            name: 'Baseline',
            readOnly: true,            cls: 'field-for-read-only',            name: 'PromoDifference',            fieldLabel: l10n.ns('tpm', 'RollingVolume').value('PromoDifference'),
        },  {
            xtype: 'numberfield',            name: 'Baseline',
            readOnly: true,            cls: 'field-for-read-only',            name: 'RollingVolumesCorrection',            fieldLabel: l10n.ns('tpm', 'RollingVolume').value('RollingVolumesCorrection'),
        },  {
            xtype: 'numberfield',            name: 'Baseline',
            readOnly: true,            cls: 'field-for-read-only',            name: 'RollingVolumesTotal',            fieldLabel: l10n.ns('tpm', 'RollingVolume').value('RollingVolumesTotal'),
        }, {            name: 'ManualRollingTotalVolumes',            xtype: 'numberfield', 
            fieldLabel: l10n.ns('tpm', 'RollingVolume').value('ManualRollingTotalVolumes'),
            minValue: 0,
        }]
    }
});


