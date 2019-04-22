Ext.define('App.view.tpm.promosupport.PromoSupportPromoTICostEditor', {
    extend: 'App.view.core.common.EditorDetailWindow',
    alias: 'widget.promolinkedticostseditor',
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
            fieldLabel: l10n.ns('tpm', 'PromoSupportPromoTICost').value('Number'),
            name: 'Number',      
        }, {
            xtype: 'singlelinedisplayfield',
            fieldLabel: l10n.ns('tpm', 'PromoSupportPromoTICost').value('Name'),
            name: 'Name',      
        }, {
            xtype: 'singlelinedisplayfield',
            fieldLabel: l10n.ns('tpm', 'PromoSupportPromoTICost').value('BrandTechName'),
            name: 'BrandTechName',      
        }, {
            xtype: 'singlelinedisplayfield',
            format: '0.00',
            fieldLabel: l10n.ns('tpm', 'PromoSupportPromoTICost').value('PlanCostTE'),
            name: 'PlanCalculation'
        }, {
            xtype: 'singlelinedisplayfield',
			format: '0.00',
            fieldLabel: l10n.ns('tpm', 'PromoSupportPromoTICost').value('ActualCostTE'),
            name: 'FactCalculation'
        }, {
            xtype: 'singlelinedisplayfield',
            fieldLabel: l10n.ns('tpm', 'PromoSupportPromoTICost').value('EventName'),
            name: 'EventName',      
        }, {
            xtype: 'singlelinedisplayfield',
            fieldLabel: l10n.ns('tpm', 'PromoSupportPromoTICost').value('StartDate'),
            name: 'StartDate',      
            renderer: Ext.util.Format.dateRenderer('d.m.Y')
        }, {
            xtype: 'singlelinedisplayfield',
            fieldLabel: l10n.ns('tpm', 'PromoSupportPromoTICost').value('EndDate'),
            name: 'EndDate',
            renderer: Ext.util.Format.dateRenderer('d.m.Y')
        }, {
            xtype: 'singlelinedisplayfield',
            fieldLabel: l10n.ns('tpm', 'PromoSupportPromoTICost').value('PromoStatusName'),
            name: 'PromoStatusName',      
        }]        
    },
});
