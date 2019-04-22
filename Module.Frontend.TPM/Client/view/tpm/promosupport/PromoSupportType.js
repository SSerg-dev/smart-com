Ext.define('App.view.tpm.promosupporttype.PromoSupportType', {
    extend: 'App.view.core.base.BaseModalWindow',
    alias: 'widget.promosupporttype',
    title: l10n.ns('tpm', 'compositePanelTitles').value('AddPromoSupportType'),
    cls: 'promo-support-type',

    minWidth: 400,
    maxWidth: 400,
    minHeight: 365,
    maxHeight: 365,

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
    }, {
        xtype: 'container',
        cls: 'custom-promo-panel-container promo-support-type-select-list-container scrollpanel',
        autoScroll: true,
        layout: {
            type: 'vbox',
            align: 'stretch',
        },
        items: [{
            xtype: 'custompromopanel',
            cls: 'custom-promo-panel',
            height: '100%',
            items: [{
                xtype: 'fieldset',
                itemId: 'fieldsetWithButtonsForPromoSupport',
                title: 'Marketing TI',
                cls: 'promo-support-type-select-list-fieldset',
                items: []
            }]
        }]
    }]
});