Ext.define('App.view.tpm.btl.BTLPromoEditor', {
    extend: 'App.view.core.common.EditorDetailWindow',
    alias: 'widget.btlpromoeditor',
    width: 800,
    minWidth: 800,
    maxHeight: 600,

    listeners: {
        afterrender: function (window) {
            window.down('#edit').setVisible(false);
        },
    },

    items: {
        xtype: 'editorform',
        items: [{
            xtype: 'singlelinedisplayfield',
            fieldLabel: l10n.ns('tpm', 'BTLPromo').value('PromoNumber'),
            name: 'PromoNumber',
        }, {
            xtype: 'singlelinedisplayfield',
            format: '0.00',
            fieldLabel: l10n.ns('tpm', 'BTL').value('PlanPromoBTL'),
            name: 'PlanPromoBTL',
        }, {
            xtype: 'singlelinedisplayfield',
            format: '0.00',
            fieldLabel: l10n.ns('tpm', 'BTL').value('ActualPromoBTL'),
            name: 'ActualPromoBTL',
        }, {
            xtype: 'singlelinedisplayfield',
            fieldLabel: l10n.ns('tpm', 'BTLPromo').value('PromoName'),
            name: 'PromoName'
        }, {
            xtype: 'singlelinedisplayfield',
            fieldLabel: l10n.ns('tpm', 'BTLPromo').value('PromoEventName'),
            name: 'PromoEventName'
        }, {
            xtype: 'singlelinedisplayfield',
            fieldLabel: l10n.ns('tpm', 'BTLPromo').value('PromoBrandTechName'),
            name: 'PromoBrandTechName',
        }, {
            xtype: 'singlelinedisplayfield',
            fieldLabel: l10n.ns('tpm', 'BTLPromo').value('PromoStatusName'),
            name: 'PromoStatusName',
        }, {
            xtype: 'singlelinedisplayfield',
            fieldLabel: l10n.ns('tpm', 'BTLPromo').value('ClientTreeFullPathName'),
            name: 'ClientTreeFullPathName',
            renderer: function (value) {
                    return renderWithDelimiter(value, ' > ', '  ');
            }
        }, {
            xtype: 'singlelinedisplayfield',
            fieldLabel: l10n.ns('tpm', 'BTLPromo').value('PromoStartDate'),
            name: 'PromoStartDate',
            renderer: Ext.util.Format.dateRenderer('d.m.Y')
        }, {
            xtype: 'singlelinedisplayfield',
            fieldLabel: l10n.ns('tpm', 'BTLPromo').value('PromoEndDate'),
            name: 'PromoEndDate',
            renderer: Ext.util.Format.dateRenderer('d.m.Y')
        }]
    },
});

