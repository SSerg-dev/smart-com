Ext.define('App.view.tpm.clientdashboard.PromoWeeksRS', {
    extend: 'Ext.panel.Panel',
    alias: 'widget.promoweeksrs',
    itemId: 'promoWeeksRS',
    cls: 'promo-weeks',
    width: '100%',
    layout: 'fit',

    items: [{
        xtype: 'container',
        width: '100%',
        overflowY: 'scroll',
        cls: 'promo-weeks-panels-container',
        itemId: 'promoWeeksRSPanelsContainer',
        layout: {
            type: 'vbox',
        },
        items: [],
    }],

    addPromoWeeksPanels: function (promoWeeksPanels) {
        var promoWeeksRSPanelsContainer = this.down('#promoWeeksRSPanelsContainer');
        if (promoWeeksRSPanelsContainer) {
            promoWeeksRSPanelsContainer.add(promoWeeksPanels);
        }
    },

    removePromoWeeksPanels: function () {
        var promoWeeksRSPanelsContainer = this.down('#promoWeeksRSPanelsContainer');
        promoWeeksRSPanelsContainer.removeAll();
    }
});