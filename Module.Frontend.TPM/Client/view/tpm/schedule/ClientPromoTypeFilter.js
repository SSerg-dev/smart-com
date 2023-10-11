Ext.define('App.view.tpm.scheduler.ClientPromoTypeFilter', {
    extend: 'App.view.core.base.BaseModalWindow',
    alias: 'widget.clientPromoTypeFilter',
    title: l10n.ns('tpm', 'ClientPromoTypeFilter').value('ClientsAndPromoType'),
    cls: 'ClientPromoTypeFilter',

    width: 1000,
    minWidth: 800,
    minHeight: 370,
    height: 520,
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
                title: l10n.ns('tpm', 'ClientPromoTypeFilter').value('Clients'),
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
                    cls: 'scrollpanel client-checkboxgroup checkboxgroup',
                    xtype: 'basetreegridview',
                    itemId: 'calendarclientview',
                    dispalyField: 'text',
                    height: 320,
                    store: {
                        type: 'simpletreestore',
                        model: 'App.model.tpm.baseclient.BaseClientView',
                        storeId: 'clientbaseviewstore',
                        //root: { expanded: true, text: "", "children": [] }
                        //Autoload не работает для treestore
                        //root: {
                        //    text: 'en',
                        //    id: 1,
                        //    expanded: true,
                        //    checked: false,
                        //    leaf: false,
                        //    objectId: 1,
                        //}
                    },
                    //columns: [
                    //    { xtype: 'treecolumn', header: 'Text', dataIndex: 'text', flex: 1 }
                    //]
                }]
            }]
        }, {
            xtype: 'panel',
            cls: 'scheduleFilterFieldset',
            layout: 'fit',
            flex: 1,
            items: [{
                //Фильтр по типам
                xtype: 'fieldset',
                title: l10n.ns('tpm', 'ClientPromoTypeFilter').value('PromoTypes'),
                layout: {
                    type: 'vbox',
                    align: 'stretch'
                },
                flex: 1,
                items: [{
                    xtype: 'checkboxfield',
                    itemId: 'selectAllTypes',
                    cls: 'selectAllFilters selectAllTypes',
                    boxLabel: l10n.ns('tpm', 'ClientPromoTypeFilter').value('SelectAll'),
                    height: 30,
                }, {
                    // Место  для скролла
                    padding: '5 0 5 10',
                    autoScroll: true,
                    overflowY: 'scroll',
                    cls: 'scrollpanel type-checkboxgroup checkboxgroup',

                    xtype: 'checkboxgroup',
                    itemId: 'typesCheckboxes',
                    columns: 1,
                    vertical: true,
                }]
            }]
            }, {
                xtype: 'panel',
                cls: 'scheduleFilterFieldset',
                layout: 'fit',
                flex: 1,
                items: [{
                    //Фильтр по Конкурентам
                    xtype: 'fieldset',
                    title: l10n.ns('tpm', 'ClientPromoTypeFilter').value('Competitors'),
                    layout: {
                        type: 'vbox',
                        align: 'stretch'
                    },
                    flex: 1,
                    items: [{
                        xtype: 'checkboxfield',
                        itemId: 'selectAllCompetitors',
                        cls: 'selectAllFilters selectAllTypes',
                        boxLabel: l10n.ns('tpm', 'ClientPromoTypeFilter').value('SelectAll'),
                        height: 30,
                    }, {
                        // Место  для скролла
                        padding: '5 0 5 10',
                        autoScroll: true,
                        overflowY: 'scroll',
                        cls: 'scrollpanel type-checkboxgroup checkboxgroup',

                        xtype: 'checkboxgroup',
                        itemId: 'competitorsCheckboxes',
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