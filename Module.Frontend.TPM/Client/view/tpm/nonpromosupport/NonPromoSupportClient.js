Ext.define('App.view.tpm.nonpromosupport.NonPromoSupportClient', {
    extend: 'App.view.core.base.BaseModalWindow',
	alias: 'widget.nonpromosupportclient',
	title: l10n.ns('tpm', 'compositePanelTitles').value('ChooseNonPromoSupportClient'),
    cls: 'promo-support-type',

    minWidth: 400,
    maxWidth: 400,
    minHeight: 150,
    maxHeight: 150,

    layout: {
        type: 'vbox',
        align: 'stretch',
        pack: 'center'
    },

    items: [{
        xtype: 'form',
        cls: 'promo-support-type-container',
        items: [{
            xtype: 'treesearchfield',
            name: 'ClientTreeId',
            cls: 'promo-support-type-client-tree-search-field',
            fieldLabel: l10n.ns('tpm', 'PromoSupport').value('ClientTreeFullPathName'),
            selectorWidget: 'clienttree',
            selectorWidgetConfig: {
                needLoadTree: true,
                needBaseClients: true
            },
            valueField: 'Id',
            displayField: 'FullPathName',
            labelSeparator: '',
            allowBlank: false,
            store: {
                storeId: 'clienttreestore',
                model: 'App.model.tpm.clienttree.ClientTree',
                autoLoad: false,
                root: {}
			},
            mapping: [{
                from: 'FullPathName',
                to: 'ClientTreeFullPathName'
            }]
        }]
    }]
});