Ext.define('App.view.tpm.coefficientsi2so.HistoricalCoefficientSI2SODetail', {
    extend: 'App.view.core.common.EditorDetailWindow',
    alias: 'widget.historicalcoefficientsi2sodetail',
    width: 800,
    minWidth: 800,
    maxHeight: 600,

    items: {
        xtype: 'editorform',
        itemId: 'historicaldetailform',
        items: [{
            xtype: 'singlelinedisplayfield',
            name: '_User',
            fieldLabel: l10n.ns('tpm', 'HistoricalCoefficientSI2SO').value('_User')
        }, {
            xtype: 'singlelinedisplayfield',
            name: '_Role',
            fieldLabel: l10n.ns('tpm', 'HistoricalCoefficientSI2SO').value('_Role')
        }, {
            xtype: 'singlelinedisplayfield',
            name: '_EditDate',
            renderer: Ext.util.Format.dateRenderer('d.m.Y H:i:s'),
            fieldLabel: l10n.ns('tpm', 'HistoricalCoefficientSI2SO').value('_EditDate')
        }, {
            xtype: 'singlelinedisplayfield',
            name: '_Operation',
            renderer: App.RenderHelper.getLocalizedRenderer('tpm.HistoricalBrand', 'OperationType'),
            fieldLabel: l10n.ns('tpm', 'HistoricalCoefficientSI2SO').value('_Operation')
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
});
