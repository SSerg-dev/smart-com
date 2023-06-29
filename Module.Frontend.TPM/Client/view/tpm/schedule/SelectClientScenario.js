Ext.define('App.view.tpm.scheduler.SelectClientScenario', {
    extend: 'App.view.core.base.BaseModalWindow',
    alias: 'widget.selectClientScenario',
    title: l10n.ns('tpm', 'SelectClientScenario').value('SaveClientScenario'),
    cls: 'ClientPromoTypeFilter',

    width: 500,
    minWidth: 500,
    minHeight: 330,
    height: 330,
    items: [{
        xtype: 'panel',
        cls: 'filter-panel',
        layout: {
            type: 'hbox',
            align: 'stretch'
        },
        items: [{
            xtype: 'panel',
            cls: 'scheduleFilterFieldset',
            layout: 'fit',
            flex: 1,
            items: [{
                //Фильтр по клиентам
                xtype: 'fieldset',
                itemId: 'clientsFieldset',
                title: l10n.ns('tpm', 'SelectClientScenario').value('Clients'),
                layout: {
                    type: 'vbox',
                    align: 'stretch'
                },
                flex: 1,
                items: [{
                    xtype: 'textfieldwithtrigger',
                    itemId: 'textFilterByClients',
                    cls: 'textFilterByClients',
                    fillerText: 'Search Client',
                    height: 30,
                }, {
                    // Место  для скролла
                    padding: '5 0 5 10',
                    autoScroll: true,
                    overflowY: 'scroll',
                    cls: 'scrollpanel radiogroup',

                    xtype: 'radiogroup',
                    itemId: 'clientsRadioGroup',
                    columns: 1,
                    vertical: true,
                }]
            }]
        }],
    }],

    buttons: [{
        text: l10n.ns('core', 'buttons').value('cancel'),
        itemId: 'close'
    }, {
        text: l10n.ns('core', 'buttons').value('ok'),
        ui: 'green-button-footer-toolbar',
        itemId: 'apply'
    }]
})
