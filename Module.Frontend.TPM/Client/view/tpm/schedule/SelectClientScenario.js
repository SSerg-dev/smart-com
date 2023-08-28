Ext.define('App.view.tpm.scheduler.SelectClientScenario', {
    extend: 'App.view.core.base.BaseModalWindow',
    alias: 'widget.selectClientScenario',
    title: l10n.ns('tpm', 'SelectClientScenario').value('SaveClientScenario'),
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
            items: [{
                xtype: 'fieldset',
                itemId: 'scenarioFieldset',
                title: l10n.ns('tpm', 'SelectClientScenario').value('ScenarioType'),
                layout: {
                    type: 'vbox',
                    align: 'stretch'
                },
                flex: 1,
                items: [
                    {
                        xtype: 'combobox',
                        itemId: 'scenarioType',
                        editable: false,
                        allowBlank: false,
                        queryMode: 'local',
                        valueField: 'text',
                        forceSelection: true,
                        store: {
                            type: 'scenariotypestore'
                        },
                        height: 30,
                    }
                ]
                },
                {
                //Фильтр по клиентам
                xtype: 'fieldset',
                itemId: 'clientsFieldset',
                title: l10n.ns('tpm', 'SelectClientScenario').value('Clients'),
                layout: {
                    type: 'vbox',
                    align: 'stretch'
                },
                flex: 3,
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
