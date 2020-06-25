Ext.define('App.view.tpm.clientdashboard.PromoWeeks', {
    extend: 'Ext.panel.Panel',
    alias: 'widget.promoweeks',
    itemId: 'promoWeeks',
    cls: 'promo-weeks',
    width: '100%',
    layout: 'fit',

    items: [{
        xtype: 'container',
        width: '100%',
        overflowY: 'scroll',
        cls: 'promo-weeks-panels-container',
        itemId: 'promoWeeksPanelsContainer',
        layout: {
            type: 'vbox',
        },
        items: [],
    }],

    addPromoWeeksPanels: function (promoWeeksPanels) {
        var promoWeeksPanelsContainer = this.down('#promoWeeksPanelsContainer');
        if (promoWeeksPanelsContainer) {
            promoWeeksPanelsContainer.add(promoWeeksPanels);
        }
    },

    removePromoWeeksPanels: function () {
        var promoWeeksPanelsContainer = this.down('#promoWeeksPanelsContainer');
        promoWeeksPanelsContainer.removeAll();
    }
});