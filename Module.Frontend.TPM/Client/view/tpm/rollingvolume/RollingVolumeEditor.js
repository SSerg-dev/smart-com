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
            readOnly: true,
            readOnlyCls: 'field-for-read-only',
        }, {
             xtype: 'singlelinedisplayfield',
             fieldLabel: l10n.ns('tpm', 'RollingVolume').value('SKU'),
             name: 'SKU',
             readOnly: true,
             readOnlyCls: 'field-for-read-only',
        }, {
            xtype: 'singlelinedisplayfield',
            fieldLabel: l10n.ns('tpm', 'RollingVolume').value('BrandTech'),
            name: 'BrandTech',
            readOnly: true,
            readOnlyCls: 'field-for-read-only',
        }, {
             xtype: 'singlelinedisplayfield',             name: 'DemandGroup',
             readOnly: true,
             readOnlyCls: 'field-for-read-only',             fieldLabel: l10n.ns('tpm', 'RollingVolume').value('DemandGroup'),
        }, {
            xtype: 'singlelinedisplayfield',            name: 'Week',
            readOnly: true,
            readOnlyCls: 'field-for-read-only',            fieldLabel: l10n.ns('tpm', 'RollingVolume').value('Week'),
        }, {
            xtype: 'singlelinedisplayfield',
            readOnly: true,
            readOnlyCls: 'field-for-read-only',
            renderer: Ext.util.Format.numberRenderer('0.00'),            name: 'PlanProductIncrementalQTY',            fieldLabel: l10n.ns('tpm', 'RollingVolume').value('PlanProductIncrementalQTY'),
        },{
            xtype: 'singlelinedisplayfield',
            readOnly: true,
            readOnlyCls: 'field-for-read-only',
            renderer: Ext.util.Format.numberRenderer('0.00'),            name: 'Actuals',            fieldLabel: l10n.ns('tpm', 'RollingVolume').value('Actuals'),
        }, {
            xtype: 'singlelinedisplayfield',
            readOnly: true,
            readOnlyCls: 'field-for-read-only',
            renderer: Ext.util.Format.numberRenderer('0.00'),            name: 'OpenOrders',            fieldLabel: l10n.ns('tpm', 'RollingVolume').value('OpenOrders'),
        }, {
            xtype: 'singlelinedisplayfield',
            readOnly: true,
            readOnlyCls: 'field-for-read-only',
            renderer: Ext.util.Format.numberRenderer('0.00'),            name: 'ActualOO',            fieldLabel: l10n.ns('tpm', 'RollingVolume').value('ActualOO'),
        }, {
            xtype: 'singlelinedisplayfield',
            readOnly: true,
            readOnlyCls: 'field-for-read-only',
            renderer: Ext.util.Format.numberRenderer('0.00'),            name: 'Baseline',             fieldLabel: l10n.ns('tpm', 'RollingVolume').value('Baseline'),
        }, {
            xtype: 'singlelinedisplayfield',
            readOnly: true,
            readOnlyCls: 'field-for-read-only',
            renderer: Ext.util.Format.numberRenderer('0.00'),            name: 'ActualIncremental',            fieldLabel: l10n.ns('tpm', 'RollingVolume').value('ActualIncremental'),
        }, {
            xtype: 'singlelinedisplayfield',
            readOnly: true,
            readOnlyCls: 'field-for-read-only',
            renderer: Ext.util.Format.numberRenderer('0.00'),            name: 'PreliminaryRollingVolumes',            fieldLabel: l10n.ns('tpm', 'RollingVolume').value('PreliminaryRollingVolumes'),
        }, {
            xtype: 'singlelinedisplayfield',
            readOnly: true,
            readOnlyCls: 'field-for-read-only',
            renderer: Ext.util.Format.numberRenderer('0.00'),            name: 'PreviousRollingVolumes',            fieldLabel: l10n.ns('tpm', 'RollingVolume').value('PreviousRollingVolumes'),
        }, {
            xtype: 'singlelinedisplayfield',
            readOnly: true,
            readOnlyCls: 'field-for-read-only',
            renderer: Ext.util.Format.numberRenderer('0.00'),            name: 'PromoDifference',            fieldLabel: l10n.ns('tpm', 'RollingVolume').value('PromoDifference'),
        },  {
            xtype: 'singlelinedisplayfield',
            readOnly: true,
            readOnlyCls: 'field-for-read-only',
            renderer: Ext.util.Format.numberRenderer('0.00'),            name: 'RollingVolumesCorrection',            fieldLabel: l10n.ns('tpm', 'RollingVolume').value('RollingVolumesCorrection'),
        },  {
            xtype: 'singlelinedisplayfield',
            readOnly: true,
            readOnlyCls: 'field-for-read-only',
            renderer: Ext.util.Format.numberRenderer('0.00'),            name: 'RollingVolumesTotal',            fieldLabel: l10n.ns('tpm', 'RollingVolume').value('RollingVolumesTotal'),
        },{            name: 'ManualRollingTotalVolumes',            xtype: 'numberfield', 
            fieldLabel: l10n.ns('tpm', 'RollingVolume').value('ManualRollingTotalVolumes'),
            minValue: 0,
        },{
            xtype: 'singlelinedisplayfield',
            readOnly: true,
            readOnlyCls: 'field-for-read-only',
            renderer: Ext.util.Format.numberRenderer('0.00'),            name: 'FullWeekDiff',            fieldLabel: l10n.ns('tpm', 'RollingVolume').value('FullWeekDiff'),
        },]
    }
});


