Ext.define('App.view.tpm.promosupport.CustomPromoSupportEditor', {
    extend: 'Ext.window.Window',
    alias: 'widget.custompromosupporteditor',
    title: l10n.ns('tpm', 'compositePanelTitles').value('PromoSupport'),
    ghost: false,
    modal: true,
    header: false,
    constrain: true,
    layout: 'fit',

    height: '95%',
    width: '95%',

    dockedItems: [{
        xtype: 'promosupportlefttoolbar',
        dock: 'left'
    }],

    items: [{
        xtype: 'panel',
        layout: 'fit',

        defaults: {
            padding: '10 15 15 15'
        },

        dockedItems: [{
            xtype: 'promosupportbottomtoolbar',
            dock: 'bottom'
        }, {
            xtype: 'promosupportformtoptoolbar',
            dock: 'top'
        }],

        items: [{
            xtype: 'panel',
            layout: 'fit',
            style: {
                background: '#829cb8',
            },

            dockedItems: [{
                xtype: 'promosupporttoptoolbar',
                dock: 'top'
            }],

            items: [{
                xtype: 'container',
                itemId: 'customPromoSupportEditorContainer',
                flex: 1,
                height: '100%',
                padding: '5 5 0 5',
                cls: 'promo-support-custom-promo-panel-container',
                layout: {
                    type: 'vbox',
                    align: 'stretch'
                },
                items: [{
                    xtype: 'promosupportform',
                    height: 'auto',
                }, {
                    //виджет для отображения прикрепленных промо в окне создания PromoSupport, 
                    //дублирует поля Promo(в отличие от PromoLinked, который показывает поля PromoSupportPromo, так сделано, потому что в PromoSupportPromo есть дубли по PromoId)
                    xtype: 'promolinkedviewer',
                    flex: 1
                }]
            }]
        }]
    }]
})