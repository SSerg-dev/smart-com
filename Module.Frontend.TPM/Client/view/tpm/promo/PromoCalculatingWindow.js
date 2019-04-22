Ext.define('App.view.tpm.promo.PromoCalculatingWindow', {
    extend: 'App.view.core.base.BaseModalWindow',
    alias: 'widget.promocalculatingwindow',

    title: l10n.ns('tpm', 'compositePanelTitles').value('PromoCalculatingWindow'),
    renderTo: Ext.getBody(),
    constrain: true,
    modal: true,
    width: 550,
    height: 500,
    layout: 'fit',
    jobEnded: false,
    upWindow: null,
    upGrid: null,

    items: [{
        xtype: 'textarea',
        readOnly: true
    }],

    buttons: [{
        text: l10n.ns('tpm', 'button').value('Close'),
        action: 'cancel',
        handler: function () {            
            this.up('window').close();
        }
    }]
});