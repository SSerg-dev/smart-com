Ext.define('App.view.tpm.promo.PromoProductSubrangeDetailsWindow', {
    extend: 'App.view.core.base.BaseModalWindow',
    alias: 'widget.promoproductsubrangedetailswindow',
    title: l10n.ns('tpm', 'compositePanelTitles').value('PromoProductSubrangeDetailsWindow'),
    cls: 'promo-budgets-details-window',

    width: "70%",
    height: "90%",
    minWidth: 800,
    minHeight: 500,

    items: [{
        xtype: 'container',
        itemId: 'containerForProductTreeGrid',
        maxHeight: '100%',
        cls: 'custom-promo-panel-container',
        layout: 'fit',
        padding: 10,
    }],

    buttons: [{
        text: l10n.ns('core', 'buttons').value('cancel'),
        itemId: 'close'
    }]
});