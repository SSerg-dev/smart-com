Ext.define('App.view.tpm.promosupport.PromoSupportPromoCostProdEditor', {
    extend: 'App.view.core.common.EditorDetailWindow',
    alias: 'widget.promolinkedcostprodeditor',
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
            fieldLabel: l10n.ns('tpm', 'PromoSupportPromoCostProd').value('Number'),
            name: 'Number',      
        }, {
            xtype: 'singlelinedisplayfield',
            fieldLabel: l10n.ns('tpm', 'PromoSupportPromoCostProd').value('Name'),
            name: 'Name',      
        }, {
            xtype: 'singlelinedisplayfield',
            fieldLabel: l10n.ns('tpm', 'PromoSupportPromoCostProd').value('BrandTechName'),
            name: 'BrandTechName',      
        }, {
            xtype: 'singlelinedisplayfield',
            format: '0.00',
            fieldLabel: l10n.ns('tpm', 'PromoSupportPromoCostProd').value('PlanCostTE'),
            name: 'PlanCostProd'
        }, {
            xtype: 'singlelinedisplayfield',
			format: '0.00',
            fieldLabel: l10n.ns('tpm', 'PromoSupportPromoCostProd').value('ActualCostTE'),
            name: 'FactCostProd'
        }, {
            xtype: 'singlelinedisplayfield',
            fieldLabel: l10n.ns('tpm', 'PromoSupportPromoCostProd').value('EventName'),
            name: 'EventName',      
        }, {
            xtype: 'singlelinedisplayfield',
            fieldLabel: l10n.ns('tpm', 'PromoSupportPromoCostProd').value('StartDate'),
            name: 'StartDate',      
            renderer: Ext.util.Format.dateRenderer('d.m.Y')
        }, {
            xtype: 'singlelinedisplayfield',
            fieldLabel: l10n.ns('tpm', 'PromoSupportPromoCostProd').value('EndDate'),
            name: 'EndDate',
            renderer: Ext.util.Format.dateRenderer('d.m.Y')
        }, {
            xtype: 'singlelinedisplayfield',
            fieldLabel: l10n.ns('tpm', 'PromoSupportPromoCostProd').value('PromoStatusName'),
            name: 'PromoStatusName',      
        }]        
    },
});
