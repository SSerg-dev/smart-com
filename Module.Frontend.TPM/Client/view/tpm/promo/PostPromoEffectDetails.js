Ext.define('App.view.tpm.promo.PostPromoEffectDetails', {
    extend: 'App.view.core.base.BaseModalWindow',
    alias: 'widget.postpromoeffectdetails',
    title: l10n.ns('tpm', 'PostPromoEffectDetails').value('mainTitle'),

    minWidth: 500,
    maxWidth: 500,
    minHeight: 200,

    layout: {
        type: 'vbox',
        align: 'stretch',
        pack: 'center'
    },

    items: [{
        xtype: 'container',
        cls: 'custom-promo-panel-container',
        layout: {
            type: 'vbox',
            align: 'stretch'
        },
        items: [{
            xtype: 'custompromopanel',
            name: 'budgetItemsPanel',
            flex: 1,
            items: [{
                xtype: 'fieldset',
                title: l10n.ns('tpm', 'PostPromoEffectDetails').value('PostPromoEffectFieldsetTitle'), 
                layout: {
                    type: 'vbox',
                    align: 'stretch',
                    pack: 'center',
                },
                padding: '5 10 10 10',
                defaults: {
                    margin: '5 0 0 0',
                },
                items: []
            }]
        }]
    }],

    buttons: [{
        text: l10n.ns('tpm', 'PostPromoEffectDetails').value('close'),
        itemId: 'close'
    }]
});