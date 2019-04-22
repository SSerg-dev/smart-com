Ext.define('App.view.tpm.promo.PromoSupport', {
    extend: 'Ext.panel.Panel',
    alias: 'widget.promosupporttab',

    clearSupportSubItems: function () {
        var xSites = this.down('container[name=support_xSites]');
        var catalog = this.down('container[name=support_Catalog]');
        var POSM = this.down('container[name=support_POSM]');

        xSites.removeAll();
        catalog.removeAll();
        POSM.removeAll();
    },

    // Добавление подстатей
    addSupportXSitesSubItem: function (record) {
        var xSites = this.down('container[name=support_xSites]');

        var subitemPanel = Ext.create('widget.custompromopanel', {
            xtype: 'custompromopanel',
            height: 100,
            flex: 1,
            items: [{
                xtype: 'fieldset',
                title: record.data.BudgetSubItemName,
                layout: 'hbox',
                items: [{
                    xtype: 'numberfield',
                    name: 'Quantity',
                    editable: false,
                    hideTrigger: true,
                    needReadOnly: true,
                    readOnly: true,
                    setReadOnly: function () { return false },
                    flex: 1,
                    padding: '0 5 5 5',
                    labelAlign: 'top',
                    fieldLabel: l10n.ns('tpm', 'Promo').value('Quantity'),
                    value: null
                }, {
                    xtype: 'numberfield',
                    name: 'ProdCostPer1',
                    editable: false,
                    hideTrigger: true,
                    needReadOnly: true,
                    readOnly: true,
                    setReadOnly: function () { return false },
                    flex: 1,
                    padding: '0 5 5 5',
                    labelAlign: 'top',
                    fieldLabel: l10n.ns('tpm', 'Promo').value('ProdCostPer1'),
                    value: null
                }, {
                    xtype: 'numberfield',
                    name: 'CostTE',
                    editable: false,
                    hideTrigger: true,
                    needReadOnly: true,
                    readOnly: true,
                    setReadOnly: function () { return false },
                    flex: 1,
                    padding: '0 5 5 5',
                    labelAlign: 'top',
                    fieldLabel: l10n.ns('tpm', 'Promo').value('CostTE'),
                    value: record.data.PlanCalculation
                }, {
                    xtype: 'numberfield',
                    name: 'ProdCostTotal',
                    editable: false,
                    hideTrigger: true,
                    needReadOnly: true,
                    readOnly: true,
                    setReadOnly: function () { return false },
                    flex: 1,
                    padding: '0 5 5 5',
                    labelAlign: 'top',
                    fieldLabel: l10n.ns('tpm', 'Promo').value('ProdCostTotal'),
                    value: null
                }]
            }]
        });

        xSites.add(subitemPanel);
    },

    addSupportCatalogSubItem: function (record) {
        var catalog = this.down('container[name=support_Catalog]');

        var subitemPanel = Ext.create('widget.custompromopanel', {
            xtype: 'custompromopanel',
            height: 100,
            flex: 1,
            items: [{
                xtype: 'fieldset',
                title: record.data.BudgetSubItemName,
                layout: 'hbox',
                items: [{
                    xtype: 'numberfield',
                    name: 'CostTE',
                    editable: false,
                    hideTrigger: true,
                    needReadOnly: true,
                    readOnly: true,
                    setReadOnly: function () { return false },
                    flex: 1,
                    padding: '0 5 5 5',
                    labelAlign: 'top',
                    fieldLabel: l10n.ns('tpm', 'Promo').value('CostTE'),
                    value: record.data.PlanCalculation
                }, {
                    xtype: 'numberfield',
                    name: 'ProdCostTotal',
                    editable: false,
                    hideTrigger: true,
                    needReadOnly: true,
                    readOnly: true,
                    setReadOnly: function () { return false },
                    flex: 1,
                    padding: '0 5 5 5',
                    labelAlign: 'top',
                    fieldLabel: l10n.ns('tpm', 'Promo').value('ProdCostTotal'),
                    value: null
                }]
            }]
        });

        catalog.add(subitemPanel);
    },

    addSupportPOSMSubItem: function (record) {
        var POSM = this.down('container[name=support_POSM]');

        var subitemPanel = Ext.create('widget.custompromopanel', {
            xtype: 'custompromopanel',
            height: 100,
            flex: 1,
            items: [{
                xtype: 'fieldset',
                title: record.data.BudgetSubItemName,
                layout: 'hbox',
                items: [{
                    xtype: 'numberfield',
                    name: 'Quantity',
                    editable: false,
                    hideTrigger: true,
                    needReadOnly: true,
                    readOnly: true,
                    setReadOnly: function () { return false },
                    flex: 1,
                    padding: '0 5 5 5',
                    labelAlign: 'top',
                    fieldLabel: l10n.ns('tpm', 'Promo').value('Quantity'),
                    value: null
                }, {
                    xtype: 'numberfield',
                    name: 'ProdCostPer1',
                    editable: false,
                    hideTrigger: true,
                    needReadOnly: true,
                    readOnly: true,
                    setReadOnly: function () { return false },
                    flex: 1,
                    padding: '0 5 5 5',
                    labelAlign: 'top',
                    fieldLabel: l10n.ns('tpm', 'Promo').value('ProdCostPer1'),
                    value: null
                }, {
                    xtype: 'numberfield',
                    name: 'CostTE',
                    editable: false,
                    hideTrigger: true,
                    needReadOnly: true,
                    readOnly: true,
                    setReadOnly: function () { return false },
                    flex: 1,
                    padding: '0 5 5 5',
                    labelAlign: 'top',
                    fieldLabel: l10n.ns('tpm', 'Promo').value('CostTE'),
                    value: record.data.PlanCalculation
                }, {
                    xtype: 'numberfield',
                    name: 'ProdCostTotal',
                    editable: false,
                    hideTrigger: true,
                    needReadOnly: true,
                    readOnly: true,
                    setReadOnly: function () { return false },
                    flex: 1,
                    padding: '0 5 5 5',
                    labelAlign: 'top',
                    fieldLabel: l10n.ns('tpm', 'Promo').value('ProdCostTotal'),
                    value: null
                }, {
                    xtype: 'numberfield',
                    name: 'Event',
                    editable: false,
                    hideTrigger: true,
                    needReadOnly: true,
                    readOnly: true,
                    setReadOnly: function () { return false },
                    flex: 1,
                    padding: '0 5 5 5',
                    labelAlign: 'top',
                    fieldLabel: l10n.ns('tpm', 'Promo').value('Event'),
                    value: null
                }]
            }]
        });

        POSM.add(subitemPanel);
    },

    items: [{
        xtype: 'container',
        cls: 'promo-editor-custom-scroll-items',        
        items: [{
            xtype: 'panel',
            name: 'support_step1',
            itemId: 'support_step1',
            minHeight: 150,
            bodyStyle: { "background-color": "#F9F9F9" },
            cls: 'promoform-item-wrap',
            header: {
                title: l10n.ns('tpm', 'promoStap').value('supportStep1'),
                cls: 'promo-header-item',
            },
            layout: 'fit',

            dockedItems: [{
                xtype: 'customtoptreetoolbar',
                dock: 'top',
                items: [{
                    xtype: 'container',
                    height: '100%',
                    flex: 1,
                    margin: '0 10px 0 10px',
                    layout: {
                        type: 'hbox',
                        align: 'middle',
                        pack: 'center'
                    },
                    items: [{
                        xtype: 'tbspacer',
                        flex: 1
                    }, {
                        xtype: 'button',
                        cls: 'promoStep-dockedBtn',
                        itemId: 'xSitesAddSubItem',
                        text: 'Add sub item',
                        tooltip: 'Add sub item',
                        glyph: 0xf415,
                        icon: '/'
                    }]
                }]
            }],

            items: [{
                xtype: 'container',
                cls: 'custom-promo-panel-container',
                name: 'support_xSites',
                layout: {
                    type: 'vbox',
                    align: 'stretch'
                },
                items: []
            }]
        }, {
            xtype: 'panel',
            name: 'support_step2',
            itemId: 'support_step2',
            minHeight: 150,
            bodyStyle: { "background-color": "#F9F9F9" },
            cls: 'promoform-item-wrap',
            header: {
                title: l10n.ns('tpm', 'promoStap').value('supportStep2'),
                cls: 'promo-header-item',
            },
            layout: 'fit',

            dockedItems: [{
                xtype: 'customtoptreetoolbar',
                dock: 'top',
                items: [{
                    xtype: 'container',
                    height: '100%',
                    flex: 1,
                    margin: '0 10px 0 10px',
                    layout: {
                        type: 'hbox',
                        align: 'middle',
                        pack: 'center'
                    },
                    items: [{
                        xtype: 'tbspacer',
                        flex: 1
                    }, {
                        xtype: 'button',
                        cls: 'promoStep-dockedBtn',
                        itemId: 'CatalogAddSubItem',
                        text: 'Add sub item',
                        tooltip: 'Add sub item',
                        glyph: 0xf415,
                        icon: '/'
                    }]
                }]
            }],

            items: [{
                xtype: 'container',
                name: 'support_Catalog',
                cls: 'custom-promo-panel-container',
                layout: {
                    type: 'vbox',
                    align: 'stretch'
                },
                items: []
            }]
        }, {
            xtype: 'panel',
            name: 'support_step3',
            itemId: 'support_step3',
            bodyStyle: { "background-color": "#99a9b1" },
            cls: 'promoform-item-wrap',
            needToSetHeight: true,
            header: {
                title: l10n.ns('tpm', 'promoStap').value('supportStep3'),
                cls: 'promo-header-item',
            },
            layout: {
                type: 'vbox',
                align: 'stretch'
            },

            dockedItems: [{
                xtype: 'customtoptreetoolbar',
                dock: 'top',
                items: [{
                    xtype: 'container',
                    height: '100%',
                    flex: 1,
                    margin: '0 10px 0 10px',
                    layout: {
                        type: 'hbox',
                        align: 'middle',
                        pack: 'center'
                    },
                    items: [{
                        xtype: 'tbspacer',
                        flex: 1
                    }, {
                        xtype: 'button',
                        cls: 'promoStep-dockedBtn',
                        itemId: 'POSMAddSubItem',
                        text: 'Add sub item',
                        tooltip: 'Add sub item',
                        glyph: 0xf415,
                        icon: '/'
                    }]
                }]
            }],

            items: [{
                xtype: 'container',
                name: 'support_POSM',
                cls: 'custom-promo-panel-container',
                minHeight: 78,
                layout: {
                    type: 'vbox',
                    align: 'stretch'
                },
                items: []
            }]
        }]
    }]
})