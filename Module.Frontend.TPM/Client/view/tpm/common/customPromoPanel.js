Ext.define('App.view.tpm.common.customPromoPanel', {
    extend: 'Ext.panel.Panel',
    alias: 'widget.custompromopanel',

    cls: 'custom-promo-panel',
    margin: 10,
    height: 'auto',
    width: 'auto',
    layout: 'fit',

    items: [{
        xtype: 'fieldset',
        title: 'Calendar Priority',
        layout: 'fit',
        items: []
    }]
})