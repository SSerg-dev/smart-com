Ext.define('App.view.tpm.coefficientsi2so.DeletedCoefficientSI2SODetail', {
    extend: 'App.view.core.common.EditorDetailWindow',
    alias: 'widget.deletedcoefficientsi2sodetail',
    width: 500,
    minWidth: 500,
    maxHeight: 600,

    items: {
        xtype: 'editorform',
        itemId: 'deleteddetailform',
        columnsCount: 1,
        items: [{
            xtype: 'singlelinedisplayfield',
            name: 'DeletedDate',
            renderer: Ext.util.Format.dateRenderer('d.m.Y H:i:s'),
            fieldLabel: l10n.ns('core', 'BaseDeletedEntity').value('DeletedDate')
        }, {
            xtype: 'singlelinedisplayfield',
            name: 'DemandCode',
            fieldLabel: l10n.ns('tpm', 'CoefficientSI2SO').value('DemandCode'),
        }, {
            xtype: 'singlelinedisplayfield',
            name: 'BrandTechName',
            fieldLabel: l10n.ns('tpm', 'CoefficientSI2SO').value('BrandTechName'),
        }, {
            xtype: 'singlelinedisplayfield',
            name: 'BrandTechBrandTech_code',
            fieldLabel: l10n.ns('tpm', 'CoefficientSI2SO').value('BrandTechBrandTech_code'),
        }, {
            xtype: 'singlelinedisplayfield',
            name: 'CoefficientValue',
            fieldLabel: l10n.ns('tpm', 'CoefficientSI2SO').value('CoefficientValue'),
        }]
    }
})