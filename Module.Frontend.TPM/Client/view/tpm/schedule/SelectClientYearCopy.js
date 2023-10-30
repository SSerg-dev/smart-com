Ext.define('App.view.tpm.scheduler.SelectClientYearCopy', {
    extend: 'App.view.core.base.BaseModalWindow',
    alias: 'widget.selectClientYearCopy',
    title: l10n.ns('tpm', 'SelectClientYearCopy').value('SelectClientYearCopy'),
    cls: 'ClientPromoTypeFilter',

    width: 500,
    minWidth: 500,
    minHeight: 450,
    height: 450,
    items: [{
        xtype: 'panel',
        cls: 'filter-panel',
        layout: {
            type: 'vbox',
            align: 'stretch'
        },
        items: [{
            xtype: 'panel',
            cls: 'scheduleFilterFieldset',
            layout: {
                type: 'vbox',
                align: 'stretch'
            },
            flex: 1,
            items: [
                {
                    //Фильтр по клиентам
                    xtype: 'fieldset',
                    itemId: 'clientsFieldset',
                    title: l10n.ns('tpm', 'SelectClientYearCopy').value('Clients'),
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
                    },
                    {
                        xtype: 'checkbox',
                        boxLabel: 'Move dates by week',
                        checked: false,
                        itemId: 'checkboxDate',
                        height: 30,
                    },
                    {
                        // Место  для скролла
                        padding: '5 0 5 10',
                        autoScroll: true,
                        overflowY: 'scroll',
                        cls: 'scrollpanel radiogroup',
                        wasValid: true,
                        xtype: 'radiogroup',
                        itemId: 'clientsRadioGroup',
                        columns: 1,
                        vertical: true,
                        flex: 1,
                    }]
                }
            ]
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
